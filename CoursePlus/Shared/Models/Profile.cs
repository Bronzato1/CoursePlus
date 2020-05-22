using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CoursePlus.Shared.Models
{
    public class Profile : IAuditable
    {
        [Required]
        public int Id { get; set; }
        public DateTime Joined { get; set; }
        public int Enrolled { get; set; }
        
        public string UserId { get; set; }
        public virtual CustomUser User { get; set; }

        public virtual List<Enrollment> Enrollments { get; set; }
        public virtual List<WatchHistory> WatchHistory { get; set; }

        public DateTime CreatedTime { get; set; }
        public DateTime? UpdatedTime { get; set; }
        public string CreatedUser { get; set; }
        public string UpdatedUser { get; set; }
    }
}
