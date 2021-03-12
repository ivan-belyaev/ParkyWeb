
using Microsoft.EntityFrameworkCore;
using ParkyAPI.Data;
using ParkyAPI.Models;
using System;
using System.Linq;

namespace ParkyAPI.DDbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _db;


        public DbInitializer(ApplicationDbContext db)
        {
            _db = db;
        }

        public void Initalize()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception)
            {
                throw new Exception("DB doesn't work");
            }

            if (_db.Users.FirstOrDefault(x => x.Role == "Admin") != null) return;

            // Create default admin

            _db.Users.Add(new User
            {
                Username = "admin",
                Password = "admin",
                Role = "Admin"
            });

            // Create default user

            _db.Users.Add(new User
            {
                Username = "test",
                Password = "test",
                Role = "User"
            });

            _db.SaveChanges();

        }
    }
}
