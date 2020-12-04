using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 简易桌面整理
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            int iProcessNum = 0;
            foreach (Process singleProc in Process.GetProcesses())
            {
                if (singleProc.ProcessName == Process.GetCurrentProcess().ProcessName)
                {
                    iProcessNum += 1;
                }
            }
            if (iProcessNum > 1)
            {
                System.Environment.Exit(0);
                return;
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
