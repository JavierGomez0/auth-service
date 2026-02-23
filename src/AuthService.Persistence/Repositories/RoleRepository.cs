using AuthService.Domain.Entities;
using AuthService.Domain.Interfaces;
using AuthService.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Persistence.Repositories;

public class RoleRepository(ApplicationDbContext context) : IRoleRepository
{
   public async Task<Role?> GetByIdAsync(string id)
   {
       return await context.Roles
       .Include(r => r.UserRoles) // Se incluye la colección de UserRoles relacionada con el rol
       .FirstOrDefaultAsync(r => r.Name == id); // Se busca el rol por su nombre utilizando FirstOrDefaultAsync, lo que devuelve null si no se encuentra ningún rol con ese nombre
   }

   public async Task<int> CountUsersInRoleAsync(string roleName)
   {
       return await context.UserRoles
       .Where(ur => ur.Role.Name == roleName) // Se filtran los UserRoles por el nombre del rol utilizando una condición que verifica que el nombre del rol coincida
       .CountAsync(); // Se cuenta el número de UserRoles que cumplen con la condición utilizando
   }

   public async Task<IReadOnlyList<User>> GetByUsernameAsync(string roleName)
   {
       return await context.UserRoles
         .Where(ur => ur.Role.Name == roleName) // Se filtran los UserRoles por el nombre del rol utilizando una condición que verifica que el nombre del rol coincida
            .Select(ur => ur.User) // Se seleccionan los usuarios relacionados con los UserRoles que cumplen con la condición utilizando Select para proyectar solo los usuarios
            .Include(u => u.UserProfile) // Se incluye la entidad UserProfile relacionada con cada usuario utilizando Include para cargar la información del perfil de cada usuario
            .Include(u => u.UserEmail) // Se incluye la entidad UserEmail relacionada con cada usuario utilizando Include para cargar la información del correo electrónico de cada usuario
            .Include(u => u.UserRoles) // Se incluye la colección de UserRoles relacionada con cada usuario utilizando Include para cargar la información de los roles de cada usuario
            .ThenInclude(ur => ur.Role) // Se incluye la entidad Role relacionada con cada
            .ToListAsync() // Se obtiene la lista de usuarios utilizando ToListAsync, lo que devuelve una lista de usuarios con su información de perfil, correo electrónico y roles relacionados
            .ContinueWith(t => (IReadOnlyList<User>)t.Result); // Se convierte el resultado a IReadOnlyList<User> utilizando ContinueWith para proyectar el resultado a la interfaz IReadOnlyList<User>
       
   }

   public async Task<IReadOnlyList<string>> GetUserRolesAsync(string userId)
   {
    return await context.UserRoles
    .Where(ur => ur.UserId == userId) // Se filtran los User
    .Select(ur => ur.Role.Name) // Se seleccionan los nombres de los roles relacionados con los UserRoles que cumplen con la condición utilizando Select para proyectar solo los nombres de los roles
    .ToListAsync() // Se obtiene la lista de nombres de roles utilizando ToListAsync
    .ContinueWith(t => (IReadOnlyList<string>)t.Result); // Se convierte el resultado a IReadOnlyList<string> utilizando ContinueWith para proyectar el resultado a la interfaz IReadOnlyList<string>
   }
}