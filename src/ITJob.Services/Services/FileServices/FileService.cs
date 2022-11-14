using Firebase.Auth;
using Firebase.Storage;
using ITJob.Entity.Entities;
using ITJob.Entity.Repositories.AlbumImageRepositories;
using ITJob.Entity.Repositories.ApplicantRepositories;
using ITJob.Entity.Repositories.CompanyRepositories;
using ITJob.Services.Utility.ErrorHandling.Object;
using ITJob.Services.ViewModels.AlbumImage;
using Microsoft.AspNetCore.Http;

namespace ITJob.Services.Services.FileServices;

public class FileService : IFileService
{
    private static string ApiKey = "AIzaSyBaYVyy9VAdCZpa9x2u9acdUIkaVQll2hY";
    private static string Bucket = "captone-dfc3c.appspot.com";
    private static string AuthEmail = "voquochuy1502@gmail.com";
    private static string AuthPassword = "123456";
    private readonly IAlbumImageRepository _albumImageRepository;
    private readonly IApplicantRepository _applicantRepository;
    private readonly ICompanyRepository _companyRepository;

    public FileService(IAlbumImageRepository albumImageRepository, IApplicantRepository applicantRepository, ICompanyRepository companyRepository)
    {
        _albumImageRepository = albumImageRepository;
        _applicantRepository = applicantRepository;
        _companyRepository = companyRepository;
    }

    public async Task<string> UploadFile(IFormFile file)
    {
        var auth = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));
        var a = await auth.SignInWithEmailAndPasswordAsync(AuthEmail, AuthPassword);
        
        string fileName = DateTime.Now.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds + ".jpg";
        var stream = file.OpenReadStream();
        
        var task = new FirebaseStorage(Bucket, new FirebaseStorageOptions
            {
                AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
            })
            .Child("images")
            .Child(fileName)
            .PutAsync(stream);
        
        String urlImage = await task;
        return urlImage;
    }

    public async Task<List<string>> UploadFiles(List<IFormFile> files)
    {
        var auth = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));
        var a = await auth.SignInWithEmailAndPasswordAsync(AuthEmail, AuthPassword);
        List<string> result = new List<string>();
        foreach (var fromFile in files)
        {
            if (fromFile.Length > 0)
            {
                string fileName = DateTime.Now.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds + ".jpg";
                var stream = fromFile.OpenReadStream();
        
                var task = new FirebaseStorage(Bucket, new FirebaseStorageOptions
                    {
                        AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                    })
                    .Child("images")
                    .Child(fileName)
                    .PutAsync(stream);
                string urlImage = await task;
                result.Add(urlImage);
            }
            
        }
        Console.WriteLine("result: " + result.Count);
        return result;
    }

    public async Task DeleteFile(string[] urlImages)
    {
        foreach (var urlImage in urlImages)
        {
            if (urlImage.Length > 0)
            {
                AlbumImage albumImage = await _albumImageRepository.GetFirstOrDefaultAsync(a => a.UrlImage == urlImage);
                if (albumImage == null)
                {
                    throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
                }

                _albumImageRepository.Delete(albumImage);
                await _albumImageRepository.SaveChangesAsync();
            }
        }
    }

    public async Task DeleteAvatar(Guid id)
    {
        Applicant applicant = await _applicantRepository.GetFirstOrDefaultAsync(a => a.Id == id);
        if (applicant == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }

        applicant.Avatar = null;
        _applicantRepository.Update(applicant);
        await _applicantRepository.SaveChangesAsync();
    }
    public async Task DeleteLogo(Guid id)
    {
        Company company = await _companyRepository.GetFirstOrDefaultAsync(a => a.Id == id);
        if (company == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }

        company.Logo = null;
        _companyRepository.Update(company);
        await _companyRepository.SaveChangesAsync();
    }
}