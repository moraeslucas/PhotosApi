using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace PhotosApi.Models
{
    [Table("Photo", Schema = "PhotoGallery")]
    public partial class Photo
    {
        [Key]
        public int PhotoId { get; set; }
        
        [Required]
        [StringLength(300)]
        public string ImageLink { get; set; }
        
        [StringLength(20)]
        public string Description { get; set; }

        [Required]
        [Column(TypeName = "datetime")]
        public DateTime Timestamp { get; set; }
    }
}
