namespace ChatApp.Mobile.Services.Device
{
    public class ShellNavigationService
    {
        private readonly Shell _shell;
        public Shell Current => _shell;
        public ShellNavigationService()
        {
            _shell = Shell.Current;
        }

        //
        // Summary:
        //     Asynchronously navigates to state, optionally animating.
        //
        // Remarks:
        //     Note that Microsoft.Maui.Controls.ShellNavigationState has implicit conversions
        //     from string and System.Uri, so developers may write code such as the following,
        //     with no explicit instantiation of the Microsoft.Maui.Controls.ShellNavigationState:
        //     await Shell.Current.GoToAsync("app://xamarin.com/xaminals/animals/monkeys");
        public Task GoToAsync(ShellNavigationState state, bool animate, IDictionary<string, object> parameters) => _shell.GoToAsync(state, animate, parameters);

        public Task GoToAsync(ShellNavigationState state, IDictionary<string, object> parameters) => _shell.GoToAsync(state, parameters);
        //
        // Summary:
        //     Asynchronously navigates to state, optionally animating.
        //
        //Remarks:
        //     Note that Microsoft.Maui.Controls.ShellNavigationState has implicit conversions
        //     from string and System.Uri, so developers may write code such as the following,
        //     with no explicit instantiation of the Microsoft.Maui.Controls.ShellNavigationState:
        //     await Shell.Current.GoToAsync("app://xamarin.com/xaminals/animals/monkeys");
        public Task GoToAsync(ShellNavigationState state, bool animate) => _shell.GoToAsync(state, animate);

        public Task GoToAsync(ShellNavigationState state) => _shell.GoToAsync(state);
    }
}
