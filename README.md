# nsAttachedProperties (NishySoftware.Wpf.AttachedProperties)

[Click here](https://nishy-software.com/ja/nsAttachedProperties/) for Japanese page (日本語ページは[こちら](https://nishy-software.com/ja/nsAttachedProperties/))

## Development status

[![Build Status (master)](https://nishy-software.visualstudio.com/nsAttachedProperties/_apis/build/status/nishy2000.nsAttachedProperties?branchName=master&label=master)](https://nishy-software.visualstudio.com/nsAttachedProperties/_build/latest?definitionId=13&branchName=master)
[![Build Status (develop)](https://nishy-software.visualstudio.com/nsAttachedProperties/_apis/build/status/nishy2000.nsAttachedProperties?branchName=develop&label=develop)](https://nishy-software.visualstudio.com/nsAttachedProperties/_build/latest?definitionId=13&branchName=develop)

[![Downloads](https://img.shields.io/nuget/dt/NishySoftware.Wpf.AttachedProperties.svg?style=flat-square)](http://www.nuget.org/packages/NishySoftware.Wpf.AttachedProperties/)
[![Nuget](https://img.shields.io/nuget/v/NishySoftware.Wpf.AttachedProperties.svg?style=flat-square)](http://nuget.org/packages/NishySoftware.Wpf.AttachedProperties)
[![Nuget (pre)](https://img.shields.io/nuget/vpre/NishySoftware.Wpf.AttachedProperties.svg?style=flat-square&label=nuget-pre)](http://nuget.org/packages/NishySoftware.Wpf.AttachedProperties)
[![Release](https://img.shields.io/github/release/nishy2000/nsAttachedProperties.svg?style=flat-square)](https://github.com/nishy2000/nsAttachedProperties/releases/latest)

[![Issues](https://img.shields.io/github/issues/nishy2000/nsAttachedProperties.svg?style=flat-square)](https://github.com/nishy2000/nsAttachedProperties/issues)
[![Issues](https://img.shields.io/github/issues-closed/nishy2000/nsAttachedProperties.svg?style=flat-square)](https://github.com/nishy2000/nsAttachedProperties/issues?q=is%3Aissue+is%3Aclosed)

[![License](https://img.shields.io/badge/license-Apatch2.0-blue.svg?style=flat-square)](https://github.com/nishy2000/nsAttachedProperties/blob/master/License)

## About

nsAttachedProperties (NishySoftware.Wpf.AttachedProperties) is a library of attached properties that can be used to extend the functionality and change the behavior of existing WPF controls.

## License

This library is under [the Apache-2.0 License](LICENSE).

## Installation

Install NuGet package(s).

```powershell
PM> Install-Package NishySoftware.Wpf.AttachedProperties
```

* [NishySoftware.Wpf.AttachedProperties](https://www.nuget.org/packages/NishySoftware.Wpf.AttachedProperties/) - nsAttachedProperties library.

## Change history

The change history of this library is [here](Changelog.md).

## Features / How to use
The attached properties provided by this library can be used to extend the functionality and change the behavior of existing WPF controls.
To specify a namespace in xaml, use `xmlns:nsAttachedProps="http://schemas.nishy-software.com/xaml/attached-properties"` or `xmlns:nsXaml="http://schemas.nishy-software.com/xaml"`.

```xml
<Window
  xmlns:nsAttachedProps="http://schemas.nishy-software.com/xaml/attached-properties"
  ...
  >
...
</Window>
```

## ScrollViewerProperties class
**Namespace**: NishySoftware.Wpf.AttachedProperties  
**Assembly**: nsAttachedProperties.Wpf.dll  
**nuget packageId**: [NishySoftware.Wpf.AttachedProperties](https://www.nuget.org/packages/NishySoftware.Wpf.AttachedProperties/)

The attached properties provided by ScrollViewerProperties are attached properties that can be used to extend the functionality and change the behavior of ScrollViewer control.
 - MouseWheelHandlingMode


### MouseWheelHandlingMode enum
Specify the handling mode of mouse wheel events of ScrollViewer for scrollable contents.

This enumeration value is used in the ScrollViewerProperties.MouseWheelHandlingMode attached property.
```csharp
public enum MouseWheelHandlingMode
{
    Inherit,
    Normal,
    OnlyVisible,
    OnlyScrollable,
}
```

| Value | Behavior |
| --- | --- |
| **Inherit** | Depends on the inheritance value of the parent. This value is default. |
| **Normal** | Always handle the mouse wheel event. Original behavior of ScrollViewer |
| **OnlyVisible** | Handle the mouse wheel event only when the vertical scroll bar is visible.  |
| **OnlyScrollable** | Handle mouse wheel events only when the vertical scroll bar can scroll in the direction of the mouse wheel rotation. |

### MouseWheelHandlingMode attached property

This attached property can be used to improve the scrolling behavior of a nested ScrollViewer by rotating the mouse wheel.

As a concrete behavior, this attached property can be used to prevent the inner ScrollViewer from always handling the mouse wheel events when the ScrollViewer is nested.

The behavior for handling mouse wheel events is specified by the MouseWheelHandlingMode enumeration value.

This attached property is mainly used for controls that use ScrollViewer to display content, such as DataGrid/ListView/ListBox.
However, it can also be set for Window, Grid, GroupBox, etc.

If this attached property is set to a FrameworkEelement other than ScrollViewer, its value will be inherited by the child ScrollViewers of that FrameworkEelement.
The ScrollViewer whose value is inherited either does not have the MouseWheelHandlingMode attached property or has the MouseWheelHandlingMode attached property set to Inherit.

If this attached property is set to a ScrollViewer, the value of that ScrollViewer will not be inherited by the child ScrollViewer.


However, in order not to affect the performance of WPF, the timing of searching for the ScrollViewer of a child element is limited to
"when the value of the attached property of that control is changed" and "when the Loaded event of that control is fired".

Therefore, the value of the attached property set in the parent control may not be reflected in the ScrollViewer of some child elements.

For example, in TabControl's TabItem, etc., the controls in its content are instantiated when the TabItem becomes active.
The Loaded event that occurs when the content is instantiated is not propagated to the TabControl or TabItem.
Therefore, the value of the attached property set in the TabControl or TabItem will not be reflected in the content in the TabItem.
In this case, it is necessary to set the attached property to the most parent control in the content of the TabItem.

If the value of the attached property is sometimes not reflected in the ScrollViewer of the child element, this may be the situation.
In this case, you can set the attached property for the control that is going to be instantiated later and it will be reflected.

#### Examples
Example for nsAttachedProps:ScrollViewerProperties.MouseWheelHandlingMode
```xml
<Window
  ...
  xmlns:nsAttachedProps="http://schemas.nishy-software.com/xaml/attached-properties">
    <ScrollViewer Grid.Row="1"
                  Width="300"
                  Height="300">
...
        <DataGrid Grid.Row="1"
                  ItemsSource="{Binding Source={StaticResource SmallDataItemsView1}}"
                  nsAttachedProps:ScrollViewerProperties.MouseWheelHandlingMode="OnlyScrollable"/>
        </DataGrid>
...
```

## TextBoxProperties class
**Namespace**: NishySoftware.Wpf.AttachedProperties  
**Assembly**: nsAttachedProperties.Wpf.dll  
**nuget packageId**: [NishySoftware.Wpf.AttachedProperties](https://www.nuget.org/packages/NishySoftware.Wpf.AttachedProperties/)

The attached properties provided by TextBoxProperties are attached properties that can be used to extend the functionality and change the behavior of TextBox control.
 - ReturnBehavior


### ReturnBehaviorMode enum
Specify the behavior of the TextBox control when the Enter key is pressed.

This enumeration value is used in the TextBoxProperties.ReturnBehavior attached property.
```csharp
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
```

| Name | Value | Behavior when inputing Enter key |
| --- | --- | --- |
| **None** | 0 | The behavior is not extended. Original behavior of TextBox. This value is default. |
| **MoveFocus** | 1 | Move the focus to the next element if the Enter key is pressed alone. Move the focus to the previous element if the Enter and Shift keys are pressed at the same time.|
| **UpdateSource** | 2 | Update binding source if TextBox.Text property is binding.  |
| **SelectAll** | 4 | Select all the text.  |
| **UpdateSourceAndMoveFocus** | 3 | ReturnBehaviorMode.MoveFocus behavior after ReturnBehaviorMode.UpdateSource behavior. |
| **UpdateSourceAndSelectAll** | 6 | ReturnBehaviorMode.SelectAll behavior after ReturnBehaviorMode.UpdateSource behavior. |

### ReturnBehavior attached property

This attached property can be used to improve the behavior of the TextBox control when the Enter key is pressed.
The behavior when the Enter key is pressed is specified using the ReturnBehaviorMode enumeration value.
There are many values defined in the ReturnBehaviorMode enumeration, but the most commonly used are MoveFocus, UpdateSource, BBBB, and UpdateSourceAndSelectAll.

If this attached property is set to a FrameworkEelement other than the TextBox control, it will be ignored.

If this attached property is set to a TextBox control that has AcceptsReturn property set to true, it will be ignored.

#### Examples
Example for nsAttachedProps:TextBoxProperties.ReturnBehavior
```xml
<Window
  ...
  xmlns:nsAttachedProps="http://schemas.nishy-software.com/xaml/attached-properties">
    <Grid>
...
        <TextBox Grid.Row="0"
                 nsAttachedProps:TextBoxProperties.ReturnBehavior="MoveFocus"
                 Text="{Binding EditBoxValue0}"/>
        <TextBox Grid.Row="1"
                 nsAttachedProps:TextBoxProperties.ReturnBehavior="UpdateSource"
                 Text="{Binding EditBoxValue1}"/>
        <TextBox Grid.Row="2"
                 nsAttachedProps:TextBoxProperties.ReturnBehavior="UpdateSourceAndSelectAll"
                 Text="{Binding EditBoxValue2}"/>
...
```
