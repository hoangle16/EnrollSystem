using Emgu.CV;
using Emgu.CV.Face;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using EnrollSystem.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EnrollSystem.IdentityFaces
{
    public class ClassifierTrain: IDisposable
    {
        #region Variables
        //Eigen
        //EigenObjectRecognizer recognizer;
        FaceRecognizer recognizer;

        //training variables
        List<Image<Gray, byte>> trainingImages = new List<Image<Gray, byte>>(); //Images
        IInputArray MyIInputArray;
        List<string> Names_List = new List<string>(); //labels
        List<int> Names_list_ID = new List<int>();
        int ContTrain, NumLabels;
        float Eigen_Distance = 0;
        string Eigen_label;
        int Eigen_threshold = 2000;

        //Class Variables
        string Error;
        bool _IsTrained = false;
        public string Recognizer_Type = "EMGU.CV.EigenFaceRecognizer";
        #endregion

        #region Constructors
        public ClassifierTrain(ICollection<TrainingImage> trainingImg)
        {
            _IsTrained = LoadTrainingData(trainingImg);
        }
        #endregion
        #region public
        public void Dispose()
        {
            recognizer = null;
            trainingImages = null;
            Names_List = null;
            Names_list_ID = null;
            GC.Collect();
        }
        public bool ReTrain(ICollection<TrainingImage> trainingImg)
        {
            return _IsTrained = LoadTrainingData(trainingImg);
        }
        public bool IsTrained
        {
            get { return _IsTrained; }
        }

        //Recognise a Grayscale Image using the trained Eigen Recogniser
        public string Recognise(Image<Gray, byte> Input_image, int Eigen_Thresh = -1)
        {
            if (_IsTrained)
            {
                FaceRecognizer.PredictionResult ER = recognizer.Predict(Input_image);

                if (ER.Label == -1)
                {
                    Eigen_label = "Unknown";
                    Eigen_Distance = 0;
                    return Eigen_label;
                }
                else
                {
                    Eigen_label = ER.Label + "_" + Names_List[Names_list_ID.IndexOf(ER.Label)];
                    Eigen_Distance = (float)ER.Distance;
                    if (Eigen_Thresh > -1) Eigen_threshold = Eigen_Thresh;

                    if (Eigen_Distance > Eigen_threshold) return Eigen_label;
                    else return "Unknown";
                }
            }
            else return "";
        } 
        //setter
        public int Set_Eigen_Threshold
        {
            set
            {
                Eigen_threshold = value;
            }
        }
        //getter
        public string Get_Eigen_Label
        {
            get
            {
                return Eigen_label;
            }
        }
        public float Get_Eigen_Distance
        {
            get
            {
                return Eigen_Distance;
            }
        }
        public string Get_Error
        {
            get { return Error; }
        }

        #endregion
        #region Private
        private bool LoadTrainingData(ICollection<TrainingImage> trainingImg)
        {
            if (trainingImg.Count > 0)
            {
                try
                {
                    //Reset data
                    Names_List.Clear();
                    Names_list_ID.Clear();
                    trainingImages.Clear();

                    //Read TrainingData
                    foreach(var trainingImage in trainingImg)
                    {
                        //check file exist
                        if (File.Exists(trainingImage.Image.Path))
                        {
                            //load image
                            trainingImages.Add(new Image<Gray, byte>(trainingImage.Image.Path));
                            Names_list_ID.Add(trainingImage.StudentId);
                            Names_List.Add(trainingImage.Student.User.Name);
                            NumLabels += 1;
                        }
                    }
                    ContTrain = NumLabels;
                    if (trainingImages.ToArray().Length != 0)
                    {
                        Mat _test = new Mat();
                        recognizer = new EigenFaceRecognizer(80, double.PositiveInfinity);
                        //covert to mat
                        List<Mat> matList = new List<Mat>();
                        foreach(var trainingImage in trainingImages)
                        {
                            matList.Add(trainingImage.Mat);
                        }

                        //training
                        recognizer.Train(matList.ToArray(), Names_list_ID.ToArray());
                        return true;
                    }
                    else return false;
                }
                catch (Exception ex)
                {
                    Error = ex.ToString();
                    return false;
                }
            }
            else return false;
        }
        #endregion
    }
}
