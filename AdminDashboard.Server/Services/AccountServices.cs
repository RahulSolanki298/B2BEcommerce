using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AdminDashboard.Server.Helpers;
using AdminDashboard.Server.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;


namespace AdminDashboard.Server.Services
{
    public class AccountService
    {
        private readonly HttpClient _httpClient;
        private readonly JwtTokenService _jwtTokenService;
        private readonly NavigationManager _navigationManager;

        public AccountService(HttpClient httpClient, JwtTokenService jwtTokenService, NavigationManager navigationManager)
        {
            _httpClient = httpClient;
            _jwtTokenService = jwtTokenService;
            _navigationManager = navigationManager;
        }

        public async Task<SuccessResponse> AdminLoginAsync(AdminLoginModel model)
        {
            // Send the login request to the Web API
            var response = await _httpClient.PostAsJsonAsync($"{AppStatic.ApiUrl}/api/account/admin-login", model);

            if (response.IsSuccessStatusCode)
            {
                // If login is successful, read the JWT token from the response
                var successResponse = await response.Content.ReadFromJsonAsync<SuccessResponse>();

                // Assuming the JWT token is returned in the SuccessResponse
                // Set the token in a cookie for future API requests (in case of Blazor Server)
                if (successResponse?.Token != null)
                {
                    // Save the token in a cookie (or other storage like local storage/session if needed)
                    // Here we assume the `Token` property exists on SuccessResponse
                    SetTokenInCookie(successResponse.Token);
                }

                return successResponse;
            }
            else
            {
                // If the login fails, throw an exception with the error message
                throw new Exception("Invalid credentials or unauthorized.");
            }
        }

        // Save the token in a cookie
        private void SetTokenInCookie(string token)
        {
            // This can be stored in HttpContext or any other storage mechanism
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true, // Only set to true if using HTTPS
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(1) // Adjust the expiration time as needed
            };

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Optionally, set the token in the browser's cookie
            _navigationManager.ToAbsoluteUri("/");  // Ensure page reloads if necessary
        }

        public async Task LogoutAsync()
        {
            // Perform logout by removing the JWT token (if stored in a cookie)
            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.UtcNow.AddDays(-1) // Expire the token cookie
            };

            // Remove the token from the browser cookies (or clear session storage)
            _httpClient.DefaultRequestHeaders.Authorization = null; // Reset Authorization header
            _navigationManager.NavigateTo("/login"); // Redirect to login page after logout
        }
    }

}
