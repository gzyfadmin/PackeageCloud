using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace YfCloud.App.Module.LogStatus
{
    /// <summary>
    /// 写操作日志
    /// </summary>
    public class LogWrite
    {
        /// <summary>
        /// 写
        /// </summary>
        /// <param name="FileName"></param>
        /// <param name="logText"></param>
        public static void Write(string FileName,string logText)
        {
            try
            {
                string strPath = Path.Combine(AppContext.BaseDirectory, "sysLog");

                if (Directory.Exists(strPath) == false)//如果不存在就创建file文件夹
                {
                    Directory.CreateDirectory(strPath);
                }

                string filePath = Path.Combine(strPath, DateTime.Now.ToString("yyyyMMdd") + FileName);

                using (StreamWriter sw = new StreamWriter(filePath, true, System.Text.Encoding.UTF8))
                {
                    sw.WriteLine(logText);
                }
            }
            catch(Exception ex)
            {

                
            }
        }
    }
}
