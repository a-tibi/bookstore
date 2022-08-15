﻿using Bookstore.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Bookstore.ViewModel
{
	public class BookAuthorViewModel
	{
		public int Id { get; set; }

		[Required]
		[MaxLength(20)]
		[MinLength(2)]
		public string Title { get; set; }

		[Required]
		[StringLength(25, MinimumLength = 3)]
		public string Description { get; set; }
		public int AuthorId { get; set; }
		public List<Author> Authors { get; set; }
		public IFormFile File { get; set; }
		public string ImageUrl { get; set; }
	}
}
