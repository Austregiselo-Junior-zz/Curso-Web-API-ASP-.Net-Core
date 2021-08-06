using Curso_Web_API_ASP_.Net_Core_Essencial.Models.Entitys;
using Curso_Web_API_ASP_.Net_Core_Essencial.Models.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Curso_Web_API_ASP_.Net_Core_Essencial.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class AutorizacaoController : Controller
    {
        private readonly UserManager<IdentityUser> UserManager;
        private readonly SignInManager<IdentityUser> SignInManager;

        public AutorizacaoController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }


        /// <summary>
        /// Método que verifica se a API ta atendento.
        /// </summary>
        /// <returns>Horário atual da requisição</returns>
        [HttpGet]
        public ActionResult<string> Get()
        {
            return "API :: Acessado em :" + DateTime.UtcNow.ToLongDateString();
        }

        /// <summary>
        /// Método para registro do usuário
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>Token de validação</returns>
        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] UsuarioEntity entity)
        {
            try
            {
                var user = new IdentityUser { UserName = entity.Nome, Email = entity.Email, EmailConfirmed = true };

                var result = await UserManager.CreateAsync(user, entity.Password);

                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }
                else
                {
                    await SignInManager.SignInAsync(user, false);
                    var service = new GeracaodeToken();
                    return Ok(service.GerarToken(entity));
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao tentar registrar. \n{ex.Message}");
            }

        }

        /// <summary>
        /// Método que verifica as credenciais do usuário, com menssagem de OK ou BadRequest para o usuário.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] UsuarioEntity entity)
        {
            try
            {
                var result = await SignInManager.PasswordSignInAsync(entity.Nome, entity.Password, isPersistent: false, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    var service = new GeracaodeToken();
                    return Ok(service.GerarToken(entity));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, $"Login inválido. \n User: {result}");
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao tentar logar. \n{ex.Message}");
            }
        }

       
    }
}
