using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using LiteGuard;
using OneInc.ADEditor.Common.Utils;
using OneInc.ADEditor.Core.Repositories;
using OneInc.ADEditor.SharePoint;
using OneInc.ADEditor.SharePoint.Services.Interfaces;

namespace OneInc.ADEditor.Dal.Repositories
{
    public class PhotoRepository : IPhotoRepository
    {
        private readonly ISharePointListService _listService;
        private readonly SharePointSettings _settings;

        public PhotoRepository(ISharePointListService listService, SharePointSettings settings)
        {
            Guard.AgainstNullArgument(nameof(listService), listService);
            Guard.AgainstNullArgument(nameof(settings), settings);

            _listService = listService;
            _settings = settings;
        }

        public byte[] DownloadPhoto(string photoUrl)
        {
            var task = DownloadPhotoAsync(photoUrl);
            Task.WaitAll(task);

            return task.Result;
        }

        public Task<byte[]> DownloadPhotoAsync(string photoUrl)
        {
            if (photoUrl.IsEmpty())
            {
                return Task.FromResult(new byte[0]);
            }

            return Task.Run(() =>
            {
                if (_listService.TryGetFileByUrl(_settings.PhotoLibraryName, photoUrl, out var photo))
                {    
                    return Task.FromResult(photo);
                }
                
                return DownloadPhotoSafeAsync(photoUrl);
            });
        }

        private static async Task<byte[]> DownloadPhotoSafeAsync(string photoUrl)
        {
            using (var client = new WebClient())
            {
                try
                {
                    return await client.DownloadDataTaskAsync(photoUrl);
                }
                catch
                {
                    return new byte[0];
                }
            }
        }

        public IDictionary<T, Task<byte[]>> DownloadPhotosAsync<T>(IDictionary<T, string> links)
        {
            return links.ToDictionary(link => link.Key, link => DownloadPhotoAsync(link.Value));
        }

        public string UploadPhoto(string email, byte[] data)
        {
            return _listService.UploadFileAndExecute(_settings.PhotoLibraryName, $"{email}.jpg", data);
        }
    }
}