using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using static System.Net.WebRequestMethods;



namespace Trucker_Enhancer
{


    public partial class Form1 : Form
    {
        bool widescreen_done;
        int numberofprofiles;
        string[] profiles;
        int selectedprofile;
        byte[,] profilevars = new byte[30,30];
        int[] kasaprofilu = new int[30];
        string[,] profile = new string[30,3];
        string path;
        bool no_intro;
        bool better_distance;
        bool custom_music;
        bool headlight_disabled;
        bool unlock_races;
        bool trucker2;
        public Form1()
        {
            InitializeComponent();
        }

        void Widescreen_Patch(bool apply)
        {
            if (apply == true)
            {
                System.IO.File.Create(path + "\\D3D8.dll").Close();
                using (var stream = new FileStream(path + "\\D3D8.dll", FileMode.Open, FileAccess.Write))
                {
                    stream.Write(Properties.Resources.D3D8, 0, Properties.Resources.D3D8.Length);
                }
                System.IO.File.Create(path + "\\hook.cfg").Close();
                using (var stream = new FileStream(path + "\\hook.cfg", FileMode.Open, FileAccess.Write))
                {
                    stream.Write(Properties.Resources.hook, 0, Properties.Resources.hook.Length);
                }
                System.Media.SystemSounds.Asterisk.Play();
                System.Console.WriteLine("Dodano patch");
                System.Windows.Forms.MessageBox.Show("Widescreen fix zastosowany!", "Trucker Enhancer");
            }
            else
            {
                System.IO.File.Delete(path + "\\D3D8.dll");
                System.IO.File.Delete(path + "\\hook.cfg");
                System.Media.SystemSounds.Asterisk.Play();
                System.Console.WriteLine("Usunięty patch");
                System.Windows.Forms.MessageBox.Show("Patch cofnięty!", "Trucker Enhancer");
            }

        }
        void Update_Widescreen()
        {
            resolutionlist.Items.Clear();
            resolutionlist.Items.Insert(0, "1280x720");
            resolutionlist.Items.Insert(1, "1366x768");
            resolutionlist.Items.Insert(2, "1600x900");
            resolutionlist.Items.Insert(3, "1920x1080");


            if (trucker2 == false)
            {
                if (System.IO.File.Exists(path + "\\D3D8.dll") == true) widescreen_done = true;
                else widescreen_done = false;
                widescreenbutton.Enabled = true;
                if (widescreen_done == true)
                {
                    widescreenbutton.Text = "Usuń";
                    widescreenlabel.Text = "Patch już zastosowany";
                    resbutton.Enabled = true;
                    resolutionlist.Enabled = true;
                }
                else
                {
                    widescreenbutton.Text = "Patchuj";
                    widescreenlabel.Text = "Brak patcha szerokoekranowego";
                    resbutton.Enabled = false;
                    resolutionlist.Enabled = false;
                }
            }
            else
            {
                widescreenbutton.Enabled = false;
                widescreenbutton.Text = "Patchuj";
                widescreenlabel.Text = "Niedostępny dla Trucker 2";
                resbutton.Enabled = false;
                resolutionlist.Enabled = false;
            }
        }
        void Load_profiles()
        {
            listaprofili.Items.Clear();
            string[] files = Directory.GetFiles(path + "\\save", "*.prf", SearchOption.TopDirectoryOnly);
            numberofprofiles = files.Length;
            Console.WriteLine("{0} profili.", numberofprofiles);
            profiles = files;
            int index;
            index = 0;
            foreach (string file in files)
            {
                FileInfo f = new FileInfo(file);
                profile[index, 0] = f.Name;
                using (var stream = new FileStream(path + "\\save\\" + f.Name, FileMode.Open, FileAccess.Read))
                {
                    string name = "";
                    byte[] buffer = new byte[28];
                    stream.Position = 5;
                    stream.Read(buffer, 0, 20);
                    name = System.Text.Encoding.UTF8.GetString(buffer, 0, 20);
                    profile[index, 1] = name;
                    if (trucker2 == false) stream.Position = 232;
                    else stream.Position = 640;
                    stream.Read(buffer, 0, 4);
                    kasaprofilu[index] = BitConverter.ToInt32(buffer, 0);
                    if (trucker2 == false) stream.Position = 236;
                    else stream.Position = 644;
                    stream.Read(buffer, 0, 1);
                    profilevars[index, 1] = buffer[0];
                    if (trucker2 == false)
                    {
                        stream.Position = 128;
                        stream.Read(buffer, 0, 1);
                        profilevars[index, 2] = buffer[0];
                    }
                }
                listaprofili.Items.Insert(index, profile[index, 1]);
                index++;
                Console.WriteLine(files);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Process[] pname = Process.GetProcessesByName("trucker");
            if (pname.Length != 0)
            {
                System.Media.SystemSounds.Hand.Play();
                MessageBox.Show("Gra jest obecnie uruchomiona, zamknij ją przed modowaniem!", "Trucker Enhancer");
                System.Console.WriteLine("Gra jest obecnie uruchomiona, zamknij ją przed modowaniem!");
                Application.Exit();
            }
        }

        private void listaprofili_SelectedIndexChanged(object sender, EventArgs e)
        {
            nazwaprofilutextbox.Text = profile[((uint)listaprofili.SelectedIndex), 1];
            profilkasa.Value = kasaprofilu[((uint)listaprofili.SelectedIndex)];
            selectedprofile = ((int)listaprofili.SelectedIndex);
            if (profilevars[selectedprofile, 1] == 0x00) radioButton1.PerformClick();
            if (profilevars[selectedprofile, 1] == 0x01) radioButton2.PerformClick();
            if (profilevars[selectedprofile, 1] == 0x02) radioButton3.PerformClick();
            if (profilevars[selectedprofile, 2] == 0x00 & trucker2 == false) checkBox5.Enabled = true;
            else
            {
                checkBox5.Enabled = false;
                checkBox5.Checked = false;
            }
            if (listaprofili.Items.Count != 0) saveprofile.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrEmpty(fbd.SelectedPath))
                {
                    path = fbd.SelectedPath;
                    if (System.IO.File.Exists(path + "\\trucker.exe") == true) //detekcja truckera 1
                    {
                        Console.WriteLine("Wykryto Trukcera 1");
                        tabPage3.Visible = true;
                        trucker2 = false;
                        money.Enabled = true;
                        checkBox2.Enabled = true;
                        checkBox3.Enabled = true;
                        checkBox4.Enabled = true;
                        checkBox5.Enabled = true;
                        label1.Text = path;
                        Load_profiles();
                        Update_Widescreen();
                    }
                    else if (System.IO.File.Exists(path + "\\trucker2.exe") == true) //detekcja truckera 2, dwójka jest kompletnie niegrywalna, ale jak już mi się nudzi to dodam
                    {
                        Console.WriteLine("Wykryto Trukcera 2");
                        System.Media.SystemSounds.Asterisk.Play();
                        System.Windows.Forms.MessageBox.Show("Wykryty Trucker 2, liczba opcji ograniczona!", "Trucker Enhancer");
                        tabPage3.Visible = false;
                        money.Enabled = false;
                        checkBox2.Enabled = false;
                        checkBox3.Enabled = false;
                        checkBox4.Enabled = false;
                        checkBox5.Enabled = false;
                        trucker2 = true;
                        label1.Text = path;
                        Load_profiles();
                        Update_Widescreen();
                    }
                    else
                    {
                        System.Media.SystemSounds.Hand.Play();
                        System.Windows.Forms.MessageBox.Show("Nie znaleziono gry w tym folderze!", "Trucker Enhancer");
                        System.Console.WriteLine("Zły folder");
                        path = null;
                    }
                }
            }
        }

        private void run_Click(object sender, EventArgs e)
        {
            if (path == null)
            {
                System.Media.SystemSounds.Hand.Play();
                System.Windows.Forms.MessageBox.Show("Ustaw ścieżkę do folderu z grą!", "Trucker Enhancer");
                System.Console.WriteLine("Nie ustawiono ścieżki");
            }
            else 
            {
                Int32 kasa = (int)money.Value;
                byte[] bytes = BitConverter.GetBytes(kasa);

                if (no_intro == true)
                {
                    if (trucker2 == false) using (var stream = new FileStream(path + "\\trucker.exe", FileMode.Open, FileAccess.ReadWrite))
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
                    if (trucker2 == true) using (var stream = new FileStream(path + "\\trucker2.exe", FileMode.Open, FileAccess.ReadWrite))
                        {
                            stream.Position = 920296;
                            stream.WriteByte(0x00);
                        }
                }
                else
                {
                    if (trucker2 == false) using (var stream = new FileStream(path + "\\trucker.exe", FileMode.Open, FileAccess.ReadWrite))
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
                    if (trucker2 == true) using (var stream = new FileStream(path + "\\trucker2.exe", FileMode.Open, FileAccess.ReadWrite))
                        {
                            stream.Position = 920296;
                            stream.WriteByte(0x69);
                        }
                }
                if (trucker2 == false)
                {    
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
                }
                System.Media.SystemSounds.Asterisk.Play();
                System.Windows.Forms.MessageBox.Show("Spatchowano!", "Trucker Enhancer");
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

        private void saveprofile_Click(object sender, EventArgs e)
        {
            if (path != null) using (var stream = new FileStream(path + "\\save\\" + profile[selectedprofile, 0], FileMode.Open, FileAccess.ReadWrite))
            {
                byte[] bytes = Encoding.ASCII.GetBytes(nazwaprofilutextbox.Text);
                stream.Position = 5;
                stream.Write(bytes, 0, bytes.Length);
                //sprawdzanie czy wpisana nazwa jest krótsza niż 20 znaków, jeśli tak, dodajemy puste znaki na końcu
                if (bytes.Length != 20)
                {
                    stream.Position = 5 + bytes.Length;
                    while (stream.Position <= 25)
                    {   
                        stream.WriteByte(0x00);
                        stream.Position++;
                    }
                }
                Int32 kasa = (int)profilkasa.Value;
                bytes = BitConverter.GetBytes(kasa);
                if (trucker2 == false) stream.Position = 232;
                if (trucker2 == true) stream.Position = 640;
                stream.Write(bytes, 0, 4);
                if (trucker2 == false) stream.Position = 236;
                if (trucker2 == true) stream.Position = 644;
                stream.WriteByte(profilevars[selectedprofile, 1]);
                if(unlock_races == true)
                {
                    stream.Position = 40;
                    while (stream.Position <= 128)
                    {
                        stream.WriteByte(0x03);
                        stream.Position++;
                        stream.Position++;
                        stream.Position++;
                    }
                }
            }
            System.Media.SystemSounds.Asterisk.Play();
            System.Console.WriteLine("Zapisano");
            System.Windows.Forms.MessageBox.Show("Zapisano!", "Trucker Enhancer");
            Load_profiles();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            profilevars[selectedprofile, 1] = 0x00;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            profilevars[selectedprofile, 1] = 0x01;
        }
        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            profilevars[selectedprofile, 1] = 0x02;
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            unlock_races = checkBox5.Checked;
        }

        private void widescreenbutton_Click(object sender, EventArgs e)
        {
            if (widescreen_done == false) Widescreen_Patch(true);
            if (widescreen_done == true) Widescreen_Patch(false);
            Update_Widescreen();
        }

        private void resbutton_Click(object sender, EventArgs e)
        {
            using (var stream = new FileStream(path + "\\hook.cfg", FileMode.Open, FileAccess.Write))
            {
                if (resolutionlist.SelectedIndex == 0)
                {
                    stream.Position = 16;
                    stream.WriteByte(0x31);
                    stream.WriteByte(0x32);
                    stream.WriteByte(0x38);
                    stream.WriteByte(0x30);
                    stream.Position = 29;
                    stream.WriteByte(0x37);
                    stream.WriteByte(0x32);
                    stream.WriteByte(0x30);
                    stream.WriteByte(0x00);
                }
                if (resolutionlist.SelectedIndex == 1)
                {
                    stream.Position = 16;
                    stream.WriteByte(0x31);
                    stream.WriteByte(0x33);
                    stream.WriteByte(0x36);
                    stream.WriteByte(0x36);
                    stream.Position = 29;
                    stream.WriteByte(0x37);
                    stream.WriteByte(0x36);
                    stream.WriteByte(0x38);
                    stream.WriteByte(0x00);
                }
                if (resolutionlist.SelectedIndex == 2)
                {
                    stream.Position = 16;
                    stream.WriteByte(0x31);
                    stream.WriteByte(0x36);
                    stream.WriteByte(0x30);
                    stream.WriteByte(0x30);
                    stream.Position = 29;
                    stream.WriteByte(0x39);
                    stream.WriteByte(0x30);
                    stream.WriteByte(0x30);
                    stream.WriteByte(0x00);
                }
                if (resolutionlist.SelectedIndex == 3)
                {
                    stream.Position = 16;
                    stream.WriteByte(0x31);
                    stream.WriteByte(0x39);
                    stream.WriteByte(0x32);
                    stream.WriteByte(0x30);
                    stream.Position = 29;
                    stream.WriteByte(0x31);
                    stream.WriteByte(0x30);
                    stream.WriteByte(0x38);
                    stream.WriteByte(0x30);
                }
            }
            System.Media.SystemSounds.Asterisk.Play();
            System.Console.WriteLine("Zapisano");
            System.Windows.Forms.MessageBox.Show("Zapisano!", "Trucker Enhancer");
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/izawartka/trucker-hook");
        }
    }
}
