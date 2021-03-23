
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using ParkyAPI.Data;
using ParkyAPI.Models;
using System;
using System.IO;
using System.Linq;

namespace ParkyAPI.DDbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public DbInitializer(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
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

             // add national parks
            _db.NationalParks.Add(new NationalPark
            {
                Name = "Bryce National Park",
                State = "UT",
                Established = new DateTime(1981, 1, 1),
                Picture = File.ReadAllBytes(Path.Combine(_webHostEnvironment.ContentRootPath, "DbInitializer\\img\\BryceNationalPark.jpg"))
            });

            _db.NationalParks.Add(new NationalPark
            {
                Name = "Glacier National Park",
                State = "YW",
                Established = new DateTime(1845, 1, 1),
                Picture = File.ReadAllBytes(Path.Combine(_webHostEnvironment.ContentRootPath, "DbInitializer\\img\\GlacierNationalPark.jpg"))                
            });

            _db.SaveChanges();

            // add trails
            var park = _db.NationalParks.Where(x => x.Name == "Bryce National Park").Single();

            _db.Trails.Add(new Trail
            {
                NationalParkId = park.Id,
                Name = "Navajo Loop Trail",
                Distance = 1.4,
                Elevation = 400,
                Difficulty = Trail.DifficultyType.Moderate
            });

            _db.Trails.Add(new Trail
            {
                NationalParkId = park.Id,
                Name = "Queens Garden Trail",
                Distance = 1.8,
                Elevation = 980,
                Difficulty = Trail.DifficultyType.Difficult
            });

            _db.Trails.Add(new Trail
            {
                NationalParkId = park.Id,
                Name = "Rim Trail",
                Distance = 5.5,
                Elevation = 400,
                Difficulty = Trail.DifficultyType.Easy
            });

            park = _db.NationalParks.Where(x => x.Name == "Glacier National Park").Single();

            _db.Trails.Add(new Trail
            {
                NationalParkId = park.Id,
                Name = "Grinnell Glacier Trail",
                Distance = 5.5,
                Elevation = 400,
                Difficulty = Trail.DifficultyType.Moderate
            });

            _db.Trails.Add(new Trail
            {
                NationalParkId = park.Id,
                Name = "Highline Trail",
                Distance = 2.1,
                Elevation = 300,
                Difficulty = Trail.DifficultyType.Easy
            });

            _db.SaveChanges();
        }
    }
}
