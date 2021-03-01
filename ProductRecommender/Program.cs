using System;
using System.IO;
using Microsoft.ML;
using Microsoft.ML.Trainers;

namespace ProductRecommender
{
    class Program
    {
        static void Main(string[] args)
        {
            MLContext mlContext = new MLContext();
            (IDataView trainingDataView, IDataView testDataView) = LoadData(mlContext);

        }
        public static (IDataView training, IDataView test) LoadData (MLContext mlContext)
        {
            var trainDataPath = Path.Combine(Environment.CurrentDirectory, "Data", "recommendation-raiting-train.csv");
            var testDataPath = Path.Combine(Environment.CurrentDirectory, "Data", "recommendation-raitings-test.csv");

            IDataView trainDataView = mlContext.Data.LoadFromTextFile<MovieRating>(trainDataPath, hasHeader: true, separatorChar: ',');
            IDataView testDataView = mlContext.Data.LoadFromTextFile<MovieRating>(testDataPath, hasHeader: true, separatorChar: ',');
            return (trainDataView, testDataView);
        }
    }
}
