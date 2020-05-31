using Blazor.ModalDialog;
using CoursePlus.Client.Services;
using CoursePlus.Shared.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace CoursePlus.Client.Pages.Admin
{
    public class QuizGeneratorBase : ComponentBase
    {
        [Inject] public IQuizService QuizService { get; set; }
        [Inject] public IModalDialogService ModalDialog { get; set; }

        public List<QuizTopic> SomeQuizzes { get; set; } = new List<QuizTopic>();

        //protected override async Task OnInitializedAsync()
        //{
            
        //}
        protected int CountElements(QuizTopic OneQuiz, EnumLanguages language)
        {
            return OneQuiz.Items.Count(x => x.Language == language);
        }
        public async Task CreateQuizzes()
        {
            var result = await QuizService.CreateQuizzesFromJsonOfOpenQuizzDB();
            await ModalDialog.ShowMessageBoxAsync("Quizzes creation", $"Operation finished. Total quizzes created: {result}", MessageBoxButtons.OK);
        }
    }
}
