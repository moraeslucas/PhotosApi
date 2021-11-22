using Microsoft.AspNetCore.Mvc;
using PhotosApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotosApi.Data
{
    public interface IPhotoRepository
    {
        Task<int> AddPhoto(Photo photo);
        Task<IEnumerable<Photo>> GetPhotosByFilterDesc(int? skip, int? rowsNumber);
        Task<Photo> GetPhotoById(int id);
        Task<int> UpdatePhoto(Photo photo);
        Task<int> DeletePhoto(Photo photo);
        bool PhotoExists(int id);

    }
}
