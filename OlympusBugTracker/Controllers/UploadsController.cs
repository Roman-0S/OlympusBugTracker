using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using OlympusBugTracker.Data;
using OlympusBugTracker.Models;

namespace OlympusBugTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UploadsController(ApplicationDbContext context) : ControllerBase
    {
        private int? _companyId => User.FindFirst("CompanyId") != null ? int.Parse(User.FindFirst("CompanyId")!.Value) : null;

        [HttpGet("{id:guid}")]
        [OutputCache(VaryByRouteValueNames = ["id"], Duration = 60 * 60)]
        public async Task<IActionResult> GetImage(Guid Id)
        {

            FileUpload? file = await context.Images.FirstOrDefaultAsync(i => i.Id == Id);

            if (file is null) return NotFound();

            TicketAttachment? attachment = await context.TicketAttachments.Include(a => a.Ticket)
                                                                             .ThenInclude(t => t.Project)
                                                                          .FirstOrDefaultAsync(a => a.UploadId == Id);

            if (attachment is null)
            {
                return File(file.Data!, file.Type!);
            }
            else
            {
                if (_companyId is null || attachment.Ticket?.Project?.CompanyId != _companyId)
                {
                    return Unauthorized();
                }
                else
                {
                    return File(file.Data!, file.Type!, attachment.FileName);
                }
            }
        }


    }
}
