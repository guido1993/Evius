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
using System.Net.NetworkInformation;
using Evius.Resources;

namespace Evius.Views
{

    public partial class ManageDrivePage : PhoneApplicationPage
    {
        public ManageDrivePage()
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
                    if (box_offered.Items.Count.ToString() == "0")
                    {
                        box_loading.Visibility = Visibility.Visible;

                        WebClient rss = new WebClient();
                        rss.DownloadStringCompleted += rss_offered;
                        rss.DownloadStringAsync(new Uri("YOURURL" + v_email + "&password=" + v_password));
                    }
                }

                if (box_panorama.SelectedIndex == 1)
                {
                    if (box_request.Items.Count.ToString() == "0")
                    {
                        box_loading.Visibility = Visibility.Visible;

                        WebClient rss = new WebClient();
                        rss.DownloadStringCompleted += rss_requested;
                        rss.DownloadStringAsync(new Uri("YOURURL" + v_email + "&password=" + v_password));
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

            if (NetworkInterface.GetIsNetworkAvailable())
            {
                box_loading.Visibility = Visibility.Visible;

                WebClient rss = new WebClient();
                rss.DownloadStringCompleted += rss_offered;
                rss.DownloadStringAsync(new Uri("YOURURL" + v_email + "&password=" + v_password));
            }
        }

        void rss_offered(object sender, DownloadStringCompletedEventArgs e)
        {
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                try
                {
                    box_offered.Items.Clear();

                    var rss_information = from rss in XElement.Parse(e.Result).Descendants("item") select rss;
                    foreach (var name_information in rss_information)
                    {
                        string v_id = name_information.Element("id").Value;
                        string v_image = name_information.Element("event_image").Value;
                        string v_confermati = name_information.Element("event_conf").Value;
                        string v_waiting = name_information.Element("event_wait").Value;
                        string v_type = name_information.Element("type").Value;

                        box_offered.Items.Add(new Data() { Id = v_id, Image = v_image, Confermati = v_confermati, Waiting = v_waiting, Type =v_type });
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

        void rss_requested(object sender, DownloadStringCompletedEventArgs e)
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
                        string v_image = name_information.Element("event_image").Value;
                        string v_user_image = name_information.Element("user_image").Value;
                        string v_confermato = name_information.Element("drive_conf").Value;
                        string v_event_date = name_information.Element("event_date").Value;
                        string v_type = name_information.Element("type").Value;
                        string v_event_name = name_information.Element("event_name").Value;

                        if (v_confermato == "-1")
                            v_confermato = "refused :(";
                        else if (v_confermato == "0")
                            v_confermato = "waiting ...";
                        else
                            v_confermato = "confirmed :)";

                        box_request.Items.Add(new Data() { Id = v_id, Image = v_image, Confermato = v_confermato, Waiting = v_event_date, Type = v_type, UserImage = v_user_image, Name = v_event_name });
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

        private void View_OfferPage(object sender, RoutedEventArgs e)
        {
            var button = sender as HyperlinkButton;
            NavigationService.Navigate(new Uri("/Views/OfferPage.xaml?msg=" + button.CommandParameter.ToString(), UriKind.Relative));
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
                rss_login.DownloadStringAsync(new Uri("YOURURL" + v_email + "&password=" + v_password + "&type=RequiredOnOffert&id=" + button.CommandParameter.ToString()));

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

                        if (message == "0x000008")
                        {
                            return_message = AppResources.MessageRequiredOnOffertRemove.ToString();
                            MessageBox.Show(return_message);
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

                            if (box_panorama.SelectedIndex == 0)
                            {
                                box_loading.Visibility = Visibility.Visible;

                                WebClient rss = new WebClient();
                                rss.DownloadStringCompleted += rss_offered;
                                rss.DownloadStringAsync(new Uri("YOURURL" + v_email + "&password=" + v_password));
                            }

                            if (box_panorama.SelectedIndex == 1)
                            {
                                box_loading.Visibility = Visibility.Visible;

                                WebClient rss = new WebClient();
                                rss.DownloadStringCompleted += rss_requested;
                                rss.DownloadStringAsync(new Uri("YOURURL" + v_email + "&password=" + v_password));
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
    }
}