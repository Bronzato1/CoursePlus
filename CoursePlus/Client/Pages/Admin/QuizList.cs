using Blazor.ModalDialog;
using CoursePlus.Client.Services;
using CoursePlus.Shared.Infrastructure;
using CoursePlus.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace CoursePlus.Client.Pages.Admin
{
    public class QuizGeneratorBase : ComponentBase, IDisposable
    {
        [Inject] public IQuizService QuizService { get; set; }
        [Inject] public ICategoryService CategoryService { get; set; }
        [Inject] public IModalDialogService ModalDialog { get; set; }

        public int PageSize { get; set; } = 8;
        public PaginatedList<QuizTopic> SomeQuizzes { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public FilterModel CurrentFilterModel = new FilterModel();
        public SortOrderModel CurrentSortOrderModel = new SortOrderModel() { SortOrder = EnumSortOrder.NewestFirst };
        public EditContext EditContextForSortOrderModel;
        public EditContext EditContextForFilterModel;

        public Dictionary<string, string> GetCurrentFilters 
        { 
            get 
            {
                var filters = new Dictionary<string, string>();

                if (CurrentFilterModel.DifficultyFilter.HasValue)
                    filters.Add("Difficulty", CurrentFilterModel.DifficultyFilter.Value.ToString());

                if (CurrentFilterModel.DurationFilter.HasValue)
                    filters.Add("Duration", CurrentFilterModel.DurationFilter.Value.ToString());

                if (CurrentFilterModel.CategoryFilter.HasValue)
                    filters.Add("CategoryId", CurrentFilterModel.CategoryFilter.Value.ToString());

                if (CurrentFilterModel.ClassmentFilter.HasValue)
                {
                    switch (CurrentFilterModel.ClassmentFilter.Value)
                    {
                        case EnumClassment.Featured:
                            filters.Add("Featured", "true");
                            break;
                        case EnumClassment.Popular:
                            filters.Add("Popular", "true");
                            break;
                    }
                }

                return filters;
            }
        }
        public Dictionary<string, string> GetCurrentSortOrder
        {
            get 
            {
                var sortOrder = new Dictionary<string, string>();

                if (CurrentSortOrderModel.SortOrder.HasValue)
                {
                    switch (CurrentSortOrderModel.SortOrder.Value)
                    {
                        case EnumSortOrder.NewestFirst:
                            sortOrder.Add("Id", "desc");
                            break;
                        case EnumSortOrder.OldestFirst:
                            sortOrder.Add("Id", "asc");
                            break;
                        case EnumSortOrder.MostPlayed:
                            sortOrder.Add("Popular", "asc");
                            sortOrder.Add("Id", "desc");
                            break;
                    }
                }
                return sortOrder;
            }
        }
        public class FilterModel
        {
            public EnumDifficulty? DifficultyFilter { get; set; }
            public EnumDuration? DurationFilter { get; set; }
            public int? CategoryFilter { get; set; }
            public EnumRating? RatingFilter { get; set; }
            public EnumPeriode? PeriodeFilter { get; set; }
            public EnumClassment? ClassmentFilter { get; set; }
        }
        public class SortOrderModel
        {
            public EnumSortOrder? SortOrder { get; set; }
        }
        public void Dispose()
        {
            EditContextForSortOrderModel.OnFieldChanged -= OnFieldChanged;
        }

        protected override async Task OnInitializedAsync()
        {
            EditContextForSortOrderModel = new EditContext(CurrentSortOrderModel);
            EditContextForSortOrderModel.OnFieldChanged += OnFieldChanged;

            EditContextForFilterModel = new EditContext(CurrentSortOrderModel);
            EditContextForFilterModel.OnFieldChanged += OnFieldChanged;

            await FilterPlaylists();
            Categories = await CategoryService.GetCategories();
        }
        protected void OnFieldChanged(object sender, FieldChangedEventArgs e)
        {
            _ = FilterPlaylists();
        }
        protected string PageSizeIndicator(int pageSize)
        {
            if (PageSize.Equals(pageSize))
            {
                return "list-style-type-disk";
            }
            return string.Empty;
        }
        protected async Task PageIndexChanged(PaginatedList<QuizTopic> context, int newPageNumber)
        {
            if (newPageNumber < 1 || newPageNumber > context.TotalPages)
            {
                return;
            }

            var cptr = context.Items.Count;
            var data = await QuizService.GetQuizzes(pageNumber: newPageNumber, pageSize: PageSize, sortOrder: GetCurrentSortOrder, filters: GetCurrentFilters);

            foreach (var elm in data.Items)
            {
                context.Items.Add(elm);
            }

            context.Items.RemoveRange(0, cptr);
            context.PageIndex = data.PageIndex;
            context.TotalPages = data.TotalPages;

            StateHasChanged();
        }
        protected async Task FilterPlaylists()
        {
            SomeQuizzes = await QuizService.GetQuizzes(pageSize: PageSize, sortOrder: GetCurrentSortOrder, filters: GetCurrentFilters);
            StateHasChanged();
        }
        protected async Task SetPageSize(int size)
        {
            PageSize = size;
            await FilterPlaylists();
        }
        protected async Task CreateQuizzes()
        {
            var result = await QuizService.CreateQuizzesFromJsonOfOpenQuizzDB();
            await ModalDialog.ShowMessageBoxAsync("Quizzes creation", $"Operation finished. Total quizzes created: {result}", MessageBoxButtons.OK);
        }
        protected async Task AddQuiz()
        {

        }
    }
}
