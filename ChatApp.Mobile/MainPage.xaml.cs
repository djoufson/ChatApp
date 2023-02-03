namespace ChatApp.Mobile;

public partial class MainPage : ContentPage
{
	IMessageConnection _connection;
	public MainPage(IMessageConnection connection)
	{
		_connection = connection;
		Task.Run(async () => await _connection.ConnectAsync());
		_connection.OnMessageRecieved += MessageRecieved;
		InitializeComponent();
	}
	private void MessageRecieved(object sender, string message)
	{
		MainThread.BeginInvokeOnMainThread(async () => await DisplayAlert("New Message", message, "OK"));
	}
	private async void OnCounterClicked(object sender, EventArgs e)
	{
		await _connection.SendMessageAsync(myChatMessage.Text);
		myChatMessage.Text = string.Empty;
	}
}

