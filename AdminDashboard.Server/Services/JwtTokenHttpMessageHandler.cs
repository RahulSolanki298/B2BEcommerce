using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;

namespace AdminDashboard.Server.Services
{
    public class JwtTokenHttpMessageHandler : DelegatingHandler
    {
        private readonly JwtTokenService _jwtTokenService;

        public JwtTokenHttpMessageHandler(JwtTokenService jwtTokenService)
        {
            _jwtTokenService = jwtTokenService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = _jwtTokenService.GetToken();

            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
