using PAYCALC.Models;
using PAYCALC.Resources;
using System;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PAYCALC.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FlyoutHeader : ContentView
    {
        public Settings settingsViewModel;

        public FlyoutHeader()
        {
            InitializeComponent();

            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                case Device.Android:
                    labAppVersion.Text = $"{AppInfo.VersionString}." + $"{AppInfo.BuildString}"; // Application Version (1.0.0)
                    break;

                case Device.UWP:
                    labAppVersion.Text = $"{AppInfo.VersionString}"; // Application Version (1.0.0)
                    break;

                default:
                    break;
            }

            BindingContext = settingsViewModel = new Settings();

            //try
            //{
            PickerLanguages.SelectedIndex = settingsViewModel.LangCollection.IndexOf(settingsViewModel?.LangCollection.Where(X => X.LANGNAME == App.AppLanguage).FirstOrDefault());
            PickerThemes.SelectedIndex = settingsViewModel.ThemesCollection.IndexOf(settingsViewModel?.ThemesCollection.Where(X => X.THEMENAME == App.AppTheme).FirstOrDefault());
            //}
            //catch (Exception)
            //{
            //    //PickerLanguages.SelectedIndex = settingsViewModel.LangCollection.IndexOf(settingsViewModel?.LangCollection.Where(X => X.LANGNAME == "en").FirstOrDefault());
            //    //PickerThemes.SelectedIndex = settingsViewModel.ThemesCollection.IndexOf(settingsViewModel?.ThemesCollection.Where(X => X.THEMENAME == "myOSTheme").FirstOrDefault());
            //}

            PickerLanguages.SelectedIndexChanged += OnLanguagesChanged;
            PickerThemes.SelectedIndexChanged += OnThemesChanged;
        }

        private void OnLanguagesChanged(object sender, EventArgs e)
        {
            try
            {
                Preferences.Set("currentLanguage", settingsViewModel.LangCollection[PickerLanguages.SelectedIndex].LANGNAME);
                AppResource.Culture = new System.Globalization.CultureInfo(App.AppLanguage);

                //((App)Application.Current).MainPage = new MainPage(); // Refresh App
                ((App)Application.Current).MainPage = new AppShell(); // Refresh App
            }
            catch (Exception)
            {
                return;
            }
        }

        private void OnThemesChanged(object sender, EventArgs e)
        {
            try
            {
                Xamarin.Essentials.Preferences.Set("currentTheme", settingsViewModel.ThemesCollection[PickerThemes.SelectedIndex].THEMENAME);

                switch (settingsViewModel.ThemesCollection[PickerThemes.SelectedIndex].THEMENAME)
                {
                    case "myDarkTheme":
                        App.Current.UserAppTheme = OSAppTheme.Dark;
                        break;

                    case "myLightTheme":
                        App.Current.UserAppTheme = OSAppTheme.Light;
                        break;

                    case "myOSTheme":
                        App.Current.UserAppTheme = OSAppTheme.Unspecified;
                        break;

                    default:
                        App.Current.UserAppTheme = OSAppTheme.Unspecified;
                        break;
                }
            }
            catch (Exception)
            {
                return;
            }
        }


        //------------------Tapped----------------------
        #region Tapped events

        private void Tapped_mailAuthor(object sender, EventArgs e)
        {
            Launcher.OpenAsync(new Uri("mailto:plowand@outlook.com"));
        }

        private void OpenReviewStore(object sender, EventArgs e)
        {
            switch (Xamarin.Forms.Device.RuntimePlatform)
            {
                case Xamarin.Forms.Device.Android:
                    OpenGooglePlay();
                    break;
                case Xamarin.Forms.Device.UWP:
                    OpenMicrosoftStore();
                    break;
                default:
                    break;
            }
        }

        private void OpenStore(object sender, EventArgs e)
        {
            switch (Xamarin.Forms.Device.RuntimePlatform)
            {
                case Xamarin.Forms.Device.Android:
                    OpenMicrosoftStore();
                    break;
                case Xamarin.Forms.Device.UWP:
                    OpenGooglePlay();
                    break;
                default:
                    break;
            }
        }

        private void OpenMicrosoftStore()
        {
            switch (Xamarin.Forms.Device.RuntimePlatform)
            {
                case Xamarin.Forms.Device.Android:
                    Launcher.OpenAsync(new Uri("https://www.microsoft.com/store/apps/9PD74NTTR9ZD"));
                    break;
                case Xamarin.Forms.Device.UWP:
                    Launcher.OpenAsync(new Uri("ms-windows-store://pdp/?productid=9PD74NTTR9ZD"));
                    break;
                default:
                    break;
            }
        }

        private void OpenGooglePlay()
        {
            Launcher.OpenAsync(new Uri("https://play.google.com/store/apps/details?id=com.plowand.paycalc"));
        }

        #endregion Tapped events
    }
}