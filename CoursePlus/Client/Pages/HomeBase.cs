using CoursePlus.Client.Services;
using CoursePlus.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursePlus.Client.Pages
{
    public class HomeBase : ComponentBase
    {
        [Inject] IQuizService QuizService { get; set; }

        public List<QuizTopic> PopularQuizzes { get; set; }

        protected override async Task OnInitializedAsync()
        {
            //PopularQuizzes = await QuizService.GetPopularQuizzes();
        }
    }
}
