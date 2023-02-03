using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;

namespace ChatApp.Mobile.ViewModels;

public partial class LoginViewModel : BaseViewModel
{
    [ObservableProperty] private string _userEmail;
    [ObservableProperty] private string _userPassword;
    [ObservableProperty] private bool _isBusy;
    public LoginViewModel()
    {
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
            var response = await MyClient.SendRequestAsync<object>(MyHttpMethods.POST, "account/login", content: content);
            if (response is null)
            {
                IsBusy = false;
                return;
            }
            IsBusy = false;
        }
        catch(Exception e)
        {
            Debug.WriteLine(e.Message);
        }
        finally
        {
            IsBusy = false;
        }
    }
}
