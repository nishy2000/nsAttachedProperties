/* ==============================
** Copyright 2022 nishy software
**
**      First Author : nishy software
**		Create : 2022/02/25
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

    public class TextBoxProperties : DependencyObject
    {
        #region ReturnBehavior Property

        [Flags]
        public enum ReturnBehaviorMode
        {
            None = 0,
            MoveFocus = 1,
            UpdateSource = 2,
            SelectAll = 4,

            // combination
            UpdateSourceAndMoveFocus = 3,
            UpdateSourceAndSelectAll = 6
        }

        /// <summary>
        /// Using a DependencyProperty as the backing store for ReturnBehavior.
        /// This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty ReturnBehaviorProperty =
            DependencyProperty.RegisterAttached("ReturnBehavior", typeof(ReturnBehaviorMode), typeof(TextBoxProperties),
                new PropertyMetadata(ReturnBehaviorMode.None, OnReturnBehaviorChanged));

        public static ReturnBehaviorMode GetReturnBehavior(DependencyObject d)
        {
            return (ReturnBehaviorMode)d.GetValue(ReturnBehaviorProperty);
        }

        public static void SetReturnBehavior(DependencyObject d, ReturnBehaviorMode value)
        {
            d.SetValue(ReturnBehaviorProperty, value);
        }

        static void OnReturnBehaviorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(e.NewValue is ReturnBehaviorMode mode)) { return; }
            if (!(d is TextBox tb)) { return; }

            if (mode != ReturnBehaviorMode.None)
            {
                tb.PreviewKeyDown += OnPreviewKeyDown;
            }
            else
            {
                tb.PreviewKeyDown -= OnPreviewKeyDown;
            }
        }

        static void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != System.Windows.Input.Key.Enter
                || !(sender is TextBox tb)
                || tb.AcceptsReturn)
            {
                return;
            }

            var mode = GetReturnBehavior(tb);

            if (mode == ReturnBehaviorMode.None) { return; }

            var modifiers = Keyboard.Modifiers;

            if (modifiers == ModifierKeys.None)
            {
                if (mode.HasFlag(ReturnBehaviorMode.UpdateSource))
                {
                    var bindingExpression = BindingOperations.GetBindingExpression(tb, TextBox.TextProperty);
                    bindingExpression?.UpdateSource();
                    e.Handled = true;
                }

                if (mode.HasFlag(ReturnBehaviorMode.SelectAll))
                {
                    tb.SelectAll();
                    e.Handled = true;
                }
            }

            if (modifiers == ModifierKeys.None || modifiers == ModifierKeys.Shift)
            {
                if (mode.HasFlag(ReturnBehaviorMode.MoveFocus))
                {
                    var direction = modifiers == ModifierKeys.Shift ? FocusNavigationDirection.Previous : FocusNavigationDirection.Next;
                    var focused = FocusManager.GetFocusedElement(tb) as FrameworkElement;
                    if (focused == null && tb.IsFocused)
                    {
                        focused = tb;
                    }
                    focused?.MoveFocus(new TraversalRequest(direction));

                    e.Handled = true;
                }
            }
        }

        #endregion
    }
}