using EmgucvApp.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
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
            if (string.IsNullOrEmpty(tbName.Text.Trim()))
            {
                MessageBox.Show("请填写姓名");
                return;
            }
            UserInfo userInfo = new UserInfo()
            {
                Name = tbName.Text.Trim(),
                Index = int.Parse(tbNo.Text.Trim())
            };
            ReadWriteConfig.WriteConfig("UserNum", tbNo.Text.Trim());
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
            string userNumStr = ReadWriteConfig.GetConfig("UserNum");
            int userNum = int.Parse(userNumStr);
            tbNo.Text = (userNum + 1).ToString();
        }
    }
}
