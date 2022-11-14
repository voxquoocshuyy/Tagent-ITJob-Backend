using ITJob.Services.Enum;
using ITJob.Services.Utility.Paging;
using ITJob.Services.ViewModels.Role;

namespace ITJob.Services.Services.RoleServices;

public interface IRoleService
{
    IList<GetRoleDetail> GetRolePage(PagingParam<RoleEnum.RoleSort> paginationModel,
        SearchRoleModel searchRoleModel);

    public Task<GetRoleDetail> GetRoleById(Guid id);

    public Task<GetRoleDetail> CreateRoleAsync(CreateRoleModel requestBody);

    public Task<GetRoleDetail> UpdateRoleAsync(Guid id, UpdateRoleModel requestBody);

    public Task DeleteRoleAsync(Guid id);

    public Task<int> GetTotal();
}