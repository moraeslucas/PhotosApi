using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PhotosApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotosApi.Data
{
    public class PhotoRepository : IPhotoRepository
    {
        private readonly MyDatabaseContext _context;
        private readonly ILogger<PhotoRepository> _logger;

        public PhotoRepository(MyDatabaseContext context, ILogger<PhotoRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        /* Depending on the app's size, the pattern 'Query Specification' could be used, since it's designed
           as the place where the definitions of a query (optional sorting and paging logic) can be put   */
        public async Task<IEnumerable<Photo>> GetPhotosByFilterDesc(int? skip, int? rowsNumber)
        {
            try
            {
                var photoQuery = _context.Photos.AsQueryable();
                //"OrderBy" separated so photoQuery can remain IQueryable, avoiding more casts
                photoQuery = photoQuery.OrderByDescending(pho => pho.LastModified);

                if (skip != null)
                    photoQuery = photoQuery.Skip((int)skip);

                if (rowsNumber != null)
                    photoQuery = photoQuery.Take((int)rowsNumber);

                return await photoQuery.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task<Photo> GetPhotoById(int id)
        {
            try
            {
                return await _context.Photos.FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task<int> AddPhoto(Photo photo)
        {
            try
            {
                /*The LastModified column is changed by
                  the Database whenever there is an Add.*/
                _context.Photos.Add(photo);
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task<int> UpdatePhoto(Photo photo)
        {
            try
            {
                /*Another possibility is to create a Trigger to
                  change the last modified time automatically*/
                photo.LastModified = DateTime.Now;

                _context.Entry(photo).State = EntityState.Modified;
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task<int> DeletePhoto(Photo photo)
        {
            try
            {
                _context.Photos.Remove(photo);
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public bool PhotoExists(int id)
        {
            return _context.Photos.Any(e => e.PhotoId == id);
        }
    }
}
