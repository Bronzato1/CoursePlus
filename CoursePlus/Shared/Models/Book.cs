using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CoursePlus.Shared.Models
{
    public class Book : IAuditable
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public string Author { get; set; }
        public string PurchaseLink { get; set; }
        public int PageCount { get; set; }
        [Required]
        public EnumLanguages Language { get; set; }
        public DateTime PublishingDate { get; set; }

        public bool Featured { get; set; }
        public bool Popular { get; set; }

        [Required]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public int? ImageId { get; set; }
        public virtual Image Image { get; set; }

        public int? ThumbnailId { get; set; }
        public virtual Thumbnail Thumbnail { get; set; }

        public DateTime CreatedTime { get; set; }
        public DateTime? UpdatedTime { get; set; }
        public string CreatedUser { get; set; }
        public string UpdatedUser { get; set; }
    }
}
