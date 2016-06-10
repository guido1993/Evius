using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Xml.Linq;
using System.IO.IsolatedStorage;
using evius;
using System.Diagnostics;
using System.Reflection;
using System.Collections.ObjectModel;
using System.Collections;
using System.Net.NetworkInformation;
using Evius.Resources;
using Microsoft.Phone.Tasks;

namespace Evius
{
    public partial class HomePage : PhoneApplicationPage
    {
        public HomePage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            box_panorama.DefaultItem = (PanoramaItem)box_panorama.Items[1];

            if (NavigationService.CanGoBack)
            {
                while (NavigationService.BackStack.Any())
                {
                    NavigationService.RemoveBackEntry();
                }
            }

            string v_email = "", v_password = "";

            try
            {
                v_email = IsolatedStorageSettings.ApplicationSettings["email"].ToString();
                v_password = IsolatedStorageSettings.ApplicationSettings["password"].ToString();
            }
            catch { }

            if (NetworkInterface.GetIsNetworkAvailable())
            {
                box_loading.Visibility = Visibility.Visible;

                WebClient rss = new WebClient();
                rss.DownloadStringCompleted += rss_lab_activity;
                rss.DownloadStringAsync(new Uri("YOURURI" + v_email + "&password=" + v_password));
            }

        }

        private void box_panorama_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string v_email = "", v_password = "";

            try
            {
                v_email = IsolatedStorageSettings.ApplicationSettings["email"].ToString();
                v_password = IsolatedStorageSettings.ApplicationSettings["password"].ToString();
            }
            catch { }

            if (NetworkInterface.GetIsNetworkAvailable())
            {

                if (box_panorama.SelectedIndex == 1)
                {
                    if (box_activity.ItemCount.ToString() == "0")
                    {
                        box_loading.Visibility = Visibility.Visible;

                        WebClient rss = new WebClient();
                        rss.DownloadStringCompleted += rss_lab_activity;
                        rss.DownloadStringAsync(new Uri("YOURURI" + v_email + "&password=" + v_password));
                    }
                }

                if (box_panorama.SelectedIndex == 2)
                {
                    if (box_notification.ItemCount.ToString() == "0")
                    {
                        box_loading.Visibility = Visibility.Visible;

                        WebClient rss = new WebClient();
                        rss.DownloadStringCompleted += rss_lab_notification;
                        rss.DownloadStringAsync(new Uri("YOURURI" + v_email + "&password=" + v_password));
                    }
                }

                if (box_panorama.SelectedIndex == 3)
                {
                    if (box_event.ItemCount.ToString() == "0")
                    {
                        box_loading.Visibility = Visibility.Visible;

                        WebClient rss = new WebClient();
                        rss.DownloadStringCompleted += rss_lab_event;
                        rss.DownloadStringAsync(new Uri("YOURURI" + v_email + "&password=" + v_password));
                    }
                }
            }
        }

        void rss_lab_activity(object sender, DownloadStringCompletedEventArgs e)
        {
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                try
                {
                    ObservableCollection<Data> activity_list = new ObservableCollection<Data>();
                    box_activity.ItemsSource = activity_list;

                    var rss_information = from rss in XElement.Parse(e.Result).Descendants("item") select rss;
                    foreach (var name_information in rss_information)
                    {
                        string v_id = name_information.Element("id").Value;
                        string v_username = name_information.Element("username").Value;
                        string v_user_id = name_information.Element("user_id").Value;
                        string v_user_image = name_information.Element("user_image").Value;
                        string v_name = name_information.Element("event_name").Value;
                        string v_image = name_information.Element("event_image").Value;
                        string v_follower = name_information.Element("event_follower").Value;
                        string v_date = name_information.Element("date").Value;
                        string v_type = name_information.Element("type").Value;

                        activity_list.Add(new Data() { Id = v_id, UserId = v_user_id, Image = v_image, Username = v_username, Photo = v_user_image, Date = v_date, Name = v_name, Follower = v_follower, Type = v_type });
                        box_activity.ItemsSource = activity_list;
                    }

                    box_panorama.Visibility = Visibility.Visible;
                }

                catch (TargetInvocationException ex)
                {
                    string return_message = AppResources.MessageNoData.ToString();
                    MessageBox.Show(return_message);

                    IsolatedStorageSettings.ApplicationSettings["email"] = "";
                    IsolatedStorageSettings.ApplicationSettings["password"] = "";
                    IsolatedStorageSettings.ApplicationSettings["id"] = "";
                    IsolatedStorageSettings.ApplicationSettings.Save();

                    NavigationService.Navigate(new Uri("/Views/MainPage.xaml", UriKind.Relative));

                }
            }
            else
            {
                string return_message = AppResources.MessageNoInternet;
                MessageBox.Show(return_message);
            }

            box_loading.Visibility = Visibility.Collapsed;

        }

        void rss_lab_notification(object sender, DownloadStringCompletedEventArgs e)
        {
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                try
                {

                    ObservableCollection<Data> notification_list = new ObservableCollection<Data>();
                    box_notification.ItemsSource = notification_list;

                    var rss_information = from rss in XElement.Parse(e.Result).Descendants("item") select rss;
                    foreach (var name_information in rss_information)
                    {
                        string v_id = name_information.Element("id").Value;
                        string v_username = name_information.Element("username").Value;
                        string v_user_id = name_information.Element("user_id").Value;
                        string v_image = name_information.Element("user_image").Value;
                        string v_name = name_information.Element("event_name").Value;
                        string v_date = name_information.Element("date").Value;
                        string v_type = name_information.Element("type").Value;
                        notification_list.Add(new Data() { Id = v_id, UserId = v_user_id, Image = v_image, Username = v_username, Date = v_date, Name = v_name, Type = v_type });
                        box_notification.ItemsSource = notification_list;
                    }
                }
                catch (TargetInvocationException ex)
                {
                    string return_message = AppResources.MessageNoData.ToString();
                    MessageBox.Show(return_message);
                }
            }
            else
            {
                string return_message = AppResources.MessageNoInternet.ToString();
                MessageBox.Show(return_message);
            }

            box_loading.Visibility = Visibility.Collapsed;

        }

        void rss_lab_event(object sender, DownloadStringCompletedEventArgs e)
        {
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                try
                {
                    ObservableCollection<Data> event_list = new ObservableCollection<Data>();
                    box_event.ItemsSource = event_list;

                    var rss_information = from rss in XElement.Parse(e.Result).Descendants("item") select rss;
                    foreach (var name_information in rss_information)
                    {
                        string v_id = name_information.Element("id").Value;
                        string v_image = name_information.Element("event_image").Value;
                        string v_follower = name_information.Element("event_follower").Value;
                        string v_date = name_information.Element("date").Value;
                        string v_type = name_information.Element("type").Value;
                        event_list.Add(new Data() { Id = v_id, Image = v_image, Date = v_date, Type = v_type, Follower = v_follower });
                        box_event.ItemsSource = event_list;
                    }
                }
                catch (TargetInvocationException ex)
                {
                    string return_message = AppResources.MessageNoData.ToString();
                    MessageBox.Show(return_message);
                }
            }
            else
            {
                string return_message = AppResources.MessageNoInternet.ToString();
                MessageBox.Show(return_message);
            }

            box_loading.Visibility = Visibility.Collapsed;
        }

        private void View_ActivitiesPage(object sender, RoutedEventArgs e)
        {
            string return_message = AppResources.MessageNoReady.ToString();
            MessageBox.Show(return_message);
        }

        private void View_ProfilePage(object sender, RoutedEventArgs e)
        {
            var button = sender as HyperlinkButton;
            NavigationService.Navigate(new Uri("/Views/ProfilePage.xaml?msg=" + button.CommandParameter.ToString(), UriKind.Relative));
        }

        private void View_MessagesPage(object sender, RoutedEventArgs e)
        {
            string return_message = AppResources.MessageNoReady.ToString();
            MessageBox.Show(return_message);
        }

        private void View_ClanPage(object sender, RoutedEventArgs e)
        {
            string return_message = AppResources.MessageNoReady.ToString();
            MessageBox.Show(return_message);
        }

        private void View_ManageDrivePage(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/ManageDrivePage.xaml", UriKind.Relative));
        }

        private void View_BugReportPage(object sender, RoutedEventArgs e)
        {
            EmailComposeTask emailComposeTask = new EmailComposeTask();

            emailComposeTask.Subject = "Support - Evius";
            emailComposeTask.Body = "Write here about your issue or a feedback you'd like to share with us :)";
            emailComposeTask.To = "evius@outlook.com";

            emailComposeTask.Show();
        }

        private void View_SettingsPage(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/SettingsPage.xaml", UriKind.Relative));
        }

        private void View_DetailsEventPage(object sender, RoutedEventArgs e)
        {
            var button = sender as HyperlinkButton;
            NavigationService.Navigate(new Uri("/Views/DetailsEventPage.xaml?msg=" + button.CommandParameter.ToString(), UriKind.Relative));
        }

        private void Logout(object sender, RoutedEventArgs e)
        {
            IsolatedStorageSettings.ApplicationSettings["email"] = "";
            IsolatedStorageSettings.ApplicationSettings["password"] = "";
            IsolatedStorageSettings.ApplicationSettings["id"] = "";
            IsolatedStorageSettings.ApplicationSettings.Save();

            NavigationService.Navigate(new Uri("/Views/MainPage.xaml", UriKind.Relative));
        }

        private void PullToRefresh_Activity(object sender, EventArgs args)
        {
            string v_email = "", v_password = "";

            try
            {
                v_email = IsolatedStorageSettings.ApplicationSettings["email"].ToString();
                v_password = IsolatedStorageSettings.ApplicationSettings["password"].ToString();
            }
            catch { }

            WebClient rss = new WebClient();
            rss.DownloadStringCompleted += rss_lab_activity;
            rss.DownloadStringAsync(new Uri("YOURURI" + v_email + "&password=" + v_password));
        }

        private void PullToRefresh_Notification(object sender, EventArgs args)
        {
            string v_email = "", v_password = "";

            try
            {
                v_email = IsolatedStorageSettings.ApplicationSettings["email"].ToString();
                v_password = IsolatedStorageSettings.ApplicationSettings["password"].ToString();
            }
            catch { }

            WebClient rss = new WebClient();
            rss.DownloadStringCompleted += rss_lab_activity;
            rss.DownloadStringAsync(new Uri("YOURURI" + v_email + "&password=" + v_password));
        }

        private void PullToRefresh_Events(object sender, EventArgs args)
        {
            string v_email = "", v_password = "";

            try
            {
                v_email = IsolatedStorageSettings.ApplicationSettings["email"].ToString();
                v_password = IsolatedStorageSettings.ApplicationSettings["password"].ToString();
            }
            catch { }

            WebClient rss = new WebClient();
            rss.DownloadStringCompleted += rss_lab_activity;
            rss.DownloadStringAsync(new Uri("YOURURI" + v_email + "&password=" + v_password));
        }

        private void OnDataArrived(IEnumerable newData)
        {
            box_activity.ItemsSource = newData;
            box_activity.StopPullToRefreshLoading(true);
        }

        private void View_Search(object sender, System.Windows.Input.GestureEventArgs e)
        {
            string return_message = AppResources.MessageNoReady.ToString();
            MessageBox.Show(return_message);
        }

        private void View_Add_Event(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/AddEventPage.xaml", UriKind.Relative));
        }

    }
}