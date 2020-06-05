using CoursePlus.Client.Services;
using CoursePlus.Shared.Infrastructure;
using CoursePlus.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursePlus.Client.Pages
{
    public class QuizListBase : ComponentBase, IDisposable
    {
        [Inject] public IQuizService QuizService { get; set; }
        [Inject] public ICategoryService CategoryService { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }

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

            await FilterQuizzes();
            Categories = await CategoryService.GetCategories();
        }
        protected void OnFieldChanged(object sender, FieldChangedEventArgs e)
        {
            _ = FilterQuizzes();
        }
        protected void ViewQuiz(QuizTopic OneQuiz)
        {
            NavigationManager.NavigateTo("/quiz/" + OneQuiz.Id);
        } 
        protected async Task FilterQuizzes()
        {
            SomeQuizzes = await QuizService.GetQuizzes(pageSize: PageSize, sortOrder: GetCurrentSortOrder, filters: GetCurrentFilters);
            StateHasChanged();
        }
        protected async void PageIndexChanged(PaginatedList<QuizTopic> context, int newPageNumber)
        {
            if (newPageNumber < 1 || newPageNumber > context.TotalPages)
            {
                return;
            }

            var cptr = context.Items.Count;

            var filters = new Dictionary<string, string>();
           
            var data = await QuizService.GetQuizzes(pageNumber: newPageNumber, filters: filters);

            foreach (var elm in data.Items)
            {
                context.Items.Add(elm);
            }

            context.Items.RemoveRange(0, cptr);

            context.PageIndex = data.PageIndex;
            context.TotalPages = data.TotalPages;

            StateHasChanged();
        }
    }
}
