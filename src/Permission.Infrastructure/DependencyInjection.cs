using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Permissions.Domain.AggregatesModel.EmployeeAggregate;
using Permissions.Domain.AggregatesModel.PermissionAggregate;
using Permissions.Domain.SeedWork;
using Permissions.Infrastructure.Repositories;
using Permissions.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permissions.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration Configuration, bool IsDevelopment)
        {
            services.AddDbContext<DbContext, PermissionDbContext>(options =>
                options
                    .UseSqlServer(Configuration.GetConnectionString(nameof(PermissionDbContext))
                     , options => options.EnableRetryOnFailure()
                     )
                    //.UseLoggerFactory(loggerFactory)
                    //.ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning))
                    //.EnableSensitiveDataLogging(IsDevelopment)

                );

            services.AddScoped<IUnitOfWork, PermissionDbContext>();
            services.AddTransient<IEmployeeRepository, EmployeeRepository>();
            services.AddTransient<IPermissionRepository, PermissionRepository>();
            services.AddTransient<IPermissionElasticService, PermissionElasticService>();

            return services;
        }
    }
}
