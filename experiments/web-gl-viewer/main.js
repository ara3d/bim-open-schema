// main.js
import { initViewer, buildSceneFromBimGeometry } from "./viewer.js";
import { loadBimGeometryFromZip } from "./parquetLoader.js";

const canvasContainer = document.getElementById("canvasContainer");
const dropOverlay = document.getElementById("dropOverlay");
const statusBar = document.getElementById("statusBar");
const fileInput = document.getElementById("fileInput");
const openButton = document.getElementById("openButton");

// Initialize Three.js viewer
initViewer(canvasContainer);

// Wire up UI
setupDragAndDrop();
setupFileInput();

function setStatus(msg) {
  console.log(msg);
  statusBar.textContent = msg;
}

function setupDragAndDrop() {
  window.addEventListener("dragover", (e) => {
    e.preventDefault();
    dropOverlay.classList.add("visible");
  });

  window.addEventListener("dragleave", (e) => {
    if (e.target === document.documentElement || e.target === document.body) {
      dropOverlay.classList.remove("visible");
    }
  });

  window.addEventListener("drop", (e) => {
    e.preventDefault();
    dropOverlay.classList.remove("visible");

    const file = e.dataTransfer.files[0];
    if (!file) return;
    handleZipFile(file);
  });
}

function setupFileInput() {
  openButton.addEventListener("click", () => fileInput.click());
  fileInput.addEventListener("change", () => {
    const file = fileInput.files[0];
    if (file) handleZipFile(file);
  });
}

async function handleZipFile(file) {
  try {
    setStatus(`Reading file: ${file.name}...`);
    const arrayBuffer = await file.arrayBuffer();

    // JSZip is loaded globally from index.html (non-module script)
    const zip = await JSZip.loadAsync(arrayBuffer);
    setStatus("ZIP loaded. Reading Parquet tables...");

    const bim = await loadBimGeometryFromZip(zip);

    setStatus("Building Three.js scene from BIM geometry...");
    buildSceneFromBimGeometry(bim);
    setStatus("Done. Use mouse to orbit / zoom.");
  } catch (err) {
    console.error(err);
    setStatus("Error loading ZIP: " + err.message);
  }
}
