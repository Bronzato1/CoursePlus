using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CoursePlus.Shared.Models
{
    public class Playlist : IAuditable
    {
        [Required]
        public int Id { get; set; }
        [Required(ErrorMessage = "{0} is required.")]
        public string Title { get; set; }
        [Required(ErrorMessage = "{0} is required.")]
        public string SubTitle { get; set; }
        [Required(ErrorMessage = "{0} is required.")]
        public string Description { get; set; }
        [Required]
        public int Price { get; set; }
        [Required(ErrorMessage = "Category is required.")]

        public int? CategoryId { get; set; }
        public Category Category { get; set; }

        [Required(ErrorMessage = "{0} is required.")]
        public EnumLanguages? Language { get; set; }
        [Required(ErrorMessage = "{0} is required.")]
        public EnumDifficulty? Difficulty { get; set; }

        public int? ImageId { get; set; }
        public virtual Image Image { get; set; }

        [Required(ErrorMessage = "Profile is required.")]
        public int? ProfileId { get; set; }
        //[ForeignKey("ProfileId")]
        public Profile Profile { get; set; }

        public bool Featured { get; set; }
        public bool Popular { get; set; }

        public virtual List<Enrollment> Enrollments { get; set; }

        public int? ThumbnailId { get; set; }
        public virtual Thumbnail Thumbnail { get; set; }

        public List<Chapter> Chapters { get; set; }

        public DateTime CreatedTime { get; set; }
        public DateTime? UpdatedTime { get; set; }
        public string CreatedUser { get; set; }
        public string UpdatedUser { get; set; }
    }
}
