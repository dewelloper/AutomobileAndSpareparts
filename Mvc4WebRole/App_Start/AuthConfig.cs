using Microsoft.Web.WebPages.OAuth;

namespace Mvc4WebRole
{
    public static class AuthConfig
    {
        public static void RegisterAuth()
        {
            OAuthWebSecurity.RegisterFacebookClient("356233614502937", "bebabbe38d71df9befc7870e5e5ca752");

            OAuthWebSecurity.RegisterGoogleClient();
        }
    }
}
