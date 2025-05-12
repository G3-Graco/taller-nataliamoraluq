using TallerBlazorApp.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace TallerBlazorApp.Data.Auth
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {

        [Inject]
        private TokenContainer? tokenContainer { get; set; }

        private readonly TokenContainer _tokenContainer;
        private ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());

        //public CustomAuthenticationStateProvider(ISessionStorageService sessionStorageService)
        //{
        //    _sessionStorageService = sessionStorageService;
        //}

        public CustomAuthenticationStateProvider(TokenContainer sessionStorageService)
        {
            _tokenContainer = sessionStorageService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            //var token = await _sessionStorageService.GetItemAsync<string>("token");
            var token = _tokenContainer?.token;
            if (string.IsNullOrEmpty(token))
            {
                return await Task.FromResult(new AuthenticationState(_anonymous));
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwt = tokenHandler.ReadJwtToken(token);
            var identity = new ClaimsIdentity(jwt.Claims, "jwt");
            var user = new ClaimsPrincipal(identity);
            return await Task.FromResult(new AuthenticationState(user));
        }

        public void AuthenticateUser(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwt = tokenHandler.ReadJwtToken(token);
                var identity = new ClaimsIdentity(jwt.Claims, "jwt");
                var user = new ClaimsPrincipal(identity);
                var state = new AuthenticationState(user);
                NotifyAuthenticationStateChanged(Task.FromResult(state));
            }
            catch (Exception ex)
            {

            }
        }
    }
}
