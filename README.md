# OdometerCounterViewMaui

An animated odometer-style counter control for .NET MAUI applications. This control provides smooth scrolling transitions for numbers, perfect for displaying changing values like mileage, scores, currency, or counts.

## Demo

| Android | iPhone |
| :---: | :---: |
| <img src="ODOMETER.gif" height="400" /> | <img src="ODOMETER iPhone.gif" height="400" /> |

## Features

- **Smooth Animations**: Digits roll smoothly to their new values with a customizable visual style.
- **Customizable Appearance**: Full support for standard font customization including:
    - `FontSize`
    - `FontFamily`
    - `FontAttributes`
    - `TextColor`
    - `DurationMs`
- **Flexible Data Binding**: Simply bind the `Value` property to any integer to trigger the animation.
- **Dynamic Sizing**: The control automatically sizes itself based on the number of digits and font settings.
- **Performance Optimized**: efficiently handles updates to minimize UI thread impact.

## Usage

Simply add the namespace and use the `OdometerView` control in your XAML.

### Basic Usage

```xml
<xmlns:controls="clr-namespace:OdometerCounterViewMaui;assembly=OdometerCounterViewMaui">

<controls:OdometerView 
    Value="12345"
    FontSize="50"
    DurationMs="500"
    TextColor="{StaticResource Primary}" />
```

### Binding Example

```xml
<controls:OdometerView 
    Value="{Binding CurrentCount}"
    FontSize="32"
    FontAttributes="Bold"
    DurationMs="1500"
    TextColor="Black" />
```

## Installation

[![NuGet](https://img.shields.io/nuget/v/OdometerCounterViewMaui.svg)](https://www.nuget.org/packages/OdometerCounterViewMaui/)

You can install the package via the NuGet Package Manager or the CLI:

```bash
dotnet add package OdometerCounterViewMaui
```