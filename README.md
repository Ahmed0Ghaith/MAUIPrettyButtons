# MauiPrettyButtons 🎯

> A collection of beautifully animated, press-responsive buttons for **.NET MAUI**.

[![NuGet](https://img.shields.io/nuget/v/MauiPrettyButtons?style=flat-square&color=6C63FF)](https://www.nuget.org/packages/MauiPrettyButtons)
[![Platform](https://img.shields.io/badge/Platform-iOS%20%7C%20Android%20%7C%20Windows%20%7C%20macOS-blue?style=flat-square)](https://learn.microsoft.com/en-us/dotnet/maui/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg?style=flat-square)](LICENSE)

---

## ✨ Button Showcase

| Button | Description |
|--------|-------------|
| **LoadingButton** | Spins a loader and disables itself while `IsLoading = true` |
| **FloatingActionButton** | Material FAB with bounce press, extended mode, and badge support |
| **RippleButton** | Press-ripple with selectable `Wave` or `Liquid` animation |
| **PulseButton** | Continuously pulses to attract attention (great for CTAs) |
| **ToggleButton** | Two-state button with smooth color/text crossfade |
| **IconButton** | Icon-only in Filled, Ghost, or Outline style with optional label |
| **MorphButton** | Morphs to a circle showing a success icon after click, then resets |
| **OutlinedButton** | Classic ghost/outline button with scale press animation |

All buttons feature:
- ⚡ **Press scale animation** (configurable scale + easing)
- 🎨 **Full color & corner radius customization**
- 🔗 **ICommand binding** + `Clicked` / `Pressed` / `Released` events
- ♿ **Disabled state** (opacity + tap blocked)
- 🌑 **Optional drop shadow**

---

## 📦 Installation

```bash
dotnet add package MauiPrettyButtons
```

Or via NuGet Package Manager:
```
Install-Package MauiPrettyButtons
```

---

## 🚀 Setup

Register in `MauiProgram.cs`:

```csharp
using MauiPrettyButtons;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiPrettyButtons(); // 👈 Add this
        return builder.Build();
    }
}
```

Add the XMLNS namespace to your XAML pages:

```xml
<ContentPage
    xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
    xmlns:mab="http://MauiPrettyButtons.com/dotnet">
```

---

## 🧩 Usage Examples

### LoadingButton

```xml
<mab:LoadingButton
    Text="Submit Order"
    LoadingText="Processing..."
    IsLoading="{Binding IsBusy}"
    Command="{Binding SubmitCommand}"
    Background="#6C63FF"
    CornerRadius="14"
    SpinnerColor="White"
    PaddingButton="24,16" />
```

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `Text` | string | "Button" | Label in idle state |
| `LoadingText` | string | "Loading..." | Label while loading |
| `IsLoading` | bool | false | Activates spinner & disables tap |
| `SpinnerColor` | Color | White | Spinner arc color |
| `SpinnerSize` | double | 22 | Spinner diameter |

---

### FloatingActionButton

```xml
<!-- Circle FAB -->
<mab:FloatingActionButton
    Icon="+"
    Size="56"
    Background="#FF5252"
    Command="{Binding AddCommand}"
    BadgeCount="{Binding NotificationCount}" />

<!-- Extended FAB -->
<mab:FloatingActionButton
    Icon="+"
    Text="New Task"
    IsExtended="True"
    Background="#6C63FF" />
```

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `Icon` | string | "+" | Glyph/emoji icon |
| `IconFontFamily` | string | null | Custom icon font (e.g. MaterialIcons) |
| `Text` | string | "" | Label (only shown when `IsExtended=True`) |
| `IsExtended` | bool | false | Extended FAB mode |
| `Size` | double | 56 | Diameter (height in extended mode) |
| `BadgeCount` | int | 0 | Notification badge (hidden when 0) |

---

### RippleButton

```xml
<mab:RippleButton
    Text="Tap Me"
    RippleColor="White"
    RippleOpacity="0.35"
    AnimationMode="Liquid"
    Background="#00BCD4"
    CornerRadius="8" />
```

---

### PulseButton

```xml
<mab:PulseButton
    Text="Subscribe Now"
    IsPulsing="True"
    PulseScale="1.06"
    PulseDuration="900"
    Background="#FF5252" />
```

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `IsPulsing` | bool | true | Start/stop the pulse loop |
| `PulseScale` | double | 1.05 | Max scale during pulse |
| `PulseDuration` | uint | 900 | Half-period in ms |

---

### ToggleButton

```xml
<mab:ToggleButton
    OffText="☆ Follow"
    OnText="★ Following"
    IsToggled="{Binding IsFollowing, Mode=TwoWay}"
    OffBackgroundColor="#6C63FF"
    OnBackgroundColor="#4CAF50"
    ToggledCommand="{Binding ToggleCommand}" />
```

Code-behind event:
```csharp
toggleBtn.Toggled += (s, isOn) => Console.WriteLine($"Now: {isOn}");
```

---

### IconButton

```xml
<!-- Ghost style -->
<mab:IconButton
    Icon="♡"
    LabelText="Like"
    ButtonStyle="Ghost"
    IconColor="#FF5252" />

<!-- Filled style -->
<mab:IconButton
    Icon="★"
    ButtonStyle="Filled"
    Background="#FFC107"
    IconColor="White"
    IconSize="20" />

<!-- Outline style -->
<mab:IconButton
    Icon="⚙"
    ButtonStyle="Outline"
    IconColor="#6C63FF" />
```

---

### MorphButton

```xml
<mab:MorphButton
    Text="Confirm Payment"
    SuccessIcon="✓"
    ResetAfterMillis="2000"
    Background="#4CAF50"
    Command="{Binding ConfirmCommand}" />
```

---

### OutlinedButton

```xml
<mab:OutlinedButton
    Text="Learn More"
    StrokeColor="#6C63FF"
    TextColor="#6C63FF"
    StrokeThickness="2"
    CornerRadius="10" />
```

---

## ⚙️ Shared Base Properties

All buttons inherit from `AnimatedButtonBase` which exposes:

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `Command` | ICommand | null | Executed on tap |
| `CommandParameter` | object | null | Passed to Command |
| `IsEnabledButton` | bool | true | Enable/disable (with 0.45 opacity) |
| `PressScale` | double | 0.93 | Scale on press (0–1) |
| `PressAnimationDuration` | uint | 100 | Press anim speed (ms) |
| `CornerRadius` | float | 12 | Border corner radius |
| `Background` | Brush | Solid #6C63FF | Button background (supports gradients) |
| `ButtonBackgroundColor` | Color | #6C63FF | Legacy alias for solid color background |
| `ShadowEnabled` | bool | true | Drop shadow |

### Events

```csharp
button.Clicked  += (s, e) => { };
button.Pressed  += (s, e) => { };
button.Released += (s, e) => { };
```

---

## 🎨 Theming Example

```xml
<ResourceDictionary>
    <Style TargetType="mab:LoadingButton">
        <Setter Property="Background" Value="{StaticResource PrimaryBrush}" />
        <Setter Property="CornerRadius" Value="16" />
        <Setter Property="ShadowEnabled" Value="True" />
        <Setter Property="FontSize" Value="15" />
    </Style>
</ResourceDictionary>
```

---

## 📋 Requirements

- .NET 9.0
- .NET MAUI (net9.0-android / net9.0-ios / net9.0-maccatalyst / net9.0-windows10.0.19041.0)

---

## 📄 License

MIT © 2025 — Free to use in personal and commercial projects.
