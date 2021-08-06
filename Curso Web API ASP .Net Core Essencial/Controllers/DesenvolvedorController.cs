using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Curso_Web_API_ASP_.Net_Core_Essencial.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class DesenvolvedorController : Controller
    {
        private readonly IConfiguration Configuration;

        public DesenvolvedorController(IConfiguration config)
        {
            Configuration = config;
        }

        /// <summary>
        /// Ação para receber os dados de nome e contato do autor.
        /// </summary>
        /// <returns></returns>
        [HttpGet("Desenvolvedor")]
        public string GetDesenvolvedor()
        {
            var dev = Configuration["Desenvolvedor"];
            var linkedin = Configuration["Linkedin"];
            return $"Desenvolvedor: {dev}, \n Linkedin: {linkedin}";
        }

    }
}
