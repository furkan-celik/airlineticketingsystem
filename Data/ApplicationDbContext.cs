using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser, AppRole, string>
    {
        public static ApplicationDbContext _context;

        public DbSet<Address> Addresses { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<CreditCard> CreditCards { get; set; }
        public DbSet<Flight> Flights { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<OfferTicket> OfferTickets { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Airport> Airports { get; set; }
        public DbSet<SeatType> SeatTypes { get; set; }
        public DbSet<Ticket> Tickets { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            _context = this;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<CreditCard>().HasKey(o => new { o.CardNumber, o.CardDate });
            builder.Entity<AppUser>().Property(p => p.Gender).HasConversion(v => v.ToString(), v => (Genders)Enum.Parse(typeof(Genders), v));

            builder.Entity<OfferTicket>().HasKey(o => new { o.OfferId, o.TicketId });
        }
    }
}
