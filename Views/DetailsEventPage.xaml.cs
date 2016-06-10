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
using System.Reflection;
using System.Xml.Linq;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Net.NetworkInformation;
using Evius.Resources;
using System.Device.Location;
using evius;

namespace Evius.Views
{
    public partial class DetailsEventPage : PhoneApplicationPage
    {
        public DetailsEventPage()
        {
            InitializeComponent();
        }

       protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            string id = NavigationContext.QueryString["msg"];
            base.OnNavigatedTo(e);

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
                rss.DownloadStringCompleted += rss_lab_event;
                rss.DownloadStringAsync(new Uri("YOURURI" + v_email + "&password=" + v_password + "&id=" + id));
            }

        }

       void rss_lab_event(object sender, DownloadStringCompletedEventArgs e)
       {
           if (NetworkInterface.GetIsNetworkAvailable())
           {
               try
               {
                   var rss_information = from rss in XElement.Parse(e.Result).Descendants("item") select rss;
                   foreach (var name_information in rss_information)
                   {
                       string v_id = name_information.Element("id").Value;
                       string v_name = name_information.Element("event_name").Value;
                       string v_event_image = name_information.Element("event_image").Value;
                       string v_latitude = name_information.Element("latitude").Value;
                       string v_longitude = name_information.Element("longitude").Value;
                       string v_date_start = name_information.Element("date_start").Value;
                       string v_date_end = name_information.Element("date_end").Value;
                       string v_info = name_information.Element("info").Value;
                       string v_follower = name_information.Element("follower").Value;
                       string v_settings = name_information.Element("settings").Value;
                       string v_type = name_information.Element("type").Value;

                       try
                       {
                           double d_latitude = double.Parse(v_latitude.Replace(".", ","));
                           double d_longitude = double.Parse(v_longitude.Replace(".", ","));

                           GeoCoordinate Map = new GeoCoordinate(d_latitude, d_longitude);
                           box_map.Center = Map;
                       }
                       catch
                       {
                           GeoCoordinate Map = new GeoCoordinate(0.0000, 0.0000);
                           box_map.Center = Map;
                       }

                       if (v_settings == "1")
                       {
                           box_follow.Text = AppResources.TextDetailsEventNoFollow.ToString();
                           box_action.Visibility = Visibility.Visible;
                       }
                       if (v_settings == "-1")
                       {
                           box_follow.Text = AppResources.TextDetailsEventFollow.ToString();
                           box_action.Visibility = Visibility.Collapsed;
                       }

                       box_id.Text = v_id;
                       box_name.Text = v_name;
                       box_image.ImageSource = new BitmapImage(new Uri(v_event_image, UriKind.Absolute));
                       box_follower.Text = v_follower;
                       box_info.Text = v_info;
                       box_date_start.Text = v_date_start;
                       box_date_end.Text = v_date_end;

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

       void rss_lab_offer(object sender, DownloadStringCompletedEventArgs e)
       {
           if (NetworkInterface.GetIsNetworkAvailable())
           {
               try
               {
                   var rss_information = from rss in XElement.Parse(e.Result).Descendants("item") select rss;
                   foreach (var name_information in rss_information)
                   {
                       string v_limited = name_information.Element("limited").Value;
                       string v_info = name_information.Element("info").Value;
                       string v_city = name_information.Element("city").Value;

                       box_new_limited.Text = v_limited;
                       box_new_info.Text = v_info;
                       box_new_city.Text = v_city;

                       box_change.IsPopupOpen = true;
                       box_offer_change.Visibility = Visibility.Visible;
                       box_request_change.Visibility = Visibility.Collapsed;

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

       void rss_lab_request(object sender, DownloadStringCompletedEventArgs e)
       {
           if (NetworkInterface.GetIsNetworkAvailable())
           {
               try
               {
                   box_request.Items.Clear();

                   var rss_information = from rss in XElement.Parse(e.Result).Descendants("item") select rss;
                   foreach (var name_information in rss_information)
                   {
                       string v_id = name_information.Element("id").Value;
                       string v_user_id = name_information.Element("user_id").Value;
                       string v_username = name_information.Element("username").Value;
                       string v_user_image = name_information.Element("user_image").Value;
                       string v_info = name_information.Element("info").Value;
                       string v_city = name_information.Element("city").Value;
                       string v_settings = name_information.Element("settings").Value;
                       string v_type = name_information.Element("type").Value;

                       if (v_settings == "1") v_settings = AppResources.TextDetailsEventRemoveRequest.ToString();
                       if (v_settings == "-1") v_settings = AppResources.TextDetailsEventMakeRequest.ToString();

                       box_request.Items.Add(new Data() { Id = v_id, UserId = v_user_id, Username = v_username, UserImage = v_user_image, Info = v_info, City = v_city, Title = v_settings, Type = v_type });
                   }

                   box_change.IsPopupOpen = true;
                   box_offer_change.Visibility = Visibility.Collapsed;
                   box_request_change.Visibility = Visibility.Visible;

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

       private void Click_Follow(object sender, RoutedEventArgs e)
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
               rss_login.DownloadStringAsync(new Uri("YOURURI" + v_email + "&password=" + v_password + "&type=FollowAndUnfollowEvent&id=" + box_id.Text));
           }

       }

       private void Click_Request(object sender, RoutedEventArgs e)
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
               rss_login.DownloadStringAsync(new Uri("YOURURI" + v_email + "&password=" + v_password + "&type=RequiredOnOffert&id=" + button.CommandParameter.ToString()));
           
           }

       }

       private void Click_Offer(object sender, RoutedEventArgs e)
       {

           string v_email = "", v_password = "";

           try
           {
               v_email = IsolatedStorageSettings.ApplicationSettings["email"].ToString();
               v_password = IsolatedStorageSettings.ApplicationSettings["password"].ToString();
           }
           catch { }

           string v_limited = box_new_limited.Text;
           string v_info = box_new_info.Text;
           string v_city = box_new_city.Text;

           if (NetworkInterface.GetIsNetworkAvailable())
           {
               box_loading.Visibility = Visibility.Visible;

               WebClient rss_login = new WebClient();
               rss_login.DownloadStringCompleted += rss_lab;
               rss_login.DownloadStringAsync(new Uri("YOURURI" + v_email + "&password=" + v_password + "&type=ModifyAnOffert&id=" + box_id.Text + "&limited=" + v_limited + "&info=" + v_info + "&city=" + v_city));

           }

       }

        void rss_lab(object sender, DownloadStringCompletedEventArgs e)
        {
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                try
                {
                    var rss_information = from rss in XElement.Parse(e.Result).Descendants("return") select rss;
                    foreach (var name_information in rss_information)
                    {
                        string message = name_information.Element("message").Value;
                        string return_message = "";

                        string v_email = "", v_password = "";

                        try
                        {
                            v_email = IsolatedStorageSettings.ApplicationSettings["email"].ToString();
                            v_password = IsolatedStorageSettings.ApplicationSettings["password"].ToString();
                        }
                        catch { }

                        if (message == "0x000005")
                        {
                            return_message = AppResources.MessageEventRemove.ToString();
                            MessageBox.Show(return_message);

                            if (NetworkInterface.GetIsNetworkAvailable())
                            {
                                box_loading.Visibility = Visibility.Visible;

                                WebClient rss = new WebClient();
                                rss.DownloadStringCompleted += rss_lab_event;
                                rss.DownloadStringAsync(new Uri("YOURURI" + v_email + "&password=" + v_password + "&id=" + box_id.Text));
                            }

                        }

                        if (message == "0x000006")
                        {
                            return_message = AppResources.MessageEventAdd.ToString();
                            MessageBox.Show(return_message);

                            if (NetworkInterface.GetIsNetworkAvailable())
                            {
                                box_loading.Visibility = Visibility.Visible;

                                WebClient rss = new WebClient();
                                rss.DownloadStringCompleted += rss_lab_event;
                                rss.DownloadStringAsync(new Uri("YOURURL" + v_email + "&password=" + v_password + "&id=" + box_id.Text));
                            }
                        }

                        if (message == "0x000007")
                        {
                            return_message = AppResources.MessageRequiredOnOffertAdd.ToString();
                            MessageBox.Show(return_message);

                            if (NetworkInterface.GetIsNetworkAvailable())
                            {
                                box_loading.Visibility = Visibility.Visible;

                                WebClient rss_login = new WebClient();
                                rss_login.DownloadStringCompleted += rss_lab_request;
                                rss_login.DownloadStringAsync(new Uri("YOURURI" + v_email + "&password=" + v_password + "&id=" + box_id.Text));

                            }

                        }

                        if (message == "0x000008")
                        {
                            return_message = AppResources.MessageRequiredOnOffertRemove.ToString();
                            MessageBox.Show(return_message);

                            if (NetworkInterface.GetIsNetworkAvailable())
                            {
                                box_loading.Visibility = Visibility.Visible;

                                WebClient rss_login = new WebClient();
                                rss_login.DownloadStringCompleted += rss_lab_request;
                                rss_login.DownloadStringAsync(new Uri("YOURURI" + v_email + "&password=" + v_password + "&id=" + box_id.Text));

                            }

                        }

                        if (message == "0x000009")
                        {
                            return_message = AppResources.MessageModifyAnOffertSaved.ToString();
                            MessageBox.Show(return_message);

                            if (NetworkInterface.GetIsNetworkAvailable())
                            {
                                box_loading.Visibility = Visibility.Visible;

                                WebClient rss_login = new WebClient();
                                rss_login.DownloadStringCompleted += rss_lab_offer;
                                rss_login.DownloadStringAsync(new Uri("YOURURI" + v_email + "&password=" + v_password + "&id=" + box_id.Text));

                            }

                        }

                        if (message == "0x000010")
                        {
                            return_message = AppResources.MessageModifyAnOffertError.ToString();
                            MessageBox.Show(return_message);

                            if (NetworkInterface.GetIsNetworkAvailable())
                            {
                                box_loading.Visibility = Visibility.Visible;

                                WebClient rss_login = new WebClient();
                                rss_login.DownloadStringCompleted += rss_lab_offer;
                                rss_login.DownloadStringAsync(new Uri("YOURURI" + v_email + "&password=" + v_password + "&id=" + box_id.Text));

                            }

                        }

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

        private void View_Box_Offer(object sender, System.Windows.Input.GestureEventArgs e)
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
                rss_login.DownloadStringCompleted += rss_lab_offer;
                rss_login.DownloadStringAsync(new Uri("YOURURL" + v_email + "&password=" + v_password + "&id=" + box_id.Text));

            }

        }

        private void View_Box_Request(object sender, System.Windows.Input.GestureEventArgs e)
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
                rss_login.DownloadStringCompleted += rss_lab_request;
                rss_login.DownloadStringAsync(new Uri("YOURURL" + v_email + "&password=" + v_password + "&id=" + box_id.Text));

            }
        }

    }
}