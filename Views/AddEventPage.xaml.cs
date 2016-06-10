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

namespace Evius.Views
{
    public partial class AddEventPage : PhoneApplicationPage
    {
        public AddEventPage()
        {
            InitializeComponent();
        }

        private void Click_Login(object sender, RoutedEventArgs e)
        {
            string return_message = AppResources.MessageNoReady.ToString();
            MessageBox.Show(return_message);
        }

    }
}