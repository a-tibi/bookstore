using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bookstore.Models;
using Bookstore.Models.Repositories;
using Bookstore.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.Controllers
{
    public class BookController : Controller
    {
		private readonly IBookstoreRepository<Book> bookRepository;
		private readonly IBookstoreRepository<Author> authorRepository;
		private readonly IHostingEnvironment hosting;

		public BookController(IBookstoreRepository<Book> bookRepository, IBookstoreRepository<Author> authorRepository, IHostingEnvironment hosting)
		{
			this.bookRepository = bookRepository;
			this.authorRepository = authorRepository;
			this.hosting = hosting;
		}
        // GET: Book
        public ActionResult Index()
        {
			var books = bookRepository.List();
            return View(books);
        }

		public ActionResult Search(string term)
		{
			var books = bookRepository.Search(term);
			return View("Index", books);
		}

        // GET: Book/Details/5
        public ActionResult Details(int id)
        {
			var book = bookRepository.Find(id);
            return View(book);
        }

        // GET: Book/Create
        public ActionResult Create()
        {
			var model = new BookAuthorViewModel
			{
				Authors = FillSelectList()
			};
            return View(model);
        }

        // POST: Book/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BookAuthorViewModel model)
        {
			if (ModelState.IsValid)
			{
				try
				{
					string fileName = UploadFile(model.File);

					if (model.AuthorId == -1)
					{
						ViewBag.Message = "Please select author!";
						model.Authors = FillSelectList();
						return View(model);
					}

					var author = authorRepository.Find(model.AuthorId);
					var book = new Book
					{
						Id = model.Id,
						Title = model.Title,
						Description = model.Description,
						Author = author,
						ImageUrl = fileName
					};

					bookRepository.Add(book);

					return RedirectToAction(nameof(Index));
				}
				catch
				{
					return View();
				}
			}

			ModelState.AddModelError("","You have to fill the required details!");
			model.Authors = FillSelectList();
			return View(model);
        }

        // GET: Book/Edit/5
        public ActionResult Edit(int id)
        {
			var book = bookRepository.Find(id);
			var authors = FillSelectList();
			var authorId = book.Author == null ? 0 : book.Author.Id;
			var model = new BookAuthorViewModel
			{
				Id = book.Id,
				Title = book.Title,
				Description = book.Description,
				AuthorId = authorId,
				Authors = authors,
				ImageUrl = book.ImageUrl
			};

            return View(model);
        }

        // POST: Book/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, BookAuthorViewModel model)
        {
            try
            {
				var fileName = UploadFile(model.File, model.ImageUrl);

				var author = authorRepository.Find(model.AuthorId);
				var book = new Book
				{
					Id = model.Id,
					Title = model.Title,
					Description = model.Description,
					Author = author,
					ImageUrl = fileName
				};

				bookRepository.Update(model.Id, book);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Book/Delete/5
        public ActionResult Delete(int id)
        {
			var book = bookRepository.Find(id);
            return View(book);
        }

        // POST: Book/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Book book)
        {
            try
            {
				bookRepository.Delete(id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

		List<Author> FillSelectList()
		{
			var authors = authorRepository.List().ToList();
			authors.Insert(0, new Author { Id = -1, FullName = "---- Select Author ----" });

			return authors;
		}

		string UploadFile(IFormFile file)
		{
			var fileName = string.Empty;

			if (file != null)
			{
				fileName = file.FileName;
				var path = Path.Combine(hosting.WebRootPath, "uploads", fileName);
				file.CopyTo(new FileStream(path, FileMode.Create));
			}

			return fileName;
		}

		string UploadFile(IFormFile file, string oldFile)
		{
			var fileName = oldFile;

			if (file != null)
			{
				fileName = file.FileName;
				var filePath = Path.Combine(hosting.WebRootPath, "uploads", fileName);
				file.CopyTo(new FileStream(filePath, FileMode.Create));

				// Delete old file
				var oldFilePath = Path.Combine(hosting.WebRootPath, "uploads", oldFile);
				System.IO.File.Delete(oldFilePath);
			}

			return fileName;
		}
    }
}