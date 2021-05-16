using ApplicationCore.Endpoints.Products;
using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace kursfinderparser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CourseLibrary _courseLibrary;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadCategories();

        }

        private async void LoadCategories()
        {
            var categories = await Parser.GetCategories();
            _courseLibrary = new CourseLibrary() { Categories = categories };

            foreach (var category in _courseLibrary.Categories)
            {
                tv_courses.Items.Add(CreateCategory(category));
            }
        }

        private TreeViewItem CreateCategory(Category category)
        {
            var item = new TreeViewItem();
            item.Header = category.Name;
            item.Tag = category.Url;
            item.Items.Add(null);
            item.Expanded += OnCategoryExpanded;

            return item;
        }

        private TreeViewItem CreateTopic(Topic topic)
        {
            var item = new TreeViewItem();
            item.Header = topic.Name;
            item.Tag = topic.Url;
            item.Items.Add(null);
            item.Expanded += OnTopicExpanded;

            return item;
        }

        private TreeViewItem CreateCourse(Course course)
        {
            var item = new TreeViewItem();
            item.Header = course.Name;
            item.Tag = course.InnerUrl;
            item.GotFocus += OnCourseClicked;

            return item;
        }

        private async void OnCategoryExpanded(object sender, RoutedEventArgs e)
        {
            var item = sender as TreeViewItem;
            if (item.Items[0] != null)
            {
                return;
            }

            var category = _courseLibrary.Categories.Find(x => x.Url == item.Tag.ToString());
            var topics = await Parser.GetTopicsByCategory(category);
            category.Topics = topics;

            item.Items.Clear();
            foreach (var topic in topics)
            {
                item.Items.Add(CreateTopic(topic));
            }
        }

        private async void OnTopicExpanded(object sender, RoutedEventArgs e)
        {
            var item = sender as TreeViewItem;
            if (item.Items[0] != null)
            {
                return;
            }

            Topic topic = null;

            // Ищем тему с нужным URL по всем категориям
            foreach (var category in _courseLibrary.Categories)
            {
                topic = category.Topics.Find(x => x.Url == item.Tag.ToString());
                if(topic != null)
                {
                    break;
                }
            }


            var courses = await Parser.GetCoursesByTopic(topic);
            topic.Courses = courses;

            item.Items.Clear();
            foreach (var course in courses)
            {
                item.Items.Add(CreateCourse(course));
            }
        }
        private async void ParseCourseToProduct(object sender, RoutedEventArgs e)
        {
            // Ищем курс в нужным url по всем категориям и всем темам
            List<Course> courses = new List<Course>();
            foreach (var category in _courseLibrary.Categories.Take(2))
            {
                var topics = await Parser.GetTopicsByCategory(category);
                foreach (var topic in topics.Take(2))
                {
                    var coursesLOL = await Parser.GetCoursesByTopic(topic);
                    foreach (var item in coursesLOL.Take(2))
                    {
                        var fullCourse =  await Parser.GetCourseDetailedInfo(item);
                        if(fullCourse!=null)
                            courses.Add(fullCourse);

                    }
                }
            }
            foreach (var course in courses)
            {
                CreateProductRequest product = new CreateProductRequest()
                {
                    ProductName = course.Name,
                    Description = course.Description,
                    PreviewImage = course.Image,
                    Properties = new List<Property>()
                    {
                        new Property("Продолжительность", new List<PropertyItem>()
                        {
                            new PropertyItem(course.Duration, double.Parse(new String(course.Price.Where(Char.IsDigit).ToArray())))
                        })
                    },
                    Url = course.OuterUrl
                };
                //Добавить отправку этих товаров по АПИ, метод апи должен предоставить Миша
                //

            }


        }
        private async void OnCourseClicked(object sender, RoutedEventArgs e)
        {
            var item = sender as TreeViewItem;

            Course course = null;
            bool isFound = false;

            // Ищем курс в нужным url по всем категориям и всем темам
            foreach (var category in _courseLibrary.Categories)
            {
                if (isFound)
                {
                    break;
                }
                foreach (var topic in category.Topics)
                {
                    course = topic.Courses.Find(x => x.InnerUrl == item.Tag.ToString());
                    if (course != null)
                    {
                        isFound = true;
                        break;
                    }
                }
            }

            if (!course.IsLoaded)
            {
                _ = await Parser.GetCourseDetailedInfo(course);
            }
            SetCourseData(course);

            if (grid_courseDetails.Visibility == Visibility.Hidden)
            {
                grid_courseDetails.Visibility = Visibility.Visible;
            }
        }

        private void SetCourseData(Course course)
        {
            tb_courseName.Text = course.Name;
            tb_coursePrice.Text = course.Price;
            tb_courseDescription.Text = course.Description;
            tb_courseCompany.Text = course.Company;
            tb_courseFilters.Text = course.Filters;
            tb_courseTags.Text = course.Tags;
            tb_courseUrl.NavigateUri = new Uri(course.OuterUrl);
            tb_courseDuration.Text = course.Duration;
            tb_courseImage.Source = CreateImage(course.Image);
        }

        private BitmapImage CreateImage(byte[] img)
        {
            var image = new BitmapImage();
            using (var mem = new MemoryStream(img))
            {
                mem.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = mem;
                image.EndInit();
            }
            image.Freeze();
            return image;
        }


        private void OnLinkClick(object sender, RoutedEventArgs e)
        {
            var link = sender as Hyperlink;
            var url = link.NavigateUri.ToString();
            Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });
            e.Handled = true;
        }
    }
}
