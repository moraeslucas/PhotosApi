using NUnit.Framework;
using PhotosApi.Controllers;
using PhotosApi.Models;
using System;
using System.Threading.Tasks;
using FakeItEasy;
using System.Linq;
using PhotosApi.Data;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PhotosApiTests.Controllers
{
    [TestFixture]
    public class PhotosControllerTests
    {
        const int _auxId = 5;
        IPhotoDataAccess _fakeDataAccess;

        [SetUp]
        public void SetUp()
        {
            _fakeDataAccess = A.Fake<IPhotoDataAccess>();
        }

        private PhotosController CreatePhotosController()
        {
            return new PhotosController(_fakeDataAccess);
        }

        [Test]
        public async Task PostPhoto_AddPhoto_ReturnsCreated()
        {
            // Arrange
            Photo fakePhoto = A.Fake<Photo>();

            // Act
            PhotosController photosController = this.CreatePhotosController();
            var result = await photosController.PostPhoto(fakePhoto);

            // Assert
            Assert.IsInstanceOf<CreatedAtActionResult>(result.Result);
        }

        [Test]
        public async Task PostPhoto_AddPhoto_ReturnsSameProperties()
        {
            #region Arrange
            string imageLink = "image.jpg",
                   description = "test";
            DateTime timestamp = DateTime.Now;

            Photo fakePhoto = A.Fake<Photo>();
            fakePhoto.ImageLink = imageLink;
            fakePhoto.Description = description;
            fakePhoto.Timestamp = timestamp;
            #endregion

            // Act
            PhotosController photosController = this.CreatePhotosController();
            var result = await photosController.PostPhoto(fakePhoto);

            // Assert
            Photo photoResult = ((CreatedAtActionResult)result.Result).Value as Photo;
            //Since there are multiple asserts, there is a separated message for each one
            Assert.AreEqual(imageLink, photoResult.ImageLink, "ImageLink not equal");
            Assert.AreEqual(description, photoResult.Description, "Description not equal");
            Assert.AreEqual(timestamp, photoResult.Timestamp, "Timestamp not equal");
        }

        [Test]
        public async Task GetPhotos_SingleRowsNumber_ReturnsSameQty()
        {
            // Arrange
            var fakePhotos = A.CollectionOfDummy<Photo>(_auxId).AsEnumerable();
            // Set up a method call to return a specific result
            A.CallTo(() => _fakeDataAccess.GetPhotosByFilter(0, _auxId)).Returns(Task.FromResult(fakePhotos));

            // Act
            PhotosController photosController = this.CreatePhotosController();
            var actionResult = await photosController.GetPhotos(0, _auxId);

            // Assert
            var result = ((OkObjectResult)actionResult.Result).Value as IEnumerable<Photo>;
            Assert.AreEqual(_auxId, result.Count());
        }

        [Test]
        public async Task GetPhoto_BySingleId_ReturnsSameId()
        {
            // Arrange
            Photo fakePhoto = A.Fake<Photo>();
            fakePhoto.PhotoId = _auxId;
            A.CallTo(() => _fakeDataAccess.GetPhotoById(_auxId)).Returns(Task.FromResult(fakePhoto));

            // Act
            PhotosController photosController = this.CreatePhotosController();
            var result = await photosController.GetPhoto(_auxId);

            // Assert
            Assert.AreEqual(fakePhoto.PhotoId, result.Value.PhotoId);
        }

        [Test]
        public async Task GetPhoto_BySingleId_ReturnsNotFound()
        {
            // Arrange
            Photo fakePhoto = null;
            A.CallTo(() => _fakeDataAccess.GetPhotoById(_auxId)).Returns(Task.FromResult(fakePhoto));

            // Act
            PhotosController photosController = this.CreatePhotosController();
            var result = await photosController.GetPhoto(_auxId);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result.Result);
        }

        [Test]
        public async Task PutPhoto_BySingleId_ReturnsNoContent() //"No Content" indicates the request has succeeded 
        {
            // Arrange
            Photo fakePhoto = A.Fake<Photo>();
            fakePhoto.PhotoId = _auxId;

            // Act
            PhotosController photosController = this.CreatePhotosController();
            var result = await photosController.PutPhoto(_auxId, fakePhoto);

            // Assert
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task PutPhoto_BySingleId_ReturnsBadRequest()
        {
            // Arrange
            Photo fakePhoto = A.Fake<Photo>();
            fakePhoto.PhotoId = _auxId -1;

            // Act
            PhotosController photosController = this.CreatePhotosController();
            var result = await photosController.PutPhoto(_auxId, fakePhoto);

            // Assert
            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public async Task PutPhoto_BySingleId_ThrowsNotFound()
        {
            // Arrange
            Photo fakePhoto = A.Fake<Photo>();
            fakePhoto.PhotoId = _auxId;

            A.CallTo(() => _fakeDataAccess.UpdatePhoto(fakePhoto))
                                          .ThrowsAsync(new DbUpdateConcurrencyException(string.Empty));

            // Act
            PhotosController photosController = this.CreatePhotosController();
            var result = await photosController.PutPhoto(_auxId, fakePhoto);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result); /*The Controller should return NotFound if this
                                                           exception is thrown with a NON-EXISTENT Photo*/
        }

        [Test]
        public async Task PutPhoto_BySingleId_ThrowsHttp500()
        {
            // Arrange
            Photo fakePhoto = A.Fake<Photo>();
            fakePhoto.PhotoId = _auxId;

            A.CallTo(() => _fakeDataAccess.UpdatePhoto(fakePhoto))
                                          .ThrowsAsync(new DbUpdateConcurrencyException(string.Empty));
            A.CallTo(() => _fakeDataAccess.PhotoExists(_auxId)).Returns(true);

            // Act
            PhotosController photosController = this.CreatePhotosController();
            var result = await photosController.PutPhoto(_auxId, fakePhoto);

            // Assert
            int statusCode = ((StatusCodeResult)result).StatusCode;
            Assert.AreEqual(statusCode, 500); /*The Controller should return 500 (internal server error)
                                                if this exception is thrown with an EXISTENT Photo    */
        }

        [Test]
        public async Task DeletePhoto_BySingleId_ReturnsNoContent() //This "No Content" also indicates the request has succeeded 
        {
            // Arrange
            Photo fakePhoto = A.Fake<Photo>();
            A.CallTo(() => _fakeDataAccess.GetPhotoById(_auxId)).Returns(Task.FromResult(fakePhoto));

            // Act
            PhotosController photosController = this.CreatePhotosController();
            var result = await photosController.DeletePhoto(_auxId);

            // Assert
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task DeletePhoto_BySingleId_ReturnsNotFound()
        {
            // Arrange
            Photo fakePhoto = null;
            A.CallTo(() => _fakeDataAccess.GetPhotoById(_auxId)).Returns(Task.FromResult(fakePhoto));

            // Act
            PhotosController photosController = this.CreatePhotosController();
            var result = await photosController.DeletePhoto(_auxId);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }
    }
}
