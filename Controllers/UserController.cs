using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using UsersAPI.Models;
using UsersAPI.Repositories;

namespace UsersAPI.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class UserController : Controller
	{
		public readonly IUserRepository _userRepository;

		public UserController(IUserRepository userRepository) => _userRepository = userRepository;

		[HttpGet("user/{id}")]
		public async Task<ActionResult<User>> GetUser(int id)
		{
			var user = await _userRepository.Get(id);
			if (user == null)
			{
				return NotFound();
			}
			return Ok(user);
		}

		[HttpGet("user/all")]
		public async Task<ActionResult<IEnumerable<User>>> GetUsers()
		{
			var users = await _userRepository.GetAll();
			return Ok(users);
		}

		[HttpPost("create")]
		public async Task<ActionResult> CreateUser(CreateUserDto createUserDto)
		{
			User user = new()
			{
				Name = createUserDto.Name,
				Password = createUserDto.Password
			};

			await _userRepository.Add(user);
			return Ok();
		}

		[HttpDelete("delete/{id}")]
		public async Task<ActionResult> DeleteUser(int id)
		{
			await _userRepository.Delete(id);
			return Ok();
		}

		[HttpPut("update/{id}")]
		public async Task<ActionResult> UpdateUser(int id, UpdateUserDto updateUserDto)
		{
			User user = new()
			{
				Id = id,
				Password = updateUserDto.Password
			};

			await _userRepository.Update(user);
			return Ok();
		}

		[HttpPost("upload")]
		public async Task<ActionResult> UploadUsers(IFormFile file)
		{
			await _userRepository.UploadUsers(file);
			return Ok();
		}
	}
}
