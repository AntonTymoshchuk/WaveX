using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;

namespace WaveX.NavigationControls
{
    public class Stripe : IRecolorable
    {
        private Panel GroundPanel;
        private Panel StripePanel;
        private Label CaptionLabel;
        private Panel SelectionPanel;

        private List<Button> ItemButtons = new List<Button>();

        private Color Color;

        private Thread RecolorEngine;
        private Thread SelectionPanelEngine;

        private int TargetItemButtonLeft, TargetItemButtonWidth;

        public Stripe(Form ParentForm, string Caption, string[] Items)
        {
            GroundPanel = new Panel();
            GroundPanel.TabIndex = 0;
            GroundPanel.TabStop = false;
            GroundPanel.Location = new Point(51, 31);
            GroundPanel.Size = new Size(ParentForm.Width - 52, ParentForm.Height - 32);
            GroundPanel.BorderStyle = BorderStyle.None;
            GroundPanel.BackColor = ParentForm.BackColor;

            StripePanel = new Panel();
            StripePanel.TabIndex = 0;
            StripePanel.TabStop = false;
            StripePanel.Location = new Point(0, 0);
            StripePanel.Size = new Size(GroundPanel.Width, 50);
            StripePanel.BorderStyle = BorderStyle.None;
            StripePanel.BackColor = Color.FromArgb(15, 15, 15);

            CaptionLabel = new Label();
            CaptionLabel.TabIndex = 0;
            CaptionLabel.Location = new Point(3, 7);
            CaptionLabel.AutoSize = true;
            CaptionLabel.FlatStyle = FlatStyle.Flat;
            CaptionLabel.BorderStyle = BorderStyle.None;
            CaptionLabel.BackColor = Color.Transparent;
            CaptionLabel.Font = new Font("Calibri", 20);
            CaptionLabel.ForeColor = Color.White;
            CaptionLabel.Text = Caption;
            CaptionLabel.TextAlign = ContentAlignment.MiddleLeft;

            for (int i = 0; i < Items.Length; i++)
            {
                ItemButtons.Add(new Button());
                ItemButtons[i].TabIndex = 0;
                ItemButtons[i].TabStop = false;
                ItemButtons[i].AutoSize = true;
                ItemButtons[i].FlatStyle = FlatStyle.Flat;
                ItemButtons[i].BackColor = StripePanel.BackColor;
                ItemButtons[i].FlatAppearance.MouseOverBackColor = ItemButtons[i].BackColor;
                ItemButtons[i].FlatAppearance.MouseDownBackColor = ItemButtons[i].BackColor;
                ItemButtons[i].FlatAppearance.BorderSize = 0;

                if (i == 0)
                    ItemButtons[i].Font = new Font("Calibri", 14, FontStyle.Bold);
                else
                    ItemButtons[i].Font = new Font("Calibri", 14);

                ItemButtons[i].ForeColor = Color.White;
                ItemButtons[i].Text = Items[i];
                ItemButtons[i].TextAlign = ContentAlignment.MiddleCenter;
                ItemButtons[i].MouseEnter += ItemButton_MouseEnter;
                ItemButtons[i].MouseClick += ItemButton_MouseClick;
                ItemButtons[i].MouseLeave += ItemButton_MouseLeave;
                StripePanel.Controls.Add(ItemButtons[i]);
            }

            int ItemButtonsTotalWidth = 0;
            for (int i = 0; i < ItemButtons.Count; i++)
                ItemButtonsTotalWidth += ItemButtons[i].Width;

            ItemButtons[0].Location = new Point((StripePanel.Width - CaptionLabel.Width) / 2 - ItemButtonsTotalWidth / 2 + CaptionLabel.Width, 9);
            for (int i = 1; i < ItemButtons.Count; i++)
                ItemButtons[i].Location = new Point(ItemButtons[i - 1].Right + 2, 9);

            SelectionPanel = new Panel();
            SelectionPanel.TabIndex = 0;
            SelectionPanel.TabStop = false;
            SelectionPanel.Location = new Point(ItemButtons[0].Left, ItemButtons[0].Bottom);
            SelectionPanel.Size = new Size(ItemButtons[0].Width, 2);
            SelectionPanel.BorderStyle = BorderStyle.None;
            SelectionPanel.BackColor = Color.White;

            StripePanel.Controls.Add(CaptionLabel);
            StripePanel.Controls.Add(SelectionPanel);

            GroundPanel.Controls.Add(StripePanel);

            ParentForm.Controls.Add(GroundPanel);
        }

        public void Show(object sender, EventArgs e)
        {
            GroundPanel.BringToFront();
        }

        private void ItemButton_MouseEnter(object sender, EventArgs e)
        {
            Button ItemButton = sender as Button;
            ItemButton.Font = new Font("Calibri", 14, FontStyle.Bold);
        }

        private void ItemButton_MouseClick(object sender, EventArgs e)
        {
            Button ItemButton = sender as Button;
            TargetItemButtonLeft = ItemButton.Left;
            TargetItemButtonWidth = ItemButton.Width;

            SelectionPanelEngine = new Thread(SelectionPanelReplacing);
            SelectionPanelEngine.Start();
        }

        private void ItemButton_MouseLeave(object sender, EventArgs e)
        {
            Button ItemButton = sender as Button;
            ItemButton.Font = new Font("Calibri", 14);
        }

        private void SelectionPanelReplacing()
        {
            if (SelectionPanel.Left < TargetItemButtonLeft && SelectionPanel.Width < TargetItemButtonWidth)
            {
                for (int i = SelectionPanel.Left, j = SelectionPanel.Width; i < TargetItemButtonLeft || j < TargetItemButtonWidth; i++, j++)
                {
                    SelectionPanel.Invoke((MethodInvoker)(delegate
                    {
                        SelectionPanel.Left++;
                        SelectionPanel.Width++;
                    }));
                }
                return;
            }
            else if (SelectionPanel.Left < TargetItemButtonLeft && SelectionPanel.Width > TargetItemButtonWidth)
            {
                for (int i = SelectionPanel.Left, j = SelectionPanel.Width; i < TargetItemButtonLeft || j > TargetItemButtonWidth; i++, j--)
                {
                    SelectionPanel.Invoke((MethodInvoker)(delegate
                    {
                        SelectionPanel.Left++;
                        SelectionPanel.Width--;
                    }));
                }
                return;
            }
            else if (SelectionPanel.Left > TargetItemButtonLeft && SelectionPanel.Width < TargetItemButtonWidth)
            {
                for (int i = SelectionPanel.Left, j = SelectionPanel.Width; i > TargetItemButtonLeft || j < TargetItemButtonWidth; i--, j++)
                {
                    SelectionPanel.Invoke((MethodInvoker)(delegate
                    {
                        SelectionPanel.Left--;
                        SelectionPanel.Width++;
                    }));
                }
                return;
            }
            else if (SelectionPanel.Left > TargetItemButtonLeft && SelectionPanel.Width > TargetItemButtonWidth)
            {
                for (int i = SelectionPanel.Left, j = SelectionPanel.Width; i > TargetItemButtonLeft || j > TargetItemButtonWidth; i--, j--)
                {
                    SelectionPanel.Invoke((MethodInvoker)(delegate
                    {
                        SelectionPanel.Left--;
                        SelectionPanel.Width--;
                    }));
                }
                return;
            }
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

            bool GroundPanelRecolorAviliable = true;

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
                
                StripePanel.Invoke((MethodInvoker)(delegate { StripePanel.BackColor = Color.FromArgb(Red, Green, Blue); }));

                for (int i = 0; i < ItemButtons.Count; i++)
                {
                    ItemButtons[i].Invoke((MethodInvoker)(delegate
                    {
                        ItemButtons[i].BackColor = Color.FromArgb(Red, Green, Blue);
                        ItemButtons[i].FlatAppearance.MouseOverBackColor = Color.FromArgb(Red, Green, Blue);
                        ItemButtons[i].FlatAppearance.MouseDownBackColor = Color.FromArgb(Red, Green, Blue);
                    }));
                }

                if (Red == Color.R && Green == Color.G && Blue == Color.B)
                    GroundPanelRecolorAviliable = false;
                if (GroundPanelRecolorAviliable == true)
                    GroundPanel.Invoke((MethodInvoker)(delegate { GroundPanel.BackColor = Color.FromArgb(Red, Green, Blue); }));

                if (Red == Color.R + 15 && Green == Color.G + 15 && Blue == Color.B + 15)
                    break;

                Thread.Sleep(1);
            }
        }
    }
}
