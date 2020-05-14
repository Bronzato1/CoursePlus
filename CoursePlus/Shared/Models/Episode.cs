using System;
using System.Collections.Generic;
using System.Text;

namespace CoursePlus.Shared.Models
{
    public class Episode : IAuditable
    {
        public int Id { get; set; }
        public string VideoUrl { get; set; }
        public string Title { get; set; }

        public int ChapterId { get; set; }
        public Chapter Chapter { get; set; }

        public DateTime CreatedTime { get; set; }
        public DateTime? UpdatedTime { get; set; }
        public string CreatedUser { get; set; }
        public string UpdatedUser { get; set; }
    }
}
