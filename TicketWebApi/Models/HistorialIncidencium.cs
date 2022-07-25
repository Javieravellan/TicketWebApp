using System;
using System.Collections.Generic;

namespace TicketWebApi.Models
{
    public partial class HistorialIncidencium
    {
        public int HistorialAtencionId { get; set; }
        public string UsuarioSoporte { get; set; } = null!;
        public DateTime FechaAtencion { get; set; }
        public string Comentario { get; set; } = null!;
        public int TicketId { get; set; }

        public virtual Ticket? Ticket { get; set; } = null!;
    }
}
