using System;
using System.ComponentModel.DataAnnotations;

namespace NasaAstronomyPicture.Api.Models
{
    public class NasaAstronomyPictureModel
    {

        [Key]
        public int Id { get; set; }

        public Guid ImageGUID { get; set; }

        [Required]
        public string Date { get; set; }

        [Required]
        public string HdUrl { get; set; }

        [Required]
        public string Media_Type { get; set; }

        [Required] 
        public string Url { get; set; }

        [Required] 
        public string Title { get; set; }

        public string Copyright { get; set; }
    }
}
