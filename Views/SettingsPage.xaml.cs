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
    public partial class SettingsPage : PhoneApplicationPage
    {
        public SettingsPage()
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

            if (box_panorama.SelectedIndex == 0)
            {

                if (NetworkInterface.GetIsNetworkAvailable())
                {
                    box_loading.Visibility = Visibility.Visible;

                    WebClient rss = new WebClient();
                    rss.DownloadStringCompleted += rss_lab_settings;
                    rss.DownloadStringAsync(new Uri("YOURURL" + v_email + "&password=" + v_password));
                }
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            string v_email = "", v_password = "", v_notification = "", v_position = "";

            v_notification = IsolatedStorageSettings.ApplicationSettings["SettingsNotification"].ToString();
            v_position = IsolatedStorageSettings.ApplicationSettings["SettingsPosition"].ToString();

            if (v_notification == "1") SettingsNotification.IsChecked = true;
            if (v_position == "1") SettingsPosition.IsChecked = true;

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
                rss.DownloadStringCompleted += rss_lab_settings;
                rss.DownloadStringAsync(new Uri("YOURURL" + v_email + "&password=" + v_password));
            }
        }

        void rss_lab_settings(object sender, DownloadStringCompletedEventArgs e)
        {
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                try
                {
                    var rss_information = from rss in XElement.Parse(e.Result).Descendants("item") select rss;
                    foreach (var name_information in rss_information)
                    {
                        string v_view_email = name_information.Element("view_email").Value;
                        string v_view_telephone = name_information.Element("view_telephone").Value;
                        string v_view_born = name_information.Element("view_born").Value;
                        string v_username = name_information.Element("user_name").Value;
                        string v_born = name_information.Element("user_born").Value;
                        string v_email = name_information.Element("user_email").Value;
                        string v_telephone = name_information.Element("user_telephone").Value;

                        if (v_view_email == "1") ChangeViewEmail.IsChecked = true;
                        if (v_view_telephone == "1") ChangeViewTelephone.IsChecked = true;
                        if (v_view_born == "1") ChangeViewBorn.IsChecked = true;

                        box_username.Text = v_username;
                        box_born.Text = v_born;
                        box_email.Text = v_email;
                        box_telephone.Text = v_telephone;
                    }

                    box_panorama.Visibility = Visibility.Visible;

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

        private void Click_ToggleSwitch(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var button = sender as ToggleSwitch;
            string type = button.Name.ToString();

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
                rss.DownloadStringCompleted += rss_lab;
                rss.DownloadStringAsync(new Uri("YOURURL" + v_email + "&password=" + v_password + "&type=" + type));
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

                        if (message == "0x000013") { 
                            return_message = AppResources.MessageSettingsEnable.ToString();
                            MessageBox.Show(return_message);
                        }

                        if (message == "0x000014") { 
                            return_message = AppResources.MessageSettingsDisable.ToString();
                            MessageBox.Show(return_message);
                        }

                        if (message == "0x000018") { 
                            return_message = AppResources.MessageChangePasswordOldError.ToString();
                            MessageBox.Show(return_message);
                        }

                        if (message == "0x000019") { 
                            return_message = AppResources.MessageChangePasswordNewError.ToString();
                            MessageBox.Show(return_message);
                        }

                        if (message == "0x000020")
                        {
                            return_message = AppResources.MessageChangePasswordOk.ToString();
                            MessageBox.Show(return_message);
                        }

                        if (message == "0x000021")
                        {
                            return_message = AppResources.MessageChangeBornDateOk.ToString();
                            MessageBox.Show(return_message);

                            box_born.Text = box_date.ValueString.ToString();
                        }

                        if (message == "0x000022")
                        {
                            return_message = AppResources.TextSettingsNewTelephone.ToString();
                            MessageBox.Show(return_message);

                            box_telephone.Text = box_new_telephone.Text;

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

        private void Click_App_ToggleSwitch(object sender, System.Windows.Input.GestureEventArgs e)
        {
            box_loading.Visibility = Visibility.Visible;

            var button = sender as ToggleSwitch;
            string type = button.Name.ToString();

            string value = button.IsChecked.ToString();
            if (value == "False") value = "0";
            if (value == "True") value = "1";

            IsolatedStorageSettings.ApplicationSettings[type] = value;
            IsolatedStorageSettings.ApplicationSettings.Save();

            box_loading.Visibility = Visibility.Collapsed;

        }

        private void View_Box_Password(object sender, System.Windows.Input.GestureEventArgs e)
        {
            box_change.IsPopupOpen = true;
            box_password_change.Visibility = Visibility.Visible;
            box_born_change.Visibility = Visibility.Collapsed;
            box_about_change.Visibility = Visibility.Collapsed;
            box_telephone_change.Visibility = Visibility.Collapsed;
        }

        private void View_Box_Born(object sender, System.Windows.Input.GestureEventArgs e)
        {
            box_change.IsPopupOpen = true;
            box_password_change.Visibility = Visibility.Collapsed;
            box_born_change.Visibility = Visibility.Visible;
            box_about_change.Visibility = Visibility.Collapsed;
            box_telephone_change.Visibility = Visibility.Collapsed;
        }

        private void View_Box_About(object sender, System.Windows.Input.GestureEventArgs e)
        {
            box_change.IsPopupOpen = true;
            box_password_change.Visibility = Visibility.Collapsed;
            box_born_change.Visibility = Visibility.Collapsed;
            box_about_change.Visibility = Visibility.Visible;
            box_telephone_change.Visibility = Visibility.Collapsed;
        }

        private void View_Box_Telephone(object sender, System.Windows.Input.GestureEventArgs e)
        {
            box_change.IsPopupOpen = true;
            box_password_change.Visibility = Visibility.Collapsed;
            box_born_change.Visibility = Visibility.Collapsed;
            box_about_change.Visibility = Visibility.Collapsed;
            box_telephone_change.Visibility = Visibility.Visible;
        }

        private void Click_Change_Password(object sender, RoutedEventArgs e)
        {
            string v_old_password = box_old_password.Password;
            string v_new_password = box_new_password.Password;
            string v_repeat_new_password = box_repeat_new_password.Password;

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
                rss_login.DownloadStringAsync(new Uri("YOURURL" + v_email + "&password=" + v_password + "&type=ChangePassword&old_password=" + v_old_password + "&new_password=" + v_new_password + "&repeat_new_password=" + v_repeat_new_password));
            }

        }

        private void Click_Change_Born(object sender, RoutedEventArgs e)
        {
            string v_date = box_date.ValueString.ToString();

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
                rss_login.DownloadStringAsync(new Uri("YOURURL" + v_email + "&password=" + v_password + "&type=ChangeBornDate&value=" + v_date));
            }

        }

        private void Click_Change_Telephone(object sender, RoutedEventArgs e)
        {
            string v_telephone = box_new_telephone.Text;

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
                rss_login.DownloadStringAsync(new Uri("YOURURL" + v_email + "&password=" + v_password + "&type=ChangeTelephone&value=" + v_telephone));
            }

        }

    }
}