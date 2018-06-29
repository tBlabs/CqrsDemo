using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Model.validation.services;

namespace Model.config
{
    public class DependencyInjectionConfig : IDependencyInjectionConfig
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IMyValidator, MyValidator>();
        }
    }
}
