using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using evius;
using System.IO.IsolatedStorage;
using System.Xml.Linq;
using System.Reflection;
using System.Windows.Media.Imaging;
using System.Net.NetworkInformation;
using Evius.Resources;

namespace Evius.Views.ManageDrivePages
{
    public partial class OfferPage : PhoneApplicationPage
    {
        public OfferPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            string id = NavigationContext.QueryString["msg"];
            base.OnNavigatedTo(e);

            box_id.Text = id;

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
                rss.DownloadStringCompleted += rss_offer_conf;
                rss.DownloadStringAsync(new Uri("YOURURL" + v_email + "&password=" + v_password + "&id=" + id + "&type=1"));
            }
            
        }

        void rss_offer_conf(object sender, DownloadStringCompletedEventArgs e)
        {
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                try
                {
                    box_accepted.Items.Clear();

                    var rss_information = from rss in XElement.Parse(e.Result).Descendants("item") select rss;
                    foreach (var name_information in rss_information)
                    {
                        string v_id = name_information.Element("id").Value;
                        string v_image = name_information.Element("user_image").Value;
                        string v_type = name_information.Element("type").Value;
                        string v_username = name_information.Element("username").Value;
                        string v_user_vote = name_information.Element("user_vote").Value;
                        string v_user_vote_number = name_information.Element("user_vote_number").Value;
                        string v_user_id = name_information.Element("user_id").Value;

                        string rating_1 = "", rating_2 = "", rating_3 = "";
                        try
                        {
                            int vote = int.Parse(v_user_vote);

                            rating_1 = "/Images/All/star_empty.png";
                            rating_2 = "/Images/All/star_empty.png";
                            rating_3 = "/Images/All/star_empty.png";
                            if (vote >= 1) rating_1 = "/Images/All/star_half.png";
                            if (vote >= 2) rating_1 = "/Images/All/star_full.png";
                            if (vote >= 3) rating_2 = "/Images/All/star_half.png";
                            if (vote >= 4) rating_2 = "/Images/All/star_full.png";
                            if (vote >= 5) rating_3 = "/Images/All/star_half.png";
                            if (vote >= 6) rating_3 = "/Images/All/star_full.png";
                        }
                        catch { }

                        box_accepted.Items.Add(new Data() { Id = v_id, UserId = v_user_id, UserImage = v_image, Username = v_username, Vote = v_user_vote_number, Rating1 = rating_1, Rating2 = rating_2, Rating3 = rating_3, Type = v_type });
                    }

                    box_accepted.Visibility = Visibility.Visible;
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

        void rss_offer_wait(object sender, DownloadStringCompletedEventArgs e)
        {
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                try
                {
                    box_waiting.Items.Clear();
                    var rss_information = from rss in XElement.Parse(e.Result).Descendants("item") select rss;
                    foreach (var name_information in rss_information)
                    {
                        string v_id = name_information.Element("id").Value;
                        string v_image = name_information.Element("user_image").Value;
                        string v_type = name_information.Element("type").Value;
                        string v_username = name_information.Element("username").Value;
                        string v_user_id = name_information.Element("user_id").Value;
                        string v_user_vote = name_information.Element("user_vote").Value;
                        string v_user_vote_number = name_information.Element("user_vote_number").Value;

                        string rating_1 = "", rating_2 = "", rating_3 = "";
                        try
                        {
                            int vote = int.Parse(v_user_vote);

                            rating_1 = "/Images/All/star_empty.png";
                            rating_2 = "/Images/All/star_empty.png";
                            rating_3 = "/Images/All/star_empty.png";
                            if (vote >= 1) rating_1 = "/Images/All/star_half.png";
                            if (vote >= 2) rating_1 = "/Images/All/star_full.png";
                            if (vote >= 3) rating_2 = "/Images/All/star_half.png";
                            if (vote >= 4) rating_2 = "/Images/All/star_full.png";
                            if (vote >= 5) rating_3 = "/Images/All/star_half.png";
                            if (vote >= 6) rating_3 = "/Images/All/star_full.png";
                        }
                        catch { }

                        box_waiting.Items.Add(new Data() { Id = v_id, UserId = v_user_id, UserImage = v_image, Username = v_username, Vote = v_user_vote_number, Rating1 = rating_1, Rating2 = rating_2, Rating3 = rating_3, Type = v_type });
                    }

                    box_waiting.Visibility = Visibility.Visible;
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

        void rss_offer_rejected(object sender, DownloadStringCompletedEventArgs e)
        {
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                try
                {
                    box_rejected.Items.Clear();

                    var rss_information = from rss in XElement.Parse(e.Result).Descendants("item") select rss;
                    foreach (var name_information in rss_information)
                    {
                        string v_id = name_information.Element("id").Value;
                        string v_image = name_information.Element("user_image").Value;
                        string v_type = name_information.Element("type").Value;
                        string v_username = name_information.Element("username").Value;
                        string v_user_id = name_information.Element("user_id").Value;
                        string v_user_vote = name_information.Element("user_vote").Value;
                        string v_user_vote_number = name_information.Element("user_vote_number").Value;

                        string rating_1 = "", rating_2 = "", rating_3 = "";
                        try
                        {
                            int vote = int.Parse(v_user_vote);

                            rating_1 = "/Images/All/star_empty.png";
                            rating_2 = "/Images/All/star_empty.png";
                            rating_3 = "/Images/All/star_empty.png";
                            if (vote >= 1) rating_1 = "/Images/All/star_half.png";
                            if (vote >= 2) rating_1 = "/Images/All/star_full.png";
                            if (vote >= 3) rating_2 = "/Images/All/star_half.png";
                            if (vote >= 4) rating_2 = "/Images/All/star_full.png";
                            if (vote >= 5) rating_3 = "/Images/All/star_half.png";
                            if (vote >= 6) rating_3 = "/Images/All/star_full.png";
                        }
                        catch { }

                        box_rejected.Items.Add(new Data() { Id = v_id, UserId = v_user_id, UserImage = v_image, Username = v_username, Vote = v_user_vote_number, Rating1 = rating_1, Rating2 = rating_2, Rating3 = rating_3, Type = v_type });
                    }
                    box_rejected.Visibility = Visibility.Visible;
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

        private void View_Box_Accepted(object sender, System.Windows.Input.GestureEventArgs e)
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

                box_accepted.Visibility = Visibility.Collapsed;
                box_waiting.Visibility = Visibility.Collapsed;
                box_rejected.Visibility = Visibility.Collapsed;

                WebClient rss = new WebClient();
                rss.DownloadStringCompleted += rss_offer_conf;
                rss.DownloadStringAsync(new Uri("YOURURL" + v_email + "&password=" + v_password + "&id=" + box_id.Text + "&type=1"));
            }
        }

        private void View_Box_Waiting(object sender, System.Windows.Input.GestureEventArgs e)
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

                box_accepted.Visibility = Visibility.Collapsed;
                box_waiting.Visibility = Visibility.Collapsed;
                box_rejected.Visibility = Visibility.Collapsed;

                WebClient rss = new WebClient();
                rss.DownloadStringCompleted += rss_offer_wait;
                rss.DownloadStringAsync(new Uri("YOURURL" + v_email + "&password=" + v_password + "&id=" + box_id.Text + "&type=0"));
            }
        }

        private void View_Box_Rejected(object sender, System.Windows.Input.GestureEventArgs e)
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

                box_accepted.Visibility = Visibility.Collapsed;
                box_waiting.Visibility = Visibility.Collapsed;
                box_rejected.Visibility = Visibility.Collapsed;

                WebClient rss = new WebClient();
                rss.DownloadStringCompleted += rss_offer_rejected;
                rss.DownloadStringAsync(new Uri("YOURURL" + v_email + "&password=" + v_password + "&id=" + box_id.Text + "&type=-1"));
            }
        }

        private void View_ChatPage(object sender, RoutedEventArgs e)
        {
            string return_message = AppResources.MessageNoReady.ToString();
            MessageBox.Show(return_message);
        }

        private void Click_Action(object sender, RoutedEventArgs e)
        {
            var button = sender as HyperlinkButton;

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
                rss_login.DownloadStringAsync(new Uri("YOURURL" + v_email + "&password=" + v_password + "&type=ModifyAnRequest&id=" + button.DataContext.ToString() + "&value=" + button.CommandParameter.ToString()));
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

                        if (message == "0x000011")
                        {
                            string return_message = AppResources.MessageUserStatusChange.ToString();
                            MessageBox.Show(return_message);
                        }
                        if (message == "0x000012")
                        {
                            string return_message = AppResources.MessageUserStatusChangeError.ToString();
                            MessageBox.Show(return_message);
                        }

                        string v_email = "", v_password = "";

                        try
                        {
                            v_email = IsolatedStorageSettings.ApplicationSettings["email"].ToString();
                            v_password = IsolatedStorageSettings.ApplicationSettings["password"].ToString();
                        }
                        catch { }

                        if (box_accepted.Visibility.ToString() == "Visible")
                        {
                            WebClient rss = new WebClient();
                            rss.DownloadStringCompleted += rss_offer_conf;
                            rss.DownloadStringAsync(new Uri("YOURURL" + v_email + "&password=" + v_password + "&id=" + box_id.Text + "&type=1"));
                        }
                        else
                        {
                            if (box_waiting.Visibility.ToString() == "Visible")
                            {
                                WebClient rss = new WebClient();
                                rss.DownloadStringCompleted += rss_offer_wait;
                                rss.DownloadStringAsync(new Uri("YOURURL" + v_email + "&password=" + v_password + "&id=" + box_id.Text + "&type=0"));
                            }
                            else
                            {
                                WebClient rss = new WebClient();
                                rss.DownloadStringCompleted += rss_offer_rejected;
                                rss.DownloadStringAsync(new Uri("YOURURL" + v_email + "&password=" + v_password + "&id=" + box_id.Text + "&type=-1"));
                            }
                        }

                        box_loading.Visibility = Visibility.Visible;

                        box_accepted.Visibility = Visibility.Collapsed;
                        box_waiting.Visibility = Visibility.Collapsed;
                        box_rejected.Visibility = Visibility.Collapsed;

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

    }
}