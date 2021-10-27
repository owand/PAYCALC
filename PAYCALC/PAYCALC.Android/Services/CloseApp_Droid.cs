using PAYCALC.Droid.Services;
using PAYCALC.Services;

[assembly: Xamarin.Forms.Dependency(typeof(CloseApp_Droid))]
namespace PAYCALC.Droid.Services
{
    public class CloseApp_Droid : ICloseApplication
    {
        public void CloseApp()
        {
            Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
        }
    }
}