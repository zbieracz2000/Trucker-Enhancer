using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using static System.Net.WebRequestMethods;



namespace Trucker_Enchancer
{


    public partial class Form1 : Form
    {
        string path;
        bool no_intro;
        bool better_distance;
        bool custom_music;
        bool headlight_disabled;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrEmpty(fbd.SelectedPath))
                {
                    path = fbd.SelectedPath;
                    if (System.IO.File.Exists(path + "\\trucker.exe") == true)
                    {
                        label1.Text = path;
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("Nie znaleziono gry w tym folderze!", "Trucker Enchancer");
                        path = null;
                    }
                }
            }
        }

        private void run_Click(object sender, EventArgs e)
        {
            if (path == null)
            {
                System.Windows.Forms.MessageBox.Show("Ustaw ścieżkę do folderu z grą!", "Trucker Enchancer");
            }
            else 
            {
                Int32 kasa = (int)money.Value;
                byte[] bytes = BitConverter.GetBytes(kasa);

                if (no_intro == true)
                {
                    using (var stream = new FileStream(path + "\\trucker.exe", FileMode.Open, FileAccess.ReadWrite))
                    {
                        stream.Position = 1303556;
                        stream.WriteByte(0x0C);
                        stream.Position = 1303568;
                        stream.WriteByte(0x00);
                        stream.Position = 1303570;
                        stream.WriteByte(0x00);
                        stream.Position = 1303572;
                        stream.WriteByte(0x00);
                        stream.Position = 1303574;
                        stream.WriteByte(0x00);
                        stream.Position = 1303576;
                        stream.WriteByte(0x00);
                        stream.Position = 1303578;
                        stream.WriteByte(0x00);
                        stream.Position = 1303580;
                        stream.WriteByte(0x00);
                        stream.Position = 1303582;
                        stream.WriteByte(0x09);
                    }
                }
                else
                {
                    using (var stream = new FileStream(path + "\\trucker.exe", FileMode.Open, FileAccess.ReadWrite))
                    {
                        stream.Position = 1303556;
                        stream.WriteByte(0x6C);
                        stream.Position = 1303568;
                        stream.WriteByte(0x6C);
                        stream.Position = 1303570;
                        stream.WriteByte(0x6F);
                        stream.Position = 1303572;
                        stream.WriteByte(0x67);
                        stream.Position = 1303574;
                        stream.WriteByte(0x6F);
                        stream.Position = 1303576;
                        stream.WriteByte(0x2E);
                        stream.Position = 1303578;
                        stream.WriteByte(0x61);
                        stream.Position = 1303580;
                        stream.WriteByte(0x76);
                        stream.Position = 1303582;
                        stream.WriteByte(0x69);
                    }
                }
                if (custom_music == true)
                {
                    using (var stream = new FileStream(path + "\\trucker.exe", FileMode.Open, FileAccess.ReadWrite))
                    {
                        stream.Position = 1334578;
                        stream.WriteByte(0x63);
                        stream.Position = 1334579;
                        stream.WriteByte(0x75);
                        stream.Position = 1334580;
                        stream.WriteByte(0x73);
                        stream.Position = 1334581;
                        stream.WriteByte(0x74);
                        stream.Position = 1334582;
                        stream.WriteByte(0x6F);
                        stream.Position = 1334583;
                        stream.WriteByte(0x6D);
                        stream.Position = 1334584;
                        stream.WriteByte(0x00);
                        stream.Position = 1334585;
                        stream.WriteByte(0x00);
                        stream.Position = 1334586;
                        stream.WriteByte(0x00);
                        stream.Position = 1334587;
                        stream.WriteByte(0x00);
                    }
                }
                else
                {
                    using (var stream = new FileStream(path + "\\trucker.exe", FileMode.Open, FileAccess.ReadWrite))
                    {
                        stream.Position = 1334578;
                        stream.WriteByte(0x68);
                        stream.Position = 1334579;
                        stream.WriteByte(0x79);
                        stream.Position = 1334580;
                        stream.WriteByte(0x70);
                        stream.Position = 1334581;
                        stream.WriteByte(0x65);
                        stream.Position = 1334582;
                        stream.WriteByte(0x72);
                        stream.Position = 1334583;
                        stream.WriteByte(0x61);
                        stream.Position = 1334584;
                        stream.WriteByte(0x76);
                        stream.Position = 1334585;
                        stream.WriteByte(0x65);
                        stream.Position = 1334586;
                        stream.WriteByte(0x72);
                        stream.Position = 1334587;
                        stream.WriteByte(0x73);
                    }
                }
                if (better_distance == true)
                {
                    using (var stream = new FileStream(path + "\\cfg\\L_mgla_full.cfg", FileMode.Open, FileAccess.ReadWrite))
                    {
                        stream.Position = 5;
                        stream.WriteByte(0x33);
                        stream.Position = 19;
                        stream.WriteByte(0x33);
                    }
                    using (var stream = new FileStream(path + "\\cfg\\L_noc_full.cfg", FileMode.Open, FileAccess.ReadWrite))
                    {
                        stream.Position = 5;
                        stream.WriteByte(0x33);
                        stream.Position = 19;
                        stream.WriteByte(0x33);
                    }
                    using (var stream = new FileStream(path + "\\cfg\\L_normalny_full.cfg", FileMode.Open, FileAccess.ReadWrite))
                    {
                        stream.Position = 5;
                        stream.WriteByte(0x33);
                        stream.Position = 19;
                        stream.WriteByte(0x33);
                    }
                    using (var stream = new FileStream(path + "\\cfg\\L1full.cfg", FileMode.Open, FileAccess.ReadWrite))
                    {
                        stream.Position = 5;
                        stream.WriteByte(0x33);
                        stream.Position = 19;
                        stream.WriteByte(0x33);
                    }
                    using (var stream = new FileStream(path + "\\cfg\\ter02_dzien_full.cfg", FileMode.Open, FileAccess.ReadWrite))
                    {
                        stream.Position = 5;
                        stream.WriteByte(0x34);
                        stream.Position = 19;
                        stream.WriteByte(0x34);
                    }
                    using (var stream = new FileStream(path + "\\cfg\\ter02_mgla_full.cfg", FileMode.Open, FileAccess.ReadWrite))
                    {
                        stream.Position = 5;
                        stream.WriteByte(0x34);
                        stream.Position = 19;
                        stream.WriteByte(0x34);
                    }
                    using (var stream = new FileStream(path + "\\cfg\\ter02_noc_full.cfg", FileMode.Open, FileAccess.ReadWrite))
                    {
                        stream.Position = 5;
                        stream.WriteByte(0x33);
                        stream.Position = 19;
                        stream.WriteByte(0x33);
                    }
                }
                else
                {
                    using (var stream = new FileStream(path + "\\cfg\\L_mgla_full.cfg", FileMode.Open, FileAccess.ReadWrite))
                    {
                        stream.Position = 5;
                        stream.WriteByte(0x31);
                        stream.Position = 19;
                        stream.WriteByte(0x31);
                    }
                    using (var stream = new FileStream(path + "\\cfg\\L_noc_full.cfg", FileMode.Open, FileAccess.ReadWrite))
                    {
                        stream.Position = 5;
                        stream.WriteByte(0x31);
                        stream.Position = 19;
                        stream.WriteByte(0x31);
                    }
                    using (var stream = new FileStream(path + "\\cfg\\L_normalny_full.cfg", FileMode.Open, FileAccess.ReadWrite))
                    {
                        stream.Position = 5;
                        stream.WriteByte(0x31);
                        stream.Position = 19;
                        stream.WriteByte(0x31);
                    }
                    using (var stream = new FileStream(path + "\\cfg\\L1full.cfg", FileMode.Open, FileAccess.ReadWrite))
                    {
                        stream.Position = 5;
                        stream.WriteByte(0x31);
                        stream.Position = 19;
                        stream.WriteByte(0x31);
                    }
                    using (var stream = new FileStream(path + "\\cfg\\ter02_dzien_full.cfg", FileMode.Open, FileAccess.ReadWrite))
                    {
                        stream.Position = 5;
                        stream.WriteByte(0x32);
                        stream.Position = 19;
                        stream.WriteByte(0x32);
                    }
                    using (var stream = new FileStream(path + "\\cfg\\ter02_mgla_full.cfg", FileMode.Open, FileAccess.ReadWrite))
                    {
                        stream.Position = 5;
                        stream.WriteByte(0x32);
                        stream.Position = 19;
                        stream.WriteByte(0x32);
                    }
                    using (var stream = new FileStream(path + "\\cfg\\ter02_noc_full.cfg", FileMode.Open, FileAccess.ReadWrite))
                    {
                        stream.Position = 5;
                        stream.WriteByte(0x31);
                        stream.Position = 19;
                        stream.WriteByte(0x31);
                    }
                }
                if (headlight_disabled == true)
                {
                    using (var stream = new FileStream(path + "\\trucker.exe", FileMode.Open, FileAccess.ReadWrite))
                    {
                        stream.Position = 1335098;
                        stream.WriteByte(0x00);
                    }
                }
                else
                {
                    using (var stream = new FileStream(path + "\\trucker.exe", FileMode.Open, FileAccess.ReadWrite))
                    {
                        stream.Position = 1335098;
                        stream.WriteByte(0x78);
                    }
                }
                using (var stream = new FileStream(path + "\\trucker.exe", FileMode.Open, FileAccess.ReadWrite))
                {
                    stream.Position = 508444;
                    stream.WriteByte(bytes[0]);
                    stream.Position = 508445;
                    stream.WriteByte(bytes[1]);
                    stream.Position = 508446;
                    stream.WriteByte(bytes[2]);
                    stream.Position = 508447;
                    stream.WriteByte(bytes[3]);
                }
                System.Windows.Forms.MessageBox.Show("Spatchowano!", "Trucker Enchancer");
            }
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            no_intro = checkBox1.Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
           custom_music = checkBox2.Checked;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            better_distance = checkBox3.Checked;
        }

        private void money_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            headlight_disabled = checkBox4.Checked;
        }
    }
}
