using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alura.ListaLeitura.Modelos;
using Alura.ListaLeitura.Persistencia;
using Alura.WebAPI.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Lista = Alura.ListaLeitura.Modelos.ListaLeitura;

namespace Alura.ListaLeitura.Api.Controllers.v2
{
    [Authorize]
    [ApiController]
    [ApiVersion("2.0")]
    [ApiExplorerSettings(GroupName = "v2")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ListasLeituraController : ControllerBase
    {
        private readonly IRepository<Livro> _repo;

        public ListasLeituraController(IRepository<Livro> repository)
        {
            _repo = repository;
        }

        private Lista CriaLista(TipoListaLeitura tipo)
        {
            return new Lista
            {
                Tipo = tipo.ParaString(),
                Livros = _repo.All
                    .Where(l => l.Lista == tipo)
                    .Select(l => l.ToApi())
                    .ToList()
            };
        }

        [HttpGet("{tipo}")]
        [SwaggerOperation(
            Summary = "Recupera a lista de leitura identificada por seu {tipo}.",
            Produces = new[] { "application/json", "application/xml" }
        )]
        [ProducesResponseType(200, Type = typeof(Lista))]
        [ProducesResponseType(500, Type = typeof(ErrorResponse))]
        public IActionResult Recuperar(
            [FromRoute][SwaggerParameter("Tipo da lista a ser obtida.")] TipoListaLeitura tipo)
        {
            var lista = CriaLista(tipo);
            return Ok(lista);
        }
    }
}