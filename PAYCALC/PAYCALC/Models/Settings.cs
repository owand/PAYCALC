using PAYCALC.Resources;
using System.Collections.ObjectModel;

namespace PAYCALC.Models
{
    public class Settings : ViewModelBase
    {
        public ObservableCollection<LangModel> LangCollection { get; }
        public ObservableCollection<ThemesModel> ThemesCollection { get; }

        public Settings()
        {
            LangCollection = new ObservableCollection<LangModel>()
            {
                new LangModel { LANGDISPLAY = AppResource.LanguageRus, LANGNAME = "ru" },
                new LangModel { LANGDISPLAY = AppResource.LanguageEng, LANGNAME = "en" },
                new LangModel { LANGDISPLAY = AppResource.LanguageUkr, LANGNAME = "uk" }
            };

            ThemesCollection = new ObservableCollection<ThemesModel>()
            {
                new ThemesModel { THEMEDISPLAY = AppResource.ThemesDarkName, THEMENAME = "myDarkTheme" },
                new ThemesModel { THEMEDISPLAY = AppResource.ThemesLightName, THEMENAME = "myLightTheme" },
                new ThemesModel { THEMEDISPLAY =  AppResource.ThemesOSName, THEMENAME = "myOSTheme" }
            };
        }


    }
}
