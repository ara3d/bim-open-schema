using System;
using System.Windows.Forms;
using Ara3D.Utils;

namespace Ara3D.BIMOpenSchema.Revit2025
{
    public partial class BIMOpenSchemaExporterForm : Form
    {
        public class ExportSettings
        {
            public DirectoryPath Folder;
            public bool IncludeLinks;
            public bool IncludeGeometry;
        }

        public void UpdateProgress(int index, int count)
        {
            progressBar1.Minimum = 0;
            progressBar1.Maximum = count + 1;
            progressBar1.Value = Math.Clamp(index, progressBar1.Minimum, count);
        }

        public Autodesk.Revit.DB.Document CurrentDocument;
        public Action<ExportSettings> ExportAction;

        public BIMOpenSchemaExporterForm()
        {
            InitializeComponent();
            var defaultExportDir = DefaultFolder;
            try
            {
                // Try to create the default dir if it doesn't exists 
                defaultExportDir.Create();
            }
            catch (Exception ex)
            {
                // Quietly use the "my documents"
                defaultExportDir = new DirectoryPath(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
            }
            exportDirTextBox.Text = defaultExportDir;
            FormClosing += (_, args) =>
            {
                args.Cancel = true;
                Hide(); // Hide the form instead of closing it
            };
        }

        public void Show(Autodesk.Revit.DB.Document doc, Action<ExportSettings> export)
        {
            progressBar1.Value = 0;
            if (doc == null)
            {
                MessageBox.Show("No Revit document is available. Please open a Revit document first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (export == null)
            {
                MessageBox.Show("Internal error: no export action provided.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ExportAction = export;
            CurrentDocument = doc;
            Show();
        }

        public static Utils.DirectoryPath DefaultFolder =>
            new DirectoryPath(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)).RelativeFolder("BIMOpenSchemaExports");

        private void linkLabel1_Click(object sender, EventArgs e)
        {
            try
            {
                var url = @"https://github.com/ara3d/bim-open-schema";
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to open link: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void chooseFolderButton_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.SelectedPath = GetCurrentExportFolder().GetFullPath();
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                exportDirTextBox.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        public ExportSettings GetExportSettings()
        {
            var settings = new ExportSettings
            {
                Folder = GetCurrentExportFolder(),
                IncludeLinks = checkBoxIncludeLinks.Checked,
            };
            return settings;
        }

        public DirectoryPath CurrentDocumentDir
            => new Utils.FilePath(CurrentDocument.PathName).GetDirectory();

        public DirectoryPath GetCurrentExportFolder()
        {
            var folder = exportDirTextBox.Text;
            if (string.IsNullOrEmpty(folder))
            {
                folder = DefaultFolder;
            }
            return new DirectoryPath(folder);
        }

        private void buttonExport_Click(object sender, EventArgs e)
        {
            var settings = GetExportSettings();

            var folder = settings.Folder;
            try
            {
                if (!System.IO.Directory.Exists(folder))
                {
                    MessageBox.Show($"The folder {folder} does not exist. Please choose a valid folder.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                folder.TestWrite();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"The folder {folder} was not writeable. Error {ex.Message}. Please choose a different folder.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                ExportAction.Invoke(settings);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred during export: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
