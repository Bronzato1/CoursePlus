using CoursePlus.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoursePlus.Shared.PageModels
{
    public class BookEdit
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int PageCount { get; set; }
        public EnumLanguages Language { get; set; }
        public DateTime PublishingDate { get; set; }
    }
}
