using AuthService.Domain.Entities;
 
namespace AuthService.Domain.Interfaces;
 
public interface IRoleRepository
{
    Task<Role?> GetByIdAsync(string id);
    Task<int> CountUsersInRoleAsync(string roleName);
    Task<IReadOnlyList<User>> GetByUsernameAsync(string roleName);
    Task<IReadOnlyList<string>> GetUserRolesAsync(string userId);
}