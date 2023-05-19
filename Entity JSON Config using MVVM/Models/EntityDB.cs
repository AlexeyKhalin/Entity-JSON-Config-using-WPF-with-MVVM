using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace Entity_JSON_Config_using_MVVM.Models
{
    public class ApplicationContext : DbContext
    {
        //We have DefaultConnection in our JSON file
        //YOU WILL NEED TO DOWNLOAD Microsoft.Extensions.Configuration.Json NUGET PACKAGE
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var MyConfig = new ConfigurationBuilder();
            MyConfig.AddJsonFile("Models/DBConfig.json", optional: false).Build();
            var config = MyConfig.Build();
            string connectionString = config.GetConnectionString("DefaultConnection");
            var options = optionsBuilder.UseSqlite(connectionString).Options;
        }

        public DbSet<SAVE> Save { get; set; } = null!;
        public ApplicationContext() => Database.EnsureCreated();

    }
    public class SAVE
    {
        public int Id { get; set; }
        public string? Data { get; set; }
    }

    class EntityDB
    {
        private string data;
        public string Data
        {
            get { return data; }
            set
            {
                data = value;
            }
        }
        public static void ClearDB()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                List<SAVE> ToDelete = db.Save.ToList();
                db.Save.RemoveRange(ToDelete);
                db.Database.ExecuteSqlRaw("DELETE FROM sqlite_sequence WHERE name = 'Save'; INSERT INTO sqlite_sequence (name, seq) VALUES ('Save', 0)");
                db.SaveChanges();
            }
        }

        public string? SaveData(string DataToSaveToDB, string DataToReturn)
        {
            using var context = new ApplicationContext();
            var NewSave = new SAVE //This war will save data in DB with SAVE class
            {
                Data = DataToSaveToDB
            };
            context.Save.Add(NewSave);
            context.SaveChanges();
            using (ApplicationContext db = new ApplicationContext())
            {
                var saves = db.Save.ToList();
                DataToReturn = null;
            foreach (SAVE S in saves)
            {
                DataToReturn += $"{S.Id} - {S.Data}\n";
                //You can also use array if you wish
            }
        }
            return DataToReturn;
        }
    }
}
