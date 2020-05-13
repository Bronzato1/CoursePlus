using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursePlus.Client.Pages
{
    public class CourseDetailBase : ComponentBase
    {
        [Parameter]
        public int Id { get; set; }
    }
}
