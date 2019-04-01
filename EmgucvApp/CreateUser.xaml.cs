using EmgucvApp.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EmgucvApp
{
    /// <summary>
    /// CreateUser.xaml 的交互逻辑
    /// </summary>
    public partial class CreateUser : Window
    {
        int userNum = 0;
        string samplePath = System.IO.Path.Combine(System.Windows.Forms.Application.StartupPath, "sample");
        public CreateUser()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCreateUser_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrEmpty(tbName.Text.Trim()))
            {
                MessageBox.Show("请填写姓名");
                return;
            }
            UserInfo userInfo = new UserInfo()
            {
                Name = tbName.Text.Trim(),
                Index = int.Parse(tbNo.Text.Trim())
            };
            string sampleFilePath = System.IO.Path.Combine(samplePath, tbNo.Text.Trim());
            Directory.CreateDirectory(sampleFilePath);

            MainWindow mainWindow = new MainWindow(userInfo);
            mainWindow.Show();
        }

        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (!Directory.Exists(samplePath))
            {
                Directory.CreateDirectory(samplePath);
            }
            else
            {
                string[] dirs = Directory.GetDirectories(samplePath);
                userNum = dirs.Length;
            }
            tbNo.Text = (userNum + 1).ToString();
        }
    }
}
