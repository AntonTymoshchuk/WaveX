using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace WaveX.ViewControls
{
    public class Border
    {
        private Panel[] BorderItems = new Panel[4];

        public Border(Form ParentForm)
        {
            BorderItems[0] = new Panel();
            BorderItems[0].TabIndex = 0;
            BorderItems[0].TabStop = false;
            BorderItems[0].Location = new Point(0, 0);
            BorderItems[0].Size = new Size(ParentForm.Width, 1);
            BorderItems[0].BorderStyle = BorderStyle.None;
            BorderItems[0].BackColor = Color.White;

            BorderItems[1] = new Panel();
            BorderItems[1].TabIndex = 0;
            BorderItems[1].TabStop = false;
            BorderItems[1].Location = new Point(ParentForm.Width - 1, 0);
            BorderItems[1].Size = new Size(1, ParentForm.Height);
            BorderItems[1].BorderStyle = BorderStyle.None;
            BorderItems[1].BackColor = Color.White;

            BorderItems[2] = new Panel();
            BorderItems[2].TabIndex = 0;
            BorderItems[2].TabStop = false;
            BorderItems[2].Location = new Point(0, ParentForm.Height - 1);
            BorderItems[2].Size = new Size(ParentForm.Width, 1);
            BorderItems[2].BorderStyle = BorderStyle.None;
            BorderItems[2].BackColor = Color.White;

            BorderItems[3] = new Panel();
            BorderItems[3].TabIndex = 0;
            BorderItems[3].TabStop = false;
            BorderItems[3].Location = new Point(0, 0);
            BorderItems[3].Size = new Size(1, ParentForm.Height);
            BorderItems[3].BorderStyle = BorderStyle.None;
            BorderItems[3].BackColor = Color.White;

            for (int i = 0; i < BorderItems.Length; i++)
                ParentForm.Controls.Add(BorderItems[i]);
        }
    }
}
