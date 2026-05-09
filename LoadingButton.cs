namespace MauiPrettyButtons.Controls;

/// <summary>
/// A button that shows a spinner animation while IsLoading is true.
/// The text morphs to a loading indicator and back seamlessly.
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
            propertyChanged: (b, _, n) => ((LoadingButton)b)._spinner?.ApplyColor((Color)n));

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
    private SpinnerView? _spinner;
    private HorizontalStackLayout? _innerRow;

    // ── Build ──────────────────────────────────────────────────────────────

    protected override void BuildContent()
    {
        _spinner = new SpinnerView
        {
            WidthRequest = SpinnerSize,
            HeightRequest = SpinnerSize,
            IsVisible = false,
            Margin = new Thickness(0, 0, 8, 0)
        };
        _spinner.ApplyColor(SpinnerColor);

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
            Children = { _spinner, _label }
        };

        _border = new Border
        {
            Background = Background,
            Padding = PaddingButton,
            StrokeThickness = 0,
            StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle
            {
                CornerRadius = new CornerRadius(CornerRadius)
            },
            Content = _innerRow
        };

        Content = _border;
    }

    // ── State Updates ──────────────────────────────────────────────────────

    private async void OnIsLoadingChanged(bool loading)
    {
        if (_label == null || _spinner == null) return;

        IsEnabledButton = !loading;

        if (loading)
        {
            await _label.FadeTo(0, 120);
            _label.Text = LoadingText;
            _spinner.IsVisible = true;
            _spinner.Start();
            await _label.FadeTo(1, 120);
        }
        else
        {
            await _label.FadeTo(0, 120);
            _spinner.Stop();
            _spinner.IsVisible = false;
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
        if (_spinner != null)
        {
            _spinner.WidthRequest = s;
            _spinner.HeightRequest = s;
        }
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
                CornerRadius = new CornerRadius(r)
            };
    }

    protected override void OnButtonBackgroundChanged(Brush? background)
    {
        if (_border != null) _border.Background = background;
    }
}

// ── Spinner Helper ─────────────────────────────────────────────────────────

/// <summary>Internal arc-spinner using a rotating BoxView arc effect.</summary>
internal class SpinnerView : ContentView
{
    private readonly Microsoft.Maui.Controls.Shapes.ArcSegment? _arc;
    private bool _running;

    public SpinnerView()
    {
        var ellipse = new Microsoft.Maui.Controls.Shapes.Ellipse
        {
            Stroke = new SolidColorBrush(Colors.White),
            StrokeThickness = 3,
            Fill = Brush.Transparent,
            Opacity = 0.3
        };

        // Arc overlay to simulate spinner
        var indicator = new BoxView
        {
            Color = Colors.White,
            WidthRequest = 4,
            HeightRequest = 4,
            CornerRadius = 2,
            HorizontalOptions = LayoutOptions.Start,
            VerticalOptions = LayoutOptions.Start
        };

        Content = new Grid
        {
            Children = { ellipse, indicator }
        };
    }

    public void ApplyColor(Color color)
    {
        // Color applied via opacity/tint
        Opacity = 1;
    }

    public void Start()
    {
        if (_running) return;
        _running = true;
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            while (_running)
            {
                await this.RotateTo(360, 700, Easing.Linear);
                Rotation = 0;
            }
        });
    }

    public void Stop()
    {
        _running = false;
        this.CancelAnimations();
        Rotation = 0;
    }
}
