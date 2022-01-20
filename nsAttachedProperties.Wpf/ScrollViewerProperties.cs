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
            Inherit,    // not used to set local current value
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
                new PropertyMetadata(MouseWheelHandlingMode.Inherit, OnMouseWheelHandlingModeChanged));

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
                // The value of ScrollViewer is not inherited by the child ScrollViewers.

                sv.PreviewMouseWheel -= OnPreviewMouseWheel;   // remove if registered
                if (mode == MouseWheelHandlingMode.Inherit)
                {
                    // Gets the inherited value of the parent
                    mode = GetInheritMouseWheelHandlingMode(sv);
                    if (mode == MouseWheelHandlingMode.Inherit)
                    {
                        mode = MouseWheelHandlingMode.Normal;
                    }
                    //  Reassigns mode value as a local current value
                    sv.SetCurrentValue(MouseWheelHandlingModeProperty, mode);
                }
                else if (mode == MouseWheelHandlingMode.Normal)
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
                // The value of FrameworkEelement other than ScrollViewer is inherited by the child ScrollViewers.

                if (d is FrameworkElement fe)
                {
                    fe.Loaded -= ScrollViewerOwer_Loaded;   // remove if registered
                    if (mode != MouseWheelHandlingMode.Inherit)
                    {
                        fe.Loaded += ScrollViewerOwer_Loaded;
                    }
                    if (fe.IsLoaded)
                    {
                        if (mode == MouseWheelHandlingMode.Inherit)
                        {
                            // Gets the inherited value of the parent
                            mode = GetInheritMouseWheelHandlingMode(fe);
                            if (mode == MouseWheelHandlingMode.Inherit)
                            {
                                // Set normal mode to children ScrollViewers
                                mode = MouseWheelHandlingMode.Normal;
                            }
                        }
                        // Reassigns mode value as a local value to children ScrollViewers
                        UpdateChildScrollViewers(fe, mode);
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
                var mode = GetMouseWheelHandlingMode(fe);
                UpdateChildScrollViewers(fe, mode);
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
                case MouseWheelHandlingMode.Inherit:
                case MouseWheelHandlingMode.Normal:
                    handleEvent = true;
                    break;
                case MouseWheelHandlingMode.OnlyVisible:
                    handleEvent = sv.ComputedVerticalScrollBarVisibility == Visibility.Visible;
                    break;
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

        static bool UpdateChildScrollViewers(FrameworkElement fe, MouseWheelHandlingMode mode)
        {
            var children = EnumChildScrollViewersWithoutMouseWheelHandlingMode(fe);
            if (children.Any())
            {
                foreach (var sv in children)
                {
                    if (mode == MouseWheelHandlingMode.Inherit)
                    {
                        sv.ClearValue(MouseWheelHandlingModeProperty);
                    }
                    else
                    {
                        sv.SetCurrentValue(MouseWheelHandlingModeProperty, mode);
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        static bool IsDefaultMouseWheelHandlingMode(DependencyObject d)
        {
            var vs = DependencyPropertyHelper.GetValueSource(d, MouseWheelHandlingModeProperty);
            return vs.BaseValueSource == BaseValueSource.Default;
        }

        static MouseWheelHandlingMode GetInheritMouseWheelHandlingMode(FrameworkElement fe)
        {
            var mode = MouseWheelHandlingMode.Inherit;
            DependencyObject parent = fe;
            while (mode == MouseWheelHandlingMode.Inherit
                && (parent = VisualTreeHelper.GetParent(parent)) != null)
            {
                mode = GetMouseWheelHandlingMode(parent);
            }
            return mode;
        }

        static IEnumerable<ScrollViewer> EnumChildScrollViewersWithoutMouseWheelHandlingMode(DependencyObject frameworkElement)
        {
            if (frameworkElement == null) yield break;

            var count = VisualTreeHelper.GetChildrenCount(frameworkElement);
            for (int i = 0; i < count; i++)
            {
                var child = VisualTreeHelper.GetChild(frameworkElement, i);
                if (IsDefaultMouseWheelHandlingMode(child)
                    || GetMouseWheelHandlingMode(child) == MouseWheelHandlingMode.Inherit)
                {
                    if (!(child is System.Windows.Controls.Primitives.TextBoxBase))
                    {
                        if (child is ScrollViewer sv)
                        {
                            yield return sv;
                        }
                        var children = EnumChildScrollViewersWithoutMouseWheelHandlingMode(child);
                        foreach (var j in children)
                        {
                            yield return j;
                        }
                    }
                }
            }
        }

        #endregion
    }
}
