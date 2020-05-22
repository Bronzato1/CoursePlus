using CoursePlus.Client.Services;
using CoursePlus.Shared.Infrastructure;
using CoursePlus.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursePlus.Client.Pages
{
    public class PlaylistDetailBase : ComponentBase
    {
        [Parameter]
        public int Id { get; set; }
        [Inject]
        IJSRuntime JSRuntime { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        [Inject]
        public IPlaylistService PlaylistService { get; set; }

        public Playlist OnePlaylist { get; set; }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
        }

        protected override async Task OnInitializedAsync()
        {
            OnePlaylist = await PlaylistService.GetPlaylist(Id);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await JSRuntime.InvokeVoidAsync("resetSticky", ".playlist-card-trailer");
        }
    }
}
