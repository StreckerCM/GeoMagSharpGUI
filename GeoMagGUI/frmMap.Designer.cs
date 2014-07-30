namespace GeoMagGUI
{
    partial class frmMap
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMap));
            this.gMapControlLocation = new GMap.NET.WindowsForms.GMapControl();
            this.SuspendLayout();
            // 
            // gMapControlLocation
            // 
            this.gMapControlLocation.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gMapControlLocation.Bearing = 0F;
            this.gMapControlLocation.CanDragMap = false;
            this.gMapControlLocation.GrayScaleMode = false;
            this.gMapControlLocation.LevelsKeepInMemmory = 5;
            this.gMapControlLocation.Location = new System.Drawing.Point(9, 9);
            this.gMapControlLocation.MarkersEnabled = true;
            this.gMapControlLocation.MaxZoom = 18;
            this.gMapControlLocation.MinZoom = 0;
            this.gMapControlLocation.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.ViewCenter;
            this.gMapControlLocation.Name = "gMapControlLocation";
            this.gMapControlLocation.NegativeMode = false;
            this.gMapControlLocation.PolygonsEnabled = true;
            this.gMapControlLocation.RetryLoadTile = 0;
            this.gMapControlLocation.RoutesEnabled = true;
            this.gMapControlLocation.ShowTileGridLines = false;
            this.gMapControlLocation.Size = new System.Drawing.Size(443, 319);
            this.gMapControlLocation.TabIndex = 0;
            this.gMapControlLocation.Zoom = 0D;
            // 
            // frmMap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(460, 336);
            this.Controls.Add(this.gMapControlLocation);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmMap";
            this.Text = "Map";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMap_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmMap_FormClosed);
            this.Shown += new System.EventHandler(this.frmMap_Shown);
            this.ResizeEnd += new System.EventHandler(this.frmMap_ResizeEnd);
            this.ResumeLayout(false);

        }

        #endregion

        private GMap.NET.WindowsForms.GMapControl gMapControlLocation;
    }
}