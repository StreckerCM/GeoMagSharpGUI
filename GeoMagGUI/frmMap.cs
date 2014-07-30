using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using GMap.NET.WindowsForms.ToolTips;

namespace GeoMagGUI
{
    public partial class frmMap : Form
    {
        PointLatLng setPoint;
        
        public frmMap(FrmMain fmain, double latitude, double longitude)
        {
            InitializeComponent();

            Owner = fmain;

            // Initialize map:
            gMapControlLocation.MapProvider = GMap.NET.MapProviders.BingMapProvider.Instance;

            //Bing Zoom Levels
            gMapControlLocation.MinZoom = 1;

            gMapControlLocation.MaxZoom = 22;

            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerAndCache;

            setPoint = new PointLatLng(latitude, longitude);

        }

        public void SetCoordinates(double latitude, double longitude)
        {
            setPoint = new PointLatLng(latitude, longitude);

            RefreshMap();
        }

        public void RefreshMap()
        {
            gMapControlLocation.Position = setPoint;

            GMapOverlay markersOverlay = new GMapOverlay(gMapControlLocation, "markers");

            GMapMarkerCross marker = new GMapMarkerCross(setPoint);

            markersOverlay.Markers.Add(marker);

            gMapControlLocation.Overlays.Add(markersOverlay);

            gMapControlLocation.Zoom = 15;

            gMapControlLocation.ReloadMap();
        }

        private void frmMap_Shown(object sender, EventArgs e)
        {
            RefreshMap();
        }

        private void frmMap_FormClosed(object sender, FormClosedEventArgs e)
        {
            var fmain = (FrmMain)Owner;

            fmain.PubMap = null;
        }

        private void frmMap_ResizeEnd(object sender, EventArgs e)
        {
            gMapControlLocation.ReloadMap();
        }

        private void frmMap_FormClosing(object sender, FormClosingEventArgs e)
        {
            gMapControlLocation.Dispose();
        }
    }
}
