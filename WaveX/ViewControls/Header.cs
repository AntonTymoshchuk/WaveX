using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;

namespace WaveX.ViewControls
{
    public class Header : IRecolorable
    {
        private Panel GroundPanel;
        private PictureBox ImagePictureBox;
        private Label CaptionLabel;
        private Button MinimizeButton;
        private Button CloseButton;

        private bool DragStarted = false;
        private int DragStartX, DragStartY;

        private Thread RecolorEngine;

        private Color Color;

        private Form ParentForm;

        public Header(Form ParentForm, Image Image, string Caption)
        {
            this.ParentForm = ParentForm;

            GroundPanel = new Panel();
            GroundPanel.TabIndex = 0;
            GroundPanel.TabStop = false;
            GroundPanel.Location = new Point(1, 1);
            GroundPanel.Size = new Size(ParentForm.Width - 2, 30);
            GroundPanel.BorderStyle = BorderStyle.None;
            GroundPanel.BackColor = Color.FromArgb(15, 15, 15);
            GroundPanel.MouseDown += Control_MouseDown;
            GroundPanel.MouseMove += Control_MouseMove;
            GroundPanel.MouseUp += Control_MouseUp;

            ImagePictureBox = new PictureBox();
            ImagePictureBox.Location = new Point(3, 0);
            ImagePictureBox.Size = new Size(GroundPanel.Height, GroundPanel.Height);
            ImagePictureBox.BorderStyle = BorderStyle.None;
            ImagePictureBox.BackColor = Color.Transparent;
            ImagePictureBox.SizeMode = PictureBoxSizeMode.CenterImage;
            ImagePictureBox.Image = Image;
            ImagePictureBox.MouseDown += Control_MouseDown;
            ImagePictureBox.MouseMove += Control_MouseMove;
            ImagePictureBox.MouseUp += Control_MouseUp;

            CaptionLabel = new Label();
            CaptionLabel.TabIndex = 0;
            CaptionLabel.Location = new Point(ImagePictureBox.Right, 5);
            CaptionLabel.AutoSize = true;
            CaptionLabel.FlatStyle = FlatStyle.Flat;
            CaptionLabel.BorderStyle = BorderStyle.None;
            CaptionLabel.BackColor = Color.Transparent;
            CaptionLabel.Font = new Font("Calibri", 12);
            CaptionLabel.ForeColor = Color.White;
            CaptionLabel.TextAlign = ContentAlignment.MiddleLeft;
            CaptionLabel.Text = Caption;
            CaptionLabel.MouseDown += Control_MouseDown;
            CaptionLabel.MouseMove += Control_MouseMove;
            CaptionLabel.MouseUp += Control_MouseUp;

            MinimizeButton = new Button();
            MinimizeButton.TabIndex = 0;
            MinimizeButton.TabStop = false;
            MinimizeButton.Location = new Point(GroundPanel.Width - GroundPanel.Height * 2, 0);
            MinimizeButton.Size = new Size(GroundPanel.Height, GroundPanel.Height);
            MinimizeButton.FlatStyle = FlatStyle.Flat;
            MinimizeButton.BackColor = GroundPanel.BackColor;
            MinimizeButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(30, 30, 30);
            MinimizeButton.FlatAppearance.MouseDownBackColor = Color.FromArgb(45, 45, 45);
            MinimizeButton.FlatAppearance.BorderSize = 0;
            MinimizeButton.Image = new Bitmap(@"..\..\Downloads\Image_Minimize_20_White.png");
            MinimizeButton.ImageAlign = ContentAlignment.MiddleCenter;
            MinimizeButton.Click += MinimizeButton_Click;

            CloseButton = new Button();
            CloseButton.TabIndex = 0;
            CloseButton.TabStop = false;
            CloseButton.Location = new Point(GroundPanel.Width - GroundPanel.Height, 0);
            CloseButton.Size = new Size(GroundPanel.Height, GroundPanel.Height);
            CloseButton.FlatStyle = FlatStyle.Flat;
            CloseButton.BackColor = GroundPanel.BackColor;
            CloseButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(30, 30, 30);
            CloseButton.FlatAppearance.MouseDownBackColor = Color.FromArgb(45, 45, 45);
            CloseButton.FlatAppearance.BorderSize = 0;
            CloseButton.Image = new Bitmap(@"..\..\Downloads\Image_Close_20_White.png");
            CloseButton.ImageAlign = ContentAlignment.MiddleCenter;
            CloseButton.Click += CloseButton_Click;

            GroundPanel.Controls.Add(ImagePictureBox);
            GroundPanel.Controls.Add(CaptionLabel);
            GroundPanel.Controls.Add(MinimizeButton);
            GroundPanel.Controls.Add(CloseButton);

            ParentForm.Controls.Add(GroundPanel);
        }

        private void Control_MouseDown(object sender, MouseEventArgs e)
        {
            DragStarted = true;
            DragStartX = e.X;
            DragStartY = e.Y;
        }

        private void Control_MouseMove(object sender, MouseEventArgs e)
        {
            if (DragStarted == true)
            {
                ParentForm.Left = ParentForm.Left + e.X - DragStartX;
                ParentForm.Top = ParentForm.Top + e.Y - DragStartY;
            }
        }

        private void Control_MouseUp(object sender, MouseEventArgs e)
        {
            DragStarted = false;
        }

        private void MinimizeButton_Click(object sender, EventArgs e)
        {
            ParentForm.WindowState = FormWindowState.Minimized;
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            ParentForm.Close();
        }

        public void RecolorTo(Color Color)
        {
            this.Color = Color;
            RecolorEngine = new Thread(SmoothRedrawing);
            RecolorEngine.Start();
        }

        public void SmoothRedrawing()
        {
            int Red = GroundPanel.BackColor.R;
            int Green = GroundPanel.BackColor.G;
            int Blue = GroundPanel.BackColor.B;

            while (true)
            {
                if (Red < Color.R)
                    Red++;
                else if (Red > Color.R)
                    Red--;

                if (Green < Color.G)
                    Green++;
                else if (Green > Color.G)
                    Green--;

                if (Blue < Color.B)
                    Blue++;
                else if (Blue > Color.B)
                    Blue--;

                GroundPanel.Invoke((MethodInvoker)(delegate { GroundPanel.BackColor = Color.FromArgb(Red, Green, Blue); }));

                MinimizeButton.Invoke((MethodInvoker)(delegate
                {
                    MinimizeButton.BackColor = Color.FromArgb(Red, Green, Blue);
                    MinimizeButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(Red + 15, Green + 15, Blue + 15);
                    MinimizeButton.FlatAppearance.MouseDownBackColor = Color.FromArgb(Red + 30, Green + 30, Blue + 30);
                }));

                CloseButton.Invoke((MethodInvoker)(delegate
                {
                    CloseButton.BackColor = Color.FromArgb(Red, Green, Blue);
                    CloseButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(Red + 15, Green + 15, Blue + 15);
                    CloseButton.FlatAppearance.MouseDownBackColor = Color.FromArgb(Red + 30, Green + 30, Blue + 30);
                }));

                if (Red == Color.R && Green == Color.G && Blue == Color.B)
                    break;

                Thread.Sleep(1);
            }
        }
    }
}
