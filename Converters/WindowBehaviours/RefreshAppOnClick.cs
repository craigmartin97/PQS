using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Converters.Behaviours.WindowBehaviours
{
    public static class RefreshAppOnClick
    {/// <summary>
        /// Attached property to buttons to close host window
        /// </summary>
        public static readonly DependencyProperty RefreshWindowProperty =
            DependencyProperty.RegisterAttached
            (
                "IsWindowClosed",
                typeof(bool),
                typeof(RefreshAppOnClick),
                new PropertyMetadata(false, RefreshWindowPropertyChanged)
            );

        public static bool GetRefresh(DependencyObject obj)
        {
            return (bool)obj.GetValue(RefreshWindowProperty);
        }

        public static void SetRefresh(DependencyObject obj, bool value)
        {
            obj.SetValue(RefreshWindowProperty, value);
        }


        public static void RefreshWindowPropertyChanged(DependencyObject property, DependencyPropertyChangedEventArgs args)
        {
            Button button = property as Button;
            if (button != null) button.Click += OnClick;
        }

        private static void OnClick(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }
    }
}
