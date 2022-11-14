using AutoMapper;
using ITJob.Entity.Entities;
using ITJob.Entity.Repositories.BlockRepositories;
using ITJob.Services.Enum;
using ITJob.Services.Utility;
using ITJob.Services.Utility.ErrorHandling.Object;
using ITJob.Services.Utility.Paging;
using ITJob.Services.ViewModels.Block;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ITJob.Services.Services.BlockServices;

public class BlockService : IBlockService
{
    private readonly IBlockRepository _blockRepository;
    private readonly IMapper _mapper;

    public BlockService(IBlockRepository blockRepository, IMapper mapper)
    {
        _blockRepository = blockRepository;
        _mapper = mapper;
    }
    public IList<GetBlockDetail> GetBlockPage(PagingParam<BlockEnum.BlockSort> paginationModel, SearchBlockModel searchBlockModel)
    {
        IQueryable<Block> queryBlock = _blockRepository.Table.Include(c => c.Applicant).Include(c => c.Company);
        queryBlock = queryBlock.GetWithSearch(searchBlockModel);
        // Apply sort
        queryBlock = queryBlock.GetWithSorting(paginationModel.SortKey.ToString(), paginationModel.SortOrder);
        // Apply Paging
        queryBlock = queryBlock.GetWithPaging(paginationModel.Page, paginationModel.PageSize).AsQueryable();
        var result = _mapper.ProjectTo<GetBlockDetail>(queryBlock);
        return result.ToList();
    }

    public async Task<GetBlockDetail> GetBlockById(Guid id)
    {
        Block block = await _blockRepository.GetFirstOrDefaultAsync(e => e.Id == id);
        if (block == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        var result = _mapper.Map<GetBlockDetail>(block);
        return result;
    }

    public async Task<GetBlockDetail> CreateBlockAsync(CreateBlockModel requestBody)
    {
        Block block = _mapper.Map<Block>(requestBody);
        if (block == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        await _blockRepository.InsertAsync(block);
        await _blockRepository.SaveChangesAsync();
        GetBlockDetail blockDetail = _mapper.Map<GetBlockDetail>(block);
        return blockDetail;
    }

    public async Task<GetBlockDetail> UpdateBlockAsync(Guid id, UpdateBlockModel requestBody)
    {
        if (id != requestBody.Id)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        Block block = await _blockRepository.GetFirstOrDefaultAsync(alu => alu.Id == requestBody.Id);
        if (block == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        block = _mapper.Map(requestBody, block);
        _blockRepository.Update(block);
        await _blockRepository.SaveChangesAsync();
        GetBlockDetail blockDetail = _mapper.Map<GetBlockDetail>(block);
        return blockDetail;
    }

    public async Task DeleteBlockAsync(Guid id)
    {
        Block? block = await _blockRepository.GetFirstOrDefaultAsync(alu => alu.Id == id);
        if (block == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        _blockRepository.Delete(block);
        await _blockRepository.SaveChangesAsync();
    }

    public async Task<int> GetTotal()
    {
        return await _blockRepository.GetAll().CountAsync();
    }
}