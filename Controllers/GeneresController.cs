using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using MoviesAPI.Dtos;
using System.Runtime.InteropServices;

namespace MoviesAPI.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class GeneresController : ControllerBase
	{
		private ApplicationDbContext _db;
        public GeneresController(ApplicationDbContext db)
		{
			_db= db;
		}
        [HttpGet]
		public async Task<IActionResult> GetAllAsync()
		{
			var genres = await _db.Genres.ToListAsync();
			return Ok(genres);
		}
		[HttpPost("{catagory}")]
		public async Task<IActionResult> Create(string catagory)
		{
			Genre genre = new Genre() { Name = catagory };
			await _db.Genres.AddAsync(genre);
			_db.SaveChanges();
			return Ok(genre);
		}
		[HttpPut("{catagory}")]
		public async Task<IActionResult> update(string catagory,Dto test)
		{
			var genre=await _db.Genres.Where(i=>catagory==i.Name).FirstOrDefaultAsync();
			if (genre == null)
				return NotFound("NO genere with this name");
			genre.Name = test.name;
			_db.SaveChanges();
			return Ok(genre);
		}
		[HttpDelete("{name}")]
		public async Task<IActionResult>Delete(string name)
		{
			var genre =await _db.Genres.Where(g=>g.Name==name).FirstOrDefaultAsync();
			if (genre == null)
			{
				return NotFound($"NO genre with name {name}");
			}
			_db.Remove(genre);
			_db.SaveChanges();
			return Ok(genre);
		}
	}
}
