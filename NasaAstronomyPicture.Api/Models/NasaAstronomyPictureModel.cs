using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NasaAstronomyPicture.Api.Models
{
    public class NasaAstronomyPictureModel
    {

        [Key]
        public int Id { get; set; }

        public string Copyright { get; set; }

        public string DateString { get; set; }

        public string HdUrl { get; set; }

        public string MediaType { get; set; }

        public string SdUrl { get; set; }

        public string Title { get; set; }

    }
}
