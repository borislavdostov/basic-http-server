﻿using System.Collections.Generic;
using BasicHttpServer.HTTP;
using BasicHttpServer.MvcFramework;
using BattleCards.Data;
using BattleCards.Services;
using Microsoft.EntityFrameworkCore;

namespace BattleCards
{
    public class Startup : IMvcApplication
    {
        public void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.Add<IUsersService, UsersService>();
            serviceCollection.Add<ICardsService, CardsService>();
        }

        public void Configure(List<Route> routeTable)
        {
            new ApplicationDbContext().Database.Migrate();
        }
    }
}
