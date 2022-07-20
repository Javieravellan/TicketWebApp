using Microsoft.AspNetCore.Mvc;
using TicketWebApi.Models;

namespace TicketWebApi.Controller;

[ApiController]
[Route("api/[controller]")]
public class TicketSysController : ControllerBase
{
    private readonly TicketDbContext _context;

    public TicketSysController(TicketDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<ActionResult<Ticket>> CreateTicket(Ticket ticket)
    {
        if (ticket == null) 
        {
            return BadRequest("No se proporcionaron los datos para el ticket.");
        }
        if (ticket.FechaIngreso == null) ticket.FechaIngreso = DateTime.Today;
        // Saving...
        _context.Tickets.Add(ticket);
        await _context.SaveChangesAsync();
        return ticket;
    }

    [HttpGet]
    public  ActionResult<List<Ticket>> GetAllTickets() 
    {
        var tickets = _context.Tickets.Select(t => new Ticket() {
            TicketId = t.TicketId,
            PersonaSolicitante = t.PersonaSolicitante,
            Asunto = t.Asunto,
            Descripcion = t.Descripcion,
            FechaIngreso = t.FechaIngreso
        }).ToList();

        if (tickets.Count == 0) 
        {
            return NotFound("No hay tickets registrados");
        }

        return Ok(tickets);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Ticket>> GetTicketById(int id)
    {
        if (id == 0)
            return BadRequest("El ID es incorrecto.");
        var ticketEncontrado = await _context.Tickets.FindAsync(id);
        if (ticketEncontrado == null)
            return NotFound(String.Format("El Ticket con ID {0} no está registrado", id));
        
        return Ok(ticketEncontrado);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Ticket>> UpdateTicket(int id, Ticket ticket)
    {
        if (id == 0)
            return BadRequest("El ID es incorrecto.");
        Ticket? ticketEncontrado = await _context.Tickets.FindAsync(id);

        if (ticketEncontrado is null)
            return NotFound(String.Format("El Ticket con ID {0} no está registrado", id));
        
        ticketEncontrado!.Asunto = ticket.Asunto;
        ticketEncontrado.Descripcion = ticket.Descripcion;
        ticketEncontrado.PersonaSolicitante = ticket.PersonaSolicitante;
        if (ticket.HistorialIncidencia.Count > 0)
            ticketEncontrado.HistorialIncidencia.Union<HistorialIncidencium>(ticket.HistorialIncidencia);
        
        await _context.SaveChangesAsync();
        return Ok(ticketEncontrado);
    }

    [HttpGet("tickets/fecha")]
    public ActionResult<List<Ticket>> GetAllTicketsByFechaIngreso(DateTime f1, DateTime f2)
    {
        if (f1 == null || f2 == null)
            return BadRequest("La fecha proporcionada es null");
        if (f1 > f2)
            return BadRequest("El límite inferior es mayor que el límite superior.");
        
        var ticketsEncontrados = _context.Tickets.Where(t => t.FechaIngreso >= f1 && t.FechaIngreso <= f2)
        .ToList();

        if (ticketsEncontrados.Count == 0)
            return NotFound("No hay tickets en el periodo especificado.");
        
        return Ok(ticketsEncontrados);
    }

    [HttpGet("tickets/nombreSolicitante")]
    public ActionResult<List<Ticket>> GetTicketsByNombreSolicitante(string nombreSolicitante)
    {
        if (string.IsNullOrEmpty(nombreSolicitante))
            return BadRequest("El nombre del solicitante es incorrecto");
        var tickets = _context.Tickets.Where(t => t.PersonaSolicitante == nombreSolicitante).ToList();
        
        if (tickets == null)
            return NotFound(string.Format("No hay tickets registrados a nombre de {0}.", nombreSolicitante));
        
        return Ok(tickets);
    }
}