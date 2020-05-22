using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CoursePlus.Shared.Models
{
    public class WatchHistory : IAuditable
    {
        [Key, Column(Order = 0)]
        public int EpisodeId { get; set; }
        public virtual Episode Episode { get; set; }

        [Key, Column(Order = 1)]
        public int ProfileId { get; set; }
        public virtual Profile Profile { get; set; }

        public DateTime CreatedTime { get; set; }
        public DateTime? UpdatedTime { get; set; }
        public string CreatedUser { get; set; }
        public string UpdatedUser { get; set; }
    }
}
