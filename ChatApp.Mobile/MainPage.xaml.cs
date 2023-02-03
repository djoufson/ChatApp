namespace ChatApp.Mobile;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}

	private async void OnCounterClicked(object sender, EventArgs e)
	{
        var content = new Dictionary<string, string>()
		{
			{"email", "djoufson@example.com"},
			{"password", "String 1"}
		};
		var response = await MyClient.SendRequestAsync<string>(MyHttpMethods.POST, "account/login", content);
		await DisplayAlert("Response", response, "OK");
	}
}

