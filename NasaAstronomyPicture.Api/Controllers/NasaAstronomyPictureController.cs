using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NasaAstronomyPicture.Api.Models;

using System.Runtime.Caching;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Text.Json;
using System.Net.Http.Json;
using NasaAstronomyPicture.Api.Domain;
using System.Net;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace NasaAstronomyPicture.Api.Controllers
{
    [Route("api/nasa-astronomy-picture")]
    [ApiController]
    public class NasaAstronomyPictureController : ControllerBase
    {

        private NasaAstronomyContext _context;
        private readonly IHttpClientFactory _clientFactory;

   

        public NasaAstronomyPictureController(NasaAstronomyContext context, IHttpClientFactory clientFactory)
        {
            _context = context;
            _clientFactory = clientFactory;
        }


       // GET api/nasa-astronomy-picture
       [HttpGet]
        public async Task<ActionResult> GetAllAsync()
        {
            var result = await _context.NasaAstronomyPictureModel.ToListAsync();
            if (result == null) return NotFound();

            return Ok(result);
        }



        // GET api/nasa-astronomy-picture/{dateString}
        [HttpGet("{dateString}")]
        public async Task<ActionResult> GetAsync(string dateString)
        {

            string errorString = string.Empty;
            try
            {
                var date = ParseISO8601String(dateString);

                if (string.IsNullOrEmpty(date))
                {
                    errorString = $"{ dateString } is not the correct format. Should be yyyy-MM-dd, dateString";
                    return BadRequest(errorString);
                }

                // var url = $"https://api.nasa.gov/planetary/apod?api_key=DEMO_KEY&date={ date }";
                //var client = _clientFactory.CreateClient();
                //var response = await client.GetFromJsonAsync<NasaAstronomy>(url);

                var client = _clientFactory.CreateClient("meta");
                var httpResponse = await client.GetFromJsonAsync<NasaAstronomy>($"apod?api_key=DEMO_KEY&date={ date }");

                if (httpResponse != null)
                {
                    string filename = GetFileNameFromUrl(httpResponse.hdurl);
                    var streamToReadFrom = await client.GetByteArrayAsync(httpResponse.hdurl);
                    using MemoryStream outStream = new (streamToReadFrom);
                    System.IO.File.WriteAllBytes(@"C:\CafeStream\Projects\VSCode\"+ filename, outStream.ToArray());

                }

                var model = new NasaAstronomyPictureModel
                {
                    DateString = httpResponse.date,
                    Title = httpResponse.title,
                    MediaType = httpResponse.mediatype,
                    SdUrl = httpResponse.url,
                    HdUrl = httpResponse.hdurl,
                    Copyright = httpResponse.copyright
                };

                _context.NasaAstronomyPictureModel.Add(model);
                await _context.SaveChangesAsync();



                return Ok(model);
            }
            catch (Exception ex)
            {
                errorString = $"Internal Server Error.Please Contact your Administrator : { ex.Message }";
                return StatusCode(StatusCodes.Status500InternalServerError, errorString);
            }

        }

    






        private static readonly string[] formats = { 
            // Basic formats
            "yyyyMMddTHHmmsszzz",
            "yyyyMMddTHHmmsszz",
            "yyyyMMddTHHmmssZ",
            // Extended formats
            "yyyy-MM-ddTHH:mm:sszzz",
            "yyyy-MM-ddTHH:mm:sszz",
            "yyyy-MM-ddTHH:mm:ssZ",
            "yyyy-MM-dd",

            // All of the above with reduced accuracy
            "yyyyMMddTHHmmzzz",
            "yyyyMMddTHHmmzz",
            "yyyyMMddTHHmmZ",
            "yyyy-MM-ddTHH:mmzzz",
            "yyyy-MM-ddTHH:mmzz",
            "yyyy-MM-ddTHH:mmZ",
            // Accuracy reduced to hours
            "yyyyMMddTHHzzz",
            "yyyyMMddTHHzz",
            "yyyyMMddTHHZ",
            "yyyy-MM-ddTHHzzz",
            "yyyy-MM-ddTHHzz",
            "yyyy-MM-ddTHHZ"
        };

        private static String ParseISO8601String(string dateString)
        {
            try
            {

                Regex r = new Regex(@"^\d{4}-\d{2}-\d{2}$");
                if (r.IsMatch(dateString))
                    return dateString;

                return null;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static string GetFileNameFromUrl(string url)
        {
            if (!Uri.TryCreate(url, UriKind.Absolute, out Uri uri))
                uri = new Uri(url);

            return Path.GetFileName(uri.LocalPath);
        }



    }

}
