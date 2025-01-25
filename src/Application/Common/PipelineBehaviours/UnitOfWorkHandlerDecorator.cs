using EventSourcingExample.Application.Abstraction.Persistence;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EventSourcingExample.Application.Common.PipelineBehaviours
{
    public class UnitOfWorkBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
	where TRequest : IRequest<TResponse>
	{
		private readonly IUnitOfWork _unitOfWork;

		public UnitOfWorkBehaviour(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
		{
			var response = await next();
			await _unitOfWork.CommitAsync(cancellationToken);
			return response;
		}
	}
}