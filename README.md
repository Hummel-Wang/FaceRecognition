# FaceRecognition
Face Recognition Based on C#

人脸识别三种方式：

1.EmguCv,Opencv的.Net版本

2.OpencvSharp，Opencv的C#封装版本

3.FaceRecognitionDotNet，基于Dlib实现的.Net版本

使用注意事项：

1.三种实现方式仅支持x64编译平台

2.EmguCv和OpencvSharp版本直接通过NuGet安装package即可，版本分别为：EmguCV.3.1.0.1和OpenCvSharp3-AnyCPU.4.0.0.20181129

3.FaceRecognitionDotNet版本直接通过NuGet安装Dlib，版本为：DlibDotNet.19.16.0.20190223. 另外需要引用项目中Dll文件夹中两个dll文件。

说明：

1.FaceRecognitionDotNet版本也可直接通过NuGet安装FaceRecognitionDotNet来实现。当初调研时看到了有人编译过的源码更好用，所以放弃了。有心人可以尝试。
https://www.cnblogs.com/RainbowInTheSky/p/10247921.html

2.在FaceRecognitionDotNet版本Demo开发过程中，我发现Emgucv依赖库多，曾想尝试用opencvsharp来代替，但是在人脸检测过程中，始终无法获取到人脸数据，最终放弃了。有心人若尝试成功，烦请留言告知一声。
