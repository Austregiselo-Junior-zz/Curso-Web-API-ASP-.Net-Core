using Curso_Web_API_ASP_.Net_Core_Essencial.Models.Entitys;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Curso_Web_API_ASP_.Net_Core_Essencial.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class AutorizacaoController : Controller
    {
        private readonly UserManager<IdentityUser> UserManager;
        private readonly SignInManager<IdentityUser> SignInManager;
        private readonly IConfiguration Config;

        public AutorizacaoController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration config)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            Config = config;
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

                    return Ok(GerarToken(entity));
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
                    return Ok(GerarToken(entity));
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

        /// <summary>
        /// Método para gerar o token Jwt. (O código abaixo está retornando um erro!!!)
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns>Retorna um objeto.</returns>
        public string GerarToken(UsuarioEntity usuario)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(Config["Jwt:Key"]);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim(ClaimTypes.Name, usuario.Nome.ToString()),
                    new Claim(ClaimTypes.Email, usuario.Email.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddHours(2),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }


        }
    }
}
