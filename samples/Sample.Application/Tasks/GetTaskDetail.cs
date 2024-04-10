using MediatorEndpoint;
using MediatR;
using Sample.Application.Tasks.Types;

namespace Sample.Application.Tasks;

[Query]
public record GetTaskDetail(Guid TaskId) : IRequest<TaskItemHeader>
{
}
public record TaskItemHeader(Guid Id, string Title, TaskPriority Priority, bool IsCompleted, DocumentHeader[] Documents)
{
}
public record DocumentHeader(Guid Id, string Filename)
{
}
public class GetTaskDetailHandler(ApplicationContext context) : IRequestHandler<GetTaskDetail, TaskItemHeader>
{
    public async Task<TaskItemHeader> Handle(GetTaskDetail request, CancellationToken cancellationToken)
    {
        //var task = await context.Tasks.Include(x => x.Documents).SingleOrDefaultAsync(x => x.Id == request.TaskId, cancellationToken);
        //return new TaskItemHeader(task.Id, task.Title, task.Priority, task.IsCompleted, task.Documents.Select(x => new DocumentHeader(x.Id, x.Filename)).ToArray());

        throw new NotImplementedException();
    }
}