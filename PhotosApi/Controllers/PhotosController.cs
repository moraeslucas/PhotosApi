using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            //Uses DI to inject the database context into the controller.
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

            return photo;
        }

        // PUT: api/Photos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
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
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
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
