using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using BlazorApp.Models;
using BlazorApp.Repositories;

namespace BlazorApp.Hubs{
    public class ImageHub : Hub
    {
        private readonly IImageUploadRepository _repository;

        public ImageHub(IImageUploadRepository repository)
        {
            _repository = repository;
        }

        public async Task BroadcastImageUpdate(string imageId)
        {
            await Clients.All.SendAsync("ReceiveImageUpdate", imageId);
        }

        public async Task BroadcastImageDelete(int imageId)
        {
            await Clients.All.SendAsync("ReceiveImageDelete", imageId);
        }

        public async Task BroadcastOthersImageDelete(string imageName)
        {
            await Clients.All.SendAsync("ReceiveOthersImageDelete", imageName);
        }

        public async Task UploadImageToDb(ImageFile image, string userId)
        {
            await _repository.UploadImageToDb(image, userId);
            await BroadcastImageUpdate(image.Id.ToString()); 
        }

        public async Task UploadImageToOthersUsers(ImageFile image, string userId)
        {
            await _repository.UploadImageToOtherUsers(image, userId);
            await BroadcastImageUpdate(image.Id.ToString());
        }

        public async Task DeleteImageFromDb(int imageId, string userId)
        {
            await _repository.DeleteImageFromDb(imageId, userId);
            await BroadcastImageDelete(imageId);
        }

        public async Task DeleteImageFromOtherUsers(string imageName)
        {
            await _repository.DeleteImageFromOtherUsers(imageName);
            await BroadcastOthersImageDelete(imageName);
        }
    }
}
