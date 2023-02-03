using Android.App;
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
		Preferences.Set(Utilities.Constants.DeviceTokenKey, token);
	}
}
