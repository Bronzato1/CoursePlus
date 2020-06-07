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
        public Dictionary<int, int> UserChoices { get; set; } = new Dictionary<int, int>();

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
    }
}
