using System;
using System.Collections.Generic;
using System.Linq;
using IdentityServer4.Models;
using IdentityServer4.Services.InMemory;

namespace Crossroads.Server
{
    public class IdentityServerConfig
    {
        public static List<InMemoryUser> GetUsers()
        {
            return new List<InMemoryUser>
            {
                new InMemoryUser
                {
                    Subject = "1",
                    Username = "alice",
                    Password = "password"
                },
                new InMemoryUser
                {
                    Subject = "2",
                    Username = "bob",
                    Password = "password"
                }
            };
        }

        public static IEnumerable<Scope> GetScopes()
        {
            var scopes = new List<Scope>
            {
                StandardScopes.OpenId,
                StandardScopes.ProfileAlwaysInclude,
                StandardScopes.EmailAlwaysInclude,
                StandardScopes.OfflineAccess,
                StandardScopes.RolesAlwaysInclude,

                new Scope
                {
                    Enabled = true,
                    Name = "apiAccess",
                    DisplayName = "ApiAccess",
                    Description = "API Access",
                    Type = ScopeType.Resource,
                    Claims = new List<ScopeClaim>
                    {
                        new ScopeClaim("role"),
                        new ScopeClaim(StandardScopes.Email.Name),
                    },
                }
            };

            scopes.AddRange(StandardScopes.All);

            return scopes;
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "crossroads",
                    ClientName = "Crossroads Client",
                    ClientUri = "http://localhost:5000",
                    AllowedGrantTypes = GrantTypes.CodeAndClientCredentials,
                    RequireConsent = false,
                    AllowAccessToAllScopes = false,
                    AllowRememberConsent = true,

                    ClientSecrets = new List<Secret>
                    {
                        new Secret("secret".Sha256())
                    },

                    AccessTokenLifetime = 1200, // 20 minutes

                    RefreshTokenUsage = TokenUsage.OneTimeOnly,
                    RefreshTokenExpiration = TokenExpiration.Sliding,
                    SlidingRefreshTokenLifetime = 28800, // 8 hours
                    AbsoluteRefreshTokenLifetime = 57600, // 16 hours

                    RedirectUris = new List<string>
                    {
                        "http://localhost:5000"
                    },

                    PostLogoutRedirectUris = new List<string>
                    {
                        "http://localhost:5000"
                    },

                    AllowedCorsOrigins = new List<string>
                    {
                        "http://localhost:5000"
                    },

                    AllowedScopes = new List<string>
                    {
                        StandardScopes.OpenId.Name,
                        StandardScopes.Profile.Name,
                        StandardScopes.Email.Name,
                        StandardScopes.Roles.Name,
                        StandardScopes.OfflineAccess.Name,
                        "apiAccess"
                    }
                }
            };
        }
    }
}
