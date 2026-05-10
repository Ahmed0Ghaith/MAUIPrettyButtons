using System.Windows.Input;

namespace MauiPrettyButtons.Controls;

/// <summary>Visual feedback style when the button is pressed.</summary>
public enum PressAnimationStyle
{
    /// <summary>Scale down slightly (default).</summary>
    Scale,
    /// <summary>Reduce opacity.</summary>
    Opacity,
    /// <summary>Scale and opacity together.</summary>
    ScaleAndOpacity,
    /// <summary>Move down slightly (material “sink”).</summary>
    Sink,
    /// <summary>Slight clockwise rotation.</summary>
    Tilt,
    /// <summary>Scale with a bouncy release curve.</summary>
    Bounce
}

/// <summary>
/// Base class for all animated buttons. Provides press scale animation,
/// command binding, and shared styling properties.
/// </summary>
public abstract class AnimatedButtonBase : ContentView
{
    /// <summary>Use uniform <see cref="CornerRadius"/> for this corner (XAML-friendly default).</summary>
    public const float InheritCornerRadius = -1f;

    // ── Bindable Properties ────────────────────────────────────────────────

    public static readonly BindableProperty CommandProperty =
        BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(AnimatedButtonBase));

    public static readonly BindableProperty CommandParameterProperty =
        BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(AnimatedButtonBase));

    public static readonly BindableProperty IsEnabledButtonProperty =
        BindableProperty.Create(nameof(IsEnabledButton), typeof(bool), typeof(AnimatedButtonBase), true,
            propertyChanged: (b, _, n) => ((AnimatedButtonBase)b).OnEnabledChanged((bool)n));

    public static readonly BindableProperty PressScaleProperty =
        BindableProperty.Create(nameof(PressScale), typeof(double), typeof(AnimatedButtonBase), 0.93);

    public static readonly BindableProperty PressAnimationDurationProperty =
        BindableProperty.Create(nameof(PressAnimationDuration), typeof(uint), typeof(AnimatedButtonBase), (uint)100);

    public static readonly BindableProperty PressAnimationStyleProperty =
        BindableProperty.Create(nameof(PressAnimationStyle), typeof(PressAnimationStyle), typeof(AnimatedButtonBase), PressAnimationStyle.Scale);

    public static readonly BindableProperty CornerRadiusProperty =
        BindableProperty.Create(nameof(CornerRadius), typeof(float), typeof(AnimatedButtonBase), 12f,
            propertyChanged: OnCornerRelatedPropertyChanged);

    public static readonly BindableProperty CornerRadiusTopLeftProperty =
        BindableProperty.Create(nameof(CornerRadiusTopLeft), typeof(float), typeof(AnimatedButtonBase), InheritCornerRadius,
            propertyChanged: OnCornerRelatedPropertyChanged);

    public static readonly BindableProperty CornerRadiusTopRightProperty =
        BindableProperty.Create(nameof(CornerRadiusTopRight), typeof(float), typeof(AnimatedButtonBase), InheritCornerRadius,
            propertyChanged: OnCornerRelatedPropertyChanged);

    public static readonly BindableProperty CornerRadiusBottomLeftProperty =
        BindableProperty.Create(nameof(CornerRadiusBottomLeft), typeof(float), typeof(AnimatedButtonBase), InheritCornerRadius,
            propertyChanged: OnCornerRelatedPropertyChanged);

    public static readonly BindableProperty CornerRadiusBottomRightProperty =
        BindableProperty.Create(nameof(CornerRadiusBottomRight), typeof(float), typeof(AnimatedButtonBase), InheritCornerRadius,
            propertyChanged: OnCornerRelatedPropertyChanged);

    private static void OnCornerRelatedPropertyChanged(BindableObject bindable, object? oldValue, object? newValue)
    {
        if (bindable is AnimatedButtonBase a)
            a.OnCornerRadiusChanged(a.CornerRadius);
    }

    public static readonly BindableProperty ButtonBackgroundColorProperty =
        BindableProperty.Create(nameof(ButtonBackgroundColor), typeof(Color), typeof(AnimatedButtonBase), Color.FromArgb("#6C63FF"),
            propertyChanged: (b, _, n) => ((AnimatedButtonBase)b).Background = new SolidColorBrush((Color)n));

    public static readonly BindableProperty ShadowEnabledProperty =
        BindableProperty.Create(nameof(ShadowEnabled), typeof(bool), typeof(AnimatedButtonBase), true,
            propertyChanged: (b, _, n) => ((AnimatedButtonBase)b).OnShadowChanged((bool)n));

    // ── Properties ────────────────────────────────────────────────────────

    public ICommand? Command
    {
        get => (ICommand?)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public object? CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    public bool IsEnabledButton
    {
        get => (bool)GetValue(IsEnabledButtonProperty);
        set => SetValue(IsEnabledButtonProperty, value);
    }

    /// <summary>Scale factor applied on press (default 0.93)</summary>
    public double PressScale
    {
        get => (double)GetValue(PressScaleProperty);
        set => SetValue(PressScaleProperty, value);
    }

    /// <summary>Duration in milliseconds for press animation (default 100ms)</summary>
    public uint PressAnimationDuration
    {
        get => (uint)GetValue(PressAnimationDurationProperty);
        set => SetValue(PressAnimationDurationProperty, value);
    }

    public PressAnimationStyle PressAnimationStyle
    {
        get => (PressAnimationStyle)GetValue(PressAnimationStyleProperty);
        set => SetValue(PressAnimationStyleProperty, value);
    }

    public float CornerRadius
    {
        get => (float)GetValue(CornerRadiusProperty);
        set => SetValue(CornerRadiusProperty, value);
    }

    /// <summary>Top-left corner radius, or <see cref="InheritCornerRadius"/> to use <see cref="CornerRadius"/>.</summary>
    public float CornerRadiusTopLeft
    {
        get => (float)GetValue(CornerRadiusTopLeftProperty);
        set => SetValue(CornerRadiusTopLeftProperty, value);
    }

    /// <summary>Top-right corner radius, or <see cref="InheritCornerRadius"/> to use <see cref="CornerRadius"/>.</summary>
    public float CornerRadiusTopRight
    {
        get => (float)GetValue(CornerRadiusTopRightProperty);
        set => SetValue(CornerRadiusTopRightProperty, value);
    }

    /// <summary>Bottom-left corner radius, or <see cref="InheritCornerRadius"/> to use <see cref="CornerRadius"/>.</summary>
    public float CornerRadiusBottomLeft
    {
        get => (float)GetValue(CornerRadiusBottomLeftProperty);
        set => SetValue(CornerRadiusBottomLeftProperty, value);
    }

    /// <summary>Bottom-right corner radius, or <see cref="InheritCornerRadius"/> to use <see cref="CornerRadius"/>.</summary>
    public float CornerRadiusBottomRight
    {
        get => (float)GetValue(CornerRadiusBottomRightProperty);
        set => SetValue(CornerRadiusBottomRightProperty, value);
    }

    public Color ButtonBackgroundColor
    {
        get => (Background as SolidColorBrush)?.Color ?? Colors.Transparent;
        set => Background = new SolidColorBrush(value);
    }

    public bool ShadowEnabled
    {
        get => (bool)GetValue(ShadowEnabledProperty);
        set => SetValue(ShadowEnabledProperty, value);
    }

    // ── Events ────────────────────────────────────────────────────────────

    public event EventHandler? Clicked;
    public event EventHandler? Pressed;
    public event EventHandler? Released;

    // ── Constructor ───────────────────────────────────────────────────────

    protected AnimatedButtonBase()
    {
        Background ??= new SolidColorBrush(Color.FromArgb("#6C63FF"));

        var tap = new TapGestureRecognizer();
        tap.Tapped += OnTapped;
        GestureRecognizers.Add(tap);
        BuildContent();
        ApplyShadow(ShadowEnabled);

        PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(Background))
                OnButtonBackgroundChanged(Background);
        };

        OnButtonBackgroundChanged(Background);
    }

    // ── Abstract / Virtual ────────────────────────────────────────────────

    /// <summary>Subclasses build their visual content here.</summary>
    protected abstract void BuildContent();

    /// <summary>
    /// Resolved corner radii for masks (top-left, top-right, bottom-left, bottom-right).
    /// Corners set to <see cref="InheritCornerRadius"/> use the uniform <see cref="CornerRadius"/> value.
    /// </summary>
    protected CornerRadius GetEffectiveCornerRadius()
    {
        double u = CornerRadius;
        double tl = CornerRadiusTopLeft == InheritCornerRadius ? u : CornerRadiusTopLeft;
        double tr = CornerRadiusTopRight == InheritCornerRadius ? u : CornerRadiusTopRight;
        double bl = CornerRadiusBottomLeft == InheritCornerRadius ? u : CornerRadiusBottomLeft;
        double br = CornerRadiusBottomRight == InheritCornerRadius ? u : CornerRadiusBottomRight;
        return new CornerRadius(tl, tr, bl, br);
    }

    protected virtual void OnCornerRadiusChanged(float radius) { }
    protected virtual void OnButtonBackgroundChanged(Brush? background) { }
    protected virtual void OnShadowChanged(bool enabled) => ApplyShadow(enabled);
    protected virtual void OnEnabledChanged(bool enabled) => Opacity = enabled ? 1.0 : 0.45;

    // ── Press Animation ───────────────────────────────────────────────────

    private bool _isPressed;

    /// <summary>
    /// Call from platform-specific or pointer gesture handlers to trigger press animation.
    /// </summary>
    public async Task AnimatePressAsync()
    {
        if (_isPressed) return;
        _isPressed = true;
        Pressed?.Invoke(this, EventArgs.Empty);
        uint d = PressAnimationDuration;
        switch (PressAnimationStyle)
        {
            case PressAnimationStyle.Scale:
            case PressAnimationStyle.Bounce:
                await this.ScaleTo(PressScale, d, Easing.CubicOut);
                break;
            case PressAnimationStyle.Opacity:
                await this.FadeTo(0.86, d, Easing.CubicOut);
                break;
            case PressAnimationStyle.ScaleAndOpacity:
                await Task.WhenAll(
                    this.ScaleTo(PressScale, d, Easing.CubicOut),
                    this.FadeTo(0.88, d, Easing.CubicOut));
                break;
            case PressAnimationStyle.Sink:
                await this.TranslateTo(0, 4, d, Easing.CubicOut);
                break;
            case PressAnimationStyle.Tilt:
                await this.RotateTo(5, d, Easing.CubicOut);
                break;
            default:
                await this.ScaleTo(PressScale, d, Easing.CubicOut);
                break;
        }
    }

    /// <summary>
    /// Call to trigger release animation.
    /// </summary>
    public async Task AnimateReleaseAsync()
    {
        if (!_isPressed) return;
        _isPressed = false;
        Released?.Invoke(this, EventArgs.Empty);
        uint d = PressAnimationDuration;
        switch (PressAnimationStyle)
        {
            case PressAnimationStyle.Scale:
                await this.ScaleTo(1.0, d, Easing.SpringOut);
                break;
            case PressAnimationStyle.Bounce:
                await this.ScaleTo(1.0, (uint)(d + d / 2), Easing.BounceOut);
                break;
            case PressAnimationStyle.Opacity:
                await this.FadeTo(1.0, d, Easing.SpringOut);
                break;
            case PressAnimationStyle.ScaleAndOpacity:
                await Task.WhenAll(
                    this.ScaleTo(1.0, d, Easing.SpringOut),
                    this.FadeTo(1.0, d, Easing.SpringOut));
                break;
            case PressAnimationStyle.Sink:
                await this.TranslateTo(0, 0, d, Easing.SpringOut);
                break;
            case PressAnimationStyle.Tilt:
                await this.RotateTo(0, d, Easing.SpringOut);
                break;
            default:
                await this.ScaleTo(1.0, d, Easing.SpringOut);
                break;
        }
    }

    // ── Shadow ────────────────────────────────────────────────────────────

    private void ApplyShadow(bool enabled)
    {
        if (enabled)
        {
            Shadow = new Shadow
            {
                Brush = new SolidColorBrush(Colors.Black),
                Offset = new Point(0, 4),
                Radius = 12,
                Opacity = 0.25f
            };
        }
        else
        {
            ClearValue(ShadowProperty);
        }
    }

    // ── Tap Handling ──────────────────────────────────────────────────────

    private async void OnTapped(object? sender, TappedEventArgs e)
    {
        if (!IsEnabledButton) return;
        await AnimatePressAsync();
        await AnimateReleaseAsync();

        if (Command?.CanExecute(CommandParameter) == true)
            Command.Execute(CommandParameter);

        Clicked?.Invoke(this, EventArgs.Empty);
        OnButtonClicked();
    }

    /// <summary>Override to handle button click in subclasses.</summary>
    protected virtual void OnButtonClicked() { }
}
