using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CoursePlus.Shared.Models
{
    public class Enrollment
    {
        [Key, Column(Order = 0)]
        public int CourseId { get; set; }
        public virtual Course Course { get; set; }

        [Key, Column(Order = 1)]
        public int StudentId { get; set; }
        public virtual Student Student { get; set; }
    }
}
