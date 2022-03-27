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

            if (!(d is Control ctrl)) { return; }
            if (!(ctrl is TextBox) && !(ctrl is ComboBox)) { return; }

            if (mode != ReturnBehaviorMode.None)
            {
                ctrl.PreviewKeyDown += OnPreviewKeyDown;
            }
            else
            {
                ctrl.PreviewKeyDown -= OnPreviewKeyDown;
            }
        }

        static MethodInfo _getTemplateChildMethodInfo;
        static DependencyObject GetTemplateChild(FrameworkElement fe, string childName)
        {
            if (fe == null)
            {
                return null;
            }

            if (_getTemplateChildMethodInfo == null)
            {
                // Get PropInfo of internal property using reflection
                var feType = typeof(FrameworkElement);
                _getTemplateChildMethodInfo = feType.GetMethod("GetTemplateChild", BindingFlags.NonPublic | BindingFlags.Instance);
            }

            return _getTemplateChildMethodInfo?.Invoke(fe, new object [] { childName }) as DependencyObject;
        }

        static void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != System.Windows.Input.Key.Enter
                || !(sender is DependencyObject owner))
            {
                return;
            }

            var tb = sender as TextBox;
            ComboBox cb = null;
            if (tb == null)
            {
                cb = sender as ComboBox;
                if (cb != null
                    && cb.IsEditable && !cb.IsReadOnly)
                {
                    tb = GetTemplateChild(cb, "PART_EditableTextBox") as TextBox;
                }
            }

            if (tb == null
                || tb.AcceptsReturn)
            {
                return;
            }

            var mode = GetReturnBehavior(owner);

            if (mode == ReturnBehaviorMode.None) { return; }

            var modifiers = Keyboard.Modifiers;

            if (modifiers == ModifierKeys.None)
            {
                if (mode.HasFlag(ReturnBehaviorMode.UpdateSource))
                {
                    var dp = cb != null ? ComboBox.TextProperty : TextBox.TextProperty;
                    var bindingExpression = BindingOperations.GetBindingExpression(owner, dp);
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