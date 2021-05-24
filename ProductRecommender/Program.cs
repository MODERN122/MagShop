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
            var fileName = @"../../../Data/Clothing_Shoes_and_Jewelry_5.json";
            var jsonString = File.ReadAllText(fileName);
            jsonString = jsonString.Replace('\n', ',');
            var reviewsSrc = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ReviewModel>>("[" + jsonString + "]");
            MLContext mlContext = new MLContext();
            //reviewsSrc = reviewsSrc.Take(1000).ToList();
            int countTest = reviewsSrc.Count() / 5;
            var trainDataView = mlContext.Data.LoadFromEnumerable(reviewsSrc);
            var testDataView = mlContext.Data.LoadFromEnumerable(reviewsSrc.Take(countTest));


            var model = TrainModel(mlContext, trainDataView);
            EvaluateModel(mlContext, testDataView, model);


            var badReviews = reviewsSrc.Where(x => x.overall < 4);
            var goodReviews = reviewsSrc.Where(x => x.overall > 3);
            Console.WriteLine("Count bad reviews = " + badReviews.Count());
            Console.WriteLine("Count good reviews = " + goodReviews.Count());

            var trainDataViewBad = mlContext.Data.LoadFromEnumerable(badReviews);
            var testDataViewBad = mlContext.Data.LoadFromEnumerable(badReviews.Take(badReviews.Count()/5));
            var trainDataViewGood = mlContext.Data.LoadFromEnumerable(goodReviews);
            var testDataViewGood = mlContext.Data.LoadFromEnumerable(goodReviews.Take(goodReviews.Count()/5));

            Console.WriteLine("Train and evaluate context with bad overall reviews");
            var badModel = TrainModel(mlContext, trainDataViewBad);
            EvaluateModel(mlContext, testDataViewBad, badModel);

            Console.WriteLine("Train and evaluate context with good overall reviews");
            var goodModel = TrainModel(mlContext, trainDataViewGood);
            EvaluateModel(mlContext, testDataViewGood, goodModel);
            
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
            return model;
        }
        private static void EvaluateModel(MLContext mlContext, IDataView testDataView, ITransformer model)
        {
            Console.WriteLine("========================== Evaluating the model =============================");
            var prediction = model.Transform(testDataView);
            var metrics = mlContext
                .Regression
                .Evaluate(prediction, labelColumnName: $"{nameof(ReviewModel.overall)}", scoreColumnName: $"{nameof(ProductPrediction.Score)}");
            Console.WriteLine("Root Mean Squared Error : " + metrics.RootMeanSquaredError.ToString());
            Console.WriteLine("RSquared: " + metrics.RSquared.ToString());
        }
    }
}
