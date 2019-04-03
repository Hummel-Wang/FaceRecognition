using OpenCvSharp;
using OpencvSharpApp.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OpencvSharpApp
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        private UserInfo _userInfo;
        private FaceTools _faceTools = new FaceTools();
        VideoCapture capture;
        FaceDetectedObj currentfdo;//点击鼠标时的人脸检测对象
        System.Timers.Timer aTimer = new System.Timers.Timer();
        Mat currentFrame = null;
        private string faceSamplesPath = System.Windows.Forms.Application.StartupPath + "\\trainedFaces";

        /// <summary>
        /// 人脸检测是否正在进行
        /// </summary>
        private bool IsRunning = false;

        /// <summary>
        /// 视频捕获设备
        /// </summary>
        private VideoCapture _capture = null;

        /// <summary>
        /// 是否停止人脸检测
        /// </summary>
        private volatile bool ShouldStop = true;

        /// <summary>
        /// 是否停止等待下一帧
        /// </summary>
        private ManualResetEvent WakeupResetEvent = new ManualResetEvent(true);

        public MainWindow(UserInfo userInfo)
        {
            InitializeComponent();
            _userInfo = userInfo;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            CloseCapture();
        }

        private void StartCamera_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IsRunning)
                {   // 关闭人脸检测
                    CloseCapture();
                    IsRunning = false;
                }
                else
                {   // 开启人脸检测
                    _capture = new VideoCapture(0);

                    ShouldStop = false;
                    WakeupResetEvent.Reset();
                    if (ThreadPool.QueueUserWorkItem(new WaitCallback((state) =>
                    {
                        try
                        {
                            //此前Demo中可以正确获取到Fps，后来再重新建项目时一直获取到的值为0。 Why？？？
                            //int millisecondsTimeout = (int)Math.Round(1000.0 / Capture.Fps); // 计算间隔
                            int millisecondsTimeout = (int)Math.Round(1000.0 / 30); // 计算间隔
                            Console.WriteLine(_capture.Fps);
                            while (true)
                            {
                                if (ShouldStop) break; // 是否停止人脸检测

                                // 获取图像帧
                                Mat Bgr = new Mat();
                                if (_capture.Read(Bgr))
                                {
                                    if (!Bgr.Empty())
                                    {
                                        OpenCvSharp.Rect rectangleRoi = new OpenCvSharp.Rect(140, 50, 400, 430);
                                        Bgr = new Mat(Bgr, rectangleRoi);
                                        currentFrame = Bgr;
                                        //Stopwatch stopwatch = new Stopwatch();
                                        //stopwatch.Start();

                                        FaceDetectedObj faceObject = _faceTools.FaceRecognize(Bgr);

                                        //stopwatch.Stop();
                                        //Console.WriteLine("耗时：{0}s", stopwatch.ElapsedMilliseconds / 1000);

                                        originImage.Image = _faceTools.byteToImage(Bgr.ToBytes());
                                    }
                                }

                                // 是否停止等待下一帧
                                if (WakeupResetEvent.WaitOne(millisecondsTimeout)) break;
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("读取实时视频时发生异常，信息：" + ex.Message);
                        }
                    })))
                    {
                        IsRunning = true;
                    }
                    else
                    {   // 启动人脸检测线程失败
                        ShouldStop = true;
                        WakeupResetEvent.Set();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CollectionSample_Click(object sender, RoutedEventArgs e)
        {
            currentfdo = _faceTools.GetFaceRectangle(currentFrame);
            GetCurrentFaceSample(currentFrame);
        }

        private void GetCurrentFaceSample(Mat showMat)
        {
            try
            {
                currentfdo = _faceTools.GetFaceRectangle(showMat);
                if (currentfdo.originalImg != null)
                {
                    Mat grayMat = new Mat();
                    grayMat = currentfdo.originalImg.Clone(_faceTools._faceSample);
                    Cv2.Resize(grayMat, grayMat, new OpenCvSharp.Size(100, 100));
                    Cv2.CvtColor(grayMat, grayMat, ColorConversionCodes.BGR2GRAY);
                    Cv2.EqualizeHist(grayMat, grayMat);//均衡化灰度图片
                    sampleImage.Image = _faceTools.byteToImage(grayMat.ToBytes());
                    string fileName = _userInfo.Name + "_" + Guid.NewGuid().ToString() + ".jpg";
                    string path = System.IO.Path.Combine(faceSamplesPath, fileName);
                    Thread.Sleep(100);
                    sampleImage.Image.Save(path);
                    MessageBox.Show("样本保存完毕");
                }
                else
                {
                    this.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        infoShow.Text = "采样失败，没有检测到人脸";
                    }));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("采样时发生异常，信息：" + ex.Message);
            }
        }

        private void CloseCapture()
        {
            try
            {
                ShouldStop = true; // 停止人脸检测
                WakeupResetEvent.Set(); // 停止等待下一帧
                if (_capture != null)
                {
                    _capture.Release(); // 释放捕获设备
                    _capture = null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("关闭摄像头时发生异常，信息：" + ex.Message);
            }
        }
    }
}
