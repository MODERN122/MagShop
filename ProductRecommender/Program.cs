using System;
using System.Linq;
using System.IO;
using Microsoft.ML;
using Microsoft.ML.Trainers;
using Microsoft.ML.Data;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace ProductRecommender
{
    class Program
    {
        static void Main(string[] args)
        {
            var fileName = @"C:\Users\Mikhail\Documents\GitHub\MagShop\ProductRecommender/Data/Clothing_Shoes_and_Jewelry_5.json";
            var jsonString = File.ReadAllText(fileName);
            jsonString = jsonString.Replace('\n', ',');
            var reviewsSrc = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ReviewModel>>("[" + jsonString + "]");
            MLContext mlContext = new MLContext();
            //reviewsSrc = reviewsSrc.Take(1000).ToList();
            int countTest = reviewsSrc.Count() / 5;
            var trainDataView = mlContext.Data.LoadFromEnumerable<ReviewModel>(reviewsSrc.Skip(countTest));
            var testDataView = mlContext.Data.LoadFromEnumerable<ReviewModel>(reviewsSrc.Take(countTest));
            IEstimator<ITransformer> estimator = mlContext
    .Transforms
    .Conversion
    .MapValueToKey(outputColumnName: "ReviewerIdEncoded", inputColumnName: $"{nameof(ReviewModel.reviewerID)}")
    .Append(mlContext
        .Transforms
        .Conversion
        .MapValueToKey(outputColumnName: "ProductIdEncoded", inputColumnName: $"{nameof(ReviewModel.asin)}"));
            var options = new MatrixFactorizationTrainer.Options
            {
                MatrixColumnIndexColumnName = "ReviewerIdEncoded",
                MatrixRowIndexColumnName = "ProductIdEncoded",
                LabelColumnName = $"{nameof(ReviewModel.overall)}",
                NumberOfIterations = 20,
                ApproximationRank = 100
            };
            var trainerEstimator = estimator
                .Append(mlContext
                    .Recommendation()
                    .Trainers
                    .MatrixFactorization(options));
            Console.WriteLine("========================== Training the model =============================");
            ITransformer model = trainerEstimator.Fit(trainDataView);
            Console.WriteLine("========================== Evaluating the model =============================");
            var prediction = model.Transform(testDataView);
            var metrics = mlContext
                .Regression
                .Evaluate(prediction, labelColumnName: $"{nameof(ReviewModel.overall)}", scoreColumnName: $"{nameof(ProductPrediction.Score)}");
            Console.WriteLine("Root Mean Squared Error : " + metrics.RootMeanSquaredError.ToString());
            Console.WriteLine("RSquared: " + metrics.RSquared.ToString());
        }
        public class ProductPrediction
        {
            public string asin;
            public float Score;
        }
        public static (IDataView training, IDataView test) LoadData (MLContext mlContext)
        {
            var trainDataPath = Path.Combine(Environment.CurrentDirectory, "Data", "recommendation-ratings-train.csv");
            var testDataPath = Path.Combine(Environment.CurrentDirectory, "Data", "recommendation-ratings-test.csv");
            //var data = mlContext.Data.LoadFromEnumerable(new List<MLContext>());
            IDataView trainDataView = mlContext.Data.LoadFromTextFile<MovieRating>(trainDataPath, hasHeader: true, separatorChar: ',');
            IDataView testDataView = mlContext.Data.LoadFromTextFile<MovieRating>(testDataPath, hasHeader: true, separatorChar: ',');
            var products =  mlContext.Data.CreateEnumerable<MovieRating>(trainDataView, reuseRowObject: true);
            
            return (trainDataView, testDataView);
        }
        public static ITransformer BuildAndTrainModel(MLContext mlContext, IDataView trainDataView)
        {
            IEstimator<ITransformer> estimator = mlContext
                .Transforms
                .Conversion
                .MapValueToKey(outputColumnName: "userIdEncoded", inputColumnName: "userId")
                .Append(mlContext
                    .Transforms
                    .Conversion
                    .MapValueToKey(outputColumnName: "movieIdEncoded", inputColumnName: "movieId"));
            var options = new MatrixFactorizationTrainer.Options
            {
                MatrixColumnIndexColumnName = "userIdEncoded",
                MatrixRowIndexColumnName = "movieIdEncoded",
                LabelColumnName = "Label",
                NumberOfIterations = 20,
                ApproximationRank = 100
            };
            var trainerEstimator = estimator
                .Append(mlContext
                    .Recommendation()
                    .Trainers
                    .MatrixFactorization(options));
            Console.WriteLine("========================== Training the model =============================");
            ITransformer model = trainerEstimator.Fit(trainDataView);
            return model;

        }
        public static void EvaluateModel(MLContext mlContext, IDataView testDataView, ITransformer model)
        {
            Console.WriteLine("========================== Evaluating the model =============================");
            var prediction = model.Transform(testDataView);
            var metrics = mlContext
                .Regression
                .Evaluate(prediction, labelColumnName: "Label", scoreColumnName: "Score");
            Console.WriteLine("Root Mean Squared Error : " + metrics.RootMeanSquaredError.ToString());
            Console.WriteLine("RSquared: " + metrics.RSquared.ToString());
        }
        public static void UseModelForSinglePrediction(MLContext mlContext, ITransformer model)
        {
            Console.WriteLine("=========================== Making a prediction =============================");
            var predictionEngine = mlContext
                .Model
                .CreatePredictionEngine<MovieRating, MovieRatingPrediction>(model);
            for (int i = 0; i < 10; i++)
            {
                var testInput = new MovieRating { userId = 6, movieId = i};
                var movieRatingPrediction = predictionEngine.Predict(testInput);
                if (Math.Round(movieRatingPrediction.Score, 1) > 3.5)
                {
                    Console.WriteLine("Movie " + testInput.movieId + " is recommended for user " + testInput.userId + ". Score=" + movieRatingPrediction.Score);
                }
                else
                {
                    Console.WriteLine("Movie " + testInput.movieId + " is not recommended for user " + testInput.userId + ". Score=" + movieRatingPrediction.Score);
                }
            }
        }
        public static void SaveModel(MLContext mlContext, DataViewSchema trainDataViewSchema, ITransformer model)
        {
            var modelPath = Path.Combine(Environment
                .CurrentDirectory, "Data", "MovieRecommenderModel.zip");

            Console.WriteLine("========================== Saving the model to a file ==================================");
            mlContext
                .Model
                .Save(model, trainDataViewSchema, modelPath);
            MLContext mLContext = new MLContext();
            DataViewSchema schema;
            var model1 = mlContext.Model.Load(modelPath, out schema);

        }
    }
}
