using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using EventSourcingExample.Application.Common.PipelineBehaviours;
using EventSourcingExample.Application.Abstraction;
using EventSourcingExample.Domain.Entities.Banking;
using Microsoft.Extensions.Configuration;
using System;

namespace EventSourcingExample.Application
{
    public static class DependencyInjection
    {
        /// <summary>
        /// Adds MediatR, pipeline behaviours, validators and Mapper
        /// </summary>
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
            //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkBehaviour<,>));

			services.AddTransient(typeof(ISqlUnitOfWork), typeof(SqlUnitOfWork));

			if (configuration.GetValue<bool>("UseBankingEventStore"))
            {
				services.AddTransient(typeof(IEventSourcingUnitOfWork), typeof(EventSourcingUnitOfWork<BankAccount>));
			}

			services.AddTransient(typeof(IUnitOfWork), typeof(CompositeUnitOfWork));

            return services;
        }
    }
}
