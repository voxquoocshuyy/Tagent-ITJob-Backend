using AutoMapper;
using ITJob.Entity.Entities;
using ITJob.Entity.Repositories.RoleRepositories;
using ITJob.Services.Enum;
using ITJob.Services.Utility;
using ITJob.Services.Utility.ErrorHandling.Object;
using ITJob.Services.Utility.Paging;
using ITJob.Services.ViewModels.Role;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ITJob.Services.Services.RoleServices;

public class RoleService : IRoleService
{
    private readonly IRoleRepository _roleRepository;
    private readonly IMapper _mapper;

    public RoleService(IRoleRepository roleRepository, IMapper mapper)
    {
        _roleRepository = roleRepository;
        _mapper = mapper;
    }
    public IList<GetRoleDetail> GetRolePage(PagingParam<RoleEnum.RoleSort> paginationModel, SearchRoleModel searchRoleModel)
    {
        IQueryable<Role> queryRole = _roleRepository.Table.Include(c => c.Users);
        queryRole = queryRole.GetWithSearch(searchRoleModel);
        // Apply sort
        queryRole = queryRole.GetWithSorting(paginationModel.SortKey.ToString(), paginationModel.SortOrder);
        // Apply Paging
        queryRole = queryRole.GetWithPaging(paginationModel.Page, paginationModel.PageSize).AsQueryable();
        var result = _mapper.ProjectTo<GetRoleDetail>(queryRole);
        return result.ToList();
    }

    public async Task<GetRoleDetail> GetRoleById(Guid id)
    {
        Role role = await _roleRepository.GetFirstOrDefaultAsync(e => e.Id == id);
        if (role == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        var result = _mapper.Map<GetRoleDetail>(role);
        return result;
    }

    public async Task<GetRoleDetail> CreateRoleAsync(CreateRoleModel requestBody)
    {
        Role role = _mapper.Map<Role>(requestBody);
        if (role == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        await _roleRepository.InsertAsync(role);
        await _roleRepository.SaveChangesAsync();
        GetRoleDetail roleDetail = _mapper.Map<GetRoleDetail>(role);
        return roleDetail;
    }

    public async Task<GetRoleDetail> UpdateRoleAsync(Guid id, UpdateRoleModel requestBody)
    {
        if (id != requestBody.Id)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        Role role = await _roleRepository.GetFirstOrDefaultAsync(alu => alu.Id == requestBody.Id);
        if (role == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        role = _mapper.Map(requestBody, role);
        _roleRepository.Update(role);
        await _roleRepository.SaveChangesAsync();
        GetRoleDetail roleDetail = _mapper.Map<GetRoleDetail>(role);
        return roleDetail;
    }

    public async Task DeleteRoleAsync(Guid id)
    {
        Role role = await _roleRepository.GetFirstOrDefaultAsync(alu => alu.Id == id);
        if (role == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        _roleRepository.Delete(role);
        await _roleRepository.SaveChangesAsync();
    }

    public async Task<int> GetTotal()
    {
        return await _roleRepository.GetAll().CountAsync();
    }
}