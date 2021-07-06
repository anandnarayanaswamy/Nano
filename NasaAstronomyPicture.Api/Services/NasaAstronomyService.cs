using Microsoft.EntityFrameworkCore;
using NasaAstronomyPicture.Api.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NasaAstronomyPicture.Api.Helpers;

namespace NasaAstronomyPicture.Api.Services
{

    public interface INasaAstronomyService
    {

        Boolean ValidateDateString(string dateString);

        Task<List<NasaAstronomyPictureModel>> GetAllAsync();

        Task<NasaAstronomyPictureModel> GetAsync(string dateString);

        Task<NasaAstronomyPictureModel> GetAsyncbyGUID(string imageGuid);

        Task<bool> CreateAsync(NasaAstronomyPictureModel model);
    }



    public sealed class NasaAstronomyService : INasaAstronomyService, IDisposable
    {

        private readonly IHttpClientFactory _clientFactory;
        private NasaAstronomyContext _context;
        private bool disposed = false;

        public NasaAstronomyService(NasaAstronomyContext context, IHttpClientFactory clientFactory)
        {
            _context = context;
            _clientFactory = clientFactory;
        }


        #region Public Methods

        public async Task<List<NasaAstronomyPictureModel>> GetAllAsync()
        {
            try
            {
                return await _context.NasaAstronomyPictureModel.ToListAsync();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<NasaAstronomyPictureModel> GetAsync(string dateString)
        {
            try
            {

                //Validate the given datastring,  There are multiple ways to do. but for now
                //implementing via services.
                this.ValidateDateString(dateString);

                var model  = await _context.NasaAstronomyPictureModel.SingleOrDefaultAsync(a => a.Date == dateString);
                if (model != null) return model;


                //if not, get it from NASA and store it locally 
                var client = _clientFactory.CreateClient("meta");
                var httpResponse = await client.GetFromJsonAsync<NasaAstronomyPictureModel>($"apod?api_key=DEMO_KEY&date={ dateString }");
                if (httpResponse == null) return null;


                NasaAstronomyPictureModel nasaAstronomyPicture = httpResponse;
                if (!String.IsNullOrWhiteSpace(nasaAstronomyPicture.Media_Type)) {

                    Guid imageGuid = System.Guid.NewGuid();
                    string imageSeverUrl = Directory.GetCurrentDirectory();   //This may be the ImageServer URL
                    nasaAstronomyPicture.ImageGUID = imageGuid;

                    if (nasaAstronomyPicture?.Media_Type == "image")
                    {
                        if (!String.IsNullOrWhiteSpace(nasaAstronomyPicture.HdUrl))
                        {
                            var streamToReadFrom = await client.GetByteArrayAsync(nasaAstronomyPicture.HdUrl);
                            if (streamToReadFrom?.Length > 0)
                            {
                                string filename = Extensions.GetFileNameFromUrl(nasaAstronomyPicture.HdUrl);
                                string filePath = $"{imageSeverUrl}/Assets/HD/{imageGuid}/{filename}";
                                Extensions.SaveImageToAssets(streamToReadFrom, filePath);
                                nasaAstronomyPicture.HdUrl = $"/Assets/HD/{imageGuid}/{filename}";
                            }
                        }
                        if (!String.IsNullOrWhiteSpace(nasaAstronomyPicture.Url))
                        {
                            var streamToReadFrom = await client.GetByteArrayAsync(nasaAstronomyPicture.Url);
                            if (streamToReadFrom?.Length > 0)
                            {
                                string filename = Extensions.GetFileNameFromUrl(nasaAstronomyPicture.Url);
                                string filePath = $"{imageSeverUrl}/Assets/SD/{imageGuid}/{filename}";
                                Extensions.SaveImageToAssets(streamToReadFrom, filePath);
                                nasaAstronomyPicture.Url = $"/Assets/SD/{imageGuid}/{filename}";
                            }
                        }

                    }
                }



                var created = await this.CreateAsync(nasaAstronomyPicture);
                if (created)
                    return await this.GetAsyncbyGUID(nasaAstronomyPicture.ImageGUID.ToString());


                return null;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<NasaAstronomyPictureModel> GetAsyncbyGUID(string imageGuid)
        {
            try
            {
                Guid guid = Guid.Parse(imageGuid);


                return await _context.NasaAstronomyPictureModel.SingleOrDefaultAsync(x => x.ImageGUID.ToString() == guid.ToString());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> CreateAsync(NasaAstronomyPictureModel model)
        {
            try
            {
                await _context.NasaAstronomyPictureModel.AddAsync(model);
                var created = await _context.SaveChangesAsync();
                return created > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }



        public Boolean ValidateDateString(string dateString)
        {

            try
            {
                Regex r = new Regex(@"^\d{4}-\d{2}-\d{2}$");

                if (!r.IsMatch(dateString))
                    throw new FormatException($"{dateString} is not the correct format,  should be yyy-MM-dd");

                var value = DateTime.ParseExact(dateString, Extensions.formats, CultureInfo.InvariantCulture, DateTimeStyles.None);

                if (value == DateTime.MinValue)
                    throw new Exception("Invalid date");


                var StartDate = DateTime.UtcNow.AddMonths(-2);
                var EndDate = DateTime.UtcNow;

                var isInRange = (StartDate <= value) && (value <= EndDate);
                if (!isInRange)
                    throw new Exception($"DateString - {dateString}, must be within the range of current date minus two months");


                return isInRange;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion


        //#region Private Methods


        //private static readonly string[] formats = { 
        //    // Basic formats
        //    "yyyyMMddTHHmmsszzz",
        //    "yyyyMMddTHHmmsszz",
        //    "yyyyMMddTHHmmssZ",
        //    // Extended formats
        //    "yyyy-MM-ddTHH:mm:sszzz",
        //    "yyyy-MM-ddTHH:mm:sszz",
        //    "yyyy-MM-ddTHH:mm:ssZ",
        //    "yyyy-MM-dd",

        //    // All of the above with reduced accuracy
        //    "yyyyMMddTHHmmzzz",
        //    "yyyyMMddTHHmmzz",
        //    "yyyyMMddTHHmmZ",
        //    "yyyy-MM-ddTHH:mmzzz",
        //    "yyyy-MM-ddTHH:mmzz",
        //    "yyyy-MM-ddTHH:mmZ",
        //    // Accuracy reduced to hours
        //    "yyyyMMddTHHzzz",
        //    "yyyyMMddTHHzz",
        //    "yyyyMMddTHHZ",
        //    "yyyy-MM-ddTHHzzz",
        //    "yyyy-MM-ddTHHzz",
        //    "yyyy-MM-ddTHHZ"
        //};

        //private static string GetFileNameFromUrl(string url)
        //{
        //    if (!Uri.TryCreate(url, UriKind.Absolute, out Uri uri))
        //        uri = new Uri(url);

        //    return Path.GetFileName(uri.LocalPath);
        //}



        //private static string GetFileNameExtension(string fileName)
        //{
        //    return Path.GetExtension(fileName);
        //}



        //private void SaveImageToAssets(byte[] streamToReadFrom, string imagePath)
        //{
        //    try
        //    {
        //        using MemoryStream outStream = new(streamToReadFrom);

        //        var di = Directory.CreateDirectory(Path.GetDirectoryName(imagePath));
        //        System.IO.File.WriteAllBytes(imagePath, outStream.ToArray());

        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}
        //#endregion




        #region IDisposable

        private void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                    _context.Dispose();
            }
            this.disposed = true;
        }


        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
