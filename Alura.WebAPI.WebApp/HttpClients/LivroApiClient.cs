using Alura.ListaLeitura.Modelos;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using Lista = Alura.ListaLeitura.Modelos.ListaLeitura;

namespace Alura.ListaLeitura.HttpClients
{
    public class LivroApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContext;

        public LivroApiClient(IHttpContextAccessor httpContext, HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpContext = httpContext;
        }
        private void AdicionarToken() 
        {
            var token = _httpContext.HttpContext.User.FindFirstValue("Token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<Lista> GetListaLeituraAsync(TipoListaLeitura tipo)
        {
            AdicionarToken();
            var resposta = await _httpClient.GetAsync($"listaLeitura/{tipo}");
            resposta.EnsureSuccessStatusCode();
            return await resposta.Content.ReadAsAsync<Lista>();
        }

        public async Task DeleteLivroAsync(int id)
        {
            AdicionarToken();
            var resposta = await _httpClient.DeleteAsync($"livros/{id}");
            resposta.EnsureSuccessStatusCode();
        }

        public async Task<byte[]> GetCapaLivroAsync(int id)
        {
            AdicionarToken();
            var resposta = await _httpClient.GetAsync($"livros/{id}/capa");
            resposta.EnsureSuccessStatusCode();

            return await resposta.Content.ReadAsByteArrayAsync();
        }

        public async Task<LivroApi> GetLivroAsync(int id)
        {
            AdicionarToken();
            var resposta = await _httpClient.GetAsync($"livros/{id}");
            resposta.EnsureSuccessStatusCode();

            return await resposta.Content.ReadAsAsync<LivroApi>();
        }

        private string EnvolveComAspasDuplas(string valor)
        {
            return $"\"{valor}\"";
        }

        private MultipartFormDataContent CreateMultipartFormDataContent(LivroUpload model)
        {
            var content = new MultipartFormDataContent();

            content.Add(new StringContent(model.Titulo), EnvolveComAspasDuplas("titulo"));
            content.Add(new StringContent(model.Lista.ParaString()), EnvolveComAspasDuplas("autor"));

            if (string.IsNullOrEmpty(model.Subtitulo))
            {
                content.Add(new StringContent(model.Subtitulo), EnvolveComAspasDuplas("subtitulo"));
            }

            if (string.IsNullOrEmpty(model.Resumo))
            {
                content.Add(new StringContent(model.Resumo), EnvolveComAspasDuplas("resumo"));
            }

            if (string.IsNullOrEmpty(model.Autor))
            {
                content.Add(new StringContent(model.Autor), EnvolveComAspasDuplas("autor"));
            }

            if (model.Id > 0)
            {
                content.Add(new StringContent(model.Id.ToString()), EnvolveComAspasDuplas("id"));
            }

            if (model.Capa != null)
            {
                var imagemContent = new ByteArrayContent(model.Capa.ConvertToBytes());
                imagemContent.Headers.Add("content-type", "image/png");
                content.Add(imagemContent, EnvolveComAspasDuplas("capa"), EnvolveComAspasDuplas("capa.png"));
            }

            return content;
        }

        public async Task PostLivroAsync(LivroUpload model)
        {
            AdicionarToken();
            HttpContent content = CreateMultipartFormDataContent(model);
            var resposta = await _httpClient.PostAsync("livros", content);
            resposta.EnsureSuccessStatusCode();
        }

        public async Task PutLivroAsync(LivroUpload model)
        {
            AdicionarToken();
            HttpContent content = CreateMultipartFormDataContent(model);
            var resposta = await _httpClient.PutAsync("livros", content);
            resposta.EnsureSuccessStatusCode();
        }
    }
}
