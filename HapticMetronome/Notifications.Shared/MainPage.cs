/*
    Copyright (c) Microsoft Corporation All rights reserved.  
 
    MIT License: 
 
    Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
    documentation files (the  "Software"), to deal in the Software without restriction, including without limitation
    the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software,
    and to permit persons to whom the Software is furnished to do so, subject to the following conditions: 
 
    The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software. 
 
    THE SOFTWARE IS PROVIDED ""AS IS"", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
    TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
    TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

using Microsoft.Band;
using Microsoft.Band.Notifications;
using Microsoft.Band.Tiles;
using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;
using System.Threading;
using Windows.UI.Xaml.Controls;
using Windows.UI.Popups;
using Windows.UI.Xaml.Media; 

namespace HapticMetronome
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    partial class MainPage
    {
        private App viewModel;
        private DispatcherTimer metDelay;
        private int intMetDelay = 120;
        private static string toneSelect = "OneTone";

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            try {
                if (Convert.ToInt16(textBox.Text) > 0 && Convert.ToInt16(textBox.Text) < 301)
                {
                    intMetDelay = Convert.ToInt16(textBox.Text);
                    if (metDelay != null)
                    {
                        metDelay.Interval = new TimeSpan(0, 0, 0, 0, 60000 / intMetDelay);
                    }
                }
                else
                {
                    textBox.Text = intMetDelay.ToString();
                    MessageDialog msgbox = new MessageDialog("Please input a tempo from 1 to 300 bpm.");
                    msgbox.ShowAsync(); 
                }
            }
            catch (FormatException)
            {
                textBox.Text = intMetDelay.ToString();
                MessageDialog msgbox = new MessageDialog("Please input a valid tempo.");
                //Calling the Show method of MessageDialog class  
                //which will show the MessageBox  
                msgbox.ShowAsync(); 
            }
        }

        private void startButtonClick(object sender, RoutedEventArgs e)
        {
            //Start and Stop Metronome
            if (startButton.Content.Equals("START"))
            {
                startButton.Content = "STOP";
                try
                {
                    metDelay = new DispatcherTimer();
                    metDelay.Tick += metDelay_Tick; // Add event handler
                    metDelay.Interval = new TimeSpan(0, 0, 0, 0, 60000/intMetDelay);
                    metDelay.Start();
                }
                catch (Exception ex)
                {
                    this.viewModel.StatusMessage = ex.ToString();
                }
            }
            else
            {
                startButton.Content = "START";
                metDelay.Stop();
                metDelay.Tick -= metDelay_Tick; //remove event handler
            }
        }
        private void blackOut()
        {
            SolidColorBrush scb = new SolidColorBrush();
            oneToneButton.Background = scb;
            twoToneButton.Background = scb;
            oneToneHButton.Background = scb;
            twoToneHButton.Background = scb;
            threeToneHButton.Background = scb;
            alarmButton.Background = scb;
            timerButton.Background = scb;
            rampUpButton.Background = scb;
            rampDownButton.Background = scb;
        }
        private async void OneTone(object sender, RoutedEventArgs e)
        {
            try
            {
                // Get the list of Microsoft Bands paired to the phone.
                IBandInfo[] pairedBands = await BandClientManager.Instance.GetBandsAsync();
                // Connect to Microsoft Band.
                using (IBandClient bandClient = await BandClientManager.Instance.ConnectAsync(pairedBands[0]))
                {
                    //Set the toneSelector
                    toneSelect = "OneTone";
                    // send a vibration request of type alert alarm to the Band
                    await bandClient.NotificationManager.VibrateAsync(VibrationType.NotificationOneTone);
                    //indicate which tone is selected
                    blackOut();
                    oneToneButton.Background = Resources["PhoneAccentBrush"] as SolidColorBrush;
                }
            }
            catch (Exception ex)
            {
                this.viewModel.StatusMessage = ex.ToString();
            }
        }

        async void metDelay_Tick(object sender, object e)
        {
            try
            {
                switch (toneSelect)
                {

                    case "OneTone": OneTone(new object(), new RoutedEventArgs()); break;
                    case "TwoTone": TwoTone(new object(), new RoutedEventArgs()); break;
                    case "Alarm": Alarm(new object(), new RoutedEventArgs()); break;
                    case "Timer": Timer(new object(), new RoutedEventArgs()); break;
                    case "OneToneH": OneToneH(new object(), new RoutedEventArgs()); break;
                    case "ThreeToneH": ThreeToneH(new object(), new RoutedEventArgs()); break;
                    case "TwoToneH": TwoToneH(new object(), new RoutedEventArgs()); break;
                    case "RampUp": RampUp(new object(), new RoutedEventArgs()); break;
                    case "RampDown": RampDown(new object(), new RoutedEventArgs()); break;
                }
            }
            catch (AggregateException)
            { 
            }
        }

        private async void Alarm(object sender, RoutedEventArgs e)
        {
            try
            {
                // Get the list of Microsoft Bands paired to the phone.
                IBandInfo[] pairedBands = await BandClientManager.Instance.GetBandsAsync();
                // Connect to Microsoft Band.
                using (IBandClient bandClient = await BandClientManager.Instance.ConnectAsync(pairedBands[0]))
                {
                    //Set the toneSelector
                    toneSelect = "Alarm";
                    // send a vibration request of type alert alarm to the Band
                    await bandClient.NotificationManager.VibrateAsync(VibrationType.NotificationAlarm);
                    //indicate which tone is selected
                    blackOut();
                    alarmButton.Background = Resources["PhoneAccentBrush"] as SolidColorBrush;
                }
            }
            catch (Exception ex)
            {
                this.viewModel.StatusMessage = ex.ToString();
            }
        }
        private async void Timer(object sender, RoutedEventArgs e)
        {
            try
            {
                // Get the list of Microsoft Bands paired to the phone.
                IBandInfo[] pairedBands = await BandClientManager.Instance.GetBandsAsync();
                // Connect to Microsoft Band.
                using (IBandClient bandClient = await BandClientManager.Instance.ConnectAsync(pairedBands[0]))
                {
                    //Set the toneSelector
                    toneSelect = "Timer";
                    // send a vibration request of type alert alarm to the Band
                    await bandClient.NotificationManager.VibrateAsync(VibrationType.NotificationTimer);
                    //indicate which tone is selected
                    blackOut();
                    timerButton.Background = Resources["PhoneAccentBrush"] as SolidColorBrush;
                }
            }
            catch (Exception ex)
            {
                this.viewModel.StatusMessage = ex.ToString();
            }
        }
        private async void TwoTone(object sender, RoutedEventArgs e)
        {
            try
            {
                // Get the list of Microsoft Bands paired to the phone.
                IBandInfo[] pairedBands = await BandClientManager.Instance.GetBandsAsync();
                // Connect to Microsoft Band.
                using (IBandClient bandClient = await BandClientManager.Instance.ConnectAsync(pairedBands[0]))
                {
                    //Set the toneSelector
                    toneSelect = "TwoTone";
                    // send a vibration request of type alert alarm to the Band
                    await bandClient.NotificationManager.VibrateAsync(VibrationType.NotificationTwoTone);
                    //indicate which tone is selected
                    blackOut();
                    twoToneButton.Background = Resources["PhoneAccentBrush"] as SolidColorBrush;
                }
            }
            catch (Exception ex)
            {
                this.viewModel.StatusMessage = ex.ToString();
            }
        }
        private async void OneToneH(object sender, RoutedEventArgs e)
        {
            try
            {
                // Get the list of Microsoft Bands paired to the phone.
                IBandInfo[] pairedBands = await BandClientManager.Instance.GetBandsAsync();
                // Connect to Microsoft Band.
                using (IBandClient bandClient = await BandClientManager.Instance.ConnectAsync(pairedBands[0]))
                {
                    //Set the toneSelector
                    toneSelect = "OneToneH";
                    // send a vibration request of type alert alarm to the Band
                    await bandClient.NotificationManager.VibrateAsync(VibrationType.OneToneHigh);
                    //indicate which tone is selected
                    blackOut();
                    oneToneHButton.Background = Resources["PhoneAccentBrush"] as SolidColorBrush;
                }
            }
            catch (Exception ex)
            {
                this.viewModel.StatusMessage = ex.ToString();
            }
        }
        private async void RampDown(object sender, RoutedEventArgs e)
        {
            try
            {
                // Get the list of Microsoft Bands paired to the phone.
                IBandInfo[] pairedBands = await BandClientManager.Instance.GetBandsAsync();
                // Connect to Microsoft Band.
                using (IBandClient bandClient = await BandClientManager.Instance.ConnectAsync(pairedBands[0]))
                {
                    //Set the toneSelector
                    toneSelect = "RampDown";
                    // send a vibration request of type alert alarm to the Band
                    await bandClient.NotificationManager.VibrateAsync(VibrationType.RampDown);
                    //indicate which tone is selected
                    blackOut();
                    rampDownButton.Background = Resources["PhoneAccentBrush"] as SolidColorBrush;
                }
            }
            catch (Exception ex)
            {
                this.viewModel.StatusMessage = ex.ToString();
            }
        }
        private async void RampUp(object sender, RoutedEventArgs e)
        {
            try
            {
                // Get the list of Microsoft Bands paired to the phone.
                IBandInfo[] pairedBands = await BandClientManager.Instance.GetBandsAsync();
                // Connect to Microsoft Band.
                using (IBandClient bandClient = await BandClientManager.Instance.ConnectAsync(pairedBands[0]))
                {
                    //Set the toneSelector
                    toneSelect = "RampUp";
                    // send a vibration request of type alert alarm to the Band
                    await bandClient.NotificationManager.VibrateAsync(VibrationType.RampUp);
                    //indicate which tone is selected
                    blackOut();
                    rampUpButton.Background = Resources["PhoneAccentBrush"] as SolidColorBrush;
                }
            }
            catch (Exception ex)
            {
                this.viewModel.StatusMessage = ex.ToString();
            }
        }
        private async void ThreeToneH(object sender, RoutedEventArgs e)
        {
            try
            {
                // Get the list of Microsoft Bands paired to the phone.
                IBandInfo[] pairedBands = await BandClientManager.Instance.GetBandsAsync();
                // Connect to Microsoft Band.
                using (IBandClient bandClient = await BandClientManager.Instance.ConnectAsync(pairedBands[0]))
                {
                    //Set the toneSelector
                    toneSelect = "ThreeToneH";
                    // send a vibration request of type alert alarm to the Band
                    await bandClient.NotificationManager.VibrateAsync(VibrationType.ThreeToneHigh);
                    //indicate which tone is selected
                    blackOut();
                    threeToneHButton.Background = Resources["PhoneAccentBrush"] as SolidColorBrush;
                }
            }
            catch (Exception ex)
            {
                this.viewModel.StatusMessage = ex.ToString();
            }
        }
        private async void TwoToneH(object sender, RoutedEventArgs e)
        {
            try
            {
                // Get the list of Microsoft Bands paired to the phone.
                IBandInfo[] pairedBands = await BandClientManager.Instance.GetBandsAsync();
                // Connect to Microsoft Band.
                using (IBandClient bandClient = await BandClientManager.Instance.ConnectAsync(pairedBands[0]))
                {
                    //Set the toneSelector
                    toneSelect = "TwoToneH";
                    // send a vibration request of type alert alarm to the Band
                    await bandClient.NotificationManager.VibrateAsync(VibrationType.TwoToneHigh);
                    //indicate which tone is selected
                    blackOut();
                    twoToneHButton.Background = Resources["PhoneAccentBrush"] as SolidColorBrush;
                }
            }
            catch (Exception ex)
            {
                this.viewModel.StatusMessage = ex.ToString();
            }
        }
    }
}
