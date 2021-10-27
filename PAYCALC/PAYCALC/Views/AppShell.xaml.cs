using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PAYCALC.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Select a root Flyout item.
            //CurrentItem = new ShellContent { Content = new SalaryPage() };
            CurrentItem = salaryPage;
            CurrentItem.IsVisible = false;
        }
    }
}