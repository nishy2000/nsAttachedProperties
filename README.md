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

The attached properties provided by ScrollViewerProperties are attached properties that can be used to extend the functionality and change the behavior of ScrollViewer control.
 - MouseWheelHandlingMode


### MouseWheelHandlingMode enum
Specifies the handling mode of mouse wheel events of ScrollViewer for scrollable contents.

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
| --- | --- | --- |
| **Inherit** | Depends on the inheritance value of the parent. This value is default. |
| **Normal** | Always handle the mouse wheel event. Original behavior of ScrollViewer |
| **OnlyVisible** | Handle the mouse wheel event only when the vertical scroll bar is visible.  |
| **OnlyScrollable** | Handle mouse wheel events only when the vertical scroll bar can scroll in the direction of the mouse wheel rotation. |

### MouseWheelHandlingMode attached property

This attached property can be used to improve the scrolling behavior of a nested ScrollViewer by rotating the mouse wheel.

As a concrete behavior, this attached property can be used to prevent the inner ScrollViewer from always handling the mouse wheel events when the ScrollViewer is nested.

This attached property is mainly used for controls that use ScrollViewer to display content, such as DataGrid/ListView/ListBox.
However, it can also be set for Window, Grid, GroupBox, etc.

If this attached property is set to a FrameworkEelement other than ScrollViewer, its value will be inherited by the child ScrollViewers of that FrameworkEelement.
The ScrollViewer whose value is inherited either does not have the MouseWheelHandlingMode attached property or has the MouseWheelHandlingMode attached property set to Inherit.

If this attached property is set to a ScrollViewer, the value of that ScrollViewer will not be inherited by the child ScrollViewer.

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
