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
    public class PhotoDataAccess : IPhotoDataAccess
    {
        private readonly MyDatabaseContext _context;
        private readonly ILogger<PhotoDataAccess> _logger;

        public PhotoDataAccess(MyDatabaseContext context, ILogger<PhotoDataAccess> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Photo>> GetPhotosByFilter(int? skip, int? rowsNumber)
        {
            try
            {
                var photoQuery = _context.Photos.AsQueryable();
                //"OrderBy" separated so photoQuery can remain IQueryable, avoiding more casts
                photoQuery = photoQuery.OrderByDescending(pho => pho.Timestamp);

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
