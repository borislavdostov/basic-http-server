﻿using BasicHttpServer.HTTP;
using BasicHttpServer.MvcFramework;
using BattleCards.Data;
using BattleCards.ViewModels;
using BattleCards.ViewModels.Cards;
using System.Linq;

namespace BattleCards.Controllers
{
    public class CardsController : Controller
    {
        private readonly ApplicationDbContext db;

        public CardsController(ApplicationDbContext db)
        {
            this.db = db;
        }

        public HttpResponse Add()
        {
            if (!IsUserSignedIn())
            {
                return Redirect("/Users/Login");
            }

            return View();
        }

        [HttpPost("/Cards/Add")]
        public HttpResponse DoAdd(AddCardInputModel model)
        {
            if (!IsUserSignedIn())
            {
                return Redirect("/Users/Login");
            }

            if (Request.FormData["name"].Length < 5)
            {
                return Error("Name should be atleast 5 characters long!");
            }

            db.Cards.Add(new Card
            {
                Attack = model.Attack,
                Health = model.Health,
                Description = model.Description,
                Name = model.Name,
                ImageUrl = model.Image,
                Keyword = model.Keyword
            });

            db.SaveChanges();

            return Redirect("/Cards/All");
        }

        public HttpResponse All()
        {
            if (!IsUserSignedIn())
            {
                return Redirect("/Users/Login");
            }

            var cardsViewModel = db.Cards.Select(c => new CardViewModel
            {
                Name = c.Name,
                Description = c.Description,
                Attack = c.Attack,
                Health = c.Health,
                ImageUrl = c.ImageUrl,
                Type = c.Keyword
            }).ToList();

            return View(cardsViewModel);
        }

        public HttpResponse Collection()
        {
            if (!IsUserSignedIn())
            {
                return Redirect("/Users/Login");
            }

            return View();
        }
    }
}
