using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using ServerApp.Data.Models;
using System.Security.Claims;

namespace ServerApp.Auth
{
    public class AuthStateProvider : AuthenticationStateProvider
    {
        private ClaimsPrincipal _anonymous;
        private ProtectedSessionStorage _sessionStorage;

        public AuthStateProvider(ProtectedSessionStorage sessionStorage)
        {
            var identity = new ClaimsIdentity();
            _anonymous = new ClaimsPrincipal(identity);
            _sessionStorage = sessionStorage;
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var userSessionStorageResult = await _sessionStorage.GetAsync<UserSession>("UserSession");
                var userSession = userSessionStorageResult.Success ? userSessionStorageResult.Value : null;
                var principal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userSession.Id),
                new Claim(ClaimTypes.Role, userSession.Role),
            }, "CustomAuth"));
                return await Task.FromResult(new AuthenticationState(principal));
            }
            catch
            {
                return await Task.FromResult(new AuthenticationState(_anonymous));
            }
        }

        //public async Task<AuthenticationState> AuthenticateUser(User user)
        //{
        //    var identity = new ClaimsIdentity(new[]
        //    {
        //        new Claim(ClaimTypes.NameIdentifier, user.Id),
        //        new Claim(ClaimTypes.Name, user.UserName),
        //        new Claim(ClaimTypes.Role, user.Role.Value),
        //    }, "Custom Authentication");

        //    var principal = new ClaimsPrincipal(identity);
        //    _state = new AuthenticationState(principal);
        //    NotifyAuthenticationStateChanged(
        //        Task.FromResult(_state));
        //    return _state;
        //}

        public async Task UpdateAuthState(UserSession userSession)
        {
            ClaimsPrincipal userPrincipal;
            if (userSession != null)
            {
                await _sessionStorage.SetAsync("UserSession", userSession);
                userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, userSession.Id),
                    new Claim(ClaimTypes.Role, userSession.Role),
                }));
            }
            else
            {
                await _sessionStorage.DeleteAsync("UserSession");
                userPrincipal = _anonymous;
            }
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(userPrincipal)));
        }

        //public async Task<AuthenticationState> ResetAuthState()
        //{
        //    var user = new ClaimsPrincipal(new ClaimsIdentity());
        //    _state = new AuthenticationState(user);
        //    NotifyAuthenticationStateChanged(Task.FromResult(_state));
        //    return _state;
        //}
    }
}
