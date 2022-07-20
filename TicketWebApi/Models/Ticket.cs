using System;
using System.Collections.Generic;

namespace TicketWebApi.Models
{
    public partial class Ticket
    {
        public Ticket()
        {
            HistorialIncidencia = new HashSet<HistorialIncidencium>();
        }

        public int TicketId { get; set; }
        public string PersonaSolicitante { get; set; } = null!;
        public DateTime FechaIngreso { get; set; }
        public string Asunto { get; set; } = null!;
        public string Descripcion { get; set; } = null!;

        public virtual ICollection<HistorialIncidencium> HistorialIncidencia { get; set; }
    }
}
