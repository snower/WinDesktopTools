﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SystemWatch
{
    public class NetworkInterfaceWidget : Widget
    {
        private double receivedTotalBytes;
        private double sentTotalBytes;

        private Canvas canvasView;
        private string sentReceivedText;
        private string receivedText;
        private string sentText;
        private string totalReceivedText;
        private string totalSentText;

        private Font netFont;
        private Font totalNetFont;

        private Brush sentReceivedBrush;
        private Brush sentBrush;
        private Brush receivedBrush;

        private Point sentReceivedLocation;
        private Point receivedLocation;
        private Point sentLoction;
        private Point totalReceivedLocation;
        private Point totalSentLocation;

        public NetworkInterfaceWidget(Point location, Size clientSize) : base(location, clientSize)
        {
            this.netFont = new Font("微软雅黑", 7F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));
            this.totalNetFont = new Font("微软雅黑", 6.5F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));

            this.sentReceivedBrush = new SolidBrush(this.NormalColor[0]);
            this.sentBrush = new SolidBrush(this.NormalColor[1]);
            this.receivedBrush = new SolidBrush(this.NormalColor[2]);

            this.sentReceivedLocation = new Point(80, 30);
            this.receivedLocation = new Point(75, 47);
            this.sentLoction = new Point(10, 47);
            this.totalReceivedLocation = new Point(75, 65);
            this.totalSentLocation = new Point(10, 65);

            this.canvasView = new Canvas(new Point(10, 85), new Size(126, 40), 3, 123, new Color[] { this.NormalColor[0], this.NormalColor[1], this.NormalColor[1]});
            this.canvasView.RefreshLatestDataEvent += this.UpdateLatestDatas;

            Program.GetInformation().SetDataToView(Performance.DataType.NetworkInterfaceLoadPercent, this.canvasView, "", new object[] { 1 });
            Program.GetInformation().SetDataToView(Performance.DataType.NetworkInterfaceReceivedLoadPercent, this.canvasView, "", new object[] { 2 });
            Program.GetInformation().SetDataToView(Performance.DataType.NetworkInterfaceSentLoadPercent, this.canvasView, "", new object[] { 3 });
        }

        protected override void BackgroundPaint(Graphics g)
        {
            base.BackgroundPaint(g);
            this.PaintTitle(g, "Network");
            this.canvasView.BackgroundPaint(g);
        }

        protected override void Paint(Graphics g)
        {
            base.Paint(g);

            g.DrawString(this.sentReceivedText, this.netFont, this.sentReceivedBrush, this.sentReceivedLocation);
            g.DrawString(this.receivedText, this.netFont, this.receivedBrush, this.receivedLocation);
            g.DrawString(this.sentText, this.netFont, this.sentBrush, this.sentLoction);
            g.DrawString(this.totalReceivedText, this.totalNetFont, this.receivedBrush, this.totalReceivedLocation);
            g.DrawString(this.totalSentText, this.totalNetFont, this.sentBrush, this.totalSentLocation);
            this.canvasView.Paint(g);
        }

        public override void Close()
        {
            base.Close();
            this.canvasView.Close();
        }

        private void UpdateLatestDatas(object sender, Canvas.CanvasRefreshLatestDataEventArgs e)
        {
            Canvas.Data[] data = e.LatestDatas;
            if (data[0] != null)
            {
                this.sentReceivedText = "T: " + this.FormatByteSize(5, data[0].current) + "/s";
            }

            if (data[1] != null)
            {
                this.receivedText = "R: " + this.FormatByteSize(5, data[1].current) + "/s";
                this.receivedTotalBytes += data[1].current;
                this.totalReceivedText = "RT: " + this.FormatByteSize(5, this.receivedTotalBytes) + "B";
            }

            if (data[2] != null)
            {
                this.sentText = "S: " + this.FormatByteSize(5, data[2].current) + "/s";
                this.sentTotalBytes += data[2].current;
                this.totalSentText = "ST: " + this.FormatByteSize(5, this.sentTotalBytes) + "B";
            }
        }
    }
}