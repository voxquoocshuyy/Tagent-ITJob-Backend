using AutoMapper;
using ITJob.Entity.Entities;
using ITJob.Entity.Repositories.CertificateRepositories;
using ITJob.Services.Enum;
using ITJob.Services.Utility;
using ITJob.Services.Utility.ErrorHandling.Object;
using ITJob.Services.Utility.Paging;
using ITJob.Services.ViewModels.Certificate;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ITJob.Services.Services.CertificateServices;

public class CertificateService : ICertificateService
{
    private readonly ICertificateRepository _certificateRepository;
    private readonly IMapper _mapper;

    public CertificateService(ICertificateRepository certificateRepository, IMapper mapper)
    {
        _certificateRepository = certificateRepository;
        _mapper = mapper;
    }
    public IList<GetCertificateDetail> GetCertificatePage(PagingParam<CertificateEnum.CertificateSort> paginationModel, SearchCertificateModel searchCertificateModel)
    {
        IQueryable<Certificate> queryCertificate = _certificateRepository.Table.Include(c => c.SkillGroup);
        queryCertificate = queryCertificate.GetWithSearch(searchCertificateModel);
        // Apply sort
        queryCertificate = queryCertificate.GetWithSorting(paginationModel.SortKey.ToString(), paginationModel.SortOrder);
        // Apply Paging
        queryCertificate = queryCertificate.GetWithPaging(paginationModel.Page, paginationModel.PageSize).AsQueryable();
        var result = _mapper.ProjectTo<GetCertificateDetail>(queryCertificate);
        return result.ToList();
    }

    public async Task<GetCertificateDetail> GetCertificateById(Guid id)
    {
        Certificate certificate = await _certificateRepository.GetFirstOrDefaultAsync(e => e.Id == id);
        if (certificate == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        var result = _mapper.Map<GetCertificateDetail>(certificate);
        return result;
    }

    public async Task<GetCertificateDetail> CreateCertificateAsync(CreateCertificateModel requestBody)
    {
        Certificate certificate = _mapper.Map<Certificate>(requestBody);
        if (certificate == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        await _certificateRepository.InsertAsync(certificate);
        await _certificateRepository.SaveChangesAsync();
        GetCertificateDetail certificateDetail = _mapper.Map<GetCertificateDetail>(certificate);
        return certificateDetail;
    }

    public async Task<GetCertificateDetail> UpdateCertificateAsync(Guid id, UpdateCertificateModel requestBody)
    {
        if (id != requestBody.Id)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        Certificate certificate = await _certificateRepository.GetFirstOrDefaultAsync(alu => alu.Id == requestBody.Id);
        if (certificate == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        certificate = _mapper.Map(requestBody, certificate);
        _certificateRepository.Update(certificate);
        await _certificateRepository.SaveChangesAsync();
        GetCertificateDetail certificateDetail = _mapper.Map<GetCertificateDetail>(certificate);
        return certificateDetail;
    }

    public async Task DeleteCertificateAsync(Guid id)
    {
        Certificate? certificate = await _certificateRepository.GetFirstOrDefaultAsync(alu => alu.Id == id);
        if (certificate == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        _certificateRepository.Delete(certificate);
        await _certificateRepository.SaveChangesAsync();
    }

    public async Task<int> GetTotal()
    {
        return await _certificateRepository.GetAll().CountAsync();
    }
}