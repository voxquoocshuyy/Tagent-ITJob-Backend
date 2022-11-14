using ITJob.Services.Services.FileServices;
using Microsoft.AspNetCore.Mvc;

namespace ITJob.API.Controllers;
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/files")]
public class FileController : ControllerBase
{
    private static string ApiKey = "AIzaSyBaYVyy9VAdCZpa9x2u9acdUIkaVQll2hY";
    private static string Bucket = "captone-dfc3c.appspot.com";
    private static string AuthEmail = "voquochuy1502@gmail.com";
    private static string AuthPassword = "123456";
    private readonly IFileService _fileService;
    
    public FileController(IFileService fileService)
    {
        _fileService = fileService;
    }
    // /// <summary>
    // /// [Guest] Endpoint for upload image with condition
    // /// </summary>
    // /// <param name="file"></param>
    // /// <returns>Link of image in firebase storage</returns>
    // /// <response code="200">Returns the Link of image</response>
    // /// <response code="204">Returns if link of image is empty</response>
    // /// <response code="403">Return if token is access denied</response>
    // [HttpPost]
    // public async Task<ActionResult> Post(IFormFile file)
    // {
    //     var auth = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));
    //     var a = await auth.SignInWithEmailAndPasswordAsync(AuthEmail, AuthPassword);
    //
    //     string fileName = DateTime.Now.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds + ".jpg";
    //     var stream = file.OpenReadStream();
    //
    //     var task = new FirebaseStorage(Bucket, new FirebaseStorageOptions
    //         {
    //             AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
    //         })
    //         .Child("images")
    //         .Child(fileName)
    //         .PutAsync(stream);
    //
    //     try
    //     {
    //         string link = await task;
    //         return Ok(new { urlImage = link });
    //     }
    //     catch (Exception ex)
    //     {
    //         return BadRequest(ex);
    //     }
    // }
    /// <summary>
    /// [Guest] Endpoint for upload image with condition
    /// </summary>
    /// <param name="files"></param>
    /// <returns>Link of image in firebase storage</returns>
    /// <response code="200">Returns the Link of image</response>
    /// <response code="204">Returns if link of image is empty</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpPost]
    public async Task<ActionResult> PostList(List<IFormFile> files)
    {
        try
        {
            await _fileService.UploadFiles(files);
            return Ok("Upload file success!");
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }
    /// <summary>
    /// [Guest] Endpoint for user edit avatar.
    /// </summary>
    /// <param name="id"></param>
    /// <returns>A urlImage within status 200 or error status.</returns>
    /// <response code="200">Returns urlImage after update</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpPut("applicant/id")]
    public async Task<IActionResult> DeleteAvatarAsync(Guid id)
    {
        try
        {
            await _fileService.DeleteAvatar(id);
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
        return NoContent();
    }
    /// <summary>
    /// [Guest] Endpoint for user edit logo.
    /// </summary>
    /// <param name="id"></param>
    /// <returns>A logo within status 200 or error status.</returns>
    /// <response code="200">Returns logo after update</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpPut("company/id")]
    public async Task<IActionResult> DeleteLogoAsync(Guid id)
    {
        try
        {
            await _fileService.DeleteLogo(id);
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
        return NoContent();
    }
    /// <summary>
    /// [Guest] Endpoint for user edit url image.
    /// </summary>
    /// <param name="urlImage"></param>
    /// <returns>A url image within status 200 or error status.</returns>
    /// <response code="200">Returns url image after update</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpPut("urlImage")]
    public async Task<IActionResult> DeleteLogoAsync(string[] urlImage)
    {
        try
        {
            await _fileService.DeleteFile(urlImage);
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
        return NoContent();
    }
}