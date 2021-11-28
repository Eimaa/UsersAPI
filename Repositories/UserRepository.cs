using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UsersAPI.Data;
using UsersAPI.Models;

namespace UsersAPI.Repositories
{
	public class UserRepository : IUserRepository
	{
		public readonly IDataContext _db;

		public UserRepository(IDataContext db) => _db = db;

		public async Task Add(User user)
		{
			await _db.Users.AddAsync(user);
			await _db.SaveChangesAsync();
		}

		public async Task Delete(int id)
		{
			var itemToDelete = await _db.Users.FindAsync(id);
			if (itemToDelete == null)
				throw new NullReferenceException();

			_db.Users.Remove(itemToDelete);
			await _db.SaveChangesAsync();
		}

		public async Task<User> Get(int id)
		{
			return await _db.Users.FindAsync(id);
		}

		public async Task<IEnumerable<User>> GetAll()
		{
			return await _db.Users.ToListAsync();
		}

		public async Task Update(User user)
		{
			var itemToUpdate = await _db.Users.FindAsync(user.Id);

			if (itemToUpdate == null)
			{
				throw new NullReferenceException();
			}

			itemToUpdate.Password = user.Password;
			await _db.SaveChangesAsync();
		}

		public async Task UploadUsers(IFormFile file)
		{
			if (file == null || file.Length == 0)
			{
				throw new NullReferenceException();
			}

			using var reader = new StreamReader(file.OpenReadStream());

			while (reader.Peek() >= 0)
			{
				var line = await reader.ReadLineAsync();
				if (!string.IsNullOrEmpty(line))
				{
					string[] lineItems = line.Split(' ');
					var isUserExists = await _db.Users
						.Where(x => x.Name == lineItems[0].ToString())
						.AnyAsync();

					if (!isUserExists)
					{
						User newUser = new()
						{
							Name = lineItems[0],
							Password = lineItems[1]
						};

						await _db.Users.AddAsync(newUser);
						await _db.SaveChangesAsync();
					}
				}
			}
		}
	}
}
