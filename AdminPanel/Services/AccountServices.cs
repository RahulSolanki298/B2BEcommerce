using AdminPanel.Helpers;
using AdminPanel.ViewModel;

namespace AdminPanel.Services
{
    public class AccountServices
    {
        private readonly HttpClient _httpClient;

        public AccountServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<SuccessResponse> AdminLoginAsync(AdminLoginModel model)
        {
            var response = await _httpClient.PostAsJsonAsync($"{@AppStatic.ApiUrl}/api/account/admin-login", model);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<SuccessResponse>();
            }
            else
            {
                throw new Exception("Invalid credentials or unauthorized.");
            }
        }

    }
}
