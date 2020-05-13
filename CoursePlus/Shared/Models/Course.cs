using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CoursePlus.Shared.Models
{
    public class Course : IAuditable
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string SubTitle { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        public int? CategoryId { get; set; }
        public Category Category { get; set; }
        [Required]
        public EnumLanguages? Language { get; set; }
        [Required]
        public EnumDifficulties? Difficulty { get; set; }

        public int? ImageId { get; set; }
        public virtual Image Image { get; set; }
        [Required]
        public int? InstructorId { get; set; }
        public Instructor Instructor { get; set; }

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
