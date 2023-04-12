using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FengXuTLTool
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            //注册委托
            //AppDomain.CurrentDomain.AssemblyResolve += CurrentDomainAssemblyResolve;


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmMain());
        }

        /// 
        /// 加载嵌入工程中的dll
        /// 
        private static Assembly CurrentDomainAssemblyResolve(object sender, ResolveEventArgs e)
        {
            //项目的命名空间为winform1, 嵌入dll资源在libs文件夹下，所以这里用的命名空间为： winform1.libs.
            string dllName = "FengXuTLTool.libs." + new AssemblyName(e.Name).Name + ".dll";
            using (var dllStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(dllName))
            {
                byte[] dllData = new byte[dllStream.Length];
                dllStream.Read(dllData, 0, dllData.Length);
                return Assembly.Load(dllData);
            }
        }
    }
}
