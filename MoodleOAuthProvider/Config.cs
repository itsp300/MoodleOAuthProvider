using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Collections.Generic;

namespace MoodleOAuthProvider
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResource("subjects", new string[]{"subject"})
            };
        }

        public static IEnumerable<ApiResource> GetApis()
        {
            return new List<ApiResource>
            {
                new ApiResource("moodle.auth", "Moodle Authorization"),
                new ApiResource("backend.server", "Backend Server")
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientName = "Auth0",
                    ClientId = "Auth0",
                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = GrantTypes.Implicit,
                    // scopes that client has access to
                    AllowedScopes = { "moodle.auth", "openid", "profile", "email" },
                    RedirectUris ={ "https://itsp300.auth0.com/login/callback" }
                },
                new Client
                {
                    ClientName = "raspberry_pi",
                    ClientId = "ITSP300_RASPBERRY_PI",
                    ClientSecrets = {
                        new Secret("ITSP300_RASPBERRY_PI_SECRET".Sha256(), null)
                    },
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = { "backend.server" }
                },
                new Client
                {
                    ClientName = "face_rec_server",
                    ClientId = "ITSP300_FACE_REC_SERVER",
                    ClientSecrets = {
                        new Secret("ITSP300_FACE_REC_SERVER_SECRET".Sha256(), null)
                    },
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = { "backend.server" }
                },
                new Client
                {
                    ClientName = "android_app",
                    ClientId = "ITSP300_ANDROID_APP",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowedScopes = { "backend.server", "openid", "profile", "email" }
                }
            };
        }
    }
}
