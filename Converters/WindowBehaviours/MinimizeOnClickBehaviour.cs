﻿using System.Windows;
using System.Windows.Controls;

namespace Converters.Behaviours.WindowBehaviours
{
    /// <summary>
    /// MinimizeOnClickBehaviour is an attached proeprty that can be attached to buttons
    /// to minimize the buttons host window.
    /// </summary>
    public static class MinimizeOnClickBehaviour
    {
        /// <summary>
        /// Attach property to minimuze host window
        /// </summary>
        public static readonly DependencyProperty MinimizeWindowProperty =
            DependencyProperty.RegisterAttached
            (
                "IsWindowMinimized",
                typeof(bool),
                typeof(MinimizeOnClickBehaviour),
                new PropertyMetadata(false, MinimizeWindowPropertyChanged)
            );

        public static bool GetMinimizeWindowProperty(DependencyObject obj)
        {
            return (bool)obj.GetValue(MinimizeWindowProperty);
        }


        public static void SetMinimizeWindowProperty(DependencyObject obj, bool value)
        {
            obj.SetValue(MinimizeWindowProperty, value);
        }


        public static void MinimizeWindowPropertyChanged(DependencyObject property, DependencyPropertyChangedEventArgs args)
        {
            Button button = property as Button;
            if (button != null) button.Click += OnClick;
        }

        private static void OnClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                Button btn = sender as Button;
                Window window = Window.GetWindow(btn);
                if (window != null)
                {
                    window.Visibility = Visibility.Hidden;
                    window.WindowState = WindowState.Minimized;
                }

            }
        }
    }
}