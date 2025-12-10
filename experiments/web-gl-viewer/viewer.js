// viewer.js
import * as THREE from "https://unpkg.com/three@0.161.0/build/three.module.js";
import { OrbitControls } from "https://unpkg.com/three@0.161.0/examples/jsm/controls/OrbitControls.js";

let renderer;
let scene;
let camera;
let controls;
let currentRoot = null;

// Public API: initialize viewer with a container element
export function initViewer(canvasContainer) {
  renderer = new THREE.WebGLRenderer({ antialias: true });
  renderer.setPixelRatio(window.devicePixelRatio);
  renderer.setSize(window.innerWidth, window.innerHeight);
  renderer.outputEncoding = THREE.sRGBEncoding;
  canvasContainer.appendChild(renderer.domElement);

  scene = new THREE.Scene();
  scene.background = new THREE.Color(0x151515);

  camera = new THREE.PerspectiveCamera(
    50,
    window.innerWidth / window.innerHeight,
    0.1,
    10000
  );
  camera.position.set(10, 10, 10);

  controls = new OrbitControls(camera, renderer.domElement);
  controls.enableDamping = true;

  // Lighting
  const hemi = new THREE.HemisphereLight(0xffffff, 0x444444, 0.6);
  hemi.position.set(0, 1, 0);
  scene.add(hemi);

  const dir = new THREE.DirectionalLight(0xffffff, 0.8);
  dir.position.set(10, 20, 10);
  scene.add(dir);

  // Grid aligned to Z-up (BOS): grid in XY-plane, Z up
  const grid = new THREE.GridHelper(10, 10, 0x444444, 0x222222);
  grid.rotation.x = Math.PI / 2;
  scene.add(grid);

  window.addEventListener("resize", onWindowResize);
  animate();
}

function onWindowResize() {
  if (!camera || !renderer) return;
  const w = window.innerWidth;
  const h = window.innerHeight;
  camera.aspect = w / h;
  camera.updateProjectionMatrix();
  renderer.setSize(w, h);
}

function animate() {
  requestAnimationFrame(animate);
  if (controls && renderer && scene && camera) {
    controls.update();
    renderer.render(scene, camera);
  }
}

// Public API: build / rebuild the scene from a BimGeometry-like object
export function buildSceneFromBimGeometry(bim) {
  if (!scene) {
    console.warn("Viewer not initialized yet.");
    return;
  }

  // Remove previous root if any
  if (currentRoot) {
    scene.remove(currentRoot);
    currentRoot.traverse((obj) => {
      if (obj.geometry) obj.geometry.dispose?.();
      if (obj.material) {
        if (Array.isArray(obj.material)) {
          obj.material.forEach((m) => m.dispose?.());
        } else {
          obj.material.dispose?.();
        }
      }
    });
  }

  const root = new THREE.Group();
  currentRoot = root;
  scene.add(root);

  const vertexCount = bim.VertexX.length;
  const indexCount = bim.IndexBuffer.length;
  const meshCount = bim.MeshVertexOffset.length;
  const matCount = bim.MaterialRed.length;
  const elementCount = bim.ElementMeshIndex.length;

  console.log({
    vertexCount,
    indexCount,
    meshCount,
    matCount,
    elementCount,
  });

  const meshGeometries = new Array(meshCount);
  const materialCache = new Array(matCount);

  const verticesX = bim.VertexX;
  const verticesY = bim.VertexY;
  const verticesZ = bim.VertexZ;
  const indices = bim.IndexBuffer;

  const meshVertexOffset = bim.MeshVertexOffset;
  const meshIndexOffset = bim.MeshIndexOffset;

  // Build each mesh geometry by slicing shared buffers
  for (let mi = 0; mi < meshCount; mi++) {
    const vStart = meshVertexOffset[mi];
    const vEnd = mi + 1 < meshCount ? meshVertexOffset[mi + 1] : vertexCount;
    const iStart = meshIndexOffset[mi];
    const iEnd = mi + 1 < meshCount ? meshIndexOffset[mi + 1] : indexCount;

    const vCount = vEnd - vStart;
    const iCount = iEnd - iStart;

    if (vCount <= 0 || iCount <= 0) {
      console.warn(`Mesh ${mi} has no vertices or indices.`);
      continue;
    }

    const positionArray = new Float32Array(vCount * 3);
    for (let vi = 0; vi < vCount; vi++) {
      const srcIndex = vStart + vi;
      positionArray[vi * 3 + 0] = verticesX[srcIndex];
      positionArray[vi * 3 + 1] = verticesY[srcIndex];
      positionArray[vi * 3 + 2] = verticesZ[srcIndex];
    }

    const indexArray = new Uint32Array(iCount);
    for (let ii = 0; ii < iCount; ii++) {
      indexArray[ii] = indices[iStart + ii] - vStart; // localize to 0..vCount-1
    }

    const geom = new THREE.BufferGeometry();
    geom.setAttribute(
      "position",
      new THREE.BufferAttribute(positionArray, 3)
    );
    geom.setIndex(new THREE.BufferAttribute(indexArray, 1));
    geom.computeVertexNormals();

    meshGeometries[mi] = geom;
  }

  // Material helper
  function getMaterial(mi) {
    if (materialCache[mi]) return materialCache[mi];

    const r = (bim.MaterialRed[mi] ?? 255) / 255;
    const g = (bim.MaterialGreen[mi] ?? 255) / 255;
    const b = (bim.MaterialBlue[mi] ?? 255) / 255;
    const a = (bim.MaterialAlpha[mi] ?? 255) / 255;
    const roughness = (bim.MaterialRoughness[mi] ?? 128) / 255;
    const metallic = (bim.MaterialMetallic[mi] ?? 0) / 255;

    const mat = new THREE.MeshStandardMaterial({
      color: new THREE.Color(r, g, b),
      opacity: a,
      transparent: a < 0.999,
      roughness: roughness,
      metalness: metallic,
      side: THREE.FrontSide,
    });

    materialCache[mi] = mat;
    return mat;
  }

  // Build instances (elements)
  const bbox = new THREE.Box3();
  const tmpBox = new THREE.Box3();
  const elemCount = bim.ElementMeshIndex.length;

  for (let ei = 0; ei < elemCount; ei++) {
    const meshIndex = bim.ElementMeshIndex[ei];
    const matIndex = bim.ElementMaterialIndex[ei];
    const transformIndex = bim.ElementTransformIndex[ei];

    const geom = meshGeometries[meshIndex];
    if (!geom) continue;

    const mat = getMaterial(matIndex);
    const mesh = new THREE.Mesh(geom, mat);

    applyTransformFromTable(mesh, transformIndex, bim);
    root.add(mesh);

    tmpBox.setFromObject(mesh);
    if (ei === 0) bbox.copy(tmpBox);
    else bbox.union(tmpBox);
  }

  // Rotate root so that Z-up becomes Three.js Y-up
  root.rotation.x = -Math.PI / 2;

  frameCameraToBox(bbox);
}

function applyTransformFromTable(object3d, ti, bim) {
  const tx = bim.TransformTX[ti] ?? 0;
  const ty = bim.TransformTY[ti] ?? 0;
  const tz = bim.TransformTZ[ti] ?? 0;

  const qx = bim.TransformQX[ti] ?? 0;
  const qy = bim.TransformQY[ti] ?? 0;
  const qz = bim.TransformQZ[ti] ?? 0;
  const qw = bim.TransformQW[ti] ?? 1;

  const sx = bim.TransformSX[ti] ?? 1;
  const sy = bim.TransformSY[ti] ?? 1;
  const sz = bim.TransformSZ[ti] ?? 1;

  object3d.position.set(tx, ty, tz);
  object3d.quaternion.set(qx, qy, qz, qw).normalize();
  object3d.scale.set(sx, sy, sz);
  object3d.updateMatrix();
}

function frameCameraToBox(box) {
  if (!camera || !controls) return;
  if (box.isEmpty()) return;

  const size = new THREE.Vector3();
  const center = new THREE.Vector3();
  box.getSize(size);
  box.getCenter(center);

  const radius = size.length() * 0.5;
  const fov = (camera.fov * Math.PI) / 180;
  let dist = radius / Math.sin(fov / 2);
  if (!Number.isFinite(dist) || dist <= 0) dist = 10;

  camera.position.copy(center).add(new THREE.Vector3(dist, dist, dist));
  camera.near = dist / 100;
  camera.far = dist * 10;
  camera.updateProjectionMatrix();

  controls.target.copy(center);
  controls.update();
}
