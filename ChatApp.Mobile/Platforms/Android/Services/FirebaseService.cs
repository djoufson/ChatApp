using Android.App;
using AndroidX.Core.App;
using Firebase.Messaging;

namespace ChatApp.Mobile.Platforms.Android.Services;

[Service(Exported = true)]
[IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
public class FirebaseService : FirebaseMessagingService
{
	public FirebaseService()
	{

	}

	public override void OnNewToken(string token)
	{
		base.OnNewToken(token);
		Preferences.Set(Utilities.Constants.DEVICE_TOKEN_KEY, token);
		Debug.WriteLine(Preferences.Get(Utilities.Constants.DEVICE_TOKEN_KEY, ""));
	}

	public override void OnMessageReceived(RemoteMessage message)
	{
		base.OnMessageReceived(message);
		var notification = message.GetNotification();
		try
		{
			ShowNotification(notification.Title, notification.Body, message.Data);
		}
		catch(Exception e) 
		{ 
			Debug.WriteLine(e); 
		}
    }

	private void ShowNotification(string title, string body, IDictionary<string, string> data)
	{
		var notificationBuilder = new NotificationCompat.Builder(this, MainActivity.Channel_Id)
			.SetContentTitle(title)
			.SetSmallIcon(Resource.Mipmap.appicon)
			.SetContentText(body)
			.SetChannelId(MainActivity.Channel_Id)
			.SetPriority(2);

		var notificationManager = NotificationManagerCompat.From(this);
		var notification = notificationBuilder.Build();
		notificationManager.Notify(MainActivity.Notification_Id, notification);
	}
}
