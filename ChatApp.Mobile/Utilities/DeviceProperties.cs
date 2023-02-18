namespace ChatApp.Mobile.Utilities;

public class DeviceProperties
{
    public static bool IsDesktop =>
        DeviceInfo.Current.Platform.Equals(DevicePlatform.MacCatalyst) ||
        DeviceInfo.Current.Platform.Equals(DevicePlatform.macOS) ||
        DeviceInfo.Current.Platform.Equals(DevicePlatform.WinUI);
}
