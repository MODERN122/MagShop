using System;
using System.Linq;
using System.IO;
using Microsoft.ML;
using Microsoft.ML.Trainers;
using System.Collections.Generic;
using Microsoft.ML.AutoML;
using System.Threading;
using Microsoft.ML.Data;
using static Microsoft.ML.DataOperationsCatalog;

namespace ProductRecommender
{
    class Program
    {
        static void Main(string[] args)
        {
            var fileName = @"../../../Data/Clothing_Shoes_and_Jewelry_5.json";
            var jsonString = File.ReadAllText(fileName);
            jsonString = jsonString.Replace('\n', ',');
            var reviewsSrc = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ReviewModel>>("[" + jsonString + "]");
            MLContext mlContext = new MLContext();
            //reviewsSrc = reviewsSrc.Take(1000).ToList();
            int countTest = reviewsSrc.Count() / 5;
            var data = mlContext.Data.LoadFromEnumerable(reviewsSrc);
            TrainTestData splitDataView = mlContext.Data.TrainTestSplit(data, 0.2);
            var reviewer = reviewsSrc.GroupBy(x => x.reviewerID).OrderByDescending(x=>x.Count()).First();
            reviewsSrc.Where(x => x.reviewerID == reviewer.Key).Select(x => x.overall).ToList().ForEach(x => Console.WriteLine(x));

            var model = TrainModel(mlContext, splitDataView.TrainSet);
            EvaluateModel(mlContext, splitDataView.TestSet, model);
            UseModelForSinglePrediction(mlContext, model, reviewsSrc);


            //var badReviews = reviewsSrc.Where(x => x.overall < 4);
            //var goodReviews = reviewsSrc.Where(x => x.overall > 3);
            //Console.WriteLine("Count bad reviews = " + badReviews.Count());
            //Console.WriteLine("Count good reviews = " + goodReviews.Count());

            //var trainDataViewBad = mlContext.Data.LoadFromEnumerable(badReviews);
            //var testDataViewBad = mlContext.Data.LoadFromEnumerable(badReviews.Take(badReviews.Count()/5));
            //var trainDataViewGood = mlContext.Data.LoadFromEnumerable(goodReviews);
            //var testDataViewGood = mlContext.Data.LoadFromEnumerable(goodReviews.Take(goodReviews.Count()/5));

            //Console.WriteLine("Train and evaluate context with bad overall reviews");
            //var badModel = TrainModel(mlContext, trainDataViewBad);
            //EvaluateModel(mlContext, testDataViewBad, badModel);

            //Console.WriteLine("Train and evaluate context with good overall reviews");
            //var goodModel = TrainModel(mlContext, trainDataViewGood);
            //EvaluateModel(mlContext, testDataViewGood, goodModel);

        }


        private static ITransformer TrainModel(MLContext mlContext, IDataView trainDataView)
        {
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
        private static void EvaluateModel(MLContext mlContext, IDataView testDataView, ITransformer model)
        {
            Console.WriteLine("========================== Evaluating the model =============================");
            var prediction = model.Transform(testDataView);
            var metrics = mlContext
                .Regression
                .Evaluate(prediction);
            Console.WriteLine("Root Mean Squared Error : " + metrics.RootMeanSquaredError.ToString());
            Console.WriteLine("RSquared: " + metrics.RSquared.ToString());
        }
        public static void UseModelForSinglePrediction(MLContext mlContext, ITransformer model, IEnumerable<ReviewModel> products)
        {
            Console.WriteLine("=========================== Making a prediction =============================");
            var predictionEngine = mlContext
                .Model
                .CreatePredictionEngine<ReviewModel, ProductPrediction>(model);

            foreach (var product in products.GroupBy(x => x.asin).Select(x => x.Key).Take(10))
            {
                var testInput = new ReviewModel { reviewerID = "A2J4XMWKR8PPD0", asin = product };
                var movieRatingPrediction = predictionEngine.Predict(testInput);
                if (Math.Round(movieRatingPrediction.Score, 1) > 3.5)
                {
                    Console.WriteLine("Product " + testInput.asin + " is recommended for user " + testInput.reviewerID + ". Score=" + movieRatingPrediction.Score);
                }
                else
                {
                    Console.WriteLine("Product " + testInput.asin + " is not recommended for user " + testInput.reviewerID + ". Score=" + movieRatingPrediction.Score);
                }
            }
        }
    }
}
