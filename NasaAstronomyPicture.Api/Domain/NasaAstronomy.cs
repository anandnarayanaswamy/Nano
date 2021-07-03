using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NasaAstronomyPicture.Api.Domain
{
    public class NasaAstronomy
    {
        public string copyright { get; set; }

        public string date { get; set; }

        public string hdurl { get; set; }

        public string  mediatype { get; set; }
        
        public string url { get; set; }

        public string title { get; set; }

        public string explanation { get; set; }

        public string service_version { get; set; }
    }
}
