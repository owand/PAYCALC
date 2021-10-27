using PAYCALC.Services;
using System.Threading;

[assembly: Xamarin.Forms.Dependency(typeof(PAYCALC.iOS.Services.CloseApp_iOS))]
namespace PAYCALC.iOS.Services
{
    public class CloseApp_iOS : ICloseApplication
    {
        public void CloseApp()
        {
            Thread.CurrentThread.Abort();
        }
    }
}