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
        public DateTime LastModified { get; set; }

        /*The attribute is called Timestamp because previous versions of SQL Server 
          used a SQL timestamp data type before the SQL rowversion replaced it */
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
