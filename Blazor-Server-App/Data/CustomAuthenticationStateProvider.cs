using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Blazored.SessionStorage;
namespace Blazor_Server_App.Data
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        ISessionStorageService _sessionStorageService;
        public CustomAuthenticationStateProvider(ISessionStorageService sessionStorageService)
        {
            _sessionStorageService = sessionStorageService;
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {            
            var identity = new ClaimsIdentity();
            string email = await _sessionStorageService.GetItemAsync<string>("username");

            if (!string.IsNullOrEmpty(email))
            {
                identity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, email)
                }, "apiauth_type");
            }

            var user = new ClaimsPrincipal(identity);

            return await Task.FromResult(new AuthenticationState(user));
        }

        public void MarkAsAuthorize(string email)
        {
            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, email)
            }, "apiauth_type");

            var user = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }
        public void MarkAsUnAuthorize()
        {
            var identity = new ClaimsIdentity();

            var user = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

    }
}
