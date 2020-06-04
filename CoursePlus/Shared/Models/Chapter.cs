using System;
using System.Collections.Generic;
using System.Text;

namespace CoursePlus.Shared.Models
{
    public class Chapter : IAuditable
    {
        public int Id { get; set; }
        public string Title { get; set; }
        
        public List<Episode> Episodes { get; set; }

        public int QuizTopicId { get; set; }
        public QuizTopic QuizTopic { get; set; }

        public DateTime CreatedTime { get; set; }
        public DateTime? UpdatedTime { get; set; }
        public string CreatedUser { get; set; }
        public string UpdatedUser { get; set; }
    }
}
