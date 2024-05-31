using Microsoft.EntityFrameworkCore;
using OlympusBugTracker.Data;
using OlympusBugTracker.Models;
using OlympusBugTracker.Services.Interfaces;

namespace OlympusBugTracker.Services
{
    public class TicketRepository(IDbContextFactory<ApplicationDbContext> contextFactory) : ITicketRepository
    {
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

        public async Task<Ticket> AddTicketAsync(Ticket ticket, int companyId)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            ticket.Created = DateTimeOffset.Now;
            
            if (ticket.Project?.CompanyId == companyId)
            {
                context.Tickets.Add(ticket);
                await context.SaveChangesAsync();
            }

            return ticket;
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

    }
}
