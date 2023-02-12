using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ChatApp.Mobile.ViewModels;

public partial class NewMessageViewModel : BaseViewModel
{
    [ObservableProperty] private string _searchKeyword;
    [ObservableProperty] private ObservableCollection<User> _users;
    private readonly ShellNavigationService _shell;
    private readonly IDisplayService _displayService;
    private List<User> _userList;

    // CONSTRUCTOR
    public NewMessageViewModel(
        IMessageConnection messageConnection, 
        IDisplayService displayService, 
        ShellNavigationService shell) : base(messageConnection)
    {
        _shell = shell;
        _displayService = displayService;
        _userList = new();
        _users = new ();
        Task.Run(async() => await LoadContactsAsync());
        PropertyChanged += HandlePropertyChanged;
    }

    private void HandlePropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName.Equals(nameof(SearchKeyword))) SearchKeywordChanged();
    }

    private void SearchKeywordChanged()
    {
        Users.Clear();
        foreach (var user in _userList)
            if (user.UserName.Contains(SearchKeyword, StringComparison.OrdinalIgnoreCase)
                || user.Email.Contains(SearchKeyword, StringComparison.OrdinalIgnoreCase))
                Users.Add(user);
    }

    [RelayCommand]
    private async Task LoadContactsAsync()
    {
        try
        {
            var contacts = await MyClient.SendRequestAsync<BaseResponseDto<IEnumerable<User>>>(MyHttpMethods.GET, "account", null, AuthToken);
            if (contacts is null) return;
            if (!contacts.Status) return;

            _userList = new List<User>(contacts.Data);
            Users = new ObservableCollection<User>(_userList);
        }
        catch(Exception e)
        {
            await _displayService.DisplayAlert("Error", e.Message, "OK");
        }
    }

    [RelayCommand]
    private Task NavigateInboxAsync(User user)
    {
        return _shell.GoToAsync($"{nameof(InboxPage)}?WithUserEmail={user.Email}&WithUserName={user.UserName}");
    }
}
