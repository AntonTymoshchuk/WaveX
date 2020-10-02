using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using ImageProcessor;

namespace WaveX.NavigationControls
{
    public class Curtain : IRecolorable
    {
        private Panel GroundPanel;
        private Button HamburgerButton;
        private PictureBox BackgroundPictureBox;

        private List<PictureBox> ItemPictureBoxes = new List<PictureBox>();
        private List<Button> ItemButtons = new List<Button>();

        private Thread CurtainEngine;
        private Thread RecolorEngine;

        private Color Color;

        private bool CurtainShown = false;

        private int BlurringAreaOnScreenLeft, BlurringAreaInScreenTop;
        private int BlurringAreaOnScreenWidth, BlurringAreaOnScreenHeight;

        private Form ParentForm;

        public Curtain(Form ParentForm, Image[] Images, string[] Captions, Stripe[] Stripes)
        {
            this.ParentForm = ParentForm;

            GroundPanel = new Panel();
            GroundPanel.TabIndex = 0;
            GroundPanel.TabStop = false;
            GroundPanel.Location = new Point(1, 31);
            GroundPanel.Size = new Size(50, 620);
            GroundPanel.BorderStyle = BorderStyle.None;
            GroundPanel.BackColor = Color.FromArgb(15, 15, 15);

            HamburgerButton = new Button();
            HamburgerButton.TabIndex = 0;
            HamburgerButton.TabStop = false;
            HamburgerButton.Location = new Point(0, 0);
            HamburgerButton.Size = new Size(GroundPanel.Width, GroundPanel.Width);
            HamburgerButton.FlatStyle = FlatStyle.Flat;
            HamburgerButton.BackColor = GroundPanel.BackColor;
            HamburgerButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(30, 30, 30);
            HamburgerButton.FlatAppearance.MouseDownBackColor = Color.FromArgb(45, 45, 45);
            HamburgerButton.FlatAppearance.BorderSize = 0;
            HamburgerButton.Image = new Bitmap(@"..\..\Downloads\Image_Hamburger_30_White.png");
            HamburgerButton.ImageAlign = ContentAlignment.MiddleCenter;
            HamburgerButton.Click += HamburgerButton_Click;

            BackgroundPictureBox = new PictureBox();
            BackgroundPictureBox.Location = new Point(50, 0);
            BackgroundPictureBox.Size = new Size(300, GroundPanel.Height);
            BackgroundPictureBox.BorderStyle = BorderStyle.None;
            BackgroundPictureBox.BackColor = GroundPanel.BackColor;
            BackgroundPictureBox.SizeMode = PictureBoxSizeMode.Zoom;

            for (int i = 0; i < Images.Length; i++)
            {
                ItemPictureBoxes.Add(new PictureBox());
                ItemPictureBoxes[i].Location = new Point(0, HamburgerButton.Bottom + 1 + i * 50 + i);
                ItemPictureBoxes[i].Size = new Size(GroundPanel.Width, GroundPanel.Width);
                ItemPictureBoxes[i].BorderStyle = BorderStyle.None;
                ItemPictureBoxes[i].BackColor = Color.Transparent;
                ItemPictureBoxes[i].Image = Images[i];
                ItemPictureBoxes[i].SizeMode = PictureBoxSizeMode.CenterImage;
                GroundPanel.Controls.Add(ItemPictureBoxes[i]);
            }

            for (int i = 0; i < Captions.Length; i++)
            {
                ItemButtons.Add(new Button());
                ItemButtons[i].TabIndex = 0;
                ItemButtons[i].TabStop = false;
                ItemButtons[i].Location = new Point(0, HamburgerButton.Bottom + 1 + i * 50 + i);
                ItemButtons[i].Size = new Size(BackgroundPictureBox.Width, GroundPanel.Width);
                ItemButtons[i].FlatStyle = FlatStyle.Flat;
                ItemButtons[i].BackColor = Color.Transparent;
                ItemButtons[i].FlatAppearance.MouseOverBackColor = Color.FromArgb(100, 15, 15, 15);
                ItemButtons[i].FlatAppearance.MouseDownBackColor = Color.FromArgb(100, 30, 30, 30);
                ItemButtons[i].FlatAppearance.BorderSize = 0;
                ItemButtons[i].Font = new Font("Calibri", 20);
                ItemButtons[i].ForeColor = Color.White;
                ItemButtons[i].Text = Captions[i];
                ItemButtons[i].TextAlign = ContentAlignment.MiddleLeft;
                ItemButtons[i].MouseClick += ItemButton_Click;
                ItemButtons[i].MouseClick += Stripes[i].Show;
                ItemButtons[i].MouseClick += ItemButton_Click2;
                BackgroundPictureBox.Controls.Add(ItemButtons[i]);
            }

            GroundPanel.Controls.Add(HamburgerButton);
            GroundPanel.Controls.Add(BackgroundPictureBox);

            ParentForm.Controls.Add(GroundPanel);
            GroundPanel.BringToFront();
        }

        private void HamburgerButton_Click(object sender, EventArgs e)
        {
            if (CurtainShown == false)
            {
                CurtainEngine = new Thread(Show);
                CurtainEngine.Start();
                CurtainShown = true;
            }
            else if (CurtainShown == true)
            {
                CurtainEngine = new Thread(Hide);
                CurtainEngine.Start();
                CurtainShown = false;
            }
        }

        private void ItemButton_Click(object sender, EventArgs e)
        {
            Hide();
            CurtainShown = false;
        }

        private void ItemButton_Click2(object sender, EventArgs e)
        {
            GroundPanel.BringToFront();
        }

        public void Show()
        {
            BlurUnderCurtain();
            for (int i = 0; i < 300; i += 10)
            {
                GroundPanel.Invoke((MethodInvoker)(delegate { GroundPanel.Width += 10; }));
                Thread.Sleep(1);
            }
        }

        public void Hide()
        {
            for (int i = 300; i > 0; i -= 10)
            {
                GroundPanel.Invoke((MethodInvoker)(delegate { GroundPanel.Width -= 10; }));
                Thread.Sleep(1);
            }
        }

        private void BlurUnderCurtain()
        {
            BlurringAreaOnScreenLeft = 1920 * (ParentForm.Left + GroundPanel.Left + 50) / Screen.PrimaryScreen.Bounds.Width;
            BlurringAreaInScreenTop = 1080 * (ParentForm.Top + GroundPanel.Top) / Screen.PrimaryScreen.Bounds.Height;
            BlurringAreaOnScreenWidth = 1920 * 300 / Screen.PrimaryScreen.Bounds.Width;
            BlurringAreaOnScreenHeight = 1080 * GroundPanel.Height / Screen.PrimaryScreen.Bounds.Height;

            Bitmap UnderCurtainArea = new Bitmap(BlurringAreaOnScreenWidth, BlurringAreaOnScreenHeight);
            Graphics ScreenCopyer = Graphics.FromImage(UnderCurtainArea as Image);
            ScreenCopyer.CopyFromScreen(BlurringAreaOnScreenLeft, BlurringAreaInScreenTop, 0, 0, UnderCurtainArea.Size);

            ImageFactory imageFactory = new ImageFactory();
            imageFactory.Load(UnderCurtainArea);
            imageFactory.Brightness(-50);
            imageFactory.GaussianBlur(12);

            BackgroundPictureBox.Image = imageFactory.Image;
            ScreenCopyer.Dispose();
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
                if (Red < Color.R + 15)
                    Red++;
                else if (Red > Color.R + 15)
                    Red--;

                if (Green < Color.G + 15)
                    Green++;
                else if (Green > Color.G + 15)
                    Green--;

                if (Blue < Color.B + 15)
                    Blue++;
                else if (Blue > Color.B + 15)
                    Blue--;

                GroundPanel.Invoke((MethodInvoker)(delegate { GroundPanel.BackColor = Color.FromArgb(Red, Green, Blue); }));

                HamburgerButton.Invoke((MethodInvoker)(delegate
                {
                    HamburgerButton.BackColor = Color.FromArgb(Red, Green, Blue);
                    HamburgerButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(Red + 15, Green + 15, Blue + 15);
                    HamburgerButton.FlatAppearance.MouseDownBackColor = Color.FromArgb(Red + 30, Green + 30, Blue + 30);
                }));

                for (int i = 0; i < ItemButtons.Count; i++)
                {
                    ItemButtons[i].Invoke((MethodInvoker)(delegate
                    {
                        ItemButtons[i].FlatAppearance.MouseOverBackColor = Color.FromArgb(100, Red + 15, Green + 15, Blue + 15);
                        ItemButtons[i].FlatAppearance.MouseDownBackColor = Color.FromArgb(100, Red + 30, Green + 30, Blue + 30);
                    }));
                }

                if (Red == Color.R + 15 && Green == Color.G + 15 && Blue == Color.B + 15)
                    break;

                Thread.Sleep(1);
            }
        }
    }
}
