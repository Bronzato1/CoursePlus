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
    public class QuizPlayBase : ComponentBase
    {
        [Parameter] public int Id { get; set; }
        [Inject] public IJSRuntime JSRuntime { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Inject] public IQuizService QuizService { get; set; }

        public QuizTopic OneQuiz { get; set; }

        public TimeSpan StopWatchValue = new TimeSpan();
        public bool IsStopWatchRunning = false;

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
        }
        protected override async Task OnInitializedAsync()
        {
            OneQuiz = await QuizService.GetQuiz(Id);
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            //await Task.Delay(3000);
            //await JSRuntime.InvokeVoidAsync("resetSticky", ".playlist-card-trailer");
        }
        public async Task StartStopWatch()
        {
            IsStopWatchRunning = true;

            while (IsStopWatchRunning)
            {
                await Task.Delay(1000);
                if (IsStopWatchRunning)
                {
                    StopWatchValue = StopWatchValue.Add(new TimeSpan(0, 0, 1));
                    StateHasChanged();
                }
            }
        }
    }
}
