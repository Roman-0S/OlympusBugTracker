using Microsoft.EntityFrameworkCore;
using OlympusBugTracker.Data;
using OlympusBugTracker.Models;
using OlympusBugTracker.Services.Interfaces;

namespace OlympusBugTracker.Services
{
    public class TicketRepository(IDbContextFactory<ApplicationDbContext> contextFactory) : ITicketRepository
    {

        #region Tickets

        public async Task<IEnumerable<Ticket>> GetAllTicketsAsync(int companyId)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            IEnumerable<Ticket> tickets = await context.Tickets.Include(t => t.Project)
                                                                  .ThenInclude(p => p!.Users)
                                                               .Where(t => t.Project!.CompanyId == companyId && !t.Archived)
                                                               .OrderByDescending(t => t.Created)
                                                               .ToListAsync();

            return tickets;
        }

        public async Task<IEnumerable<Ticket>> GetArchivedTicketsAsync(int companyId)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            IEnumerable<Ticket> tickets = await context.Tickets.Include(t => t.Project)
                                                                 .ThenInclude(p => p!.Users)
                                                               .Where(t => t.Project!.CompanyId == companyId && t.Archived)
                                                               .OrderByDescending(t => t.Created)
                                                               .ToListAsync();

            return tickets;
        }

        public async Task<Ticket?> GetTicketByIdAsync(int ticketId, int companyId)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            Ticket? ticket = await context.Tickets.Include(t => t.Project)
                                                  .Include(t => t.TicketAttachments)
                                                     .ThenInclude(ta => ta.User)
                                                  .Include(t => t.TicketComments)
                                                     .ThenInclude(tc => tc.User)
                                                  .FirstOrDefaultAsync(t => t.Id == ticketId && t.Project!.CompanyId == companyId);

            return ticket;
        }

        public async Task<Ticket> AddTicketAsync(Ticket ticket, int companyId)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            ticket.Created = DateTimeOffset.Now;

            //if (ticket.Project?.CompanyId == companyId)
            //{
            //}
            context.Tickets.Add(ticket);
            await context.SaveChangesAsync();

            return ticket;
        }

        public async Task UpdateTicketAsync(Ticket ticket, int companyId, string userId)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            bool shouldUpdate = await context.Tickets.AnyAsync(t => t.Id == ticket.Id);

            if (shouldUpdate)
            {
                ticket.Updated = DateTimeOffset.Now;

                context.Tickets.Update(ticket);
                await context.SaveChangesAsync();
            }
        }

        public async Task ArchiveTicketAsync(int ticketId, int companyId)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            Ticket? ticket = await context.Tickets.FirstOrDefaultAsync(t => t.Id == ticketId && t.Project!.CompanyId == companyId && !t.Archived);

            if (ticket is not null)
            {
                ticket.Archived = true;

                context.Tickets.Update(ticket);
                await context.SaveChangesAsync();
            }
        }

        public async Task RestoreTicketAsync(int ticketId, int companyId)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            Ticket? ticket = await context.Tickets.FirstOrDefaultAsync(t => t.Id == ticketId && t.Project!.CompanyId == companyId && t.Archived);

            if (ticket is not null)
            {
                ticket.Archived = false;

                context.Tickets.Update(ticket);
                await context.SaveChangesAsync();
            }
        }

        #endregion

        #region Ticket Comments

        public async Task AddCommentAsync(TicketComment comment, int companyId)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            comment.Created = DateTimeOffset.Now;

            context.TicketComments.Add(comment);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<TicketComment>> GetTicketCommentsAsync(int ticketId, int companyId)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            IEnumerable<TicketComment> comments = await context.TicketComments.Include(tc => tc.User)
                                                                              .Where(tc => tc.Ticket!.Project!.CompanyId == companyId && tc.TicketId == ticketId)
                                                                              .OrderByDescending(tc => tc.Created)
                                                                              .ToListAsync();

            return comments;
        }

        public async Task<TicketComment?> GetCommentByIdAsync(int commentId, int companyId)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            TicketComment? comment = await context.TicketComments.Include(tc => tc.User).FirstOrDefaultAsync(tc => tc.Id == commentId && tc.Ticket!.Project!.CompanyId == companyId);

            return comment;
        }

        public async Task DeleteCommentAsync(int commentId, int companyId)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            TicketComment? comment = await context.TicketComments.FirstOrDefaultAsync(tc => tc.Id == commentId && tc.Ticket!.Project!.CompanyId == companyId);

            if (comment is not null)
            {
                context.TicketComments.Remove(comment);
                await context.SaveChangesAsync();
            }
        }

        public async Task UpdateCommentAsync(TicketComment comment, int companyId, string userId)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            bool shouldUpdate = await context.TicketComments.AnyAsync(tc => tc.Id == comment.Id && tc.UserId == userId && tc.Ticket!.Project!.CompanyId == companyId);

            if (shouldUpdate)
            {
                context.TicketComments.Update(comment);
                await context.SaveChangesAsync();
            }
        }

        #endregion

        #region Ticket Attachments

        public async Task<TicketAttachment> AddTicketAttachment(TicketAttachment attachment, int companyId)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            Ticket? ticket = await context.Tickets.FirstOrDefaultAsync(t => t.Id == attachment.TicketId && t.Project!.CompanyId == companyId);

            if (ticket is not null)
            {
                attachment.Created = DateTimeOffset.Now;

                context.TicketAttachments.Add(attachment);
                await context.SaveChangesAsync();

                return attachment;
            }
            else
            {
                throw new ArgumentException("Ticket not found");
            }
        }

        public async Task DeleteTicketAttachment(int attachmentId, int companyId)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            TicketAttachment? attachment = await context.TicketAttachments.Include(a => a.Upload)
                                                                         .FirstOrDefaultAsync(a => a.Id == attachmentId && a.Ticket!.Project!.CompanyId == companyId);

            if (attachment is not null)
            {
                context.Remove(attachment);
                context.Remove(attachment.Upload!);
                await context.SaveChangesAsync();
            }
        }

        #endregion

    }
}
