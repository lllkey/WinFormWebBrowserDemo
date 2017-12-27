using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace WebBrowserDemo.Utils
{
    public class LogHelper
    {
        private static readonly LogHelper Instance = new LogHelper();
        public static LogHelper GetLogHelper()
        {
            return Instance;
        }

        #region 公共属性
        public string StrStartupPath
        {
            get { return Environment.CurrentDirectory; }
        }
        public string FileName
        {
            get
            {
                return StrStartupPath + @"\Logs" + @"\SyncLog" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            }
        }
        #endregion

        #region 写日志
        public void CreateLog(string strMsg)
        {
            //1. 判断目录是否存在
            var fileLocation = StrStartupPath + @"\Logs";
            if (!Directory.Exists(fileLocation))
            {
                Directory.CreateDirectory(fileLocation);
            }

            //2. 日志写入
            using (StreamWriter myWriter = new StreamWriter(FileName, true))
            {
                try
                {
                    // string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;//程序执行位置代码块的方法名
                    string funFileName = new System.Diagnostics.StackTrace(true).GetFrame(1).GetMethod().DeclaringType.ToString(); // 调用这个函数的文件名称
                    string methodName = new System.Diagnostics.StackTrace(true).GetFrame(1).GetMethod().Name;//事件源，OnClick，但是不显示当前这个方法名。。
                    int lineNum = new System.Diagnostics.StackTrace(true).GetFrame(1).GetFileLineNumber();
                    string printInfo = "==" + DateTime.Now.ToString("yy/MM/dd HH:mm:ss:fff") + "," + funFileName + "." + methodName + ":" + lineNum + " " + strMsg +
                                       "===";
                    myWriter.WriteLine(printInfo);
                    myWriter.WriteLine("");
                    Console.WriteLine(printInfo);
                }
                finally
                {
                    myWriter.Close();
                }
            }

        }
        #endregion

        #region 错误日志
        public void ErrorLog(Exception ex)
        {
            StringBuilder msg = new StringBuilder();
            msg.Append("*************************************** \n");
            msg.AppendFormat(" 异常发生时间： {0} \n", DateTime.Now);
            //msg.AppendFormat(" 异常类型： {0} \n", ex.HResult);
            msg.AppendFormat(" 导致当前异常的 Exception 实例： {0} \n", ex.InnerException);
            msg.AppendFormat(" 导致异常的应用程序或对象的名称： {0} \n", ex.Source);
            msg.AppendFormat(" 引发异常的方法： {0} \n", ex.TargetSite);
            msg.AppendFormat(" 异常堆栈信息： {0} \n", ex.StackTrace);
            msg.AppendFormat(" 异常消息： {0} \n", ex.Message);
            msg.Append("***************************************");

            CreateLog(msg.ToString());
        }
        #endregion
    }
}
