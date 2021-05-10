using System;
using System.Collections.Generic;
using System.Text;

namespace ProductRecommender
{
    public class ReviewModel
    {
        public string reviewerID { get; set; }
        public string asin { get; set; }
        public string reviewerName { get; set; }
        public int[] helpful { get; set; }
        public string reviewText { get; set; }
        /// <summary>
        /// Оценка
        /// </summary>
        public float overall { get; set; }
        public string summary { get; set; }
        public int unixReviewTime { get; set; }
        public string reviewTime { get; set; }
    }
}
