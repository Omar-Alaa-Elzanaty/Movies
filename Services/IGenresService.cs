namespace MoviesAPI.Services
{
	public interface IGenresService
	{
		Task<IEnumerable<Genre>> GetAll();
		Task<Genre>Add(Genre genre);
		Task<Genre>Remove(Genre genre);
		Task<Genre> Delete(Genre genre);
		Task<Genre>GetById(byte id);
	}
}
