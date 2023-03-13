using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using YoutubeExplode;
using System.IO;
using YoutubeExplode.Videos;
using YoutubeExplode.Videos.Streams;
using VideoDownloader.Models;

namespace VideoDownloader.Pages
{
    public class DownloadModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        private readonly MyDbContext _dbContext;
        public DownloadModel(IHttpClientFactory httpClientFactory, MyDbContext dbContext)
        {
            _httpClientFactory = httpClientFactory;
            _dbContext = dbContext;
        }

        public async Task<IActionResult> OnGetAsync(string videoUrl)
        {
            try
            {
               
                string email = Request.Cookies["Email"];

                
                var login = new Login
                {
                    Email = email,
                    Yturl = videoUrl,
                    IsInProgress = true,
                    Timestamp = DateTime.Now,
                    Created = DateTime.Now

                };

                
                _dbContext.Logins.Add(login);
                await _dbContext.SaveChangesAsync();

                var youtube = new YoutubeClient();
                var videoId = VideoId.TryParse(videoUrl)?.Value; 

               
                var video = await youtube.Videos.GetAsync(videoId);
                var title = video.Title;
                var description = video.Description;

                
                var streamManifest = await youtube.Videos.Streams.GetManifestAsync(videoId);
                var audioStreams = streamManifest.GetAudioStreams();
                var audioStreamInfo = audioStreams.GetWithHighestBitrate();
                var muxedStreams = streamManifest.GetMuxedStreams();
                var streamInfo = muxedStreams.OrderBy(s => s.VideoQuality).FirstOrDefault();
                var videoStream = await youtube.Videos.Streams.GetAsync(streamInfo);
                var audioStream = await youtube.Videos.Streams.GetAsync(audioStreamInfo);

                
                var audioMemoryStream = new MemoryStream();
                await audioStream.CopyToAsync(audioMemoryStream);
                audioMemoryStream.Seek(0, SeekOrigin.Begin);

                
                var videoMemoryStream = new MemoryStream();
                await videoStream.CopyToAsync(videoMemoryStream);
                videoMemoryStream.Seek(0, SeekOrigin.Begin);

                
                var outputStream = new MemoryStream();
                await audioMemoryStream.CopyToAsync(outputStream);
                await videoMemoryStream.CopyToAsync(outputStream);
                outputStream.Seek(0, SeekOrigin.Begin);

                
                Response.Headers.Add("Content-Disposition", $"attachment; filename=\"{title}.mp4\"");
                Response.Headers.Add("Content-Length", outputStream.Length.ToString());

                
                await outputStream.CopyToAsync(Response.Body);

               
                login.VideoId = videoId;
                login.Title = title;
                login.Description = description;
                login.IsComplete = true;
                login.Completed = DateTime.Now;

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                var login = new Login
                {
                    Yturl = videoUrl,
                    IsFailed = true,
                    Timestamp = DateTime.Now,
                    Created = DateTime.Now
                };

                _dbContext.Logins.Add(login);
                await _dbContext.SaveChangesAsync();

                using (var client = _httpClientFactory.CreateClient())
                {
                    var response = await client.GetAsync(videoUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        var contentDisposition = response.Content.Headers.ContentDisposition;
                        string fileName = null;
                        if (contentDisposition != null)
                        {
                            fileName = contentDisposition.FileName;
                        }

                        var contentType = response.Content.Headers.ContentType.MediaType;

                        if (fileName != null)
                        {
                            Response.Headers.Add("Content-Disposition", contentDisposition.ToString());
                        }
                        else
                        {
                            Response.Headers.Add("Content-Disposition", "attachment");
                        }
                        Response.Headers.Add("Content-Type", contentType);
                        Response.Headers.Add("Content-Length", long.MaxValue.ToString()); 

                        
                        await response.Content.CopyToAsync(Response.Body);
                    }
                    else
                    {
                        return NotFound();
                    }
                }
            }

            return Page();
        }
    }
}
