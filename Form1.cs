using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 简易桌面整理
{
    public partial class Form1 : Form
    {
        public string DesktopPath { get; set; }
        public DirectoryInfo DesktopDir { get; set; }
        public Dictionary<string,string> Rules { get; set; }
        public Form1()
        {

            InitializeComponent();
            this.DesktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            this.DesktopDir = new DirectoryInfo(this.DesktopPath);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.timer_main.Interval = Config.Get<Int32>("interval");
            this.Rules = Config.Get<Dictionary<string, string>>("rules");

            if (Config.Get<Boolean>("enable_on_start"))
            {
                this.timer_main.Start();
                this.btControl.Text = "停止";
                this.lbState.Text = "运行";
                this.lbState.ForeColor = Color.Green;
            }
            if (!Config.Get<Boolean>("show_on_start"))
            {
                this.Hide();
                this.Visible = false;
                this.notifyIcon1.BalloonTipText = "我在这里!";
                this.notifyIcon1.ShowBalloonTip(5);
            }
        }
        /// <summary>
        /// 计时器每次运行时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_main_Tick(object sender, EventArgs e)
        {
            this.do_clean();
        }
        private void do_clean()
        {
            foreach(FileInfo file in this.DesktopDir.GetFiles())
            {
                var cat = this.getFileCat(file.Name);
                if (!string.IsNullOrEmpty(cat))
                {
                    this.moveFile(file, cat);
                }
            }

            foreach(DirectoryInfo dirInfo in this.DesktopDir.GetDirectories())
            {
                var cat = this.getFileCat("#dir#"+ dirInfo.Name);
                if (!string.IsNullOrEmpty(cat))
                {
                    this.moveDir(dirInfo, cat);
                }
            }
        }
        private string getFileCat(string filename)
        {
            foreach(var key in this.Rules.Keys)
            {
                if(Regex.IsMatch(filename, this.Rules[key]))
                {
                    return key;
                }
            }
            return null;
        }
        private bool moveFile(FileInfo file,string dir)
        {
            string desDir = Path.Combine(this.DesktopPath, dir);
            if (!Directory.Exists(desDir))
            {
                Directory.CreateDirectory(desDir);
            }
            try
            {
                string desFile = Path.Combine(desDir, file.Name);
                file.MoveTo(desFile);
            }
            catch
            {
                return false;
            }
            return true;
        }
        
        private bool moveDir(DirectoryInfo dirInfo,string dir)
        {
            if (this.Rules.ContainsKey(dirInfo.Name))
            {
                return true;
            }
            string desDir = Path.Combine(this.DesktopPath, dir);
            if (!Directory.Exists(desDir))
            {
                Directory.CreateDirectory(desDir);
            }
            try
            {
                string desFile = Path.Combine(desDir, dirInfo.Name);
                dirInfo.MoveTo(desFile);
            }
            catch(Exception e)
            {
                return false;
            }
            return true;
        }
        private void btControl_Click(object sender, EventArgs e)
        {
            if((sender as Button).Text == "启动")
            {
                this.timer_main.Start();
                (sender as Button).Text = "停止";
                this.lbState.Text = "运行";
                this.lbState.ForeColor = Color.Green;
            }
            else
            {
                this.timer_main.Stop();
                (sender as Button).Text = "启动";
                this.lbState.Text = "停止";
                this.lbState.ForeColor = Color.Red;
            }
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.notifyIcon1.Visible = false;
            System.Environment.Exit(0);
        }

        private void 关于ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("2020 LenShang https://space.bilibili.com/573429");
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            this.Visible = false;
            this.notifyIcon1.BalloonTipText = "我在这里!";
            this.notifyIcon1.ShowBalloonTip(5);
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            this.Show();
            this.Visible = true;
        }
    }
}
