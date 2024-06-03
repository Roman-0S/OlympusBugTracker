﻿using Microsoft.AspNetCore.Http;
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

        #region Tickets

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

        #endregion

        #region Ticket Comments

        [HttpGet("{ticketId:int}/comments")]
        public async Task<ActionResult<IEnumerable<TicketCommentDTO>>> GetTicketComments([FromRoute] int ticketId)
        {
            try
            {
                if (_companyId is not null)
                {
                    IEnumerable<TicketCommentDTO> comments = await _ticketService.GetTicketCommentsAsync(ticketId, _companyId.Value);

                    return Ok(comments);
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


        [HttpGet("comments/{commentId:int}")]
        public async Task<ActionResult<TicketCommentDTO>> GetCommentById([FromRoute] int commentId)
        {
            try
            {
                if (_companyId is not null)
                {
                    TicketCommentDTO? comment = await _ticketService.GetCommentByIdAsync(commentId, _companyId.Value);

                    if (comment is not null)
                    {
                        return Ok(comment);
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


        [HttpPost("comments")]
        public async Task<IActionResult> AddComment([FromBody] TicketCommentDTO commentDTO)
        {
            try
            {
                if (_companyId is not null)
                {
                    await _ticketService.AddCommentAsync(commentDTO, _companyId.Value);

                    return Ok();
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


        [HttpPut("comment/{commentId:int}")]
        public async Task<IActionResult> UpdateComment([FromRoute] int commentId, [FromBody] TicketCommentDTO commentDTO)
        {
            try
            {
                if (_companyId is not null)
                {
                    TicketCommentDTO? comment = await _ticketService.GetCommentByIdAsync(commentId, _companyId.Value);

                    if (comment is not null)
                    {
                        await _ticketService.UpdateCommentAsync(commentDTO, _companyId.Value, _userId);

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


        [HttpDelete("{commentId:int}")]
        public async Task<IActionResult> DeleteComment([FromRoute] int commentId)
        {
            try
            {
                if (_companyId is not null)
                {
                    TicketCommentDTO? comment = await _ticketService.GetCommentByIdAsync(commentId, _companyId.Value);

                    if (comment is not null)
                    {
                        if (comment.UserId == _userId)
                        {
                            await _ticketService.DeleteCommentAsync(commentId, _companyId.Value);

                            return Ok();
                        }
                        else
                        {
                            return BadRequest();
                        }
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

        #endregion

    }
}
