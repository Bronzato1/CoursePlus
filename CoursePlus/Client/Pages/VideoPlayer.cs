using CoursePlus.Client.Services;
using CoursePlus.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CoursePlus.Client.Pages
{
    public class VideoPlayerBase : ComponentBase
    {
        [CascadingParameter]
        private Task<AuthenticationState> authState { get; set; }
        [Parameter]
        public int Id { get; set; }
        [Inject]
        IJSRuntime JSRuntime { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        [Inject]
        public IPlaylistService PlaylistService { get; set; }
        [Inject]
        public IProfileService ProfileService { get; set; }

        public int ProfileId { get; set; }

        public Playlist OnePlaylist { get; set; }

        public Profile OneProfile { get; set; }

        protected override async Task OnInitializedAsync()
        {
            ClaimsPrincipal principal = (await authState).User;

            if (principal.Identity.IsAuthenticated)
            {
                Claim claim = principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
                String userId = claim.Value;
                OneProfile = await ProfileService.GetProfileByUserId(userId);
            }

            OnePlaylist = await PlaylistService.GetPlaylist(Id);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
                await JSRuntime.InvokeVoidAsync("Player.initialize");
        }

        protected async Task LoadYouTubeVideo(Episode OneEpisode)
        {
            await JSRuntime.InvokeVoidAsync("Player.loadYouTubeVideo", OneEpisode.VideoId);
        }

        protected async Task GoBack()
        {
            await JSRuntime.InvokeVoidAsync("History.goBack");
        }

        protected bool ProfileAlreadyWatchedThisEpisode(Episode OneEpisode)
        {
            if (OneProfile == null)
            {
                // Current user is not profile
                return false;
            }

            var watched = OneEpisode.WatchHistory.FirstOrDefault(x => x.ProfileId == OneProfile.Id) != null;
            return watched;
        }
    }
}
