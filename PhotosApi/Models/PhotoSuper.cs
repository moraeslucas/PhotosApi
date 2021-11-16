using System.ComponentModel.DataAnnotations.Schema;

namespace PhotosApi.Models
{
    /*This ia an sample to prevent over-posting attacks,
     which is when a property you don’t want to be changed is set*/
    [Table("Photo", Schema = "PhotoGallery")]
    public class PhotoSuper : Photo
    {
        public bool IsAdmin { get; set; }
    }
}