namespace MauiPrettyButtons.Controls;

/// <summary>
/// A Material Design-style Floating Action Button (FAB) with a satisfying
/// bounce press animation and optional extended label.
///
/// XAML Usage:
/// <code>
/// &lt;!-- Mini FAB --&gt;
/// &lt;mab:FloatingActionButton
///     Icon="&#xE145;"
///     Size="56"
///     ButtonBackgroundColor="#FF5252"
///     Command="{Binding AddCommand}" /&gt;
///
/// &lt;!-- Extended FAB --&gt;
/// &lt;mab:FloatingActionButton
///     Icon="&#xE145;"
///     Text="Add Item"
///     IsExtended="True"
///     ButtonBackgroundColor="#6C63FF" /&gt;
/// </code>
/// </summary>
public class FloatingActionButton : AnimatedButtonBase
{
    // ── Bindable Properties ────────────────────────────────────────────────

    public static readonly BindableProperty IconProperty =
        BindableProperty.Create(nameof(Icon), typeof(string), typeof(FloatingActionButton), "+",
            propertyChanged: (b, _, n) => ((FloatingActionButton)b).UpdateIcon());

    public static readonly BindableProperty IconFontFamilyProperty =
        BindableProperty.Create(nameof(IconFontFamily), typeof(string), typeof(FloatingActionButton), null,
            propertyChanged: (b, _, n) => ((FloatingActionButton)b).UpdateIcon());

    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(nameof(Text), typeof(string), typeof(FloatingActionButton), string.Empty,
            propertyChanged: (b, _, n) => ((FloatingActionButton)b).UpdateExtended());

    public static readonly BindableProperty IsExtendedProperty =
        BindableProperty.Create(nameof(IsExtended), typeof(bool), typeof(FloatingActionButton), false,
            propertyChanged: (b, _, n) => ((FloatingActionButton)b).UpdateExtended());

    public static readonly BindableProperty SizeProperty =
        BindableProperty.Create(nameof(Size), typeof(double), typeof(FloatingActionButton), 56.0,
            propertyChanged: (b, _, n) => ((FloatingActionButton)b).UpdateSize((double)n));

    public static readonly BindableProperty IconColorProperty =
        BindableProperty.Create(nameof(IconColor), typeof(Color), typeof(FloatingActionButton), Colors.White,
            propertyChanged: (b, _, n) => ((FloatingActionButton)b).UpdateIcon());

    public static readonly BindableProperty IconSizeProperty =
        BindableProperty.Create(nameof(IconSize), typeof(double), typeof(FloatingActionButton), 24.0,
            propertyChanged: (b, _, n) => ((FloatingActionButton)b).UpdateIcon());

    public static readonly BindableProperty TextColorProperty =
        BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(FloatingActionButton), Colors.White,
            propertyChanged: (b, _, n) => ((FloatingActionButton)b).UpdateExtended());

    public static readonly BindableProperty BadgeCountProperty =
        BindableProperty.Create(nameof(BadgeCount), typeof(int), typeof(FloatingActionButton), 0,
            propertyChanged: (b, _, n) => ((FloatingActionButton)b).UpdateBadge((int)n));

    // ── Properties ────────────────────────────────────────────────────────

    /// <summary>Icon character (use Unicode glyph or text emoji)</summary>
    public string Icon
    {
        get => (string)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    /// <summary>Font family for icon (e.g. "MaterialIcons")</summary>
    public string? IconFontFamily
    {
        get => (string?)GetValue(IconFontFamilyProperty);
        set => SetValue(IconFontFamilyProperty, value);
    }

    /// <summary>Label shown when IsExtended = true</summary>
    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public bool IsExtended
    {
        get => (bool)GetValue(IsExtendedProperty);
        set => SetValue(IsExtendedProperty, value);
    }

    /// <summary>Diameter of the FAB circle (default 56)</summary>
    public double Size
    {
        get => (double)GetValue(SizeProperty);
        set => SetValue(SizeProperty, value);
    }

    public Color IconColor
    {
        get => (Color)GetValue(IconColorProperty);
        set => SetValue(IconColorProperty, value);
    }

    public double IconSize
    {
        get => (double)GetValue(IconSizeProperty);
        set => SetValue(IconSizeProperty, value);
    }

    public Color TextColor
    {
        get => (Color)GetValue(TextColorProperty);
        set => SetValue(TextColorProperty, value);
    }

    /// <summary>Show a notification badge with count (0 = hidden)</summary>
    public int BadgeCount
    {
        get => (int)GetValue(BadgeCountProperty);
        set => SetValue(BadgeCountProperty, value);
    }

    // ── Internal Views ────────────────────────────────────────────────────

    private Border? _fabBorder;
    private Label? _iconLabel;
    private Label? _textLabel;
    private HorizontalStackLayout? _row;
    private Border? _badge;
    private Label? _badgeLabel;

    // ── Build ──────────────────────────────────────────────────────────────

    protected override void BuildContent()
    {
        PressScale = 0.88;
        PressAnimationDuration = 120;

        _iconLabel = new Label
        {
            Text = Icon,
            TextColor = IconColor,
            FontSize = IconSize,
            FontFamily = IconFontFamily,
            VerticalTextAlignment = TextAlignment.Center,
            HorizontalTextAlignment = TextAlignment.Center
        };

        _textLabel = new Label
        {
            Text = Text,
            TextColor = TextColor,
            FontSize = 14,
            FontAttributes = FontAttributes.Bold,
            VerticalTextAlignment = TextAlignment.Center,
            IsVisible = IsExtended && !string.IsNullOrEmpty(Text),
            Margin = new Thickness(6, 0, 0, 0)
        };

        _row = new HorizontalStackLayout
        {
            VerticalOptions = LayoutOptions.Center,
            HorizontalOptions = LayoutOptions.Center,
            Children = { _iconLabel, _textLabel }
        };

        _fabBorder = new Border
        {
            Background = Background,
            StrokeThickness = 0,
            WidthRequest = Size,
            HeightRequest = Size,
            StrokeShape = new Microsoft.Maui.Controls.Shapes.Ellipse(),
            Content = _row
        };

        _badgeLabel = new Label
        {
            TextColor = Colors.White,
            FontSize = 9,
            FontAttributes = FontAttributes.Bold,
            HorizontalTextAlignment = TextAlignment.Center,
            VerticalTextAlignment = TextAlignment.Center
        };

        _badge = new Border
        {
            BackgroundColor = Colors.Red,
            StrokeThickness = 0,
            WidthRequest = 18,
            HeightRequest = 18,
            StrokeShape = new Microsoft.Maui.Controls.Shapes.Ellipse(),
            HorizontalOptions = LayoutOptions.End,
            VerticalOptions = LayoutOptions.Start,
            IsVisible = false,
            Content = _badgeLabel
        };

        var root = new Grid
        {
            Children = { _fabBorder, _badge }
        };

        Content = root;

        UpdateExtended();
        UpdateBadge(BadgeCount);
    }

    // ── Updates ────────────────────────────────────────────────────────────

    private void UpdateIcon()
    {
        if (_iconLabel == null) return;
        _iconLabel.Text = Icon;
        _iconLabel.TextColor = IconColor;
        _iconLabel.FontSize = IconSize;
        _iconLabel.FontFamily = IconFontFamily;
    }

    private void UpdateExtended()
    {
        if (_fabBorder == null || _textLabel == null) return;

        bool showText = IsExtended && !string.IsNullOrEmpty(Text);
        _textLabel.IsVisible = showText;
        _textLabel.Text = Text;

        if (IsExtended)
        {
            _fabBorder.WidthRequest = -1; // auto
            _fabBorder.MinimumWidthRequest = Size;
            _fabBorder.Padding = new Thickness(16, 0);
            _fabBorder.HeightRequest = Size;
            _fabBorder.StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle
            {
                CornerRadius = GetEffectiveCornerRadius()
            };
        }
        else
        {
            double half = Size / 2.0;
            var cr = GetEffectiveCornerRadius();
            bool useCircle = cr.TopLeft >= half - 0.5 && cr.TopRight >= half - 0.5
                && cr.BottomLeft >= half - 0.5 && cr.BottomRight >= half - 0.5;
            _fabBorder.WidthRequest = Size;
            _fabBorder.HeightRequest = Size;
            _fabBorder.Padding = new Thickness(0);
            _fabBorder.StrokeShape = useCircle
                ? new Microsoft.Maui.Controls.Shapes.Ellipse()
                : new Microsoft.Maui.Controls.Shapes.RoundRectangle
                {
                    CornerRadius = cr
                };
        }
    }

    private void UpdateSize(double s)
    {
        if (_fabBorder == null) return;
        _fabBorder.WidthRequest = IsExtended ? -1 : s;
        _fabBorder.HeightRequest = s;
    }

    private void UpdateBadge(int count)
    {
        if (_badge == null || _badgeLabel == null) return;
        _badge.IsVisible = count > 0;
        _badgeLabel.Text = count > 99 ? "99+" : count.ToString();
    }

    protected override void OnButtonBackgroundChanged(Brush? background)
    {
        if (_fabBorder != null) _fabBorder.Background = background;
    }

    protected override void OnCornerRadiusChanged(float r) => UpdateExtended();
}
