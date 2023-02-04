using ChatApp.Mobile.Extensions;
using ChatApp.Mobile.Services.Device;

namespace ChatApp.Mobile.ViewModels;

public partial class LoginViewModel : BaseViewModel
{
    [ObservableProperty] private string _userEmail;
    [ObservableProperty] private string _userPassword;
    [ObservableProperty] private bool _isBusy;
    private readonly User _user;
    private readonly ShellNavigationService _shell;

    public LoginViewModel(
        ShellNavigationService shell,
        User user)
    {
        _user = user;
        _shell = shell;
        _userEmail = "djoufson@example.com";
        _userPassword = "String 1";
    }

    [RelayCommand]
    public async Task LoginAsync()
    {
        IsBusy = true;
        var content = new Dictionary<string, string>()
        {
            {"email", UserEmail },
            {"password", UserPassword }
        };
        try
        {
            var response = await MyClient.SendRequestAsync<LoginResponseDto>(MyHttpMethods.POST, LoginRoute, content: content);
            if (response is null)
            {
                IsBusy = false;
                await _shell.Current.DisplayAlert("Error", "Wrong Credentials", "OK");
                return;
            }
            App.UserEmail = response.User.Email;
            AuthToken = response.Token;
            _user.DeepCopy(response.User);

            content = new()
            {
                {"deviceToken", DeviceToken},
            };

            var updateResponse = await MyClient.SendRequestAsync<LoginResponseDto>(
                method: MyHttpMethods.PUT, 
                url: DeviceTokenRoute, 
                content: content, 
                auth: AuthToken);

            await _shell.GoToAsync("///HomePage");
            IsBusy = false;
        }
        catch(Exception e)
        {
            Debug.WriteLine(e.Message);
            await _shell.Current.DisplayAlert("Error", e.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }
}
