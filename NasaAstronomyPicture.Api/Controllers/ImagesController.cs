using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;


namespace NasaAstronomyPicture.Api.Controllers
{
    [Route("api/[controller]")]
    public class ImagesController : Controller
    {
        // GET api/<controller>/imageName
        [HttpGet("{imageName}")]
        public async Task<IActionResult> Get(string imageName)
        {
            //read and return the image here
            return await Task.FromException<EmptyResult>(new NotImplementedException());
        }

    }
}
