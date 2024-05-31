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

            IEnumerable<Ticket> tickets = await context.Tickets.Include(t => t.Project).Where(t => t.Project!.CompanyId == companyId && !t.Archived).OrderByDescending(t => t.Created).ToListAsync();

            return tickets;
        }
    }
}
