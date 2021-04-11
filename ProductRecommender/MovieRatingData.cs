using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.ML.Data;

namespace ProductRecommender
{
    public class MovieRating
    {
        [LoadColumn(0)]
        public float userId;
        [LoadColumn(1)]
        public float movieId;
        [LoadColumn(2)]
        public float Label;
        [LoadColumn(3)]
        public long Time;
    }

}
