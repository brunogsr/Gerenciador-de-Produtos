using GerenciadorProdutos.Models.Users;
using GerenciadorProdutos.Models.Users.UserDto;
using GerenciadorProdutos.Services;
using GerenciadorProdutos.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GerenciadorProdutos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet] // Busca todos os usuários
        [Authorize(Policy = "GerenteFuncionario")]
        public ActionResult<List<UserResponseDTO>> GetUsers()
        {
            var users = _userService.GetUsers();
            return Ok(users);
        }


        [HttpGet("{id}")] // Busca um usuário por ID
        [Authorize(Policy = "GerenteFuncionario")]
        public ActionResult<UserResponseDTO> GetUserById([FromRoute] int id)
        {
            var user = _userService.GetUserById(id);
            if (user == null)
            {
                return NotFound(new { message = "Usuário não encontrado." });
            }
            return Ok(user);
        }


        [HttpPost("Login")] // Faz login
        public ActionResult Login([FromBody] UserRequestsDTO request)
        {
            if (request.Email == null | request.Password == null) 
            {
                return BadRequest(new { message = "Dados inválidos." });
            }
            var token = _userService.Login(request);
            if (token == null)
            {
                return NotFound(new { message = "Usuário ou senha inválidos." });
            }

            return Ok(token);
        }

        [HttpPost("SignUp")] // Cria um usuário
        public ActionResult CreateUser([FromBody] UserRequestDTO request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest(new { message = "Dados inválidos." });
            }
            var result = _userService.CreateUser(request);
            if (result == null)
            {
                return BadRequest(new { message = "Este email já está em uso." });
            }

            return Ok(result);
        }


        [HttpPatch("{id}/Role")] // Atualiza a Role de um usuário
        [Authorize(Roles = "Gerente")]
        public ActionResult UpdateUserRole([FromRoute] int id, [FromBody] RoleRequestDTO roleRequest)
        {
            _userService.UpdateUserRole(id, roleRequest);
            return Ok(new { message = "Role atualizada." });
        }

        [HttpDelete("{id}")] // Deleta um usuário
        [Authorize(Roles = "Gerente")]
        public ActionResult DeleteUser([FromRoute] int id)
        {
            var user = _userService.GetUserById(id);
            if (user == null)
            {
                return NotFound(new { message = "Usuário não encontrado." });
            }
            _userService.DeleteUser(id);
            return Ok(new { message = "Usuário deletado." });
        }
    }
}
