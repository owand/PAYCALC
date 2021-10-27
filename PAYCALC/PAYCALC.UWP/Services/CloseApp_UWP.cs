using PAYCALC.Services;
using Windows.UI.Xaml;

[assembly: Xamarin.Forms.Dependency(typeof(PAYCALC.UWP.Services.CloseApp_UWP))]
namespace PAYCALC.UWP.Services
{
    public class CloseApp_UWP : ICloseApplication
    {
        public void CloseApp()
        {
            Application.Current.Exit();
        }
    }
}
