using GerenciadorProdutos.Models.Users;
using GerenciadorProdutos.Models.Users.UserDto;

namespace GerenciadorProdutos.Services
{
    public interface IUserService
    {
        List<UserResponseDTO> GetUsers();
        UserResponseDTO? GetUserById(int id);
        object Login(UserRequestsDTO request);
        CreateUserResponseDTO CreateUser(UserRequestDTO request);
        void UpdateUserRole(int id, RoleRequestDTO request);
        void DeleteUser(int id);
    }
}
