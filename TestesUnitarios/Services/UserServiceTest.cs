using GerenciadorProdutos.Models.Users;
using GerenciadorProdutos.Models.Users.UserDto;
using GerenciadorProdutos.Repository;
using GerenciadorProdutos.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Xunit;

public class UserServiceTests
{
    private readonly UserService _service;
    private readonly AppDbContext _context;

    public UserServiceTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _context = new AppDbContext(options);

        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();

        SeedDatabase();

        _service = new UserService(_context);
    }

    private void SeedDatabase()
    {
        var users = new List<User>
        {
            new User { Name = "Professor Francisco", Email = "email@email.com", Password = BCrypt.Net.BCrypt.HashPassword("password"), Role = "Cliente" },
            new User { Name = "Fulano", Email = "Fulano@email.com", Password = BCrypt.Net.BCrypt.HashPassword("password"), Role = "Cliente" }
        };

        _context.Users.AddRange(users);
        _context.SaveChanges();
    }

    [Fact]
    public void GetUsers_ShouldReturnAllUsers()
    {
        var result = _service.GetUsers();

        Assert.Equal(2, result.Count);
        Assert.Equal("Professor Francisco", result[0].Name);
    }

    [Fact]
    public void GetUserById_ShouldReturnCorrectUser()
    {
        var result = _service.GetUserById(1);

        Assert.NotNull(result);
        Assert.Equal("Professor Francisco", result.Name);
    }

    [Fact]
    public void Login_ShouldReturnToken_WhenCredentialsAreCorrect()
    {
        var request = new UserRequestsDTO { Email = "email@email.com", Password = "password" };

        var result = _service.Login(request);

        Assert.NotNull(result);
        Assert.Contains("Token", result.ToString());
    }

    [Fact]
    public void CreateUser_ShouldCreateNewUser()
    {
        var request = new UserRequestDTO { Name = "New User", Email = "new.user@example.com", Password = "password" };

        var result = _service.CreateUser(request);

        Assert.NotNull(result);
        Assert.Equal("Usuário criado com sucesso.", result.Message); 
        Assert.Equal(3, _context.Users.Count());
    }

    [Fact]
    public void UpdateUserRole_ShouldUpdateRole()
    {
        var request = new RoleRequestDTO { Role = "Gerente" };

        _service.UpdateUserRole(1, request);

        var user = _context.Users.Find(1);
        Assert.Equal("Gerente", user.Role);
    }

    [Fact]
    public void DeleteUser_ShouldRemoveUser()
    {
        _service.DeleteUser(1);

        var user = _context.Users.Find(1);
        Assert.Null(user);
    }
}
