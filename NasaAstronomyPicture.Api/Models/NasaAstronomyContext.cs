using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NasaAstronomyPicture.Api.Models
{
    public class NasaAstronomyContext : DbContext
    {
        public NasaAstronomyContext(DbContextOptions<NasaAstronomyContext> options)
                   : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

        //entities
        public DbSet<NasaAstronomyPictureModel> NasaAstronomyPictureModel { get; set; }
       

    }
}
