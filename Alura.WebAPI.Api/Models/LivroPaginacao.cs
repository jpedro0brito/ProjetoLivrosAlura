using Alura.ListaLeitura.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Alura.WebAPI.Api.Models
{
    public static class LivroPaginadoExtensions
    {
        public static LivroPaginado ToLivroPaginado(this IQueryable<LivroApi> query, LivroPaginacao paginacao)
        {
            var totalItens = query.Count();
            var totalPaginas = (int)Math.Ceiling(totalItens / (double)paginacao.Tamanho);
            return new LivroPaginado
            {
                Total = totalItens,
                TotalPaginas = totalPaginas,
                NumeroPagina = paginacao.Pagina,
                TamanhoPagina = paginacao.Tamanho,
                Resultando = query
                    .Skip(paginacao.Tamanho * (paginacao.Pagina - 1))
                    .Take(paginacao.Tamanho)
                    .ToList(),
                Anterior = (paginacao.Pagina > 1 ? 
                    $"livros?tamanho={paginacao.Pagina-1}&pagina={paginacao.Tamanho}" : ""),
                Proximo = (paginacao.Pagina < totalPaginas ? 
                    $"livros?tamanho={paginacao.Pagina+1}&pagina={paginacao.Tamanho}" : ""),
            };
        }
    }
    public class LivroPaginado
    {
        public int Total { get; set; }
        public int TotalPaginas { get; set; }
        public int TamanhoPagina { get; set; }
        public int NumeroPagina { get; set; }
        public IList<LivroApi> Resultando { get; set; }
        public string Anterior { get; set; }
        public string Proximo { get; set; }
    }

    public class LivroPaginacao
    {
        public int Pagina { get; set; } = 1;
        public int Tamanho { get; set; } = 25;
    }
}
