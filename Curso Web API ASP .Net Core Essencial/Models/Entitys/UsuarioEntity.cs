using Curso_Web_API_ASP_.Net_Core_Essencial.Models.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Curso_Web_API_ASP_.Net_Core_Essencial.Models.Entitys
{
    public class UsuarioEntity
    {
        public string Nome { get; set; }
        public string Password { get; set; }
        public string ConformPassword { get; set; }
        public string Email { get; set; }

    }
}
