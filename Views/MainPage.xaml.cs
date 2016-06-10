using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Evius.Resources;
using System.IO.IsolatedStorage;

namespace Evius
{
    public partial class MainPage : PhoneApplicationPage
    {

        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            if (NavigationService.CanGoBack)
            {
                NavigationService.RemoveBackEntry();
                return;
            }

            string v_email = ""; string v_password = "";

            try
            {
                v_email = IsolatedStorageSettings.ApplicationSettings["email"].ToString();
                v_password = IsolatedStorageSettings.ApplicationSettings["password"].ToString();
                
                if ((v_email != "") && (v_password != ""))
                {
                    NavigationService.Navigate(new Uri("/Views/HomePage.xaml", UriKind.Relative));
                }
                else
                {
                    IsolatedStorageSettings.ApplicationSettings["email"] = "";
                    IsolatedStorageSettings.ApplicationSettings["password"] = "";
                    IsolatedStorageSettings.ApplicationSettings["id"] = "";
                    IsolatedStorageSettings.ApplicationSettings["SettingsNotification"] = "";
                    IsolatedStorageSettings.ApplicationSettings["SettingsPosition"] = "";
                    IsolatedStorageSettings.ApplicationSettings.Save();
                }
            }
            catch {
                IsolatedStorageSettings.ApplicationSettings["email"] = "";
                IsolatedStorageSettings.ApplicationSettings["password"] = "";
                IsolatedStorageSettings.ApplicationSettings["id"] = "";
                IsolatedStorageSettings.ApplicationSettings["SettingsNotification"] = "";
                IsolatedStorageSettings.ApplicationSettings["SettingsPosition"] = "";
                IsolatedStorageSettings.ApplicationSettings.Save();
            }

        }

        private void View_LoginPage(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/LoginPage.xaml", UriKind.Relative));
        }

        private void View_RegistrationPage(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/RegistrationPage.xaml", UriKind.Relative));
        }

    }
}