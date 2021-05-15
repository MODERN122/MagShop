using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Dom;

namespace kursfinderparser
{
    public static class Parser
    {
        private static string _url;

        private static IConfiguration _configuration;

        private static IBrowsingContext _browsingContext;

        static Parser()
        {
            _url = "https://kursfinder.ru/";
            _configuration = Configuration.Default.WithDefaultLoader();
            _browsingContext = BrowsingContext.New(_configuration);
        }

        public static async Task<List<Category>> GetCategories()
        {
            var categories = new List<Category>();
            using (var context = await _browsingContext.OpenAsync(_url))
            {
                var selector = context.QuerySelectorAll(".popular-item__title");
                foreach (var item in selector)
                {
                    var category = new Category { Name = item.TextContent, Url = _url + item.Attributes.First(x => x.Name == "href").Value };
                    categories.Add(category);
                }
            }

            return categories;
        }

        public static async Task<List<Topic>> GetTopicsByCategory(Category category)
        {
            var topics = new List<Topic>();
            using (var context = await _browsingContext.OpenAsync(category.Url))
            {
                var selector = context.QuerySelectorAll(".knowledge-card__title");
                foreach (var item in selector)
                {
                    var topic = new Topic { Name = item.TextContent, Url = _url + item.Attributes.First(x => x.Name == "href").Value };
                    topics.Add(topic);
                }
            }

            return topics;
        }

        public static async Task<List<Course>> GetCoursesByTopic(Topic topic)
        {
            var courses = new List<Course>();

            using (var context = await _browsingContext.OpenAsync(topic.Url))
            {
                //var selector = context.QuerySelectorAll(".course-card__title>a");
                //foreach (var item in selector)
                //{
                //    var course = new Course { Name = item.TextContent, InnerUrl = _url + item.Attributes.First(x => x.Name == "href").Value };
                //    courses.Add(course);
                //}
                var selector = context.QuerySelectorAll(".course-card__info");
                foreach (var item in selector)
                {
                    var innerSelector = item.QuerySelector(".course-card__title>a");
                    if (innerSelector != null)
                    {
                        var course = new Course { Name = innerSelector.TextContent, InnerUrl = _url + innerSelector.Attributes.First(x => x.Name == "href").Value };
                        innerSelector = item.QuerySelector(".course-card__duration");
                        if (innerSelector != null)
                        {
                            course.Duration = innerSelector.TextContent;
                        }
                        courses.Add(course);
                    }
                    
                }

            }

            return courses;
        }

        public static async Task<Course> GetCourseDetailedInfo(Course course)
        {
            using (var context = await _browsingContext.OpenAsync(course.InnerUrl))
            {
                course.Price = context.QuerySelector(".course-page__price").TextContent.Replace("\n", "").Trim();
                course.Description = context.QuerySelector(".course-page__desc").TextContent.Replace("\n", "").Trim();
                course.OuterUrl = _url + context.QuerySelector(".course-page__foot a").Attributes.First(x => x.Name == "href").Value;
                course.Company = context.QuerySelector(".course-page__platform>a").TextContent;
                var imgUrl = _url + context.QuerySelector(".course-page__img>img").Attributes.First(x => x.Name == "src").Value;
                course.Image = GetImageByUrl(imgUrl);
                var filters = context.QuerySelectorAll(".course-page__stats-item");
                foreach (var item in filters)
                {
                    if (item.TextContent == course.Duration)
                    {
                        continue;
                    }
                    if (!string.IsNullOrEmpty(course.Filters))
                    {
                        course.Filters += ". ";
                    }
                    var content = item.TextContent.Replace("\n", "");

                    // Регулярное выражение для удаления повторных пробелов (больше одного подряд)
                    course.Filters += Regex.Replace(content, @"\s+", " ");
                }
                var tags = context.QuerySelectorAll(".course-page__tag");
                foreach (var item in tags)
                {
                    if (!string.IsNullOrEmpty(course.Tags))
                    {
                        course.Tags += ", ";
                    }
                    course.Tags += item.TextContent;
                }

                course.IsLoaded = true;
            }

            return course;
        }

        private static byte[] GetImageByUrl(string url)
        {
            using (var webClient = new WebClient())
            {
                byte[] data = webClient.DownloadData(url);
                return data;
            }
        }
    }
}
