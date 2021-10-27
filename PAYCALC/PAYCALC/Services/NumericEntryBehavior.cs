﻿using System;
using Xamarin.Forms;

namespace PAYCALC.Services
{
    public class NumericEntryBehavior : Behavior<Entry>
    {
        protected override void OnAttachedTo(Entry entry)
        {
            entry.TextChanged += OnEntryTextChanged;
            base.OnAttachedTo(entry);
        }

        protected override void OnDetachingFrom(Entry entry)
        {
            entry.TextChanged -= OnEntryTextChanged;
            base.OnDetachingFrom(entry);
        }

        protected virtual void OnEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                OSAppTheme currentTheme = Application.Current.RequestedTheme;

                if (string.IsNullOrWhiteSpace(((Entry)sender).Text))
                {
                    ((Entry)sender).BackgroundColor = Color.Red;
                }
                else if (double.Parse(((Entry)sender).Text) == 0)
                {
                    ((Entry)sender).TextColor = Color.Red;
                }
                else
                {
                    ((Entry)sender).BackgroundColor = Color.Transparent;
                    //((Entry)sender).TextColor = (Color)App.Current.Resources["TextColor"];

                    // Theme of the application
                    switch (currentTheme)
                    {
                        case OSAppTheme.Dark:
                            ((Entry)sender).TextColor = (Color)App.Current.Resources["TextColor_Dark"];
                            break;

                        case OSAppTheme.Light:
                            ((Entry)sender).TextColor = (Color)App.Current.Resources["TextColor_Light"];
                            break;

                        default:
                            break;
                    }
                }

                if (string.IsNullOrWhiteSpace(e.NewTextValue))
                {
                    ((Entry)sender).BackgroundColor = Color.Red;
                }
                else if (double.Parse(e.NewTextValue) == 0)
                {
                    ((Entry)sender).TextColor = Color.Red;
                    ((Entry)sender).BackgroundColor = Color.Transparent;
                }
                else
                {
                    ((Entry)sender).BackgroundColor = Color.Transparent;
                    //((Entry)sender).TextColor = (Color)App.Current.Resources["TextColor"];

                    // Theme of the application
                    switch (currentTheme)
                    {
                        case OSAppTheme.Dark:
                            ((Entry)sender).TextColor = (Color)App.Current.Resources["TextColor_Dark"];
                            break;

                        case OSAppTheme.Light:
                            ((Entry)sender).TextColor = (Color)App.Current.Resources["TextColor_Light"];
                            break;

                        default:
                            break;
                    }

                    return;
                }


                if (!double.TryParse(e.NewTextValue, out double result))
                {
                    ((Entry)sender).Text = e.OldTextValue;
                }
            }
            catch (Exception)
            {
                // Что-то пошло не так
                return;
            }
        }
    }
}
