using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OlympusBugTracker.Client.Models;
using OlympusBugTracker.Client.Services.Interfaces;
using OlympusBugTracker.Helpers.Extensions;

namespace OlympusBugTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private string _userId => User.GetUserId()!;

        private int? _companyId => User.FindFirst("CompanyId") != null ? int.Parse(User.FindFirst("CompanyId")!.Value) : null;

        private readonly ITicketDTOService _ticketService;

        public TicketsController(ITicketDTOService ticketService)
        {
            _ticketService = ticketService;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<TicketDTO>>> GetAllTickets()
        {
            try
            {
                if (_companyId is not null)
                {
                    IEnumerable<TicketDTO> tickets = await _ticketService.GetAllTicketsAsync(_companyId.Value);

                    return Ok(tickets);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }


        [HttpGet("archived")]
        public async Task<ActionResult<IEnumerable<TicketDTO>>> GetArchivedTickets()
        {
            try
            {
                if (_companyId is not null)
                {
                    IEnumerable<TicketDTO> tickets = await _ticketService.GetArchivedTicketsAsync(_companyId.Value);

                    return Ok(tickets);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }


        [HttpGet("ticket/{ticketId:int}")]
        public async Task<ActionResult<TicketDTO>> GetTicketById([FromRoute] int ticketId)
        {
            try
            {
                if (_companyId is not null)
                {
                    TicketDTO? ticket = await _ticketService.GetTicketByIdAsync(ticketId, _companyId.Value);

                    if (ticket is not null)
                    {
                        return Ok(ticket);
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }


        [HttpPost]
        public async Task<ActionResult<TicketDTO>> AddTicket([FromBody] TicketDTO ticket)
        {
            try
            {
                if (_companyId is not null)
                {
                    TicketDTO ticketAdded = await _ticketService.AddTicketAsync(ticket, _companyId.Value);

                    return Ok(ticketAdded);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }


        [HttpPut("{ticketId:int}")]
        public async Task<IActionResult> UpdateTicket([FromRoute] int ticketId, [FromBody] TicketDTO ticket)
        {
            try
            {
                if (_companyId is not null)
                {
                    TicketDTO? ticketToUpdate = await _ticketService.GetTicketByIdAsync(ticketId, _companyId.Value);

                    if (ticketToUpdate is not null)
                    {
                        await _ticketService.UpdateTicketAsync(ticket, _companyId.Value, _userId);

                        return Ok();
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }


        [HttpPut("archive/{ticketId:int}")]
        public async Task<IActionResult> ArchiveTicket([FromRoute] int ticketId)
        {
            try
            {
                if (_companyId is not null)
                {
                    TicketDTO? ticketToArchive = await _ticketService.GetTicketByIdAsync(ticketId, _companyId.Value);

                    if (ticketToArchive is not null)
                    {
                        await _ticketService.ArchiveTicketAsync(ticketId, _companyId.Value);

                        return Ok();
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }


        [HttpPut("restore/{ticketId:int}")]
        public async Task<IActionResult> RestoreTicket([FromRoute] int ticketId)
        {
            try
            {
                if (_companyId is not null)
                {
                    TicketDTO? ticketToRestore = await _ticketService.GetTicketByIdAsync(ticketId, _companyId.Value);

                    if (ticketToRestore is not null)
                    {
                        await _ticketService.RestoreTicketAsync(ticketId, _companyId.Value);

                        return Ok();
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        
    }
}
