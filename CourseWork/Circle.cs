using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace CourseWork
{
    class Circle : Button
    {
        public bool IsRaising { get; set; }

        protected override void OnPaint(PaintEventArgs e)
        {
            GraphicsPath gr = new GraphicsPath();
            gr.AddEllipse(0, 0, ClientSize.Width, ClientSize.Height);
            
            this.Region = new System.Drawing.Region(gr);
            base.OnPaint(e);
        }

        public Circle(bool israising)
        {
            IsRaising = israising;
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 2;
            FlatAppearance.BorderColor = Color.Black;
        }
    }
}
