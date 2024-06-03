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

            Ticket? ticket = await context.Tickets.Include(t => t.Project).FirstOrDefaultAsync(t => t.Id == ticketId && t.Project!.CompanyId == companyId);

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

            context.TicketComments.Add(comment);
            await context.SaveChangesAsync();
        }

        #endregion
    }
}
