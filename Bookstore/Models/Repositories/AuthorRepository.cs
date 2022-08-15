using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookstore.Models.Repositories
{
	public class AuthorRepository : IBookstoreRepository<Author>
	{
		List<Author> authors;

		public AuthorRepository()
		{
			authors = new List<Author>()
			{
				new Author
				{
					Id = 1,
					FullName = "Ahmad Tibi"
				},
				new Author
				{
					Id = 2,
					FullName = "Mohamad Tibi"
				},
				new Author
				{
					Id = 3,
					FullName = "Samer Tibi"
				}
			};
		}

		public void Add(Author entity)
		{
			entity.Id = authors.Max(a => a.Id) + 1;
			authors.Add(entity);
		}

		public void Delete(int id)
		{
			var author = Find(id);
			authors.Remove(author);
		}

		public Author Find(int id)
		{
			var author = authors.SingleOrDefault(a => a.Id == id);
			return author;
		}

		public IList<Author> List()
		{
			return authors;
		}

		public IList<Author> Search(string term)
		{
			var result = authors.Where(a => a.FullName.Contains(term)).ToList();
			return result;
		}

		public void Update(int id, Author newAuthor)
		{
			var author = Find(id);
			author.FullName = newAuthor.FullName;

			//var index = authors.FindIndex(a => a.Id == id);
			//authors[index] = newAuthor;

		}
	}
}
