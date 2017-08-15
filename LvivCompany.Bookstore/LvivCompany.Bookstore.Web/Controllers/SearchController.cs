﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LvivCompany.Bookstore.DataAccess.Repo;
using LvivCompany.Bookstore.Entities;
using LvivCompany.Bookstore.Web.Mapper;
using LvivCompany.Bookstore.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;


namespace LvivCompany.Bookstore.Web.Controllers
{
    public class SearchController : Controller
    {
        private IRepo<Book> _bookRepo;
        private IMapper<Book, BookViewModel> _bookmapper;

        public SearchController(IRepo<Book> bookRepo, IMapper<Book, BookViewModel> bookmapper)
        {
            _bookRepo = bookRepo;
            _bookmapper = bookmapper;
        }

        public async Task<IActionResult> Index(string SearchText)
        {
            List<Book> books = (await _bookRepo.GetAllAsync()).ToList();
            if (!string.IsNullOrEmpty(SearchText))
            {
                var result = books.Where(s => (s.Name.ToLower().Contains(SearchText.ToLower())
                || (s.BookAuthors.Select(x => x.Author).Any(v => (v.FirstName.ToLower().Contains(SearchText.ToLower()) || v.LastName.ToLower().Contains(SearchText.ToLower()))))));
                return View(new HomePageListViewModel() { Books = _bookmapper.Map(result.ToList()) });
            }
            return View(new HomePageListViewModel() { Books = _bookmapper.Map(books) });
        }
    }
}
