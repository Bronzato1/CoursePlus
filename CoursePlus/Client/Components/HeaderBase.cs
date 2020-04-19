using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursePlus.Client.Components
{
    public class HeaderBase : ComponentBase
    {
        [Parameter]
        public string Class { get; set; }

        [Parameter]
        public string UkSticky { get; set; }
    }
}
