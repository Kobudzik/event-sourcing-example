using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using EventSourcingExample.Application.Abstraction.User;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventSourcingExample.Application.Common.PipelineBehaviours
{
    public class PerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly Stopwatch _timer;
        private readonly ILogger<TRequest> _logger;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUserManagementService _userManagementService;

        public PerformanceBehaviour(
            ILogger<TRequest> logger,
            ICurrentUserService currentUserService,
            IUserManagementService userManagementService)
        {
            _timer = new Stopwatch();

            _logger = logger;
            _currentUserService = currentUserService;
            _userManagementService = userManagementService;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            _timer.Start();

            var response = await next();

            _timer.Stop();

            var elapsedMilliseconds = _timer.ElapsedMilliseconds;

            if (elapsedMilliseconds > 500)
            {
                var requestName = typeof(TRequest).Name;
                var userPublicId = _currentUserService.UserGuid();
                var userName = string.Empty;

                if (userPublicId != Guid.Empty)
                {
                    var userDetails = await _userManagementService.GetUserDetailsAsync(userPublicId);
                    userName = userDetails.UserName;
                }

                _logger.LogWarning("Long Running Request: {Name} ({ElapsedMilliseconds} milliseconds) {@UserId} {@UserName} {@Request}",
                    requestName, elapsedMilliseconds, userPublicId, userName, request);
            }

            return response;
        }
    }
}
