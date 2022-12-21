using AutoMapper;
using ITJob.Entity.Entities;
using ITJob.Entity.Repositories.AlbumImageRepositories;
using ITJob.Services.Enum;
using ITJob.Services.Services.FileServices;
using ITJob.Services.Utility;
using ITJob.Services.Utility.ErrorHandling.Object;
using ITJob.Services.Utility.Paging;
using ITJob.Services.ViewModels.AlbumImage;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ITJob.Services.Services.AlbumImageServices;

public class AlbumImageService : IAlbumImageService
{
    private readonly IAlbumImageRepository _albumImageRepository;
    private readonly IMapper _mapper;
    private readonly IFileService _fileService;

    public AlbumImageService(IAlbumImageRepository albumImageRepository, IMapper mapper, IFileService fileService)
    {
        _albumImageRepository = albumImageRepository;
        _mapper = mapper;
        _fileService = fileService;
    }
    public IList<GetAlbumImageDetail> GetAlbumImagePage(PagingParam<AlbumImageEnum.AlbumImageSort> paginationModel, SearchAlbumImageModel searchAlbumImageModel)
    {
        IQueryable<AlbumImage> queryAlbumImage = _albumImageRepository.Table.Include(c => c.ProfileApplicant).Include(c => c.JobPost);
        queryAlbumImage = queryAlbumImage.GetWithSearch(searchAlbumImageModel);
        // Apply sort
        queryAlbumImage = queryAlbumImage.GetWithSorting(paginationModel.SortKey.ToString(), paginationModel.SortOrder);
        // Apply Paging
        queryAlbumImage = queryAlbumImage.GetWithPaging(paginationModel.Page, paginationModel.PageSize).AsQueryable();
        var result = _mapper.ProjectTo<GetAlbumImageDetail>(queryAlbumImage);
        return result.ToList();
    }

    public async Task<GetAlbumImageDetail> GetAlbumImageById(Guid id)
    {
        AlbumImage albumImage = await _albumImageRepository.GetFirstOrDefaultAsync(e => e.Id == id);
        if (albumImage == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        var result = _mapper.Map<GetAlbumImageDetail>(albumImage);
        return result;
    }

    public async Task<List<GetAlbumImageDetail>> CreateAlbumImageAsync(CreateAlbumImageModel requestBody)
    {
        List<AlbumImage> result =  new List<AlbumImage>();
        if (requestBody.UploadFiles != null)
        {
            List<string> listUrl = await _fileService.UploadFiles(requestBody.UploadFiles);
            foreach (var url in listUrl)
            {
                AlbumImage albumImage = _mapper.Map<AlbumImage>(requestBody);
                if (albumImage == null)
                {
                    throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
                }
                albumImage.UrlImage = url;
                await _albumImageRepository.InsertAsync(albumImage);
                await _albumImageRepository.SaveChangesAsync();
                result.Add(albumImage);
            }
        }
        else
        {
            AlbumImage albumImage = _mapper.Map<AlbumImage>(requestBody);
            albumImage.UrlImage = "";
            await _albumImageRepository.InsertAsync(albumImage);
            await _albumImageRepository.SaveChangesAsync();
            result.Add(albumImage);
        }
        List<GetAlbumImageDetail> listResult =  _mapper.ProjectTo<GetAlbumImageDetail>(result.AsQueryable()).ToList();
        return listResult;
    }
    public async Task<CreateAlbumImageUrlModel> CreateAlbumImageUrlAsync(CreateAlbumImageUrlModel requestBody)
    {
        AlbumImage albumImage = _mapper.Map<AlbumImage>(requestBody);
        if (albumImage == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        albumImage.UrlImage = requestBody.UrlImage;
        await _albumImageRepository.InsertAsync(albumImage);
        await _albumImageRepository.SaveChangesAsync();
        CreateAlbumImageUrlModel result = _mapper.Map<CreateAlbumImageUrlModel>(albumImage);
        return result;
    }

    public async Task<GetAlbumImageDetail> UpdateAlbumImageAsync(Guid id, UpdateAlbumImageModel requestBody)
    {
        if (id != requestBody.Id)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        AlbumImage albumImage = await _albumImageRepository.GetFirstOrDefaultAsync(alu => alu.Id == requestBody.Id);
        if (albumImage == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        albumImage = _mapper.Map(requestBody, albumImage);
        if (requestBody.UploadFile == null)
        {
            albumImage.UrlImage = "";
        }
        else
        {
            albumImage.UrlImage = await _fileService.UploadFile(requestBody.UploadFile);
        }
        _albumImageRepository.Update(albumImage);
        await _albumImageRepository.SaveChangesAsync();
        GetAlbumImageDetail albumImageDetail = _mapper.Map<GetAlbumImageDetail>(albumImage);
        return albumImageDetail;
    }

    public async Task DeleteAlbumImageAsync(Guid id)
    {
        AlbumImage? albumImage = await _albumImageRepository.GetFirstOrDefaultAsync(alu => alu.Id == id);
        if (albumImage == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        _albumImageRepository.Delete(albumImage);
        await _albumImageRepository.SaveChangesAsync();
    }
    public async Task DeleteAlbumImageByProfileApplicantIdAsync(Guid profileApplicantId)
    {
        List<AlbumImage>? albumImage = _albumImageRepository.Get(alu => alu.ProfileApplicantId == profileApplicantId).ToList();
        if (albumImage == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        _albumImageRepository.Delete(albumImage);
        await _albumImageRepository.SaveChangesAsync();
    }
    public async Task<int> GetTotal()
    {
        return await _albumImageRepository.GetAll().CountAsync();
    }
}