using ITJob.Services.ViewModels.AlbumImage;
using Microsoft.AspNetCore.Http;

namespace ITJob.Services.Services.FileServices;

public interface IFileService
{
    public Task<string> UploadFile(IFormFile file);
    public Task<List<string>> UploadFiles(List<IFormFile> files);
    public Task DeleteFile(string[] urlImages);
    public Task DeleteAvatar(Guid id);
    public Task DeleteLogo(Guid id);
}