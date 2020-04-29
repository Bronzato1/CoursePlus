using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CoursePlus.Shared.Models
{
    public class File
    {
        [Required]
        public int Id { get; set; }
        public byte[] Data { get; set; }
    }
}
