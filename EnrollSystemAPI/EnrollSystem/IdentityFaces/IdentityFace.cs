using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using EnrollSystem.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EnrollSystem.IdentityFaces
{
    public class IdentityFace
    {
        #region Variables
        //Images for finding face
        Image<Bgr, Byte> currentImage;
        Image<Gray, byte> result = null;
        Image<Gray, byte> gray_frame = null;
        //Classifier
        ClassifierTrain Eigen_Recog;
        CascadeClassifier Face;

        FontFace font = FontFace.HersheyTriplex;

        //Saving Jpg
        List<Image<Gray, byte>> ImagesToWrite = new List<Image<Gray, byte>>();
        #endregion
        #region public
        public List<string> Train(List<string> filenames, int studentId, string username)
        {
            //variables
            string basePath = @"wwwroot/trainedFace";
            Directory.CreateDirectory(basePath);
            List<string> trainedImagePath = new List<string>();

            Random rand = new Random();

            Face = new CascadeClassifier(@"./Resources/Cascades/haarcascade_frontalface_default.xml");
            foreach(var filename in filenames)
            {
                currentImage = new Image<Bgr, byte>(filename);
                int imgScale = currentImage.Width > 768 ? 2 : 1;
                currentImage =  currentImage.Resize(currentImage.Width / imgScale, currentImage.Height / imgScale, Emgu.CV.CvEnum.Inter.Cubic);
                //Convert it to GrayScale
                if (currentImage != null)
                {
                    gray_frame = currentImage.Convert<Gray, Byte>();

                    //Face Detector 
                    Rectangle[] facesDetected = Face.DetectMultiScale(gray_frame, 1.05, 5, new Size(50, 50), Size.Empty);
                    
                    //Action for each element detected
                    for(int i = 0; i < facesDetected.Length; i++)
                    {
                        /*
                        facesDetected[i].X += (int)(facesDetected[i].Height * 0.15);
                        facesDetected[i].Y += (int)(facesDetected[i].Width * 0.22);
                        facesDetected[i].Height -= (int)(facesDetected[i].Height * 0.3);
                        facesDetected[i].Width -= (int)(facesDetected[i].Width * 0.35);
                        */
                        result = currentImage.Copy(facesDetected[i]).Convert<Gray, byte>().Resize(100, 100, Inter.Cubic);
                        result._EqualizeHist();

                        //draw the face detected in the 0th (gray) channel with red color
                        currentImage.Draw(facesDetected[i], new Bgr(Color.Red), 2);
                        string myLabel = studentId + "_" + username;
                        currentImage.Draw(myLabel, new Point(facesDetected[i].X - 2, facesDetected[i].Y - 2), font, 0.5, new Bgr(Color.LightGreen), 1);

                        //Save training image
                        string fileName = $"Face_{username}_{rand.Next().ToString()}.jpg";
                        string filePath = Path.Combine(basePath, fileName);
                        result.Save(filePath);
                        trainedImagePath.Add(filePath);

                        currentImage.Save(filename);

                        /*
                        CvInvoke.Imshow("image",currentImage);
                        CvInvoke.Imshow("result", result);
                        CvInvoke.WaitKey(0);
                        */
                    }
                }
            }
            return trainedImagePath;
        }
        public List<string> Recognition(ICollection<TrainingImage> trainingImages,List<string> filenames)
        {
            Eigen_Recog = new ClassifierTrain(trainingImages);
            List<string> attendanceStudentList = new List<string>();

            Random rand = new Random();

            Face = new CascadeClassifier(@"./Resources/Cascades/haarcascade_frontalface_default.xml");
            foreach (var filename in filenames)
            {
                currentImage = new Image<Bgr, byte>(filename);
                int imgScale = currentImage.Width > 768 ? 2 : 1;
                currentImage = currentImage.Resize(currentImage.Width / imgScale, currentImage.Height / imgScale, Emgu.CV.CvEnum.Inter.Cubic);

                if (currentImage != null)
                {
                    gray_frame = currentImage.Convert<Gray, Byte>();

                    //Face Detector 
                    Rectangle[] facesDetected = Face.DetectMultiScale(gray_frame, 1.05, 5, new Size(50, 50), Size.Empty);

                    for (int i = 0; i < facesDetected.Length; i++)
                    {
                        result = currentImage.Copy(facesDetected[i]).Convert<Gray, byte>().Resize(100, 100, Inter.Cubic);
                        result._EqualizeHist();

                        //draw the face detected in the 0th (gray) channel with red color
                        currentImage.Draw(facesDetected[i], new Bgr(Color.Red), 2);

                        if (Eigen_Recog.IsTrained)
                        {
                            string name = Eigen_Recog.Recognise(result);
                            //int matchValue = (int)Eigen_Recog.Get_Eigen_Distance;

                            //Draw the label for each face detected and recognized
                            currentImage.Draw(name , new Point(facesDetected[i].X - 2, facesDetected[i].Y - 2), font, 1, new Bgr(Color.LightGreen), 1);
                            attendanceStudentList.Add(name);
                        }
                        //Save attendance image
                        currentImage.Save(filename);  
                    }
                }
            }
            return attendanceStudentList;
        }
        #endregion
    }
}
