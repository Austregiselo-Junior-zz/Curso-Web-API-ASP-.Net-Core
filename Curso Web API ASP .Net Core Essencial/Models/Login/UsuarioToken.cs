using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Curso_Web_API_ASP_.Net_Core_Essencial.Models.Login
{
    public class UsuarioToken
    {
        public bool Authenticated { get; set; }
        public DateTime Expiration { get; set; }
        public string Token { get; set; }
        public string Message { get; set; }
    }
}
