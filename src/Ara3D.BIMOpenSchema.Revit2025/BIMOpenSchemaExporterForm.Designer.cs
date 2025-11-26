namespace Ara3D.BIMOpenSchema.Revit2025
{
    partial class BIMOpenSchemaExporterForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BIMOpenSchemaExporterForm));
            exportDirTextBox = new System.Windows.Forms.TextBox();
            label1 = new System.Windows.Forms.Label();
            chooseFolderButton = new System.Windows.Forms.Button();
            linkLabel1 = new System.Windows.Forms.LinkLabel();
            buttonExport = new System.Windows.Forms.Button();
            folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            checkBoxIncludeLinks = new System.Windows.Forms.CheckBox();
            progressBar1 = new System.Windows.Forms.ProgressBar();
            checkBoxMeshGeometry = new System.Windows.Forms.CheckBox();
            SuspendLayout();
            // 
            // exportDirTextBox
            // 
            exportDirTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            exportDirTextBox.Location = new System.Drawing.Point(11, 43);
            exportDirTextBox.Name = "exportDirTextBox";
            exportDirTextBox.Size = new System.Drawing.Size(573, 31);
            exportDirTextBox.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(11, 15);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(140, 25);
            label1.TabIndex = 2;
            label1.Text = "Export Directory";
            // 
            // chooseFolderButton
            // 
            chooseFolderButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            chooseFolderButton.Location = new System.Drawing.Point(591, 43);
            chooseFolderButton.Name = "chooseFolderButton";
            chooseFolderButton.Size = new System.Drawing.Size(43, 33);
            chooseFolderButton.TabIndex = 3;
            chooseFolderButton.Text = "...";
            chooseFolderButton.UseVisualStyleBackColor = true;
            chooseFolderButton.Click += chooseFolderButton_Click;
            // 
            // linkLabel1
            // 
            linkLabel1.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            linkLabel1.Location = new System.Drawing.Point(74, 237);
            linkLabel1.Name = "linkLabel1";
            linkLabel1.Size = new System.Drawing.Size(499, 25);
            linkLabel1.TabIndex = 6;
            linkLabel1.TabStop = true;
            linkLabel1.Text = "https://github.com/ara3d/bim-open-schema";
            linkLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            linkLabel1.Click += linkLabel1_Click;
            // 
            // buttonExport
            // 
            buttonExport.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            buttonExport.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            buttonExport.Location = new System.Drawing.Point(131, 177);
            buttonExport.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            buttonExport.Name = "buttonExport";
            buttonExport.Size = new System.Drawing.Size(364, 55);
            buttonExport.TabIndex = 8;
            buttonExport.Text = "Run Export";
            buttonExport.UseVisualStyleBackColor = true;
            buttonExport.Click += buttonExport_Click;
            // 
            // checkBoxIncludeLinks
            // 
            checkBoxIncludeLinks.AutoSize = true;
            checkBoxIncludeLinks.Checked = true;
            checkBoxIncludeLinks.CheckState = System.Windows.Forms.CheckState.Checked;
            checkBoxIncludeLinks.Location = new System.Drawing.Point(16, 88);
            checkBoxIncludeLinks.Name = "checkBoxIncludeLinks";
            checkBoxIncludeLinks.Size = new System.Drawing.Size(241, 29);
            checkBoxIncludeLinks.TabIndex = 9;
            checkBoxIncludeLinks.Text = "Include linked documents";
            checkBoxIncludeLinks.UseVisualStyleBackColor = true;
            // 
            // progressBar1
            // 
            progressBar1.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            progressBar1.Location = new System.Drawing.Point(11, 277);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new System.Drawing.Size(623, 33);
            progressBar1.TabIndex = 10;
            // 
            // checkBoxMeshGeometry
            // 
            checkBoxMeshGeometry.AutoSize = true;
            checkBoxMeshGeometry.Checked = true;
            checkBoxMeshGeometry.CheckState = System.Windows.Forms.CheckState.Checked;
            checkBoxMeshGeometry.Location = new System.Drawing.Point(16, 123);
            checkBoxMeshGeometry.Name = "checkBoxMeshGeometry";
            checkBoxMeshGeometry.Size = new System.Drawing.Size(225, 29);
            checkBoxMeshGeometry.TabIndex = 11;
            checkBoxMeshGeometry.Text = "Include mesh geometry";
            checkBoxMeshGeometry.UseVisualStyleBackColor = true;
            // 
            // BIMOpenSchemaExporterForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(646, 322);
            Controls.Add(checkBoxMeshGeometry);
            Controls.Add(progressBar1);
            Controls.Add(checkBoxIncludeLinks);
            Controls.Add(buttonExport);
            Controls.Add(linkLabel1);
            Controls.Add(chooseFolderButton);
            Controls.Add(label1);
            Controls.Add(exportDirTextBox);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            MinimumSize = new System.Drawing.Size(491, 326);
            Name = "BIMOpenSchemaExporterForm";
            Text = "BIM Open Schema - Parquet Exporter for Revit 2025";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.TextBox exportDirTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button chooseFolderButton;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Button buttonExport;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.CheckBox checkBoxIncludeLinks;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.CheckBox checkBoxMeshGeometry;
    }
}