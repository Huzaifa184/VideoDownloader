using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VideoDownloader.Models;

namespace VideoDownloader.Pages
{
    public class IndexModel : PageModel
    {
        private readonly MyDbContext _context;

        public IndexModel(MyDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public List<Login> Logins { get; set; }

        
        public void OnGet()
        {
            Logins = _context.Logins.ToList();
        }

        public async Task<IActionResult> OnPostAsync(string keyword)
        {
            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = "AIzaSyCYpHeDUCIFjgu0N6q8plRK3BqMSUYlKtI"
            });

            var searchRequest = youtubeService.Search.List("snippet");
            searchRequest.Q = keyword;
            searchRequest.Type = "video";
            searchRequest.MaxResults = 5;

            var searchResponse = await searchRequest.ExecuteAsync();

            Logins = new List<Login>();

            foreach (var searchResult in searchResponse.Items)
            {
                
                var videoId = searchResult.Id.VideoId;

                
                var videoRequest = youtubeService.Videos.List("snippet");
                videoRequest.Id = videoId;

                
                var videoResponse = await videoRequest.ExecuteAsync();

                
                if (videoResponse.Items.Count > 0)
                {
                    
                    string email = Request.Cookies["Email"];
                    
                    var login = new Login
                    {
                        Email = email,
                        Yturl = $"https://www.youtube.com/watch?v={videoId}",
                        VideoId = videoId,
                        Title = videoResponse.Items[0].Snippet.Title,
                        Description = videoResponse.Items[0].Snippet.Description,
                        Created = DateTime.Now
                    };

                    
                    Logins.Add(login);


                    _context.Logins.Add(login);
                }
            }

            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
