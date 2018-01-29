using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        Timer t = new Timer();
        Timer t2 = new Timer();
        int second, minute, hour, reminder = -1, interval = -1, reflag = 0;
        int secondt2, minutet2, hourt2;
        public Form1()
        {
            InitializeComponent();
            second = minute = hour = 0;
            secondt2= minutet2= hourt2 = 0;
            ReminderCheck();
            button2.Enabled = false;
            button3.Enabled = false;
            SystemEvents.PowerModeChanged += OnPowerChange;
        }
        
        // Kiểm tra biến reminder & interval để nhắc nhở nghỉ ngơi
        void ReminderCheck()
        {
            if (checkBox1.Checked)
            {
                //reminder
                if (comboBox1.SelectedIndex == comboBox1.Items.Count - 1) reminder = 60 * 4;
                if (comboBox1.SelectedIndex == comboBox1.Items.Count - 2) reminder = 60 * 2;
                if (comboBox1.SelectedIndex == comboBox1.Items.Count - 3) reminder = 60;
                if (comboBox1.SelectedIndex == comboBox1.Items.Count - 4) reminder = 45;
                if (comboBox1.SelectedIndex == comboBox1.Items.Count - 5) reminder = 30;
                if (comboBox1.SelectedIndex == comboBox1.Items.Count - 6) reminder = 15;

                //interval
                if (comboBox3.SelectedIndex == comboBox3.Items.Count - 1) interval = 30;
                if (comboBox3.SelectedIndex == comboBox3.Items.Count - 2) interval = 15;
                if (comboBox3.SelectedIndex == comboBox3.Items.Count - 3) interval = 10;
                if (comboBox3.SelectedIndex == comboBox3.Items.Count - 4) interval = 5;
            }
            else
            {
                reminder = -1;
                interval = -1;
            }
        }

        //Nút Start
        private void button1_Click(object sender, EventArgs e)
        {
            if(checkBox3.Checked)
            {
                File.AppendAllText(@"C:\Windows\System32\drivers\etc\hosts", "\r\n0.0.0.0 www.facebook.com" + Environment.NewLine);
            }
            checkBox3.Enabled = false;
            if (comboBox2.SelectedIndex == comboBox2.Items.Count - 1) hourt2 = 2;
            if (comboBox2.SelectedIndex == comboBox2.Items.Count - 2) hourt2 = 1;
            if (comboBox2.SelectedIndex == comboBox2.Items.Count - 3) minutet2 = 45;
            if (comboBox2.SelectedIndex == comboBox2.Items.Count - 4) minutet2 = 30;
            t2.Start();
            button1.Enabled = false;
            button2.Enabled = true;
            button3.Enabled = true;
        }
        //Xóa line cuối
        public static void DeleteLastLine(string filepath)
        {
            List<string> lines = File.ReadAllLines(filepath).ToList();

            File.WriteAllLines(filepath, lines.GetRange(0, lines.Count - 1).ToArray());

        }
        //Nút Stop
        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Your Deep Work Session hasn't over yet. Please keep working!\nBut if you really have an emergency, you can click Yes to Stop now", "Message", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                t2.Stop();
                secondt2 = minutet2 = hourt2 = 0;
                lblHourt2.Text = appZero(hourt2);
                lblMinutet2.Text = appZero(minutet2);
                lblSecondt2.Text = appZero(secondt2);
                button3.Enabled = false;
                button2.Enabled = false;
                button1.Enabled = true;
                if (checkBox3.Checked)
                {
                    DeleteLastLine(@"C:\Windows\System32\drivers\etc\hosts");
                }
                checkBox3.Enabled = true;
            }
            else if (dialogResult == DialogResult.No)
            {
            }
        }

        //Nút Pause
        private void button3_Click(object sender, EventArgs e)
        {
            if (button3.Text == "Pause")
            {
                t2.Stop();
                button3.Text = "Continue";
            }
            else
            {
                t2.Start();
                button3.Text = "Pause";
            }
        }

        //About
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Copyright by Phạm Hoàng Nhật\nDev Care Version 1.0\nNếu bạn thích app này thì hãy tặng Star cho mình tại: https://github.com/providence97/DevCarev1.0 \n(Bấm Ctrl + C để copy)" );
        }

        //Exit
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        //Load Form chính
        private void Form1_Load(object sender, EventArgs e)
        {
            t.Interval = 1000;
            t2.Interval = 1000;
            t2.Tick += new EventHandler(this.t2_Tick);
            t.Tick += new EventHandler(this.t_Tick);
            t.Start();
            comboBox1.SelectedIndex = comboBox1.Items.Count - 4;
            comboBox2.SelectedIndex = comboBox2.Items.Count - 3;
            comboBox3.SelectedIndex = comboBox2.Items.Count - 3;

        }

        //show Window khi right click
        private void showWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        //Minimize to Tray
        private void Form1_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == this.WindowState)
            {
                this.Hide();
            }
        }

        //Baloon
        private void notifyIcon1_MouseMove(object sender, MouseEventArgs e)
        {
            if (button1.Enabled)
            {
                notifyIcon1.Text = "Online Time: " + lblHour.Text + label9.Text + lblMinute.Text + label2.Text + lblSecond.Text;
            }
            else
            {
                notifyIcon1.Text = "Online Time:         " + lblHour.Text + label9.Text + lblMinute.Text + label2.Text + lblSecond.Text +
                    "\nDeep Work Time: " + lblHourt2.Text + label3.Text + lblMinutet2.Text + label8.Text + lblSecondt2.Text;
            }
        }

        //Thay đổi Reminder & Interval
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                if (comboBox1.SelectedIndex == comboBox1.Items.Count - 1) reminder = 60 * 4;
                if (comboBox1.SelectedIndex == comboBox1.Items.Count - 2) reminder = 60 * 2;
                if (comboBox1.SelectedIndex == comboBox1.Items.Count - 3) reminder = 60;
                if (comboBox1.SelectedIndex == comboBox1.Items.Count - 4) reminder = 45;
                if (comboBox1.SelectedIndex == comboBox1.Items.Count - 5) reminder = 30;
                if (comboBox1.SelectedIndex == comboBox1.Items.Count - 6) reminder = 15;
            }
        }
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                if (comboBox3.SelectedIndex == comboBox3.Items.Count - 1) interval = 30;
                if (comboBox3.SelectedIndex == comboBox3.Items.Count - 2) interval = 15;
                if (comboBox3.SelectedIndex == comboBox3.Items.Count - 3) interval = 10;
                if (comboBox3.SelectedIndex == comboBox3.Items.Count - 4) interval = 5;
            }
        }

        //Remind to take a break
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            ReminderCheck();
        }

        //Show window khi left click
        private void notifyIcon1_LeftMouseClick(object sender, MouseEventArgs e)
        {
            if (this.WindowState== FormWindowState.Minimized)
            {
                this.Show();
                this.WindowState = FormWindowState.Normal;
            }
        }

        // Thêm số 0 vào các chuỗi
        private string appZero(double str)
        {
            if (str <= 9) return "0" + str;
            else return str.ToString();
        }

        //Online timer
        private void t_Tick(object sender, EventArgs e)
        {
            second++;
            if (second>59)
            {
                if (reminder != -1) reminder--;
                if (reflag == 1) interval--;
                minute++;
                second = 0;
            }
            if (minute>59)
            {
                hour++;
                minute = 0 ;
            }
            if (checkBox1.Checked && reminder == 0 && reflag == 0)
            {
                reminder = -1;
                MessageBox.Show("Hey there! You've been working hard <3 Take a break now.", "DevCare_v1.0", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000);
                ReminderCheck();
                reflag = 1;
            }
            if (checkBox1.Checked && interval == 0 && reflag == 1)
            {
                interval = -1;
                MessageBox.Show("Break time is up! Get back to work now!", "DevCare_v1.0", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000);
                ReminderCheck();
                reflag = 0;
            }
            if (checkBox2.Checked && hour==8 && minute==0 &&second==0)
            {
                label6.Visible = true;
                MessageBox.Show("Jesus Christ, you've been using this computer for 8 hours!!! Want to be infertility? Turn it off NOW. Your health is more important.", "DevCare_v1.0", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000);
                
            }
            lblHour.Text = appZero(hour);
            lblMinute.Text = appZero(minute);
            lblSecond.Text = appZero(second);
        }
        
        //DeepWork Timer
        private void t2_Tick(object sender, EventArgs e)
        {
            
            if (secondt2 == 0)
            {
                if (minutet2 != 0)
                {
                    minutet2--;
                    secondt2 = 60;
                }
                else 
                {
                    if (hourt2 == 0)
                    {
                        t2.Stop();
                        secondt2 = minutet2 = hourt2 = 0;
                        lblHourt2.Text = appZero(hourt2);
                        lblMinutet2.Text = appZero(minutet2);
                        lblSecondt2.Text = appZero(secondt2);
                        button3.Enabled = false;
                        button2.Enabled = false;
                        button1.Enabled = true;
                        if (checkBox3.Checked)
                        {
                            DeleteLastLine(@"C:\Windows\System32\drivers\etc\hosts");
                        }
                        checkBox3.Enabled = true;
                        MessageBox.Show("This is the end of Deep Work Session! Chill and have a cup of tea <3.", "DevCare_v1.0", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000);
                    }
                    else
                    {
                        hourt2--;
                        minutet2 = 59;
                        secondt2 = 60;
                    }
                }
            }
            if (hourt2==0 &&minutet2==0&secondt2==0)
            {

            }
            else
            {
                secondt2--;
            }
            lblHourt2.Text = appZero(hourt2);
            lblMinutet2.Text = appZero(minutet2);
            lblSecondt2.Text = appZero(secondt2);
        }
        
        //Xử lý khi Sleep & Resume Laptop
        private void OnPowerChange(object s, PowerModeChangedEventArgs e)
        {
            switch (e.Mode)
            {
                case PowerModes.Resume:  t.Start();
                    break;
                case PowerModes.Suspend: t.Stop();
                    break;
            }
        }

        

    }
}
