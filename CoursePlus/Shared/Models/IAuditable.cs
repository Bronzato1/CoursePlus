using System;
using System.Collections.Generic;
using System.Text;

namespace CoursePlus.Shared.Models
{
    public interface IAuditable
    {
        public DateTime CreatedTime { get; set; }
        public DateTime? UpdatedTime { get; set; }
        public string CreatedUser { get; set; }
        public string UpdatedUser { get; set; }
    }
}
