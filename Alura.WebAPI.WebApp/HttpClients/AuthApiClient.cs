using Alura.ListaLeitura.Seguranca;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Alura.ListaLeitura.HttpClients
{
    public class LoginResult 
    {
        public bool Succeeded { get; set; }
        public string Token { get; set; }
        public LoginResult(string token, HttpStatusCode statusCode)
        {
            Token = token;
            Succeeded = HttpStatusCode.OK == statusCode;
        }
    }
    public class AuthApiClient
    {
        private readonly HttpClient _httpClient;

        public AuthApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<LoginResult> PostLoginAsync(LoginModel model) 
        {
            var resposta = await _httpClient.PostAsJsonAsync("login", model);
            resposta.EnsureSuccessStatusCode();
            return new LoginResult(await resposta.Content.ReadAsStringAsync(), resposta.StatusCode);
        }
    }
}
