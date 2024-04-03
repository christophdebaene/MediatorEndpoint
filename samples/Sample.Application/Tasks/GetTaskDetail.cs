using MediatorEndpoint;
using MediatR;
using Sample.Application.Tasks.Types;

namespace Sample.Application.Tasks;

[Query]
public record GetTaskDetail(Guid TaskId) : IRequest<TaskDetailModel>
{
}
public record TaskDetailModel(Guid Id, string Title, TaskPriority Priority, bool IsCompleted, int NumberDocuments)
{
}
public class GetTodoDetailHandler(ApplicationContext context) : IRequestHandler<GetTaskDetail, TaskDetailModel>
{
    public async Task<TaskDetailModel> Handle(GetTaskDetail request, CancellationToken cancellationToken)
    {
        var task = await context.Tasks.FindAsync([request.TaskId], cancellationToken);
        return new TaskDetailModel(task.Id, task.Title, task.Priority, task.IsCompleted, task.Documents.Count());
    }
}