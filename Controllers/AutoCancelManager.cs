using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class AutoCancelManager
    {
        private static AutoCancelManager _autoCancelManager { get; set; }
        public static AutoCancelManager AutoCancelManagerStatic
        {
            get
            {
                if (_autoCancelManager == null)
                    _autoCancelManager = new AutoCancelManager();
                return _autoCancelManager;
            }
        }

        public async void DeleteOverTime(Reservation reservation, ApplicationDbContext _context)
        {
            await Task.Delay(Math.Max(0, (int)(reservation.processTime + reservation.Flight.ResCancelTime - DateTime.Now).TotalMilliseconds));

            AppSettings appSettings;
            using (StreamReader r = new StreamReader("./appsettings.json"))
            {
                string json = r.ReadToEnd();
                appSettings = JsonConvert.DeserializeObject<AppSettings>(json);
                var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
                optionsBuilder.UseLazyLoadingProxies().UseMySql(appSettings.ConnectionStrings["DefaultConnection"]);
                _context = new ApplicationDbContext(optionsBuilder.Options);
                _context.Database.EnsureCreated();
            }

            if (_context.Reservations.Contains(reservation))
            {
                foreach (var item in reservation.Seats)
                {
                    item.Availability = true;
                    _context.Seats.Update(item);
                }
                _context.Reservations.Remove(reservation);
                _context.SaveChanges();
            }
        }

        public async void DeleteOverTime(int purchaseId, ApplicationDbContext _context, double delayMinutes = 10)
        {
            await Task.Delay(TimeSpan.FromMinutes(delayMinutes));

            AppSettings appSettings;
            using (StreamReader r = new StreamReader("./appsettings.json"))
            {
                string json = r.ReadToEnd();
                appSettings = JsonConvert.DeserializeObject<AppSettings>(json);
                var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
                optionsBuilder.UseLazyLoadingProxies().UseMySql(appSettings.ConnectionStrings["DefaultConnection"]);
                _context = new ApplicationDbContext(optionsBuilder.Options);
                _context.Database.EnsureCreated();
            }

            Purchase purchase = ApplicationDbContext._context.Purchases.Find(purchaseId);

            if (purchase != null && !purchase.IsProcessed)
            {
                foreach (var item in purchase.Tickets)
                {
                    foreach (var item2 in item.Seats)
                    {
                        item2.Availability = true;
                        _context.Seats.Update(item2);
                    }
                }
                _context.Purchases.Remove(purchase);
                _context.SaveChanges();
            }
        }

        public class AppSettings
        {
            public Dictionary<string, string> ConnectionStrings { get; set; }
        }
    }
}
