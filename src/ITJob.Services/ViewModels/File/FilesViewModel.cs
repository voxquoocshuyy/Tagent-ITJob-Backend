using Microsoft.AspNetCore.Http;

namespace ITJob.Services.ViewModels.File;

public class FilesViewModel
{
    public List<IFormFile>? UploadFiles { get; set; }
}