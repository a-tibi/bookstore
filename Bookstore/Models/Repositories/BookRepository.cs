using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookstore.Models.Repositories
{
	public class BookRepository : IBookstoreRepository<Book>
	{
		List<Book> books;

		public BookRepository()
		{
			books = new List<Book>()
			{
				new Book
				{
					Id = 1,
					Title = "book1",
					Description = "description book1"
				},
				new Book
				{
					Id = 2,
					Title = "book2",
					Description = "description book2"
				},
				new Book
				{
					Id = 3,
					Title = "book3",
					Description = "description book3"
				}
			};
		}

		public void Add(Book entity)
		{
			entity.Id = books.Max(b => b.Id) + 1;
			books.Add(entity);
		}

		public void Delete(int id)
		{
			var book = Find(id);
			books.Remove(book);
		}

		public Book Find(int id)
		{
			var book = books.SingleOrDefault(b => b.Id == id);
			return book;
		}

		public IList<Book> List()
		{
			return books;
		}

		public IList<Book> Search(string term)
		{
			var result = books.Where(b => b.Title.Contains(term)
									||    b.Description.Contains(term)
									||    b.Author.FullName.Contains(term)).ToList();
			return result;
		}

		public void Update(int id, Book newBook)
		{
			var book = Find(id);
			book.Title = newBook.Title;
			book.Description = newBook.Description;
			book.Author = newBook.Author;
			book.ImageUrl = newBook.ImageUrl;
		}
	}
}
