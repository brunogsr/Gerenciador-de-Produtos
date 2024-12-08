using GerenciadorProdutos.Models.Users;
using GerenciadorProdutos.Repository;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using GerenciadorProdutos.Utils;
using GerenciadorProdutos.Models.Users.UserDto;

namespace GerenciadorProdutos.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public List<UserResponseDTO> GetUsers()
        {
            return _context.Users
                .Select(user => new UserResponseDTO
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Role = user.Role
                })
                .ToList();
        }

        public UserResponseDTO? GetUserById(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                return null;
            }

            return new UserResponseDTO
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role
            };
        }


        public object Login(UserRequestsDTO request)
        {
            var userResponse = _context.Users.FirstOrDefault(user => user.Email == request.Email);
            if (userResponse == null || !BCrypt.Net.BCrypt.Verify(request.Password, userResponse.Password))
            {
                return null;
            }

            var token = TokenMethods.GenerateToken(userResponse);
            return new { Token = token, Expiration = DateTime.Now.AddHours(4) };
        }

        public CreateUserResponseDTO CreateUser(UserRequestDTO request)
        {
            var userExists = _context.Users.FirstOrDefault(user => user.Email == request.Email);
            if (userExists != null)
            {
                return new CreateUserResponseDTO { Message = "Usuário já existe." };
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);
            var user = new User
            {
                Name = request.Name,
                Email = request.Email,
                Password = hashedPassword,
                Role = "Cliente"
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return new CreateUserResponseDTO { Message = "Usuário criado com sucesso." };
        }



        public void UpdateUserRole(int id, RoleRequestDTO request)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                user.Role = request.Role;
                _context.SaveChanges();
            }
        }

        public void DeleteUser(int id)
        {
            var userResponse = _context.Users.Find(id);
            if (userResponse != null)
            {
                _context.Users.Remove(userResponse);
                _context.SaveChanges();
            }
        }
    }
}
