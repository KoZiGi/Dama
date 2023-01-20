using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dama
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            drawboard();
            Game.GenGame();
        }


        private void drawboard()
        {
            
            this.MinimumSize= new Size(8 * 80 + 15, 8 * 80 + 35);
            this.MaximumSize= new Size(8 * 80 + 15, 8 * 80 + 35);
            
            bool indent = false;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    PictureBox pbx = genpbx(i,j,indent);
                    switch (i)
                    {
                        case int g when (i<3):
                            pbx.Image = Properties.Resources.Fekete;
                            break;

                        case int g when (i >4):
                            pbx.Image = Properties.Resources.Feher;
                            break;

                        default:
                            pbx.Image = Properties.Resources.semmi;
                            break;
                    }
                    pbx.Click += delegate 
                    {
                        Game.GameLogic(pbx);  
                    };
                    pbx.Click += delegate (object sender, EventArgs e) { GameEvents.PlayerTurnValidation(pbx, this.Controls); };
                    Controls.Add(pbx);

                   


                }
                indent = !indent;
            }
        }
        private PictureBox genpbx(int i, int j,bool indent)
        {
            PictureBox pbx = new PictureBox();
            pbx.Width = 80;
            pbx.Height = 80;
            if (indent)
            {
                pbx.Name = $"_{1+2*j}{i}";
                pbx.Left = 80 + j * 160;
                pbx.Top = 80 * i;
            }
            else
            {
                pbx.Name = $"_{2*j}{i}";
                pbx.Left = j * 160;
                pbx.Top = 80 * i;
            }
            pbx.SizeMode = PictureBoxSizeMode.Zoom;
            return pbx;
        }
    }
}
