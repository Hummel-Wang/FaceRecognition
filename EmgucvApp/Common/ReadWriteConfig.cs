using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmgucvApp.Common
{
  public static  class ReadWriteConfig
    {
        /// <summary>
        /// 读取config文件内容
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetConfig(string name)
        {
            string exePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "EmgucvApp.exe");
            Configuration cfa = ConfigurationManager.OpenExeConfiguration(exePath);
            KeyValueConfigurationElement config = cfa.AppSettings.Settings[name];
            if (config != null)
            {
                return config.Value;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 设置config文件内容
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public static void WriteConfig(string name, string value)
        {
            string exePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "EmgucvApp.exe");
            Configuration cfa = ConfigurationManager.OpenExeConfiguration(exePath);
            cfa.AppSettings.Settings[name].Value = value;
            cfa.Save();
        }
    }
}
