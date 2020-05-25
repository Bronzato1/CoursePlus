﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CoursePlus.Shared.Models
{
    public class Quiz : IAuditable
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public int? CategoryId { get; set; }
        public Category Category { get; set; }

        public string ImagePath { get; set; }
        public int? ImageId { get; set; }
        public virtual Image Image { get; set; }

        public string ThumbnailPath { get; set; }
        public int? ThumbnailId { get; set; }
        public virtual Thumbnail Thumbnail { get; set; }

        public DateTime CreatedTime { get; set; }
        public DateTime? UpdatedTime { get; set; }
        public string CreatedUser { get; set; }
        public string UpdatedUser { get; set; }
    }
}