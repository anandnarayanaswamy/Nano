using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using NasaAstronomyPicture.Api.Services;
using System.IO;
using NasaAstronomyPicture.Api.Helpers;

namespace NasaAstronomyPicture.Api.Controllers
{
    [Route("api/[controller]")]
    public class ImagesController : Controller
    {
        private INasaAstronomyService _Service;

        public ImagesController(INasaAstronomyService service)
        {
            _Service = service;
        }

        // GET api/<controller>/imageName
        [HttpGet("{imageGuid}")]
        public async Task<ActionResult> GetAsync(string imageGuid)
        {
            string errorString = string.Empty;
            string imageSeverUrl = Directory.GetCurrentDirectory();   //This may be the ImageServer URL

            try
            {
                var model = await _Service.GetAsyncbyGUID(imageGuid);

                if (model == null)
                    return NoContent();

                string imageURL = string.Empty;
                if (!String.IsNullOrWhiteSpace(model.HdUrl))
                    imageURL = $"{imageSeverUrl}/{model.HdUrl}";
                else
                    imageURL = $"{imageSeverUrl}/{model.Url}";

                string fileExtension = Path.GetExtension(imageURL).ToLower();

                var image = System.IO.File.OpenRead(imageURL);
                return File(image, Extensions.GetMimeType(fileExtension));

                
            }
            catch (Exception ex)
            {
                errorString = $"Internal Server Error.Please Contact your Administrator : { ex.Message }";
                return StatusCode(StatusCodes.Status500InternalServerError, errorString);
            }

        }

         




    }
}
