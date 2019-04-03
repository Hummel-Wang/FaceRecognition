using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace OpencvSharpApp.Common
{
    public class FaceTools
    {
        #region 
        public Rect _faceSample;
        /// <summary>
        /// faceSamplePath
        /// </summary>
        public string _faceSamplesPath = System.Windows.Forms.Application.StartupPath + "\\trainedFaces";

        /// <summary>
        /// 眼睛分类器
        /// </summary>
        //private CascadeClassifier _eyeClassifier = new CascadeClassifier(System.Windows.Forms.Application.StartupPath + "\\resource\\haarcascade_eye.xml");

        /// <summary>
        /// 人脸框分类器
        /// </summary>
        private CascadeClassifier _faceClassifier = new CascadeClassifier(System.Windows.Forms.Application.StartupPath + "\\resource\\haarcascade_frontalface_alt2.xml");


        #endregion
        TrainedFaceRecognizer tfr;

        public FaceTools()
        {
            SetTrainedFaceRecognizer(FaceRecognizerType.EigenFaceRecognizer);
        }

        /// <summary>
        /// 获取已保存的所有样本文件
        /// </summary>
        /// <returns></returns>
        public TrainedFileList SetSampleFacesList()
        {
            TrainedFileList tf = new TrainedFileList();
            DirectoryInfo di = new DirectoryInfo(_faceSamplesPath);
            int i = 0;
            foreach (FileInfo fi in di.GetFiles())
            {
                //tf.trainedImages.Add(new Image<Gray, byte>(fi.FullName));
                Mat imageFile = new Mat(fi.FullName);
                Cv2.CvtColor(imageFile, imageFile, ColorConversionCodes.RGB2GRAY);
                tf.trainedImages.Add(imageFile);
                tf.trainedLabelOrder.Add(i);
                tf.trainedFileName.Add(fi.Name.Split('_')[0]);
                i++;
            }
            return tf;
        }

        /// <summary>
        /// 训练人脸识别器
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public TrainedFaceRecognizer SetTrainedFaceRecognizer(FaceRecognizerType type)
        {
            tfr = new TrainedFaceRecognizer();
            tfr.trainedFileList = SetSampleFacesList();

            switch (type)
            {
                case FaceRecognizerType.EigenFaceRecognizer:
                    tfr.faceRecognizer = OpenCvSharp.Face.EigenFaceRecognizer.Create(80, double.PositiveInfinity);

                    break;
                case FaceRecognizerType.FisherFaceRecognizer:
                    tfr.faceRecognizer = OpenCvSharp.Face.FisherFaceRecognizer.Create(80, 3500);
                    break;
                case FaceRecognizerType.LBPHFaceRecognizer:
                    tfr.faceRecognizer = OpenCvSharp.Face.LBPHFaceRecognizer.Create(1, 8, 8, 8, 100);
                    break;
            }
            if (tfr.trainedFileList.trainedImages.Count > 0 && tfr.trainedFileList.trainedLabelOrder.Count > 0)
            {
                tfr.faceRecognizer.Train(tfr.trainedFileList.trainedImages.ToArray(), tfr.trainedFileList.trainedLabelOrder.ToArray());
            }
            return tfr;
        }

        /// <summary>
        /// 获取制定图片，识别出的人脸矩形框
        /// </summary>
        /// <param name="emguImage"></param>
        /// <returns></returns>
        public FaceDetectedObj GetFaceRectangle(Mat emguImage)
        {
            FaceDetectedObj fdo = new FaceDetectedObj();
            fdo.originalImg = emguImage;
            List<Rect> faces = new List<Rect>();
            try
            {
                Rect[] facesDetected = _faceClassifier.DetectMultiScale(emguImage, 1.1, 1, HaarDetectionType.ScaleImage, new OpenCvSharp.Size(100, 120));
                faces.AddRange(facesDetected);
            }
            catch (Exception ex)
            {
                //MessageBox.Show("GetFaceRectangle" + ex.Message);
            }
            fdo.facesRectangle = faces;

            return fdo;
        }

        /// <summary>
        /// 人脸识别
        /// </summary>
        /// <param name="emguImage"></param>
        /// <returns></returns>
        public FaceDetectedObj FaceRecognize(Mat imageFrame)
        {
            FaceDetectedObj fdo = GetFaceRectangle(imageFrame);
            //Image<Gray, byte> tempImg = fdo.originalImg.ToImage<Gray, byte>();
            Mat tempImg = fdo.originalImg;
            //Add
            byte[] array = tempImg.ToBytes();
            Image image = byteToImage(array);

            #region 给识别出的所有人脸画矩形框
            using (Graphics g = Graphics.FromImage(image))
            {
                foreach (Rect face in fdo.facesRectangle)
                {
                    imageFrame.Rectangle(face, new Scalar(0, 0, 255)); //给识别出的人脸画矩形框

                    Mat grayFace = new Mat();
                    tempImg.CopyTo(grayFace);
                    OpenCvSharp.Size size = new OpenCvSharp.Size(100, 100);
                    Cv2.Resize(grayFace, grayFace, size);

                    Cv2.CvtColor(grayFace, grayFace, ColorConversionCodes.BGR2GRAY);
                    grayFace.EqualizeHist();//得到均衡化人脸的灰度图像

                    #region 得到匹配姓名，并画出
                    if (tfr.trainedFileList != null && tfr.trainedFileList.trainedImages.Count > 0)
                    {
                        int pr = tfr.faceRecognizer.Predict(grayFace);
                        string recogniseName = tfr.trainedFileList.trainedFileName[pr].ToString();
                        fdo.Name = recogniseName;
                        fdo.names.Add(recogniseName);
                        imageFrame.PutText(recogniseName, new OpenCvSharp.Point(200, 100), HersheyFonts.HersheyComplex, 1, Scalar.Red);
                    }
                    #endregion

                    _faceSample = face;
                }
            }
            #endregion
            return fdo;
        }


        public Image BytesToImage(Byte[] buffer)
        {
            var ms = new MemoryStream(buffer, 0, buffer.Length);
            var bf = new BinaryFormatter();
            object obj = bf.Deserialize(ms);
            ms.Close();
            return (Image)obj;
        }

        public Image byteToImage(byte[] myByte)
        {
            MemoryStream ms = new MemoryStream(myByte);
            Image _Image = Image.FromStream(ms);
            return _Image;
        }
    }

    #region 自定义类及访问类型
    public class TrainedFileList
    {
        //public List<Image<Gray, byte>> trainedImages = new List<Image<Gray, byte>>();
        public List<Mat> trainedImages = new List<Mat>();
        public List<int> trainedLabelOrder = new List<int>();
        public List<string> trainedFileName = new List<string>();
    }

    public class TrainedFaceRecognizer
    {
        public OpenCvSharp.Face.FaceRecognizer faceRecognizer;
        public TrainedFileList trainedFileList;
    }

    public class FaceDetectedObj
    {
        public Mat originalImg;
        public List<Rect> facesRectangle;
        public List<string> names = new List<string>();
        public string Name;
    }

    public enum FaceRecognizerType
    {
        EigenFaceRecognizer = 0,
        FisherFaceRecognizer = 1,
        LBPHFaceRecognizer = 2,
    }
    #endregion
}
