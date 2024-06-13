using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OlympusBugTracker.Client.Models;
using OlympusBugTracker.Client.Services.Interfaces;
using OlympusBugTracker.Data;
using OlympusBugTracker.Helpers;
using OlympusBugTracker.Helpers.Extensions;
using OlympusBugTracker.Models;

namespace OlympusBugTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TicketsController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        private string _userId => _userManager.GetUserId(User)!;

        private int? _companyId => User.FindFirst("CompanyId") != null ? int.Parse(User.FindFirst("CompanyId")!.Value) : null;

        private readonly ITicketDTOService _ticketService;

        public TicketsController(ITicketDTOService ticketService, UserManager<ApplicationUser> userManager)
        {
            _ticketService = ticketService;
            _userManager = userManager;
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


        [HttpGet("member")]
        public async Task<ActionResult<IEnumerable<TicketDTO>>> GetUserTickets()
        {
            try
            {
                if (_companyId is not null)
                {
                    IEnumerable<TicketDTO> tickets = await _ticketService.GetUserTicketsAsync(_userId, _companyId.Value);

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
                if (_companyId is null) return BadRequest();

                if (!User.IsInRole("Admin"))
                {
                    if (!ticket.Project!.Users.Any(u => u.Id == _userId)) return BadRequest();
                }

                TicketDTO ticketAdded = await _ticketService.AddTicketAsync(ticket, _companyId.Value);

                return Ok(ticketAdded);
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
            if (ticketId != ticket.Id) return BadRequest();

            try
            {
                if (_companyId is null) return BadRequest();

                TicketDTO? ticketToUpdate = await _ticketService.GetTicketByIdAsync(ticketId, _companyId.Value);

                if (User.IsInRole("ProjectManager"))
                {
                    if (!ticket.Project!.Users.Any(u => u.Id == _userId)) return BadRequest();
                }
                else if (User.IsInRole("Developer") || User.IsInRole("Submitter"))
                {
                    if (ticket.DeveloperUserId != _userId && ticket.SubmitterUserId != _userId) return BadRequest();

                    if (ticketToUpdate?.DeveloperUserId != ticket.DeveloperUserId) return BadRequest();
                }

                await _ticketService.UpdateTicketAsync(ticket, _companyId.Value, _userId);

                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }


        [HttpPut("archive/{ticketId:int}")]
        [Authorize(Roles = "Admin, ProjectManager")]
        public async Task<IActionResult> ArchiveTicket([FromRoute] int ticketId)
        {
            try
            {
                if (_companyId is null) return BadRequest();

                if (User.IsInRole("ProjectManager"))
                {
                    TicketDTO? ticketToArchive = await _ticketService.GetTicketByIdAsync(ticketId, _companyId.Value);

                    if (ticketToArchive is not null && !ticketToArchive.Project!.Users.Any(u => u.Id == _userId)) return BadRequest();
                }

                await _ticketService.ArchiveTicketAsync(ticketId, _companyId.Value);

                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }


        [HttpPut("restore/{ticketId:int}")]
        [Authorize(Roles = "Admin, ProjectManager")]
        public async Task<IActionResult> RestoreTicket([FromRoute] int ticketId)
        {
            try
            {
                if (_companyId is null) return BadRequest();

                if (User.IsInRole("ProjectManager"))
                {
                    TicketDTO? ticketToRestore = await _ticketService.GetTicketByIdAsync(ticketId, _companyId.Value);

                    if (ticketToRestore is not null && !ticketToRestore.Project!.Users.Any(u => u.Id == _userId)) return BadRequest();
                }

                await _ticketService.RestoreTicketAsync(ticketId, _companyId.Value);

                return Ok();
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
                if (_companyId is null) return BadRequest();

                TicketDTO? ticket = await _ticketService.GetTicketByIdAsync(commentDTO.TicketId, _companyId.Value);

                if (ticket is null) return NotFound();

                if (User.IsInRole("ProjectManager"))
                {
                    if (!ticket.Project!.Users.Any(u => u.Id == _userId)) return BadRequest();
                }
                else if (User.IsInRole("Developer") || User.IsInRole("Submitter"))
                {
                    if (ticket.DeveloperUserId != _userId && ticket.SubmitterUserId != _userId) return BadRequest();
                }

                await _ticketService.AddCommentAsync(commentDTO, _companyId.Value);

                return Ok();
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
            if (commentId != commentDTO.Id) return BadRequest();

            try
            {
                if (_companyId is null) return BadRequest();

                TicketCommentDTO? comment = await _ticketService.GetCommentByIdAsync(commentId, _companyId.Value);

                if (comment is null) return NotFound();

                if (comment.UserId != _userId) return BadRequest();

                await _ticketService.UpdateCommentAsync(commentDTO, _companyId.Value, _userId);

                return Ok();
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
                if (_companyId is null) return BadRequest();

                TicketCommentDTO? comment = await _ticketService.GetCommentByIdAsync(commentId, _companyId.Value);
                if (comment is null) return BadRequest();

                TicketDTO? ticket = await _ticketService.GetTicketByIdAsync(comment.TicketId, _companyId.Value);
                if (ticket is null) return BadRequest();

                if (User.IsInRole("ProjectManager"))
                {
                    if (!ticket.Project!.Users.Any(u => u.Id == _userId)) return BadRequest();
                }
                else if (User.IsInRole("Developer") || User.IsInRole("Submitter"))
                {
                    if (comment.UserId != _userId) return BadRequest();
                }

                await _ticketService.DeleteCommentAsync(commentId, _companyId.Value);

                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        #endregion

        #region Ticket Attachments

        [HttpPost("{Id:int}/attachments")]
        public async Task<ActionResult<TicketAttachmentDTO>> AddTicketAttachment(int Id, [FromForm] TicketAttachmentDTO attachment, [FromForm] IFormFile? file)
        {
            try
            {

                if (attachment.TicketId != Id || file is null) return BadRequest();

                ApplicationUser? user = await _userManager.GetUserAsync(User);

                TicketDTO? ticket = await _ticketService.GetTicketByIdAsync(Id, user!.CompanyId);

                if (ticket is null) return NotFound();

                if (User.IsInRole("ProjectManager"))
                {
                    if (!ticket.Project!.Users.Any(u => u.Id == _userId)) return BadRequest();
                }
                else if (User.IsInRole("Developer") || User.IsInRole("Submitter"))
                {
                    if (ticket.DeveloperUserId != _userId && ticket.SubmitterUserId != _userId) return BadRequest();
                }

                attachment.UserId = user!.Id;
                attachment.Created = DateTimeOffset.Now;

                if (string.IsNullOrWhiteSpace(attachment.FileName))
                {
                    attachment.FileName = file.FileName;
                }

                FileUpload upload = await UploadHelper.GetImageUploadAsync(file);

                TicketAttachmentDTO newAttachment = await _ticketService.AddTicketAttachment(attachment, upload.Data!, upload.Type!, user!.CompanyId);

                return Ok(newAttachment);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        [HttpDelete("attachments/{attachmentId}")]
        public async Task<IActionResult> DeleteTicketAttachment([FromRoute] int attachmentId)
        {
            if (_companyId is null) return BadRequest();

            TicketAttachmentDTO? ticketAttachment = await _ticketService.GetTicketAttachmentByIdAsync(attachmentId, _companyId.Value);

            if (ticketAttachment is null) return NotFound();

            TicketDTO? ticket = await _ticketService.GetTicketByIdAsync(ticketAttachment.TicketId, _companyId.Value);

            if (ticket is null) return NotFound();

            if (User.IsInRole("ProjectManager"))
            {
                if (!ticket.Project!.Users.Any(u => u.Id == _userId)) return BadRequest();
            }
            else if (User.IsInRole("Developer") || User.IsInRole("Submitter"))
            {
                if (ticketAttachment.UserId != _userId) return BadRequest();
            }

            await _ticketService.DeleteTicketAttachment(attachmentId, _companyId.Value);

            return NoContent();
        }

        #endregion

    }
}
