namespace GeoMagGUI
{
    partial class frmMain
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
            this.buttonSelectModel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonSelectModel
            // 
            this.buttonSelectModel.Location = new System.Drawing.Point(162, 202);
            this.buttonSelectModel.Name = "buttonSelectModel";
            this.buttonSelectModel.Size = new System.Drawing.Size(75, 23);
            this.buttonSelectModel.TabIndex = 0;
            this.buttonSelectModel.Text = "Load Model";
            this.buttonSelectModel.UseVisualStyleBackColor = true;
            this.buttonSelectModel.Click += new System.EventHandler(this.buttonSelectModel_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.buttonSelectModel);
            this.Name = "frmMain";
            this.Text = "GeoMag";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonSelectModel;
    }
}

