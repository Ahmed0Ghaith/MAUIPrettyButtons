namespace MauiPrettyButtons.Controls;

/// <summary>
/// A button that shows the built-in <see cref="ActivityIndicator"/> while IsLoading is true.
/// The label cross-fades to loading text and back seamlessly.
///
/// XAML Usage:
/// <code>
/// &lt;mab:LoadingButton
///     Text="Submit"
///     LoadingText="Submitting..."
///     IsLoading="{Binding IsBusy}"
///     Command="{Binding SubmitCommand}"
///     ButtonBackgroundColor="#6C63FF" /&gt;
/// </code>
/// </summary>
public class LoadingButton : AnimatedButtonBase
{
    // ── Bindable Properties ────────────────────────────────────────────────

    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(nameof(Text), typeof(string), typeof(LoadingButton), "Button",
            propertyChanged: (b, _, n) => ((LoadingButton)b).UpdateState());

    public static readonly BindableProperty LoadingTextProperty =
        BindableProperty.Create(nameof(LoadingText), typeof(string), typeof(LoadingButton), "Loading...",
            propertyChanged: (b, _, n) => ((LoadingButton)b).UpdateState());

    public static readonly BindableProperty IsLoadingProperty =
        BindableProperty.Create(nameof(IsLoading), typeof(bool), typeof(LoadingButton), false,
            propertyChanged: (b, _, n) => ((LoadingButton)b).OnIsLoadingChanged((bool)n));

    public static readonly BindableProperty TextColorProperty =
        BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(LoadingButton), Colors.White,
            propertyChanged: (b, _, n) => ((LoadingButton)b).UpdateTextColor((Color)n));

    public static readonly BindableProperty FontSizeProperty =
        BindableProperty.Create(nameof(FontSize), typeof(double), typeof(LoadingButton), 16.0,
            propertyChanged: (b, _, n) => ((LoadingButton)b).UpdateFontSize((double)n));

    public static readonly BindableProperty FontAttributesProperty =
        BindableProperty.Create(nameof(FontAttributes), typeof(FontAttributes), typeof(LoadingButton), FontAttributes.Bold,
            propertyChanged: (b, _, n) => ((LoadingButton)b).UpdateFontAttributes((FontAttributes)n));

    public static readonly BindableProperty SpinnerColorProperty =
        BindableProperty.Create(nameof(SpinnerColor), typeof(Color), typeof(LoadingButton), Colors.White,
            propertyChanged: (b, _, n) => ((LoadingButton)b).ApplySpinnerColor((Color)n));

    public static readonly BindableProperty SpinnerSizeProperty =
        BindableProperty.Create(nameof(SpinnerSize), typeof(double), typeof(LoadingButton), 22.0,
            propertyChanged: (b, _, n) => ((LoadingButton)b).UpdateSpinnerSize((double)n));

    public static readonly BindableProperty PaddingButtonProperty =
        BindableProperty.Create(nameof(PaddingButton), typeof(Thickness), typeof(LoadingButton), new Thickness(24, 14),
            propertyChanged: (b, _, n) => ((LoadingButton)b).UpdatePadding((Thickness)n));

    // ── Properties ────────────────────────────────────────────────────────

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public string LoadingText
    {
        get => (string)GetValue(LoadingTextProperty);
        set => SetValue(LoadingTextProperty, value);
    }

    public bool IsLoading
    {
        get => (bool)GetValue(IsLoadingProperty);
        set => SetValue(IsLoadingProperty, value);
    }

    public Color TextColor
    {
        get => (Color)GetValue(TextColorProperty);
        set => SetValue(TextColorProperty, value);
    }

    public double FontSize
    {
        get => (double)GetValue(FontSizeProperty);
        set => SetValue(FontSizeProperty, value);
    }

    public FontAttributes FontAttributes
    {
        get => (FontAttributes)GetValue(FontAttributesProperty);
        set => SetValue(FontAttributesProperty, value);
    }

    public Color SpinnerColor
    {
        get => (Color)GetValue(SpinnerColorProperty);
        set => SetValue(SpinnerColorProperty, value);
    }

    public double SpinnerSize
    {
        get => (double)GetValue(SpinnerSizeProperty);
        set => SetValue(SpinnerSizeProperty, value);
    }

    public Thickness PaddingButton
    {
        get => (Thickness)GetValue(PaddingButtonProperty);
        set => SetValue(PaddingButtonProperty, value);
    }

    // ── Internal State ────────────────────────────────────────────────────

    private Border? _border;
    private Label? _label;
    private ActivityIndicator? _activityIndicator;
    private HorizontalStackLayout? _innerRow;

    // ── Build ──────────────────────────────────────────────────────────────

    protected override void BuildContent()
    {
        _activityIndicator = new ActivityIndicator
        {
            WidthRequest = SpinnerSize,
            HeightRequest = SpinnerSize,
            IsVisible = false,
            IsRunning = false,
            Color = SpinnerColor,
            Margin = new Thickness(0, 0, 8, 0)
        };

        _label = new Label
        {
            Text = Text,
            TextColor = TextColor,
            FontSize = FontSize,
            FontAttributes = FontAttributes,
            VerticalTextAlignment = TextAlignment.Center,
            HorizontalTextAlignment = TextAlignment.Center
        };

        _innerRow = new HorizontalStackLayout
        {
            VerticalOptions = LayoutOptions.Center,
            HorizontalOptions = LayoutOptions.Center,
            Spacing = 0,
            Children = { _activityIndicator, _label }
        };

        _border = new Border
        {
            Background = Background,
            Padding = PaddingButton,
            StrokeThickness = 0,
            StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle
            {
                CornerRadius = GetEffectiveCornerRadius()
            },
            Content = _innerRow
        };

        Content = _border;
    }

    // ── State Updates ──────────────────────────────────────────────────────

    private async void OnIsLoadingChanged(bool loading)
    {
        if (_label == null || _activityIndicator == null) return;

        IsEnabledButton = !loading;

        if (loading)
        {
            await _label.FadeTo(0, 120);
            _label.Text = LoadingText;
            _activityIndicator.IsVisible = true;
            _activityIndicator.IsRunning = true;
            await _label.FadeTo(1, 120);
        }
        else
        {
            await _label.FadeTo(0, 120);
            _activityIndicator.IsRunning = false;
            _activityIndicator.IsVisible = false;
            _label.Text = Text;
            await _label.FadeTo(1, 120);
        }
    }

    private void UpdateState()
    {
        if (_label != null)
            _label.Text = IsLoading ? LoadingText : Text;
    }

    private void UpdateTextColor(Color c)
    {
        if (_label != null) _label.TextColor = c;
    }

    private void UpdateFontSize(double s)
    {
        if (_label != null) _label.FontSize = s;
    }

    private void UpdateFontAttributes(FontAttributes a)
    {
        if (_label != null) _label.FontAttributes = a;
    }

    private void UpdateSpinnerSize(double s)
    {
        if (_activityIndicator != null)
        {
            _activityIndicator.WidthRequest = s;
            _activityIndicator.HeightRequest = s;
        }
    }

    private void ApplySpinnerColor(Color color)
    {
        if (_activityIndicator != null)
            _activityIndicator.Color = color;
    }

    private void UpdatePadding(Thickness t)
    {
        if (_border != null) _border.Padding = t;
    }

    protected override void OnCornerRadiusChanged(float r)
    {
        if (_border != null)
            _border.StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle
            {
                CornerRadius = GetEffectiveCornerRadius()
            };
    }

    protected override void OnButtonBackgroundChanged(Brush? background)
    {
        if (_border != null) _border.Background = background;
    }
}
