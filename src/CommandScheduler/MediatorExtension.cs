using MediatR;
using System;

namespace CommandScheduler
{
    public static class MediatorExtension
    {
        /// <summary>
        /// Creates a new fire-and-forget job based on a given method call expression.
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="request">Request call expression that will be marshalled to a server.</param>
        /// <param name="description"></param>
        /// <returns></returns>
        public static string Enqueue(this IMediator mediator, IRequest request, string description = null)
        {
            return new AsyncCommand(new CommandsExecutor(mediator)).Enqueue(request, description);
        }

        /// <summary>
        ///  Creates a new background job based on a specified method call expression
        ///  and schedules it to be enqueued at the given moment of time.
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="request"></param>
        /// <param name="scheduleAt"></param>
        /// <param name="description"></param>
        public static void Schedule(this IMediator mediator, IRequest request, DateTimeOffset scheduleAt, string description = null)
        {
            new AsyncCommand(new CommandsExecutor(mediator)).Schedule(request, scheduleAt, description);
        }

        /// <summary>
        /// Creates a new background job based on a specified method call expression
        /// and schedules it to be enqueued at the given moment of time.
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="request"></param>
        /// <param name="name"></param>
        /// <param name="cronExpression"></param>
        /// <param name="description"></param>
        public static void ScheduleRecurring(this IMediator mediator, IRequest request, string name, string cronExpression, string description = null)
        {
            new AsyncCommand(new CommandsExecutor(mediator)).ScheduleRecurring(request, name, cronExpression, description);
        }
    }
}
