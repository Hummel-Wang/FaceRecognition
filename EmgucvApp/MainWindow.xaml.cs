using Emgu.CV;
using Emgu.CV.Structure;
using EmgucvApp.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static EmgucvApp.Common.FaceTools;

namespace EmgucvApp
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private UserInfo _userInfo;
        private FaceTools _faceTools = new FaceTools();
        Capture capture;
        FaceDetectedObj currentfdo;//点击鼠标时的人脸检测对象
        System.Timers.Timer aTimer = new System.Timers.Timer();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userInfo"></param>
        public MainWindow(UserInfo userInfo)
        {
            InitializeComponent();
            _userInfo = userInfo;
        }

        /// <summary>
        /// 启动摄像头
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartCamera_Click(object sender, RoutedEventArgs e)
        {
            CvInvoke.UseOpenCL = false;   //不使用OpneCL
            InitializeComponent();
            capture = new Capture();      //初始化摄像头
            capture.Start();     //开启摄像头
            capture.ImageGrabbed += Capture_ImageGrabbed;   //获取帧   

            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            aTimer.Interval = 1000;
            aTimer.Start();
        }

        /// <summary>
        /// 采样
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CollectionSample_Click(object sender, RoutedEventArgs e)
        {
            System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(140, 50, 400, 430);
            Mat showMat = new Mat(capture.QueryFrame(), rectangle);
            currentfdo = _faceTools.GetFaceRectangle(showMat);
            getCurrentFaceSample(0);
            collectionSample.Visibility = Visibility.Collapsed;
            saveSample.Visibility = Visibility.Visible;

        }

        /// <summary>
        /// 保存采样
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveSample_Click(object sender, RoutedEventArgs e)
        {
            string filePath = _faceTools.FaceSamplesPath + "\\" + _userInfo.Index.ToString() + "_" + System.Guid.NewGuid().ToString() + ".jpg";
            sampleImage.Image.Save(filePath);
            MessageBox.Show("样本保存完毕。");

            collectionSample.Visibility = Visibility.Visible;
            saveSample.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// 释放相机
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (capture != null)
            {
                capture.ImageGrabbed -= Capture_ImageGrabbed;
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent);
                capture.Dispose();
            }
        }

        /// <summary>
        /// 实时视频刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Capture_ImageGrabbed(object sender, EventArgs e)
        {
            Mat frame = new Mat();
            capture.Retrieve(frame, 0);    //接收数据
            System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(140, 50, 400, 430);
            Mat showMat = new Mat(frame, rectangle);
            originImage.Image = showMat;      //显示图像
        }

        /// <summary>
        /// 实时识别
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            try
            {
                Console.WriteLine(capture.Grab());
                System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(140, 50, 400, 430);
                Mat showMat = new Mat(capture.QueryFrame(), rectangle);
                FaceDetectedObj faceDetectedObj = _faceTools.FaceRecognize(showMat);
                originImage.Image = _faceTools.FaceRecognize(showMat).originalImg;
                if (faceDetectedObj.Name != null)
                {
                    this.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        infoShow.Text = "姓名为：" + faceDetectedObj.Name;
                    }));
                }
                if (capture != null && capture.Grab())
                {

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("***" + ex.Message);
            }
        }

        private void getCurrentFaceSample(int i)
        {
            try
            {
                Image<Gray, byte> result = currentfdo.originalImg.ToImage<Gray, byte>().Copy(currentfdo.facesRectangle[i]).Resize(100, 100, Emgu.CV.CvEnum.Inter.Cubic);
                result._EqualizeHist();//灰度直方图均衡化
                sampleImage.Image = result;
            }
            catch (Exception ex)
            {
                MessageBox.Show("没有检测到人脸，发生错误：" + ex.Message);
            }
        }
    }
}
