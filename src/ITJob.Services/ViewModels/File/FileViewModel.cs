using Microsoft.AspNetCore.Http;

namespace ITJob.Services.ViewModels.File;

public class FileViewModel
{
    public IFormFile? UploadFile { get; set; } = null;
}