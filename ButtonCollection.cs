namespace MauiPrettyButtons.Controls;

// ══════════════════════════════════════════════════════════════════
// RIPPLE BUTTON
// ══════════════════════════════════════════════════════════════════

/// <summary>
/// A button with a ripple-wave effect radiating from the center on press.
///
/// XAML Usage:
/// <code>
/// &lt;mab:RippleButton
///     Text="Tap Me"
///     RippleColor="#FFFFFF"
///     RippleOpacity="0.35"
///     ButtonBackgroundColor="#6C63FF" /&gt;
/// </code>
/// </summary>
public class RippleButton : AnimatedButtonBase
{
    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(nameof(Text), typeof(string), typeof(RippleButton), "Button",
            propertyChanged: (b, _, n) => ((RippleButton)b).UpdateLabel());

    public static readonly BindableProperty TextColorProperty =
        BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(RippleButton), Colors.White,
            propertyChanged: (b, _, n) => ((RippleButton)b).UpdateLabel());

    public static readonly BindableProperty FontSizeProperty =
        BindableProperty.Create(nameof(FontSize), typeof(double), typeof(RippleButton), 15.0,
            propertyChanged: (b, _, n) => ((RippleButton)b).UpdateLabel());

    public static readonly BindableProperty RippleColorProperty =
        BindableProperty.Create(nameof(RippleColor), typeof(Color), typeof(RippleButton), Colors.White);

    public static readonly BindableProperty RippleOpacityProperty =
        BindableProperty.Create(nameof(RippleOpacity), typeof(double), typeof(RippleButton), 0.35);

    public string Text { get => (string)GetValue(TextProperty); set => SetValue(TextProperty, value); }
    public Color TextColor { get => (Color)GetValue(TextColorProperty); set => SetValue(TextColorProperty, value); }
    public double FontSize { get => (double)GetValue(FontSizeProperty); set => SetValue(FontSizeProperty, value); }
    public Color RippleColor { get => (Color)GetValue(RippleColorProperty); set => SetValue(RippleColorProperty, value); }
    public double RippleOpacity { get => (double)GetValue(RippleOpacityProperty); set => SetValue(RippleOpacityProperty, value); }

    private Border? _border;
    private Label? _label;
    private BoxView? _ripple;
    private Grid? _grid;

    protected override void BuildContent()
    {
        PressScale = 0.95;

        _label = new Label
        {
            Text = Text,
            TextColor = TextColor,
            FontSize = FontSize,
            FontAttributes = FontAttributes.Bold,
            HorizontalTextAlignment = TextAlignment.Center,
            VerticalTextAlignment = TextAlignment.Center
        };

        _ripple = new BoxView
        {
            Color = RippleColor,
            Opacity = 0,
            CornerRadius = 200,
            WidthRequest = 0,
            HeightRequest = 0,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        };

        _grid = new Grid
        {
            Children = { _ripple, _label }
        };

        _border = new Border
        {
            BackgroundColor = ButtonBackgroundColor,
            Padding = new Thickness(24, 14),
            StrokeThickness = 0,
            StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle
            {
                CornerRadius = new CornerRadius(CornerRadius)
            },
            Content = _grid
        };

        Content = _border;
    }

    protected override async void OnButtonClicked()
    {
        if (_ripple == null) return;
        _ripple.Opacity = RippleOpacity;
        _ripple.WidthRequest = 0;
        _ripple.HeightRequest = 0;
        var expand = _ripple.ScaleTo(20, 400, Easing.CubicOut);
        var fade = _ripple.FadeTo(0, 400, Easing.CubicIn);
        await Task.WhenAll(expand, fade);
        _ripple.Scale = 1;
    }

    private void UpdateLabel()
    {
        if (_label == null) return;
        _label.Text = Text;
        _label.TextColor = TextColor;
        _label.FontSize = FontSize;
    }

    protected override void OnButtonBackgroundColorChanged(Color c)
    {
        if (_border != null) _border.BackgroundColor = c;
    }

    protected override void OnCornerRadiusChanged(float r)
    {
        if (_border != null)
            _border.StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle
            {
                CornerRadius = new CornerRadius(r)
            };
    }
}

// ══════════════════════════════════════════════════════════════════
// PULSE BUTTON
// ══════════════════════════════════════════════════════════════════

/// <summary>
/// A button that continuously pulses to draw attention (great for CTAs).
/// Pulse pauses on press and resumes after.
///
/// XAML Usage:
/// <code>
/// &lt;mab:PulseButton
///     Text="Subscribe"
///     IsPulsing="True"
///     PulseScale="1.06"
///     PulseDuration="900"
///     ButtonBackgroundColor="#FF5252" /&gt;
/// </code>
/// </summary>
public class PulseButton : AnimatedButtonBase
{
    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(nameof(Text), typeof(string), typeof(PulseButton), "Button",
            propertyChanged: (b, _, n) => ((PulseButton)b).UpdateLabel());

    public static readonly BindableProperty TextColorProperty =
        BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(PulseButton), Colors.White,
            propertyChanged: (b, _, n) => ((PulseButton)b).UpdateLabel());

    public static readonly BindableProperty FontSizeProperty =
        BindableProperty.Create(nameof(FontSize), typeof(double), typeof(PulseButton), 15.0,
            propertyChanged: (b, _, n) => ((PulseButton)b).UpdateLabel());

    public static readonly BindableProperty IsPulsingProperty =
        BindableProperty.Create(nameof(IsPulsing), typeof(bool), typeof(PulseButton), true,
            propertyChanged: (b, _, n) => ((PulseButton)b).OnPulsingChanged((bool)n));

    public static readonly BindableProperty PulseScaleProperty =
        BindableProperty.Create(nameof(PulseScale), typeof(double), typeof(PulseButton), 1.05);

    public static readonly BindableProperty PulseDurationProperty =
        BindableProperty.Create(nameof(PulseDuration), typeof(uint), typeof(PulseButton), (uint)900);

    public string Text { get => (string)GetValue(TextProperty); set => SetValue(TextProperty, value); }
    public Color TextColor { get => (Color)GetValue(TextColorProperty); set => SetValue(TextColorProperty, value); }
    public double FontSize { get => (double)GetValue(FontSizeProperty); set => SetValue(FontSizeProperty, value); }
    public bool IsPulsing { get => (bool)GetValue(IsPulsingProperty); set => SetValue(IsPulsingProperty, value); }
    public double PulseScale { get => (double)GetValue(PulseScaleProperty); set => SetValue(PulseScaleProperty, value); }
    public uint PulseDuration { get => (uint)GetValue(PulseDurationProperty); set => SetValue(PulseDurationProperty, value); }

    private Border? _border;
    private Label? _label;
    private bool _pulseRunning;

    protected override void BuildContent()
    {
        _label = new Label
        {
            Text = Text,
            TextColor = TextColor,
            FontSize = FontSize,
            FontAttributes = FontAttributes.Bold,
            HorizontalTextAlignment = TextAlignment.Center,
            VerticalTextAlignment = TextAlignment.Center
        };

        _border = new Border
        {
            BackgroundColor = ButtonBackgroundColor,
            Padding = new Thickness(24, 14),
            StrokeThickness = 0,
            StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle
            {
                CornerRadius = new CornerRadius(CornerRadius)
            },
            Content = _label
        };

        Content = _border;

        if (IsPulsing) StartPulse();
    }

    private void OnPulsingChanged(bool pulsing)
    {
        if (pulsing) StartPulse();
        else StopPulse();
    }

    private void StartPulse()
    {
        if (_pulseRunning) return;
        _pulseRunning = true;
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            while (_pulseRunning)
            {
                await this.ScaleTo(PulseScale, PulseDuration, Easing.SinInOut);
                await this.ScaleTo(1.0, PulseDuration, Easing.SinInOut);
            }
        });
    }

    private void StopPulse()
    {
        _pulseRunning = false;
        this.CancelAnimations();
        this.ScaleTo(1.0, 200);
    }

    private void UpdateLabel()
    {
        if (_label == null) return;
        _label.Text = Text;
        _label.TextColor = TextColor;
        _label.FontSize = FontSize;
    }

    protected override void OnButtonBackgroundColorChanged(Color c)
    {
        if (_border != null) _border.BackgroundColor = c;
    }

    protected override void OnCornerRadiusChanged(float r)
    {
        if (_border != null)
            _border.StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle
            {
                CornerRadius = new CornerRadius(r)
            };
    }
}

// ══════════════════════════════════════════════════════════════════
// TOGGLE BUTTON
// ══════════════════════════════════════════════════════════════════

/// <summary>
/// A two-state toggle button with smooth color and text crossfade animation.
///
/// XAML Usage:
/// <code>
/// &lt;mab:ToggleButton
///     OffText="Follow"
///     OnText="Following ✓"
///     IsToggled="{Binding IsFollowing}"
///     OffBackgroundColor="#6C63FF"
///     OnBackgroundColor="#4CAF50"
///     ToggledCommand="{Binding ToggleFollowCommand}" /&gt;
/// </code>
/// </summary>
public class ToggleButton : AnimatedButtonBase
{
    public static readonly BindableProperty OffTextProperty =
        BindableProperty.Create(nameof(OffText), typeof(string), typeof(ToggleButton), "Off",
            propertyChanged: (b, _, n) => ((ToggleButton)b).Refresh());

    public static readonly BindableProperty OnTextProperty =
        BindableProperty.Create(nameof(OnText), typeof(string), typeof(ToggleButton), "On",
            propertyChanged: (b, _, n) => ((ToggleButton)b).Refresh());

    public static readonly BindableProperty IsToggledProperty =
        BindableProperty.Create(nameof(IsToggled), typeof(bool), typeof(ToggleButton), false,
            propertyChanged: (b, _, n) => ((ToggleButton)b).OnToggledChanged((bool)n));

    public static readonly BindableProperty OffBackgroundColorProperty =
        BindableProperty.Create(nameof(OffBackgroundColor), typeof(Color), typeof(ToggleButton), Color.FromArgb("#6C63FF"),
            propertyChanged: (b, _, n) => ((ToggleButton)b).Refresh());

    public static readonly BindableProperty OnBackgroundColorProperty =
        BindableProperty.Create(nameof(OnBackgroundColor), typeof(Color), typeof(ToggleButton), Color.FromArgb("#4CAF50"),
            propertyChanged: (b, _, n) => ((ToggleButton)b).Refresh());

    public static readonly BindableProperty TextColorProperty =
        BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(ToggleButton), Colors.White,
            propertyChanged: (b, _, n) => ((ToggleButton)b).Refresh());

    public static readonly BindableProperty ToggledCommandProperty =
        BindableProperty.Create(nameof(ToggledCommand), typeof(System.Windows.Input.ICommand), typeof(ToggleButton));

    public string OffText { get => (string)GetValue(OffTextProperty); set => SetValue(OffTextProperty, value); }
    public string OnText { get => (string)GetValue(OnTextProperty); set => SetValue(OnTextProperty, value); }
    public bool IsToggled { get => (bool)GetValue(IsToggledProperty); set => SetValue(IsToggledProperty, value); }
    public Color OffBackgroundColor { get => (Color)GetValue(OffBackgroundColorProperty); set => SetValue(OffBackgroundColorProperty, value); }
    public Color OnBackgroundColor { get => (Color)GetValue(OnBackgroundColorProperty); set => SetValue(OnBackgroundColorProperty, value); }
    public Color TextColor { get => (Color)GetValue(TextColorProperty); set => SetValue(TextColorProperty, value); }
    public System.Windows.Input.ICommand? ToggledCommand { get => (System.Windows.Input.ICommand?)GetValue(ToggledCommandProperty); set => SetValue(ToggledCommandProperty, value); }

    public event EventHandler<bool>? Toggled;

    private Border? _border;
    private Label? _label;

    protected override void BuildContent()
    {
        _label = new Label
        {
            Text = OffText,
            TextColor = TextColor,
            FontSize = 15,
            FontAttributes = FontAttributes.Bold,
            HorizontalTextAlignment = TextAlignment.Center,
            VerticalTextAlignment = TextAlignment.Center
        };

        _border = new Border
        {
            BackgroundColor = OffBackgroundColor,
            Padding = new Thickness(24, 14),
            StrokeThickness = 0,
            StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle
            {
                CornerRadius = new CornerRadius(CornerRadius)
            },
            Content = _label
        };

        Content = _border;
    }

    protected override void OnButtonClicked()
    {
        IsToggled = !IsToggled;
        Toggled?.Invoke(this, IsToggled);
        if (ToggledCommand?.CanExecute(IsToggled) == true)
            ToggledCommand.Execute(IsToggled);
    }

    private async void OnToggledChanged(bool toggled)
    {
        if (_border == null || _label == null) return;
        await _label.FadeTo(0, 100);
        _label.Text = toggled ? OnText : OffText;
        _border.BackgroundColor = toggled ? OnBackgroundColor : OffBackgroundColor;
        await _label.FadeTo(1, 100);
    }

    private void Refresh()
    {
        if (_label == null || _border == null) return;
        _label.Text = IsToggled ? OnText : OffText;
        _label.TextColor = TextColor;
        _border.BackgroundColor = IsToggled ? OnBackgroundColor : OffBackgroundColor;
    }

    protected override void OnCornerRadiusChanged(float r)
    {
        if (_border != null)
            _border.StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle
            {
                CornerRadius = new CornerRadius(r)
            };
    }
}

// ══════════════════════════════════════════════════════════════════
// ICON BUTTON
// ══════════════════════════════════════════════════════════════════

/// <summary>
/// A compact icon-only button with optional label below,
/// press rotation/scale animation, and a ghost/filled/outline style.
///
/// XAML Usage:
/// <code>
/// &lt;mab:IconButton
///     Icon="♡"
///     Label="Like"
///     Style="Ghost"
///     IconColor="#FF5252"
///     Command="{Binding LikeCommand}" /&gt;
/// </code>
/// </summary>
public class IconButton : AnimatedButtonBase
{
    public enum IconButtonStyle { Filled, Ghost, Outline }

    public static readonly BindableProperty IconProperty =
        BindableProperty.Create(nameof(Icon), typeof(string), typeof(IconButton), "★",
            propertyChanged: (b, _, n) => ((IconButton)b).Rebuild());

    public static readonly BindableProperty LabelTextProperty =
        BindableProperty.Create(nameof(LabelText), typeof(string), typeof(IconButton), string.Empty,
            propertyChanged: (b, _, n) => ((IconButton)b).Rebuild());

    public static readonly BindableProperty IconColorProperty =
        BindableProperty.Create(nameof(IconColor), typeof(Color), typeof(IconButton), Color.FromArgb("#6C63FF"),
            propertyChanged: (b, _, n) => ((IconButton)b).Rebuild());

    public static readonly BindableProperty IconSizeProperty =
        BindableProperty.Create(nameof(IconSize), typeof(double), typeof(IconButton), 26.0,
            propertyChanged: (b, _, n) => ((IconButton)b).Rebuild());

    public static readonly BindableProperty ButtonStyleProperty =
        BindableProperty.Create(nameof(ButtonStyle), typeof(IconButtonStyle), typeof(IconButton), IconButtonStyle.Ghost,
            propertyChanged: (b, _, n) => ((IconButton)b).Rebuild());

    public static readonly BindableProperty LabelColorProperty =
        BindableProperty.Create(nameof(LabelColor), typeof(Color), typeof(IconButton), Color.FromArgb("#6C63FF"),
            propertyChanged: (b, _, n) => ((IconButton)b).Rebuild());

    public string Icon { get => (string)GetValue(IconProperty); set => SetValue(IconProperty, value); }
    public string LabelText { get => (string)GetValue(LabelTextProperty); set => SetValue(LabelTextProperty, value); }
    public Color IconColor { get => (Color)GetValue(IconColorProperty); set => SetValue(IconColorProperty, value); }
    public double IconSize { get => (double)GetValue(IconSizeProperty); set => SetValue(IconSizeProperty, value); }
    public IconButtonStyle ButtonStyle { get => (IconButtonStyle)GetValue(ButtonStyleProperty); set => SetValue(ButtonStyleProperty, value); }
    public Color LabelColor { get => (Color)GetValue(LabelColorProperty); set => SetValue(LabelColorProperty, value); }

    protected override void BuildContent()
    {
        ShadowEnabled = false;
        PressScale = 0.85;
        PressAnimationDuration = 90;
        Rebuild();
    }

    private void Rebuild()
    {
        var iconLabel = new Label
        {
            Text = Icon,
            FontSize = IconSize,
            TextColor = ButtonStyle == IconButtonStyle.Filled ? Colors.White : IconColor,
            HorizontalTextAlignment = TextAlignment.Center,
            VerticalTextAlignment = TextAlignment.Center
        };

        View container;

        if (ButtonStyle == IconButtonStyle.Filled)
        {
            container = new Border
            {
                BackgroundColor = ButtonBackgroundColor,
                WidthRequest = IconSize + 20,
                HeightRequest = IconSize + 20,
                StrokeThickness = 0,
                StrokeShape = new Microsoft.Maui.Controls.Shapes.Ellipse(),
                Content = iconLabel
            };
        }
        else if (ButtonStyle == IconButtonStyle.Outline)
        {
            container = new Border
            {
                BackgroundColor = Colors.Transparent,
                WidthRequest = IconSize + 20,
                HeightRequest = IconSize + 20,
                Stroke = new SolidColorBrush(IconColor),
                StrokeThickness = 2,
                StrokeShape = new Microsoft.Maui.Controls.Shapes.Ellipse(),
                Content = iconLabel
            };
        }
        else // Ghost
        {
            container = iconLabel;
        }

        if (!string.IsNullOrEmpty(LabelText))
        {
            var lbl = new Label
            {
                Text = LabelText,
                TextColor = LabelColor,
                FontSize = 11,
                HorizontalTextAlignment = TextAlignment.Center,
                Margin = new Thickness(0, 2, 0, 0)
            };

            Content = new VerticalStackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                Spacing = 2,
                Children = { container, lbl }
            };
        }
        else
        {
            Content = container;
        }
    }

    protected override void OnButtonBackgroundColorChanged(Color c) => Rebuild();
    protected override void OnCornerRadiusChanged(float r) => Rebuild();
}

// ══════════════════════════════════════════════════════════════════
// MORPH BUTTON
// ══════════════════════════════════════════════════════════════════

/// <summary>
/// A button that morphs shape between a rectangle (idle) and a circle (pressed),
/// great for "confirm" actions, checkmarks, etc.
///
/// XAML Usage:
/// <code>
/// &lt;mab:MorphButton
///     Text="Confirm"
///     SuccessIcon="✓"
///     Command="{Binding ConfirmCommand}" /&gt;
/// </code>
/// </summary>
public class MorphButton : AnimatedButtonBase
{
    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(nameof(Text), typeof(string), typeof(MorphButton), "Confirm",
            propertyChanged: (b, _, n) => ((MorphButton)b).UpdateState());

    public static readonly BindableProperty SuccessIconProperty =
        BindableProperty.Create(nameof(SuccessIcon), typeof(string), typeof(MorphButton), "✓");

    public static readonly BindableProperty TextColorProperty =
        BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(MorphButton), Colors.White,
            propertyChanged: (b, _, n) => ((MorphButton)b).UpdateState());

    public static readonly BindableProperty ResetAfterMillisProperty =
        BindableProperty.Create(nameof(ResetAfterMillis), typeof(int), typeof(MorphButton), 1800);

    public string Text { get => (string)GetValue(TextProperty); set => SetValue(TextProperty, value); }
    public string SuccessIcon { get => (string)GetValue(SuccessIconProperty); set => SetValue(SuccessIconProperty, value); }
    public Color TextColor { get => (Color)GetValue(TextColorProperty); set => SetValue(TextColorProperty, value); }
    public int ResetAfterMillis { get => (int)GetValue(ResetAfterMillisProperty); set => SetValue(ResetAfterMillisProperty, value); }

    private Border? _border;
    private Label? _label;
    private bool _morphed;

    protected override void BuildContent()
    {
        _label = new Label
        {
            Text = Text,
            TextColor = TextColor,
            FontSize = 15,
            FontAttributes = FontAttributes.Bold,
            HorizontalTextAlignment = TextAlignment.Center,
            VerticalTextAlignment = TextAlignment.Center
        };

        _border = new Border
        {
            BackgroundColor = ButtonBackgroundColor,
            Padding = new Thickness(24, 14),
            StrokeThickness = 0,
            WidthRequest = -1,
            HeightRequest = 50,
            StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle
            {
                CornerRadius = new CornerRadius(12)
            },
            Content = _label
        };

        Content = _border;
    }

    protected override async void OnButtonClicked()
    {
        if (_morphed || _border == null || _label == null) return;
        _morphed = true;

        // Shrink to circle
        await _border.ScaleTo(0.9, 100);
        await _label.FadeTo(0, 80);

        _label.Text = SuccessIcon;
        _label.FontSize = 22;

        _border.StrokeShape = new Microsoft.Maui.Controls.Shapes.Ellipse();
        _border.WidthRequest = 50;
        _border.Padding = new Thickness(0);

        await Task.WhenAll(
            _border.ScaleTo(1.0, 200, Easing.SpringOut),
            _label.FadeTo(1, 150)
        );

        await Task.Delay(ResetAfterMillis);
        await ResetAsync();
    }

    private async Task ResetAsync()
    {
        if (_border == null || _label == null) return;
        await _label.FadeTo(0, 100);
        _label.Text = Text;
        _label.FontSize = 15;
        _border.StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle
        {
            CornerRadius = new CornerRadius(12)
        };
        _border.WidthRequest = -1;
        _border.Padding = new Thickness(24, 14);
        await Task.WhenAll(
            _border.ScaleTo(1.0, 200, Easing.SpringOut),
            _label.FadeTo(1, 150)
        );
        _morphed = false;
    }

    private void UpdateState()
    {
        if (_label != null && !_morphed) _label.Text = Text;
    }

    protected override void OnButtonBackgroundColorChanged(Color c)
    {
        if (_border != null) _border.BackgroundColor = c;
    }
}

// ══════════════════════════════════════════════════════════════════
// OUTLINED BUTTON
// ══════════════════════════════════════════════════════════════════

/// <summary>
/// An outlined (ghost) button that fills with color on press.
///
/// XAML Usage:
/// <code>
/// &lt;mab:OutlinedButton
///     Text="Learn More"
///     StrokeColor="#6C63FF"
///     TextColor="#6C63FF"
///     FillOnPress="True" /&gt;
/// </code>
/// </summary>
public class OutlinedButton : AnimatedButtonBase
{
    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(nameof(Text), typeof(string), typeof(OutlinedButton), "Button",
            propertyChanged: (b, _, n) => ((OutlinedButton)b).UpdateLabel());

    public static readonly BindableProperty TextColorProperty =
        BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(OutlinedButton), Color.FromArgb("#6C63FF"),
            propertyChanged: (b, _, n) => ((OutlinedButton)b).UpdateLabel());

    public static readonly BindableProperty StrokeColorProperty =
        BindableProperty.Create(nameof(StrokeColor), typeof(Color), typeof(OutlinedButton), Color.FromArgb("#6C63FF"),
            propertyChanged: (b, _, n) => ((OutlinedButton)b).UpdateStroke());

    public static readonly BindableProperty StrokeThicknessProperty =
        BindableProperty.Create(nameof(StrokeThickness), typeof(double), typeof(OutlinedButton), 2.0,
            propertyChanged: (b, _, n) => ((OutlinedButton)b).UpdateStroke());

    public static readonly BindableProperty FontSizeProperty =
        BindableProperty.Create(nameof(FontSize), typeof(double), typeof(OutlinedButton), 15.0,
            propertyChanged: (b, _, n) => ((OutlinedButton)b).UpdateLabel());

    public string Text { get => (string)GetValue(TextProperty); set => SetValue(TextProperty, value); }
    public Color TextColor { get => (Color)GetValue(TextColorProperty); set => SetValue(TextColorProperty, value); }
    public Color StrokeColor { get => (Color)GetValue(StrokeColorProperty); set => SetValue(StrokeColorProperty, value); }
    public double StrokeThickness { get => (double)GetValue(StrokeThicknessProperty); set => SetValue(StrokeThicknessProperty, value); }
    public double FontSize { get => (double)GetValue(FontSizeProperty); set => SetValue(FontSizeProperty, value); }

    private Border? _border;
    private Label? _label;

    protected override void BuildContent()
    {
        ShadowEnabled = false;

        _label = new Label
        {
            Text = Text,
            TextColor = TextColor,
            FontSize = FontSize,
            FontAttributes = FontAttributes.Bold,
            HorizontalTextAlignment = TextAlignment.Center,
            VerticalTextAlignment = TextAlignment.Center
        };

        _border = new Border
        {
            BackgroundColor = Colors.Transparent,
            Padding = new Thickness(24, 14),
            Stroke = new SolidColorBrush(StrokeColor),
            StrokeThickness = StrokeThickness,
            StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle
            {
                CornerRadius = new CornerRadius(CornerRadius)
            },
            Content = _label
        };

        Content = _border;
    }

    private void UpdateLabel()
    {
        if (_label == null) return;
        _label.Text = Text;
        _label.TextColor = TextColor;
        _label.FontSize = FontSize;
    }

    private void UpdateStroke()
    {
        if (_border == null) return;
        _border.Stroke = new SolidColorBrush(StrokeColor);
        _border.StrokeThickness = StrokeThickness;
    }

    protected override void OnButtonBackgroundColorChanged(Color c) { }

    protected override void OnCornerRadiusChanged(float r)
    {
        if (_border != null)
            _border.StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle
            {
                CornerRadius = new CornerRadius(r)
            };
    }
}
