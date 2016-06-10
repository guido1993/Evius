using System;
using System.Net;
using System.Windows;
using Microsoft.Phone.Controls;

namespace Evius.Views
{
    public partial class LoginPage : PhoneApplicationPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void Click_Login(object sender, RoutedEventArgs e)
        {
            //Skipping any login server check
            NavigationService.Navigate(new Uri("/Views/HomePage.xaml", UriKind.Relative));


            //string v_email = box_email.Text;
            //string v_password = box_password.Password;

            //if (NetworkInterface.GetIsNetworkAvailable())
            //{
            //    box_loading.Visibility = System.Windows.Visibility.Visible;

            //    WebClient rss_login = new WebClient();
            //    rss_login.DownloadStringCompleted += new DownloadStringCompletedEventHandler(rss_lab);
            //    rss_login.DownloadStringAsync(new Uri("YOURURI" + v_email + "&password=" + v_password));
            //}

        }

        void rss_lab(object sender, DownloadStringCompletedEventArgs e)
        {
            //    if (NetworkInterface.GetIsNetworkAvailable())
            //    {
            //        try
            //        {
            //            var rssData = from rss in XElement.Parse(e.Result).Descendants("return") select rss;
            //            foreach (var name in rssData)
            //            {
            //                string message = name.Element("message").Value;
            //                string id = name.Element("id").Value;

            //                if (message == "0x000001")
            //                {
            //                    string v_email = box_email.Text;
            //                    string v_password = box_password.Password;

            //                    IsolatedStorageSettings.ApplicationSettings["email"] = v_email;
            //                    IsolatedStorageSettings.ApplicationSettings["password"] = v_password;
            //                    IsolatedStorageSettings.ApplicationSettings["id"] = id;
            //                    IsolatedStorageSettings.ApplicationSettings.Save();

            //                    NavigationService.Navigate(new Uri("/Views/HomePage.xaml", UriKind.Relative));

            //                }
            //                else
            //                {
            //                    if (message == "0x000002")
            //                    {
            //                        string return_message = AppResources.MessageInvalidData.ToString();
            //                        MessageBox.Show(return_message);
            //                    }

            //                }
            //            }
            //        }
            //        catch (TargetInvocationException ex)
            //        {
            //            string return_message = AppResources.MessageNoData.ToString();
            //            MessageBox.Show(return_message + ex.Message.ToString());
            //        }
            //    }
            //    else
            //    {
            //        string return_message = AppResources.MessageNoInternet.ToString();
            //        MessageBox.Show(return_message);
            //    }

            //    box_loading.Visibility = System.Windows.Visibility.Collapsed;
            //}
        }
    }
}