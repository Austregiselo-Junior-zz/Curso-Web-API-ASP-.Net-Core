using Curso_Web_API_ASP_.Net_Core_Essencial.Models.Entitys;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Curso_Web_API_ASP_.Net_Core_Essencial.Models.Services
{
    public class GeracaodeToken
    {
        private readonly IConfiguration Config;

        public GeracaodeToken(IConfiguration config)
        {
            Config = config;
        }

        public GeracaodeToken()
        {
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
