﻿using Azure.Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text;

namespace Company_API
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IConfiguration _configuration;
        public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
                                          ILoggerFactory logger,
                                          System.Text.Encodings.Web.UrlEncoder encoder,
                                          Microsoft.AspNetCore.Authentication.ISystemClock clock,
                                          IConfiguration configuration)
            : base(options, logger, encoder, clock)
        {
            _configuration = configuration;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // Retrieve the "Authorization" header
            var authorizationHeader = Request.Headers["Authorization"].ToString();

            if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
            {
                return Task.FromResult(AuthenticateResult.Fail("Missing or invalid Authorization header"));
            }

            // Extract credentials
            var encodedCredentials = authorizationHeader.Substring("Basic ".Length).Trim();
            var decodedCredentials = Encoding.UTF8.GetString(Convert.FromBase64String(encodedCredentials)).Split(':');

            if (decodedCredentials.Length != 2)
            {
                return Task.FromResult(AuthenticateResult.Fail("Invalid Basic Authentication header format"));
            }

            var username = decodedCredentials[0];
            var password = decodedCredentials[1];

            if (username == _configuration["ApiSecurity:User"] && password == _configuration["ApiSecurity:Password"])
            {
                var claims = new[] { new Claim(ClaimTypes.Name, username) };
                var identity = new ClaimsIdentity(claims, "BasicAuthentication");
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, "BasicAuthentication");

                return Task.FromResult(AuthenticateResult.Success(ticket));
            }

            return Task.FromResult(AuthenticateResult.Fail("Invalid credentials"));
        }
    }
}
