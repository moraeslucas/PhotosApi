using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PhotosApi.Data;
using PhotosApi.Models;

namespace PhotosApi.Controllers
{
    //This application can be secured by uncommenting this "Authorize"
    //[Authorize]

    /*The token [controller] is replaced with the value
      of the controller name, in this case 'Photos' */
    [Route("api/[controller]")]
    [ApiController]
    public class PhotosController : ControllerBase/*ControllerBase doesn't have the view support, which is right
                                                    for this application since its interface\view is in React */

    {
        private readonly IPhotoDataAccess _iPhoto;

        public PhotosController(IPhotoDataAccess iPhoto)
        {
            //Uses DI to inject the class that access the data source
            _iPhoto = iPhoto;
        }

        // GET: api/Photos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Photo>>> GetPhotos(int? skip, int? rowsNumber)
        {
            var photos =  await _iPhoto.GetPhotosByFilter(skip, rowsNumber);

            return Ok(photos);
        }

        // GET: api/Photos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Photo>> GetPhoto(int id)
        {
            var photo = await _iPhoto.GetPhotoById(id);

            if (photo == null)
            {
                return NotFound();
            }

            /*This is the standard for implementing optimistic locking
              in a RESTful API: To use the Etags and If-Match headers.
              PS: I'm using the optimistic concurrency from EF instead*/
            #region Etags & If-Match
            /*These "const" are placed outside this method*/
            //private const string _etagHeader = "ETag";
            //private const string _IfMatchHeader = "If-Match";
            
            //var eTag = photo.RowVersion.ToString();
            //HttpContext.Response.Headers.Add(_etagHeader, eTag);

            //if (HttpContext.Request.Headers.ContainsKey(_IfMatchHeader) &&
            //    HttpContext.Request.Headers[_IfMatchHeader] == eTag)
            //{
            //    return new StatusCodeResult(StatusCodes.Status304NotModified);
            //}
            #endregion

            return Ok(photo);
        }

        // PUT: api/Photos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPhoto(int id, Photo photo)
        {
            if (id != photo.PhotoId)
            {
                return BadRequest();
            }

            try
            {
                await _iPhoto.UpdatePhoto(photo);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PhotoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return StatusCode(500);
                }
            }

            return NoContent();
        }

        // POST: api/Photos
        [HttpPost]
        public async Task<ActionResult<Photo>> PostPhoto(Photo photo)
        {
            await _iPhoto.AddPhoto(photo);

            return CreatedAtAction("GetPhoto", new { id = photo.PhotoId }, photo);
        }

        // DELETE: api/Photos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhoto(int id)
        {
            Photo photo = await _iPhoto.GetPhotoById(id);
            if (photo == null)
            {
                return NotFound();
            }

            await _iPhoto.DeletePhoto(photo);

            return NoContent();
        }

        private bool PhotoExists(int id)
        {
            return _iPhoto.PhotoExists(id);  
        }
    }
}
