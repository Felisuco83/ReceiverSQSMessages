using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcCoreSenderSQS.Models
{
    public class MensajeUsuario
    {
        public string Asunto { get; set; }
        public string Email { get; set; }
        public string Mensaje { get; set; }
        public DateTime FechaMensaje { get; set; }
    }
}
