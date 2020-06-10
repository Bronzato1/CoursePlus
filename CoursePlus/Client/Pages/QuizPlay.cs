using CoursePlus.Client.Services;
using CoursePlus.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Syncfusion.Blazor.Popups;
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

        public const int TIMER_STOPPED = 0;
        public const int TIMER_STARTED = 1;
        public const int TIMER_PAUSED = 2;
        public const int TIMER_RESUMED = 3;
        public const int TIMER_ELAPSED = 4;

        public QuizTopic OneQuiz { get; set; }
        public Dictionary<int, int> UserChoices { get; set; } = new Dictionary<int, int>();

        public int TimerState { get; set; } = TIMER_STOPPED;
        
        public int How_many_questions_to_ask = 10;
        public int How_many_time_to_respond = 60;
        public int What_level_of_difficulty = (int)EnumDifficulty.Beginner;

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
        }
        protected override async Task OnInitializedAsync()
        {
            OneQuiz = await QuizService.GetQuiz(Id);
        }
        protected void ProposalClicked(QuizProposal OneProposal)
        {
            // Check if user already selected a proposal
            var found = UserChoices.ContainsKey(OneProposal.QuizItemId);
            // Remove previously selected proposal (if any)
            if (found) UserChoices.Remove(OneProposal.QuizItemId);
            // Add current selected proposal of the user
            UserChoices.Add(OneProposal.QuizItemId, OneProposal.Id);
        }
        protected bool Chosen(QuizProposal OneProposal)
        {
            if (UserChoices.TryGetValue(OneProposal.QuizItemId, out int proposalId))
            {
                // User already responded to this question
                // The choosen proposal of the user is proposalId
                // The correct proposal is OneProposal.Id
                return OneProposal.Id == proposalId;
            }
            return false;
        }
        protected void TimerStateChangedCallback(int state)
        {
            TimerState = state;
        }
        protected string TranslateDifficulty(int value)
        {
            switch ((EnumDifficulty)value)
            {
                case EnumDifficulty.Beginner:
                    return "Beginner";
                case EnumDifficulty.Confirmed:
                    return "Confirmed";
                case EnumDifficulty.Expert:
                    return "Expert";
                default: return string.Empty;
            }
        }
    }
}
