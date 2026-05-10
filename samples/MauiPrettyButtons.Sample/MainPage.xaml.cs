namespace MauiPrettyButtons.Sample;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
		WireEvents();
	}

	private void WireEvents()
	{
		SubmitBtn.Clicked += async (_, _) =>
		{
			SubmitBtn.IsLoading = true;
			InfoLabel.Text = "Simulating request...";
			await Task.Delay(1400);
			SubmitBtn.IsLoading = false;
			InfoLabel.Text = "Done. LoadingButton state reset.";
		};

		FollowBtn.Toggled += (_, isOn) =>
		{
			InfoLabel.Text = isOn ? "ToggleButton is ON" : "ToggleButton is OFF";
		};
	}
}
