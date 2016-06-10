using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.IO.IsolatedStorage;
using System.Xml.Linq;
using System.Reflection;
using evius;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Net.NetworkInformation;
using Evius.Resources;

namespace Evius.Views
{
    public partial class ProfilePage : PhoneApplicationPage
    {
        public ProfilePage()
        {
            InitializeComponent();
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

                if (box_panorama.SelectedIndex == 0)
                {
                    box_loading.Visibility = Visibility.Visible;

                    WebClient rss = new WebClient();
                    rss.DownloadStringCompleted += rss_lab_profile;
                    rss.DownloadStringAsync(new Uri("YOURURL" + v_email + "&password=" + v_password + "&id=" + box_msg.Text));
                }

                if (box_panorama.SelectedIndex == 1)
                {
                    if (box_event.Items.Count.ToString() == "0")
                    {
                        box_loading.Visibility = Visibility.Visible;

                        WebClient rss = new WebClient();
                        rss.DownloadStringCompleted += rss_lab_event;
                        rss.DownloadStringAsync(new Uri("YOURURL" + v_email + "&password=" + v_password + "&id=" + box_msg.Text));
                    }
                }
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            string v_email = "", v_password = "";

            try
            {
                v_email = IsolatedStorageSettings.ApplicationSettings["email"].ToString();
                v_password = IsolatedStorageSettings.ApplicationSettings["password"].ToString();
            }
            catch { }

            string id = NavigationContext.QueryString["msg"];
            base.OnNavigatedTo(e);

            box_msg.Text = id;

            if (NetworkInterface.GetIsNetworkAvailable())
            {
                box_loading.Visibility = Visibility.Visible;

                WebClient rss = new WebClient();
                rss.DownloadStringCompleted += rss_lab_profile;
                rss.DownloadStringAsync(new Uri("YOURURL" + v_email + "&password=" + v_password + "&id=" + id));
            }
        }

        void rss_lab_profile(object sender, DownloadStringCompletedEventArgs e)
        {
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                try
                {
                    var rss_information = from rss in XElement.Parse(e.Result).Descendants("item") select rss;
                    foreach (var name_information in rss_information)
                    {
                        string v_id = name_information.Element("id").Value;
                        string v_username = name_information.Element("username").Value;
                        string v_user_image = name_information.Element("user_image").Value;
                        string v_title = name_information.Element("user_title").Value;
                        string v_user_vote = name_information.Element("user_vote").Value;
                        string v_vote_number = name_information.Element("user_vote_number").Value;
                        string v_email = name_information.Element("email").Value;
                        string v_date = name_information.Element("date_born").Value;
                        string v_phonenumber = name_information.Element("telephone").Value;
                        string v_settings = name_information.Element("settings").Value;

                        if (v_title == "t1") v_title = AppResources.TextProfileUsernameTitle1.ToString();
                        if (v_title == "t2") v_title = AppResources.TextProfileUsernameTitle2.ToString();

                        int vote = int.Parse(v_user_vote);

                        if (vote >= 1) box_rating_1.Source = new BitmapImage(new Uri("/Images/All/star_half.png", UriKind.Relative));
                        if (vote >= 2) box_rating_1.Source = new BitmapImage(new Uri("/Images/All/star_full.png", UriKind.Relative));
                        if (vote >= 3) box_rating_2.Source = new BitmapImage(new Uri("/Images/All/star_half.png", UriKind.Relative));
                        if (vote >= 4) box_rating_2.Source = new BitmapImage(new Uri("/Images/All/star_full.png", UriKind.Relative));
                        if (vote >= 5) box_rating_3.Source = new BitmapImage(new Uri("/Images/All/star_half.png", UriKind.Relative));
                        if (vote >= 6) box_rating_3.Source = new BitmapImage(new Uri("/Images/All/star_full.png", UriKind.Relative));

                        if (v_settings == "1") box_action_inner.Source = new BitmapImage(new Uri("/Images/All/icon_minus.png", UriKind.Relative));
                        if (v_settings == "-1") box_action_inner.Source = new BitmapImage(new Uri("/Images/All/icon_add.png", UriKind.Relative));
                        if (v_settings == "0") box_action.Visibility = Visibility.Collapsed;

                        if (v_email == "") box_email_view.Visibility = Visibility.Collapsed;
                        if (v_phonenumber == "") box_telephone_view.Visibility = Visibility.Collapsed;
                        if (v_date == "") box_born_view.Visibility = Visibility.Collapsed;

                        box_id.Text = v_id;
                        box_username.Text = v_username;
                        box_image.ImageSource = new BitmapImage(new Uri(v_user_image, UriKind.Absolute));
                        box_title.Text = v_title;
                        box_email.Text = v_email;
                        box_born.Text = v_date;
                        box_telephone.Text = v_phonenumber;
                        box_rating_number.Text = v_vote_number;
                    }

                    box_panorama.Visibility = Visibility.Visible;
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

        void rss_lab_event (object sender, DownloadStringCompletedEventArgs e)
        {
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                try
                {
                    box_event.Items.Clear();

                    var rss_information = from rss in XElement.Parse(e.Result).Descendants("item") select rss;
                    foreach (var name_information in rss_information)
                    {
                        string v_id = name_information.Element("id").Value;
                        string v_photo = name_information.Element("event_image").Value;
                        string v_follower = name_information.Element("event_follower").Value;
                        string v_date = name_information.Element("date").Value;
                        string v_type = name_information.Element("type").Value;

                        box_event.Items.Add(new Data() { Id = v_id, Date = v_date, Follower = v_follower, Type = v_type, Photo = v_photo });
                    }
                    

                }
                catch (TargetInvocationException ex)
                {
                    string return_message = AppResources.MessageNoData.ToString();
                    MessageBox.Show(return_message + ex.Message.ToString());
                }
            }
            else
            {
                string return_message = AppResources.MessageNoInternet.ToString();
                MessageBox.Show(return_message);
            }

            box_loading.Visibility = Visibility.Collapsed;

        }

        private void Click_Action(object sender, RoutedEventArgs e)
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
                box_loading.Visibility = Visibility.Visible;

                WebClient rss_login = new WebClient();
                rss_login.DownloadStringCompleted += rss_lab;
                rss_login.DownloadStringAsync(new Uri("YOURURL" + v_email + "&password=" + v_password + "&type=FollowAndUnfollowProfile&id=" + box_id.Text));
            }

        }

        void rss_lab(object sender, DownloadStringCompletedEventArgs e)
        {
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                try
                {
                    var rssData = from rss in XElement.Parse(e.Result).Descendants("return") select rss;
                    foreach (var name in rssData)
                    {
                        string message = name.Element("message").Value;

                        if (message == "0x000003")
                        {

                            box_action_inner.Source = new BitmapImage(new Uri("/Images/All/icon_add.png", UriKind.Relative));

                            string return_message = AppResources.MessageUserRemove.ToString();
                            MessageBox.Show(return_message);
                        }
                        else
                        {
                            if (message == "0x000004")
                            {
                                box_action_inner.Source = new BitmapImage(new Uri("/Images/All/icon_minus.png", UriKind.Relative));

                                string return_message = AppResources.MessageUserAdd.ToString();
                                MessageBox.Show(return_message);
                            }

                        }
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

        private void View_DetailsEventPage(object sender, RoutedEventArgs e)
        {
            var button = sender as HyperlinkButton;
            NavigationService.Navigate(new Uri("/Views/DetailsEventPage.xaml?msg=" + button.CommandParameter.ToString(), UriKind.Relative));
        }

    }
}