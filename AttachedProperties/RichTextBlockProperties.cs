using System;
using Windows.UI.Xaml;

namespace UwpSample.AttachedProperties
{
    public class SecondLanguage : DependencyObject
    {
        public static readonly DependencyProperty CultureProperty =
        DependencyProperty.RegisterAttached(
          "Culture",
          typeof(string),
          typeof(SecondLanguage),
          new PropertyMetadata(false)
        );
        public static void SetCulture(UIElement element, string value)
        {
            element.SetValue(CultureProperty, value);
        }
        public static string GetCulture(UIElement element)
        {
            return (string)element.GetValue(CultureProperty);
        }

        public static readonly DependencyProperty TextProperty =
        DependencyProperty.RegisterAttached(
          "Text",
          typeof(string),
          typeof(SecondLanguage),
          new PropertyMetadata(false)
        );
        public static void SetText(UIElement element, string value)
        {
            element.SetValue(TextProperty, value);
        }
        public static string GetText(UIElement element)
        {
            return (string)element.GetValue(TextProperty);
        }
    }
    public class Audio : DependencyObject
    {
        public static readonly DependencyProperty SourceUriProperty =
        DependencyProperty.RegisterAttached(
          "SourceUri",
          typeof(string),
          typeof(Audio),
          new PropertyMetadata(false)
        );
        public static void SetSourceUri(UIElement element, string value)
        {
            element.SetValue(SourceUriProperty, value);
        }
        public static string GetSourceUri(UIElement element)
        {
            return (string)element.GetValue(SourceUriProperty);
        }

        public static readonly DependencyProperty TimeSpanProperty =
        DependencyProperty.RegisterAttached(
          "TimeSpan",
          typeof(string),
          typeof(Audio),
          new PropertyMetadata(false)
        );
        public static void SetTimeSpan(UIElement element, string value)
        {
            element.SetValue(TimeSpanProperty, value);
        }
        public static string GetTimeSpan(UIElement element)
        {
            return (string)element.GetValue(TimeSpanProperty);
        }
    }
    public class PauseRun : DependencyObject
    {
        public static readonly DependencyProperty IsPauseProperty =
        DependencyProperty.RegisterAttached(
          "IsPause",
          typeof(bool),
          typeof(PauseRun),
          new PropertyMetadata(false)
        );
        public static void SetIsPause(UIElement element, bool value)
        {
            element.SetValue(IsPauseProperty, value);
        }
        public static bool GetIsPause(UIElement element)
        {
            return (bool)element.GetValue(IsPauseProperty);
        }

        public static readonly DependencyProperty PauseMarkProperty =
        DependencyProperty.RegisterAttached(
          "PauseMark",
          typeof(string),
          typeof(PauseRun),
          new PropertyMetadata(false)
        );
        public static void SetPauseMark(UIElement element, string value)
        {
            element.SetValue(PauseMarkProperty, value);
        }
        public static string GetPauseMark(UIElement element)
        {
            return (string)element.GetValue(PauseMarkProperty);
        }

        public static readonly DependencyProperty PauseTextProperty =
        DependencyProperty.RegisterAttached(
          "PauseText",
          typeof(string),
          typeof(PauseRun),
          new PropertyMetadata(false)
        );
        public static void SetPauseText(UIElement element, string value)
        {
            element.SetValue(PauseTextProperty, value);
        }
        public static string GetPauseText(UIElement element)
        {
            return (string)element.GetValue(PauseTextProperty);
        }

        public static readonly DependencyProperty IsVisibleProperty =
        DependencyProperty.RegisterAttached(
          "IsVisible",
          typeof(bool),
          typeof(PauseRun),
          new PropertyMetadata(false)
        );
        public static void SetIsVisible(UIElement element, bool value = false)
        {
            element.SetValue(IsVisibleProperty, value);
        }
        public static bool GetIsVisible(UIElement element)
        {
            return (bool)element.GetValue(IsVisibleProperty);
        }
    }
    public class RubyRun : DependencyObject
    {
        public static readonly DependencyProperty IsRubyProperty =
        DependencyProperty.RegisterAttached(
          "IsRuby",
          typeof(bool),
          typeof(RubyRun),
          new PropertyMetadata(false)
        );
        public static void SetIsRuby(UIElement element, bool value)
        {
            element.SetValue(IsRubyProperty, value);
        }
        public static bool GetIsRuby(UIElement element)
        {
            return (bool)element.GetValue(IsRubyProperty);
        }

        public static readonly DependencyProperty RubyTextProperty =
        DependencyProperty.RegisterAttached(
          "RubyText",
          typeof(string),
          typeof(RubyRun),
          new PropertyMetadata(false)
        );
        public static void SetRubyText(UIElement element, string value)
        {
            element.SetValue(RubyTextProperty, value);
        }
        public static string GetRubyText(UIElement element)
        {
            return (string)element.GetValue(RubyTextProperty);
        }

        public static readonly DependencyProperty IsVisibleProperty =
        DependencyProperty.RegisterAttached(
          "IsVisible",
          typeof(bool),
          typeof(RubyRun),
          new PropertyMetadata(false)
        );
        public static void SetIsVisible(UIElement element, bool value = false)
        {
            element.SetValue(IsVisibleProperty, value);
        }
        public static bool GetIsVisible(UIElement element)
        {
            return (bool)element.GetValue(IsVisibleProperty);
        }
    }
}
