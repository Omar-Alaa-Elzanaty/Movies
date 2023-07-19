using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.Dtos;
using MoviesAPI.Migrations;

namespace MoviesAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class MoviesController : ControllerBase
	{
		private readonly ApplicationDbContext _context;
        public MoviesController(ApplicationDbContext context)
        {
            _context=context;
        }
		[HttpPost]
		public async Task<IActionResult> Create([FromForm]MovieDto Dto)
		{
			using var datastream=new MemoryStream();
			await Dto.Poster.CopyToAsync(datastream);
			var z=Dto.Poster.Length;
			var x = Path.GetExtension(Dto.Poster.FileName);

			var movie = new Movie()
			{
				GenreId = Dto.GenreId,
				Title = Dto.Title,
				Rate = Dto.Rate,
				Storeline = Dto.Storeline,
				Year = Dto.Year,
				Poster = datastream.ToArray()
			};
			await _context.AddAsync(movie);
			_context.SaveChanges();
			return Ok(movie);
		}
		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			var movies = _context.Movies
				.Include(m => m.Genre)
				.OrderByDescending(m=>m.Rate)
				.Select(m => new
				{
					m.Title,
					m.Poster,
					m.Storeline,
					m.Genre.Name,
					m.Rate,
					m.Year
				}).ToList();

			return Ok(movies);
		}
		[HttpGet("{id}")]
		public async Task<IActionResult>GetById(int id)
		{
			var movie=await _context.Movies.Include(i=>i.Genre).SingleOrDefaultAsync(i=>i.Id==id);
			if (movie == null)
				return NotFound();
			var dto = new
			{
				movie.Title,
				movie.Poster,
				movie.Storeline,
				movie.Genre.Name,
				movie.Rate,
				movie.Year
			};
			return Ok(dto);
		}
		[HttpGet("GetByGenreId")]
		public async Task<IActionResult>GetByGenreId(byte genreId)
		{
			var movies = _context.Movies
				.Include(m => m.Genre)
				.Where(i=>i.Genre.Id==genreId)
				.OrderByDescending(m => m.Rate)
				.Select(m => new
				{
					m.Title,
					m.Poster,
					m.Storeline,
					m.Genre.Name,
					m.Rate,
					m.Year
				}).ToList();
			return Ok(movies);
		}
		[HttpDelete("{id}")]
		public async Task<IActionResult>DeleteMovie(int id)
		{
			var movie = _context.Movies.Find(id);
			if (movie == null) return NotFound();
			_context.Remove(movie);
			_context.SaveChanges();
			return Ok();
		}
		[HttpPut("{id}")]
		public async Task<IActionResult> update (int id, [FromForm] MovieDto dto)
		{
			var movie = await _context.Movies.FindAsync(id);
			if (movie == null) return NotFound($"No movie was found with ID {id}");
			if (dto.Poster != null)
			{
				//logic of poster size and extension
				using var datastream = new MemoryStream();
				await dto.Poster.CopyToAsync(datastream);
				movie.Poster=datastream.ToArray();
			}
			movie.Title = dto.Title;
			movie.GenreId=dto.GenreId;
			movie.Year=dto.Year;
			movie.Storeline=dto.Storeline;
			movie.Rate=dto.Rate;
			_context.SaveChanges();
			return Ok(movie);
		}
	}
}
