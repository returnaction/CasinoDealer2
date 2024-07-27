﻿using CasinoDealer2.RepositoryFolder.BalckJackRepository;
using CasinoDealer2.RepositoryFolder.QuestionRepository;
using CasinoDealer2.UnitOfWork;

namespace CasinoDealer2.Extenstions
{
    public static class LoadServiceExtensions
    {
        public static IServiceCollection AddCustomServices(this IServiceCollection services, ConfigurationManager configuration)
        {
            //Repositories
            services.AddScoped<IUnitOfWork, CasinoDealer2.UnitOfWork.UnitOfWork>();
            services.AddScoped<IBlackJackService, BlackJackService>();
            services.AddScoped<IQuestionService, QuestionService>();
            return services;
        }
    }
}