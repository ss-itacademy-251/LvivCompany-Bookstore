﻿using System.Threading.Tasks;
using LvivCompany.Bookstore.BusinessLogic;
using LvivCompany.Bookstore.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LvivCompany.Bookstore.Web.Controllers
{
    public class HomeController : Controller
    {
        private const int CountOfBook = 8;
        private IRepo<Book> _bookRepo;
        private IMapper<Book, BookViewModel> _bookmapper;
        private HomeServices services;
        private UserManager<User> _userManager;

        public HomeController(HomeServices services, UserManager<User> userManager)
        {
            this.services = services;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page)
        {
            if (page == 0)
            {
                page = 1;
            }

            List<Book> book = (await _bookRepo.GetPageAsync(x => x.Id > 0, CountOfBook, page)).ToList();

            if (book.Count < CountOfBook)
            {
                return View(model: new HomePageListViewModel() { Books = _bookmapper.Map(book), PageNumber = page, ExistNext = false });
            }

            return View(model: new HomePageListViewModel() { Books = _bookmapper.Map(book), PageNumber = page, ExistNext = true });
            return View(await services.GetViewModelForHomePage());
        }

        [Authorize(Roles = "Seller")]
        [HttpGet]
        public async Task<IActionResult> SellersBook()
        {
            return View(services.GetSellersBook((await _userManager.GetUserAsync(HttpContext.User)).Id));
        }
    }
}