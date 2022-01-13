/* ==============================
** Copyright 2021, 2022 nishy software
**
**      First Author : nishy software
**		Create : 2021/12/07
** ============================== */

namespace NishySoftware.Wpf.AttachedProperties
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;
    using System.Windows.Media;

    public class ScrollViewerProperties : DependencyObject
    {
        #region MouseWheelHandlingMode Property

        public enum MouseWheelHandlingMode
        {
            Normal,
            OnlyVisible,
            OnlyScrollable,
        }

        /// <summary>
        /// Using a DependencyProperty as the backing store for MouseWheelHandlingMode.
        /// This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty MouseWheelHandlingModeProperty =
            DependencyProperty.RegisterAttached(nameof(MouseWheelHandlingMode), typeof(MouseWheelHandlingMode), typeof(ScrollViewerProperties),
                new PropertyMetadata(MouseWheelHandlingMode.Normal, OnMouseWheelHandlingModeChanged));

        public static MouseWheelHandlingMode GetMouseWheelHandlingMode(DependencyObject d)
        {
            return (MouseWheelHandlingMode)d.GetValue(MouseWheelHandlingModeProperty);
        }

        public static void SetMouseWheelHandlingMode(DependencyObject d, MouseWheelHandlingMode value)
        {
            d.SetValue(MouseWheelHandlingModeProperty, value);
        }

        static void OnMouseWheelHandlingModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(e.NewValue is MouseWheelHandlingMode mode)) { return; }
            if (d is ScrollViewer sv)
            {
                sv.PreviewMouseWheel -= OnPreviewMouseWheel;   // remove if registered
                if (mode == MouseWheelHandlingMode.Normal)
                {
                    SetHandlesMouseWheelScrolling(sv, true);
                }
                else
                {
                    sv.PreviewMouseWheel += OnPreviewMouseWheel;
                }
            }
            else
            {
                if (d is FrameworkElement fe)
                {
                    fe.Loaded -= ScrollViewerOwer_Loaded;   // remove if registered
                    fe.Loaded += ScrollViewerOwer_Loaded;
                    if (fe.IsLoaded)
                    {
                        UpdateChildScrollViewer(fe);
                    }
                }
                else
                {
                    return;
                }
            }
        }

        static void ScrollViewerOwer_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement fe)
            {
                UpdateChildScrollViewer(fe);
            }
        }

        static void OnPreviewMouseWheel(object s, MouseWheelEventArgs e)
        {
            if (!(s is ScrollViewer sv))
            {
                return;
            }

            var mode = GetMouseWheelHandlingMode(sv);
            var handleEvent = true;
            switch (mode)
            {
                case MouseWheelHandlingMode.OnlyScrollable:
                    if (e.Delta > 0)
                    {
                        handleEvent = sv.VerticalOffset > 0;
                    }
                    else
                    {
                        handleEvent = sv.VerticalOffset < sv.ScrollableHeight;
                    }
                    break;
                case MouseWheelHandlingMode.OnlyVisible:
                    handleEvent = sv.ComputedVerticalScrollBarVisibility == Visibility.Visible;
                    break;
                case MouseWheelHandlingMode.Normal:
                    handleEvent = true;
                    break;
            }
            SetHandlesMouseWheelScrolling(sv, handleEvent);
        }

        static PropertyInfo _handlesMouseWheelScrollingPropInfo;

        static void SetHandlesMouseWheelScrolling(ScrollViewer sv, bool enable)
        {
            if (sv == null)
            {
                return;
            }

            if (_handlesMouseWheelScrollingPropInfo == null)
            {
                // Get PropInfo of internal property using reflection
                var svType = typeof(ScrollViewer);
                _handlesMouseWheelScrollingPropInfo = svType.GetProperty("HandlesMouseWheelScrolling", BindingFlags.NonPublic | BindingFlags.Instance);
            }

            _handlesMouseWheelScrollingPropInfo?.SetValue(sv, enable);
        }

        static bool IsDefaultMouseWheelHandlingMode(DependencyObject d)
        {
            var vs = DependencyPropertyHelper.GetValueSource(d, MouseWheelHandlingModeProperty);
            return vs.BaseValueSource == BaseValueSource.Default;
        }

        static bool UpdateChildScrollViewer(FrameworkElement fe)
        {
            // Find ScrollViewer in child elements of VisualTree
            var sv = fe.GetChildOfType<ScrollViewer>();
            if (sv == null) { return false; }

            var mode = GetMouseWheelHandlingMode(fe);
            // Overwrite the current value if the mode is default
            if (IsDefaultMouseWheelHandlingMode(sv))
            {
                sv.SetCurrentValue(MouseWheelHandlingModeProperty, mode);
            }

            return true;
        }

        #endregion
    }

    static class ScrollViewerPropertiesExtenstions
    {
        public static T GetChildOfType<T>(this DependencyObject depObj)
            where T : DependencyObject
        {
            if (depObj == null) return null;

            var count = VisualTreeHelper.GetChildrenCount(depObj);
            for (int i = 0; i < count; i++)
            {
                var child = VisualTreeHelper.GetChild(depObj, i);

                var result = (child as T) ?? GetChildOfType<T>(child);
                if (result != null) return result;
            }
            return null;
        }
    }
}
