using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Xml.Linq;
using System.IO.IsolatedStorage;
using System.Reflection;
using System.Net.NetworkInformation;
using Evius.Resources;

namespace Evius.Views
{
    public partial class RegistrationPage : PhoneApplicationPage
    {
        public RegistrationPage()
        {
            InitializeComponent();
        }

        private void Click_Registration(object sender, RoutedEventArgs e)
        {
            string v_email = box_email.Text;
            string v_ripeat_email = box_repeat_email.Text;
            string v_password = box_password.Password;
            string v_username = box_username.Text;

            if (NetworkInterface.GetIsNetworkAvailable())
            {
                box_loading.Visibility = Visibility.Visible;

                WebClient rss_login = new WebClient();
                rss_login.DownloadStringCompleted += rss_lab;
                rss_login.DownloadStringAsync(new Uri("YOURURL" + v_email + "&repeat_email=" + v_ripeat_email + "&password=" + v_password + "&username=" + v_username));
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

                        if (message == "0x000017")
                        {
                            string return_message = AppResources.MessageRegistrationOK.ToString();
                            MessageBox.Show(return_message);
                        }
                        else
                        {
                            if (message == "0x000002")
                            {
                                string return_message = AppResources.MessageInvalidData.ToString();
                                MessageBox.Show(return_message);
                            }

                            if (message == "0x000015")
                            {
                                string return_message = AppResources.MessageEmailUsed.ToString();
                                MessageBox.Show(return_message);
                            }

                            if (message == "0x000016")
                            {
                                string return_message = AppResources.MessageUsernameUsed.ToString();
                                MessageBox.Show(return_message);
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