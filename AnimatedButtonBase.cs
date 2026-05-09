using System.Windows.Input;

namespace MauiPrettyButtons.Controls;

/// <summary>
/// Base class for all animated buttons. Provides press scale animation,
/// command binding, and shared styling properties.
/// </summary>
public abstract class AnimatedButtonBase : ContentView
{
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

    public static readonly BindableProperty CornerRadiusProperty =
        BindableProperty.Create(nameof(CornerRadius), typeof(float), typeof(AnimatedButtonBase), 12f,
            propertyChanged: (b, _, n) => ((AnimatedButtonBase)b).OnCornerRadiusChanged((float)n));

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

    public float CornerRadius
    {
        get => (float)GetValue(CornerRadiusProperty);
        set => SetValue(CornerRadiusProperty, value);
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
        await this.ScaleTo(PressScale, PressAnimationDuration, Easing.CubicOut);
    }

    /// <summary>
    /// Call to trigger release animation.
    /// </summary>
    public async Task AnimateReleaseAsync()
    {
        if (!_isPressed) return;
        _isPressed = false;
        Released?.Invoke(this, EventArgs.Empty);
        await this.ScaleTo(1.0, PressAnimationDuration, Easing.SpringOut);
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
            Shadow = null;
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
