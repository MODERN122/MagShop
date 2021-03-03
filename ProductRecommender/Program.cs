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
            (IDataView trainDataView, IDataView testDataView) = LoadData(mlContext);
            ITransformer model = BuildAndTrainModel(mlContext, trainDataView: trainDataView); 
            EvaluateModel(mlContext, testDataView, model);
            UseModelForSinglePrediction(mlContext, model); 
            SaveModel(mlContext, trainDataView.Schema, model);

        }
        public static (IDataView training, IDataView test) LoadData (MLContext mlContext)
        {
            var trainDataPath = Path.Combine(Environment.CurrentDirectory, "Data", "recommendation-ratings-train.csv");
            var testDataPath = Path.Combine(Environment.CurrentDirectory, "Data", "recommendation-ratings-test.csv");

            IDataView trainDataView = mlContext.Data.LoadFromTextFile<MovieRating>(trainDataPath, hasHeader: true, separatorChar: ',');
            IDataView testDataView = mlContext.Data.LoadFromTextFile<MovieRating>(testDataPath, hasHeader: true, separatorChar: ',');
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

        }
    }
}
