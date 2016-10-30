using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace Crossroads.Server.Controllers
{
    [Route("~/Auth")]
    public class AuthorizationController : Controller
    {
        /// <summary>
        /// Gets or sets the configuration.
        /// </summary>
        /// <value>
        /// The configuration.
        /// </value>
        public IConfiguration config { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationController"/> class.
        /// </summary>
        /// <param name="config">The configuration.</param>
        public AuthorizationController(IConfiguration config)
        {
            this.config = config;
        }

        /// <summary>
        /// TokenExchangeInput Class
        /// </summary>
        public class TokenExchangeInput
        {
            public string Code { get; set; }
            public string RedirectUri { get; set; }
            public string ClientId { get; set; }
            public string refresh_token { get; set; }
            public string grant_type { get; set; }
        }

        /// <summary>
        /// TokenResponse Class
        /// </summary>
        public class TokenResponse
        {
            public string token { get; set; }
            public string refresh_token { get; set; }
        }

        /// <summary>
        /// Exchanges the specified token exchange input.
        /// </summary>
        /// <param name="tokenExchangeInput">The token exchange input.</param>
        /// <returns></returns>
        [HttpPost("Token/Exchange")]
        public async Task<ActionResult> Exchange([FromBody]TokenExchangeInput tokenExchangeInput)
        {
            var identitySection = config.GetSection("Environment").GetSection("Identity");
            var baseUrl = identitySection.GetSection("Url").Value;
            var clientId = identitySection.GetSection("ClientId").Value;
            var secret = identitySection.GetSection("Secret").Value;

            var handler = new HttpClientHandler { UseProxy = false };

            var client = new TokenClient(
                $"{baseUrl}connect/token",
                clientId,
                secret, handler);

            IdentityModel.Client.TokenResponse tokenResponse = null;

            var action = string.IsNullOrEmpty(tokenExchangeInput.grant_type) ? "auth_code_request" : tokenExchangeInput.grant_type;

            if (action.Equals("refresh_token"))
            {
                tokenResponse = await client.RequestRefreshTokenAsync(tokenExchangeInput.refresh_token);
            }
            else
            {
                tokenResponse = await client.RequestAuthorizationCodeAsync(
                    tokenExchangeInput.Code,
                    tokenExchangeInput.RedirectUri);
            }

            //StoreRefreshToken(tokenResponse);

            return Json(new TokenResponse { token = tokenResponse.AccessToken, refresh_token = tokenResponse.RefreshToken });
        }


        private void StoreRefreshToken(IdentityModel.Client.TokenResponse tokenResponse)
        {
            var CookieOption = new CookieOptions();
            CookieOption.Path = "/";
            CookieOption.Domain = "";     //on localhost, Domain value is not needed !!

            Response.Cookies.Delete("rfshtkn");

            CookieOption.Expires = DateTime.Now.AddDays(30);
            CookieOption.HttpOnly = true;
            Response.Cookies.Append("rfshtkn", tokenResponse.RefreshToken, CookieOption);
        }

        /// <summary>
        /// Gets the user's info
        /// </summary>
        /// <returns></returns>
        [Route("UserInfo")]
        public async Task<IActionResult> UserInfo()
        {
            var identitySection = config.GetSection("Environment").GetSection("Identity");
            var baseUrl = identitySection.GetSection("Url").Value;
            var clientId = identitySection.GetSection("ClientId").Value;
            var secret = identitySection.GetSection("Secret").Value;

            var authHeader = HttpContext.Request.Headers["Authorization"];

            var token = authHeader.ToString().Substring("Bearer ".Length).Trim();

            if (!string.IsNullOrEmpty(token))
            {
                var handler = new HttpClientHandler { UseProxy = false };
                var client = new HttpClient(handler);
                client.BaseAddress = new System.Uri(baseUrl);
                client.SetBearerToken(token);


                var response = await client.GetStringAsync("connect/userinfo");
                var json = JToken.Parse(response);

                return Json(json);
            }

            return Unauthorized();
        }
    }
}
