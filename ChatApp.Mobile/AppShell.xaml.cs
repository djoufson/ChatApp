﻿namespace ChatApp.Mobile;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		Routing.RegisterRoute(nameof(InboxPage), typeof(InboxPage));
        Routing.RegisterRoute(nameof(NewMessagePage), typeof(NewMessagePage));
    }
}
