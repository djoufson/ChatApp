using System.Net.Security;

namespace ChatApp.Mobile.Helper;
public class DevHttpsConnectionHelper
{
    public DevHttpsConnectionHelper(int sslPort)
    {
        SslPort = sslPort;
        DevServerRootUrl = FormattableString.Invariant($"https://{DevServerName}:{SslPort}");
        LazyHttpClient = new Lazy<HttpClient>(() => new HttpClient(GetPlatformMessageHandler()));
    }

    public int SslPort { get; }

    public static string DevServerName => Constants.DEV_SERVER_NAME;

    public string DevServerRootUrl { get; set; }

    private readonly Lazy<HttpClient> LazyHttpClient;
    public HttpClient HttpClient => LazyHttpClient.Value;

    public static HttpMessageHandler GetPlatformMessageHandler()
    {
#if WINDOWS
        return null;
#elif ANDROID
        var handler = new CustomAndroidMessageHandler
        {
            ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
            {
                if (cert != null && cert.Issuer.Equals("CN=localhost"))
                    return true;
                return errors == SslPolicyErrors.None;
            }
        };
        return handler;

#else
        throw new PlatformNotSupportedException("Only Windows and Android currently supported.");
#endif
    }

#if ANDROID
    internal sealed class CustomAndroidMessageHandler : Xamarin.Android.Net.AndroidMessageHandler
    {
        protected override Javax.Net.Ssl.IHostnameVerifier GetSSLHostnameVerifier(Javax.Net.Ssl.HttpsURLConnection connection)
            => new CustomHostnameVerifier();

        private sealed class CustomHostnameVerifier : Java.Lang.Object, Javax.Net.Ssl.IHostnameVerifier
        {
            public bool Verify(string hostname, Javax.Net.Ssl.ISSLSession session)
            {
                return
                    Javax.Net.Ssl.HttpsURLConnection.DefaultHostnameVerifier.Verify(hostname, session)
                    || hostname == "10.0.2.2" && session.PeerPrincipal?.Name == "CN=localhost";
            }
        }
    }
#endif
}