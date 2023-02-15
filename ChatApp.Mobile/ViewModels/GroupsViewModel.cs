namespace ChatApp.Mobile.ViewModels;

public partial class GroupsViewModel : BaseViewModel
{
    [ObservableProperty] private ObservableCollection<Group> _groups;
    [ObservableProperty] private bool _isRefreshing;

    public GroupsViewModel()
    {

    }

    [RelayCommand]
    private async Task RefreshAsync()
    {
        IsRefreshing = true;
        await Task.Delay(1500);
        IsRefreshing = false;
    }
}
