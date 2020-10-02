using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace WaveX
{
    public partial class MainForm : Form
    {
        private ViewControls.Border Border;
        private ViewControls.Header Header;

        private NavigationControls.Curtain Curtain;
        private NavigationControls.Stripe[] Stripes;

        private PictureBox img;

        public MainForm()
        {
            InitializeComponent();
            this.Name = "MainForm";
            this.Text = "WaveX";
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(852, 652);
            this.BackColor = Color.Black;

            img = new PictureBox();
            img.Location = new Point(51, 100);
            img.Size = new Size(700, 500);
            img.BorderStyle = BorderStyle.None;
            img.BackColor = Color.Transparent;
            img.Image = new Bitmap(@"C:\Users\Lenovo\Pictures\Saved Pictures\WallpaperStudio10-79785.jpg");
            img.SizeMode = PictureBoxSizeMode.Zoom;
            img.SendToBack();
            this.Controls.Add(img);

            InitializeViewControls();
            InitializeNavigationControls();
        }

        private void InitializeViewControls()
        {
            Border = new ViewControls.Border(this);
            Header = new ViewControls.Header(this, new Bitmap(@"..\..\Downloads\Image_WaveX_26_White.png"), "WaveX");
        }

        private void InitializeNavigationControls()
        {
            Stripes = new NavigationControls.Stripe[3];
            Stripes[0] = new NavigationControls.Stripe(this, "Previus songs", new string[] { "Music", "Songs", "Genres", "Artists" });
            Stripes[1] = new NavigationControls.Stripe(this, "Play list", new string[] { "Create", "All lists", "Main"});
            Stripes[2] = new NavigationControls.Stripe(this, "Music folders", new string[] { "All folders", "Search", "System" });

            Bitmap[] Images = new Bitmap[3];
            Images[0] = new Bitmap(@"..\..\Downloads\Image_Clock_30_White.png");
            Images[1] = new Bitmap(@"..\..\Downloads\Image_MusicList_30_White.png");
            Images[2] = new Bitmap(@"..\..\Downloads\Image_Folders_30_White.png");

            string[] Captions = new string[3];
            Captions[0] = "Previus songs";
            Captions[1] = "Track list";
            Captions[2] = "Music folders";

            Curtain = new NavigationControls.Curtain(this, Images, Captions, Stripes);
        }
    }
}
