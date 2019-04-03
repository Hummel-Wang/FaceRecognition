using OpencvSharpApp.Common;
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

namespace OpencvSharpApp
{
    /// <summary>
    /// CreateUser.xaml 的交互逻辑
    /// </summary>
    public partial class CreateUser : Window
    {
        public string _faceSamplesPath = System.IO.Path.Combine(System.Windows.Forms.Application.StartupPath, "trainedFaces"); 
        public CreateUser()
        {
            InitializeComponent();
        }

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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string userNumStr = ReadWriteConfig.GetConfig("UserNum");
            int userNum = int.Parse(userNumStr);
            tbNo.Text = (userNum + 1).ToString();

            if (!Directory.Exists(_faceSamplesPath))
            {
                Directory.CreateDirectory(_faceSamplesPath);
            }
        }
    }
}
