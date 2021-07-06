using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NasaAstronomyPicture.Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Globalization;
using System.Linq;
using NasaAstronomyPicture.Api.Services;

namespace NasaAstronomyPicture.Api.Controllers
{
    [Route("api/nasa-astronomy-picture")]
    [ApiController]
    public class NasaAstronomyPictureController : ControllerBase
    {

        private INasaAstronomyService _Service;

        public NasaAstronomyPictureController(INasaAstronomyService service)
        {
            _Service = service;
        }


       // GET api/nasa-astronomy-picture
       [HttpGet]
        public async Task<ActionResult> GetAllAsync()
        {
            string errorString = string.Empty;
            try
            {
                var result = await _Service.GetAllAsync();
                if (result == null) return NotFound();

                return Ok(result);
            }
            catch (Exception ex)
            {
                errorString = $"Internal Server Error.Please Contact your Administrator : { ex.Message }";
                return StatusCode(StatusCodes.Status500InternalServerError, errorString);
            }

        }


        // GET api/nasa-astronomy-picture/{dateString}
        [HttpGet("{dateString}")]
        public async Task<ActionResult> GetAsync(string dateString)
        {
            string errorString = string.Empty;
            try
            {
                var result = await _Service.GetAsync(dateString);
                if (result == null) return NotFound();

                return Ok(result);
            }
            catch (Exception ex)
            {
                errorString = $"Internal Server Error.Please Contact your Administrator : { ex.Message }";
                return StatusCode(StatusCodes.Status500InternalServerError, errorString);
            }
        }

    }

}
