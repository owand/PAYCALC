using PAYCALC.Resources;
using PAYCALC.Services;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PAYCALC.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SalaryPage : ContentPage
    {
        /// <summary>
        /// Check the value entered in the entry and convert it
        /// </summary>
        protected Action<Entry, string> AdditionalCheck;

        private static readonly string zero = Convert.ToDouble(0.ToString()).ToString("N2");
        private static readonly double rateIncomeTAX = 18.0;
        private static double RateIncomeTAX
        {
            get => Xamarin.Essentials.Preferences.Get("RateIncomeTAX", rateIncomeTAX);
            set => Xamarin.Essentials.Preferences.Set("RateIncomeTAX", value);
        }
        private static readonly double rateMilitaryTAX = 1.5;
        private static double RateMilitaryTAX
        {
            get => Xamarin.Essentials.Preferences.Get("RateMilitaryTAX", rateMilitaryTAX);
            set => Xamarin.Essentials.Preferences.Set("RateMilitaryTAX", value);
        }
        private static readonly double rateSSC = 22.0;
        private static double RateSSC
        {
            get => Xamarin.Essentials.Preferences.Get("RateSSC", rateSSC);
            set => Xamarin.Essentials.Preferences.Set("RateSSC", value);
        }
        private static readonly double minSALARY = 6000.0;
        private static double MinSALARY
        {
            get => Xamarin.Essentials.Preferences.Get("MinSALARY", minSALARY);
            set => Xamarin.Essentials.Preferences.Set("MinSALARY", value);
        }
        private static double minSSC = MinSALARY * RateSSC / 100;
        private static double maxSSC = MinSALARY * 15 * RateSSC / 100;


        public SalaryPage()
        {
            InitializeComponent();
            LayoutChanged += OnSizeChanged; // Определяем обработчик события, которое происходит, когда изменяется ширина или высота.
        }

        // События непосредственно перед тем как страница становится видимой.
        protected override void OnAppearing()
        {
            base.OnAppearing();
            Device.BeginInvokeOnMainThread(async () =>
            {
                await System.Threading.Tasks.Task.Delay(100);

                entrRateIncomeTAX.Text = RateIncomeTAX.ToString("N2");
                entrRateMilitaryTAX.Text = RateMilitaryTAX.ToString("N2");
                entrRateSSC.Text = RateSSC.ToString("N2");
                entrMinSALARY.Text = MinSALARY.ToString("N2");
                MinSSC.Text = minSSC.ToString("N2");
                MaxSSC.Text = maxSSC.ToString("N2");

                entrSALARY.Focus();
            });
        }

        // Происходит, когда ширина или высота свойств измените значение на этот элемент.
        private void OnSizeChanged(object sender, EventArgs e)
        {
            switch (Device.Idiom)
            {
                case TargetIdiom.Desktop:
                case TargetIdiom.Tablet:
                    if (Shell.Current.Width > 1112)
                    {
                        HeaderParameters.SetValue(Grid.RowProperty, 0);
                        HeaderParameters.SetValue(Grid.ColumnProperty, 2);
                        SpacebarBottom.IsVisible = false;
                        ScrollParameters.SetValue(Grid.RowProperty, 1);
                        ScrollParameters.SetValue(Grid.ColumnProperty, 2);

                        RootGrid.RowDefinitions[1].Height = new GridLength(1, GridUnitType.Star);
                        RootGrid.RowDefinitions[2].Height = 0;
                        RootGrid.RowDefinitions[3].Height = 0;
                    }
                    else
                    {
                        HeaderParameters.SetValue(Grid.RowProperty, 2);
                        HeaderParameters.SetValue(Grid.ColumnProperty, 0);
                        SpacebarBottom.IsVisible = true;
                        ScrollParameters.SetValue(Grid.RowProperty, 3);
                        ScrollParameters.SetValue(Grid.ColumnProperty, 0);

                        RootGrid.RowDefinitions[1].Height = GridLength.Auto;
                        RootGrid.RowDefinitions[2].Height = 48;
                        RootGrid.RowDefinitions[3].Height = new GridLength(1, GridUnitType.Star);
                    }
                    break;

                case TargetIdiom.Phone:
                    break;

                default:
                    break;
            }
        }

        // Сохраняем изменения.
        private async void OnSave(object sender, EventArgs e)
        {
            try
            {
                Preferences.Set("RateIncomeTAX", 18.0);
                Preferences.Set("RateMilitaryTAX", Convert.ToDouble(entrRateMilitaryTAX.Text));
                Preferences.Set("RateSSC", Convert.ToDouble(entrRateSSC.Text));
                Preferences.Set("MinSALARY", Convert.ToDouble(entrMinSALARY.Text));
            }
            catch (Exception ex)
            {
                await DisplayAlert(AppResource.messageError, ex.Message, AppResource.messageOk);
            }
        }

        private async void OnCancel(object sender, EventArgs e)
        {
            try
            {
                entrRateIncomeTAX.Text = rateIncomeTAX.ToString("N2");
                entrRateMilitaryTAX.Text = rateMilitaryTAX.ToString("N2");
                entrRateSSC.Text = rateSSC.ToString("N2");
                entrMinSALARY.Text = minSALARY.ToString("N2");
            }
            catch (Exception ex)
            {
                await DisplayAlert(AppResource.messageError, ex.Message, AppResource.messageOk);
            }
        }

        // Событие при изменении текста в соответствующих полях.
        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(entrRateIncomeTAX.Text) &&
                !string.IsNullOrEmpty(entrRateMilitaryTAX.Text) &&
                !string.IsNullOrEmpty(entrRateSSC.Text) &&
                !string.IsNullOrEmpty(entrMinSALARY.Text))
                {
                    SaveButton.IsEnabled = true; // Кнопка Удаления записи неактивна.
                    minSSC = Convert.ToDouble(entrMinSALARY.Text) * Convert.ToDouble(entrRateSSC.Text) / 100;
                    maxSSC = Convert.ToDouble(entrMinSALARY.Text) * 15 * Convert.ToDouble(entrRateSSC.Text) / 100;
                    MinSSC.Text = minSSC.ToString("N2");
                    MaxSSC.Text = maxSSC.ToString("N2");

                    if (typeNET.IsChecked)
                    {
                        OnNETtoGROSS();
                    }
                    else if (typeGROSS.IsChecked)
                    {
                        OnGROSStoNET();
                    }
                    else if (typeCOMPANY.IsChecked)
                    {
                        OnCOMPANY();
                    }

                    OnTotalTax();
                }
                else
                {
                    SaveButton.IsEnabled = false; // Кнопка Удаления записи неактивна.
                    NET.Text = GROSS.Text = COMPANY.Text = IncomeTAX.Text = MilitaryTAX.Text = SSC.Text = TotalTax.Text = zero;
                }
            }
            catch (Exception)
            {
            }
        }

        // GROSS
        private void OnNETtoGROSS()
        {
            if (Convert.ToDouble(entrSALARY.Text) != 0)
            {
                double net = Convert.ToDouble(entrSALARY.Text);
                NET.Text = net.ToString("N2");

                double incomeTAX = Convert.ToDouble(entrSALARY.Text) / (1 - Convert.ToDouble(entrRateIncomeTAX.Text) / 100 - Convert.ToDouble(entrRateMilitaryTAX.Text) / 100) * Convert.ToDouble(entrRateIncomeTAX.Text) / 100;
                IncomeTAX.Text = incomeTAX.ToString("N2");
                double militaryTAX = Convert.ToDouble(entrSALARY.Text) / (1 - Convert.ToDouble(entrRateIncomeTAX.Text) / 100 - Convert.ToDouble(entrRateMilitaryTAX.Text) / 100) * Convert.ToDouble(entrRateMilitaryTAX.Text) / 100;
                MilitaryTAX.Text = militaryTAX.ToString("N2");

                double gross = Convert.ToDouble(entrSALARY.Text) + incomeTAX + militaryTAX;
                GROSS.Text = gross.ToString("N2");

                if (Convert.ToDouble(GROSS.Text) < Convert.ToDouble(entrMinSALARY.Text))
                {
                    SSC.Text = Convert.ToDouble(MinSSC.Text).ToString("N2");
                }
                else if (Convert.ToDouble(GROSS.Text) < Convert.ToDouble(entrMinSALARY.Text) * 15)
                {
                    double ssc = Convert.ToDouble(GROSS.Text) * Convert.ToDouble(entrRateSSC.Text) / 100;
                    SSC.Text = ssc.ToString("N2");
                }
                else
                {
                    SSC.Text = Convert.ToDouble(MaxSSC.Text).ToString("N2");
                }

                double company = Convert.ToDouble(GROSS.Text) + Convert.ToDouble(SSC.Text);
                COMPANY.Text = company.ToString("N2");

                OnTotalTax();
            }
            else
            {
                NET.Text = GROSS.Text = COMPANY.Text = IncomeTAX.Text = MilitaryTAX.Text = SSC.Text = zero;
            }
        }

        // NET
        private void OnGROSStoNET()
        {
            if (Convert.ToDouble(entrSALARY.Text) != 0)
            {
                double gross = Convert.ToDouble(entrSALARY.Text);
                GROSS.Text = gross.ToString("N2");

                double incomeTAX = Convert.ToDouble(entrSALARY.Text) * Convert.ToDouble(entrRateIncomeTAX.Text) / 100;
                IncomeTAX.Text = incomeTAX.ToString("N2");
                double militaryTAX = Convert.ToDouble(entrSALARY.Text) * Convert.ToDouble(entrRateMilitaryTAX.Text) / 100;
                MilitaryTAX.Text = militaryTAX.ToString("N2");

                double net = Convert.ToDouble(entrSALARY.Text) - incomeTAX - militaryTAX;
                NET.Text = net.ToString("N2");

                if (Convert.ToDouble(GROSS.Text) < Convert.ToDouble(entrMinSALARY.Text))
                {
                    SSC.Text = Convert.ToDouble(MinSSC.Text).ToString("N2");
                }
                else if (Convert.ToDouble(GROSS.Text) < Convert.ToDouble(entrMinSALARY.Text) * 15)
                {
                    double ssc = Convert.ToDouble(GROSS.Text) * Convert.ToDouble(entrRateSSC.Text) / 100;
                    SSC.Text = ssc.ToString("N2");
                }
                else
                {
                    SSC.Text = Convert.ToDouble(MaxSSC.Text).ToString("N2");
                }

                double company = Convert.ToDouble(GROSS.Text) + Convert.ToDouble(SSC.Text);
                COMPANY.Text = company.ToString("N2");

                OnTotalTax();
            }
            else
            {
                NET.Text = GROSS.Text = COMPANY.Text = IncomeTAX.Text = MilitaryTAX.Text = SSC.Text = zero;
            }
        }

        // COMPANY
        private void OnCOMPANY()
        {
            if (Convert.ToDouble(entrSALARY.Text) != 0)
            {
                double company = Convert.ToDouble(entrSALARY.Text);
                COMPANY.Text = company.ToString("N2");

                if ((Convert.ToDouble(COMPANY.Text) - Convert.ToDouble(COMPANY.Text) / (1 + Convert.ToDouble(entrRateSSC.Text)) * Convert.ToDouble(entrRateSSC.Text) / 100) < Convert.ToDouble(entrMinSALARY.Text))
                {
                    SSC.Text = Convert.ToDouble(MinSSC.Text).ToString("N2");
                    double gross = Convert.ToDouble(entrSALARY.Text) - Convert.ToDouble(SSC.Text);
                    GROSS.Text = gross.ToString("N2");
                }
                else if ((Convert.ToDouble(COMPANY.Text) - Convert.ToDouble(COMPANY.Text) / (1 + Convert.ToDouble(entrRateSSC.Text)) * Convert.ToDouble(entrRateSSC.Text) / 100) < Convert.ToDouble(entrMinSALARY.Text) * 15)
                {
                    double ssc = Convert.ToDouble(COMPANY.Text) / (1 + Convert.ToDouble(entrRateSSC.Text) / 100) * Convert.ToDouble(entrRateSSC.Text) / 100;
                    SSC.Text = ssc.ToString("N2");
                    double gross = Convert.ToDouble(entrSALARY.Text) - ssc;
                    GROSS.Text = gross.ToString("N2");
                }
                else
                {
                    SSC.Text = Convert.ToDouble(MaxSSC.Text).ToString("N2");
                    double gross = Convert.ToDouble(entrSALARY.Text) - Convert.ToDouble(MaxSSC.Text);
                    GROSS.Text = gross.ToString("N2");
                }

                double incomeTAX = Convert.ToDouble(GROSS.Text) * Convert.ToDouble(entrRateIncomeTAX.Text) / 100;
                IncomeTAX.Text = incomeTAX.ToString("N2");
                double militaryTAX = Convert.ToDouble(GROSS.Text) * Convert.ToDouble(entrRateMilitaryTAX.Text) / 100;
                MilitaryTAX.Text = militaryTAX.ToString("N2");

                double net = Convert.ToDouble(GROSS.Text) - incomeTAX - militaryTAX;
                NET.Text = net.ToString("N2");

                OnTotalTax();
            }
            else
            {
                NET.Text = GROSS.Text = COMPANY.Text = IncomeTAX.Text = MilitaryTAX.Text = SSC.Text = zero;
            }
        }

        // COMPANY
        private void OnTotalTax()
        {
            if (Convert.ToDouble(entrSALARY.Text) != 0)
            {
                double totaltax = Convert.ToDouble(IncomeTAX.Text) + Convert.ToDouble(MilitaryTAX.Text) + Convert.ToDouble(SSC.Text);
                TotalTax.Text = totaltax.ToString("N2");
            }
            else
            {
                TotalTax.Text = zero;
            }
        }

        private void switchNET(object sender, CheckedChangedEventArgs e)
        {
            ClearResult();

            if (typeNET.IsChecked)
            {
                typeNET.IsEnabled = false;

                typeGROSS.IsChecked = false;
                typeGROSS.IsEnabled = true;

                typeCOMPANY.IsChecked = false;
                typeCOMPANY.IsEnabled = true;

                OnNETtoGROSS();
            }
        }

        private void switchGROSS(object sender, CheckedChangedEventArgs e)
        {
            ClearResult();

            if (typeGROSS.IsChecked)
            {
                typeGROSS.IsEnabled = false;

                typeNET.IsChecked = false;
                typeNET.IsEnabled = true;

                typeCOMPANY.IsChecked = false;
                typeCOMPANY.IsEnabled = true;

                OnGROSStoNET();
            }
        }

        private void switchCOMPANY(object sender, CheckedChangedEventArgs e)
        {
            ClearResult();

            if (typeCOMPANY.IsChecked == true)
            {
                typeCOMPANY.IsEnabled = false;

                typeNET.IsChecked = false;
                typeNET.IsEnabled = true;

                typeGROSS.IsChecked = false;
                typeGROSS.IsEnabled = true;

                OnCOMPANY();
            }
        }

        private void ClearResult()
        {
            if (!typeNET.IsChecked && !typeGROSS.IsChecked && !typeCOMPANY.IsChecked)
            {
                entrSALARY.Text = "0";
                NET.Text = "0";
                GROSS.Text = "0";
                COMPANY.Text = "0";
                IncomeTAX.Text = "0";
                MilitaryTAX.Text = "0";
                SSC.Text = "0";
            }
        }

        private void OnVisibleOptions(object sender, EventArgs e)
        {
            try
            {
                switch (Device.Idiom)
                {
                    case TargetIdiom.Desktop:
                    case TargetIdiom.Tablet:
                        if (HeaderParameters.IsVisible == false)
                        {
                            HeaderParameters.IsVisible = true;
                            ScrollParameters.IsVisible = true;
                            SpacebarBottom.BackgroundColor = Color.FromHex("#515C6B");

                            RootGrid.RowDefinitions[2].Height = 48;
                        }
                        else
                        {
                            HeaderParameters.IsVisible = false;
                            ScrollParameters.IsVisible = false;
                            SpacebarBottom.BackgroundColor = Color.FromHex("Transparent");

                            RootGrid.RowDefinitions[2].Height = 0;
                        }
                        break;

                    case TargetIdiom.Phone:
                        if (HeaderParameters.IsVisible == false)
                        {
                            HeaderParameters.IsVisible = true;
                            ScrollParameters.IsVisible = true;
                            HeaderResults.IsVisible = false;
                            ScrollResult.IsVisible = false;
                            BackButton.IsVisible = true;
                            tbSettings.IsEnabled = false;
                            RootGrid.RowDefinitions[0].Height = 0;
                            RootGrid.RowDefinitions[1].Height = 0;
                            RootGrid.RowDefinitions[2].Height = 48;
                            RootGrid.RowDefinitions[3].Height = new GridLength(1, GridUnitType.Star);
                        }
                        else
                        {
                            HeaderParameters.IsVisible = false;
                            ScrollParameters.IsVisible = false;
                            HeaderResults.IsVisible = true;
                            ScrollResult.IsVisible = true;
                            BackButton.IsVisible = false;
                            tbSettings.IsEnabled = true;

                            RootGrid.RowDefinitions[0].Height = 48;
                            RootGrid.RowDefinitions[1].Height = new GridLength(1, GridUnitType.Star);
                            RootGrid.RowDefinitions[2].Height = 0;
                            RootGrid.RowDefinitions[3].Height = 0;
                        }
                        break;

                    default:
                        break;
                }
            }
            catch (Exception)
            {
            }
        }

        // hardware back button
        protected override bool OnBackButtonPressed()
        {
            try
            {
                // Begin an asyncronous task on the UI thread because we intend to ask the users permission.
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (await DisplayAlert(AppResource.messageTitleExit, AppResource.messageExit, AppResource.messageOk, AppResource.messageСancel))
                    {
                        if (Device.RuntimePlatform == Device.Android)
                        {
                            DependencyService.Get<ICloseApplication>().CloseApp();
                        }

                        base.OnBackButtonPressed();
                    }
                });
            }
            catch { return false; }

            // Always return true because this method is not asynchronous.
            // We must handle the action ourselves: see above.
            return true;
        }

    }
}