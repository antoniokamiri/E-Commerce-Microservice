using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Collections.Generic;

namespace IdentityServer.Clients
{
    public class Config
    {
        public static IEnumerable<Client> Clients =>
        new Client[]
        {
             new Client
             {
                 ClientId = "ecommerceClient",
                 AllowedGrantTypes = GrantTypes.ClientCredentials,
                 ClientSecrets ={ new Secret("secret".Sha256())},
                 AllowedScopes = { "Basket.API", "Catalog.API" , "Ordering.API" }
             }

        };
        public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new ApiScope("Basket.API", "Basket API"),
            new ApiScope("Catalog.API", "Catalog API"),
            new ApiScope("Ordering.API", "Ordering API")
        };
        public static IEnumerable<ApiResource> ApiResources =>
        new ApiResource[]
        {
        };
        public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
        };
        public static List<TestUser> TestUsers =>
        new List<TestUser>
        {
        };
    }
}
