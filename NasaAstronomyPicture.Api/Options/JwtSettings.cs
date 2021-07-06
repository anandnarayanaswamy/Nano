using System;

namespace NasaAstronomyPicture.Api.Options
{
    public class JwtSettings
    {
        public string Secret { get; set; }
        
        public TimeSpan TokenLifetime { get; set; }
    }
}