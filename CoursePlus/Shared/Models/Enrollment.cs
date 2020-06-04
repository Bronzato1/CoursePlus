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
        public int QuizTopicId { get; set; }
        public virtual QuizTopic QuizTopic { get; set; }

        [Key, Column(Order = 1)]
        public int ProfileId { get; set; }
        public virtual Profile Profile { get; set; }
    }
}
