namespace ChatApp.Mobile.Services.Device;

public class DisplayService : IDisplayService
{
    public Task DisplayAlert(string title, string message, string cancel)
        => Shell.Current.DisplayAlert(title, message, cancel);
    public Task<bool> DisplayAlert(string title, string message, string accept, string cancel)
        => Shell.Current.DisplayAlert(title,message, accept, cancel);
    public Task<string> DisplayActionSheet(string title, string cancel, string destruction, params string[] buttons) 
        => Shell.Current.DisplayActionSheet(title, cancel, destruction, buttons);
}
