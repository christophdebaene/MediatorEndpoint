using MediatorEndpoint;
using MediatR;
using Sample.Application.Tasks.Types;
using System.Text.Json.Serialization;

namespace Sample.Application.Scenarios;

[Command]
public record UploadFiles : IRequest, IFileRequest
{
    public string SetName { get; init; }

    [JsonIgnore]
    public IReadOnlyList<IFile> Files { get; set; }
}
public class UploadFilesHandler(ApplicationContext context) : IRequestHandler<UploadFiles>
{
    public async Task Handle(UploadFiles request, CancellationToken cancellationToken)
    {
        var documents = new List<Document>();
        foreach (var file in request.Files)
        {
            var data = await file.ReadAsBytesAsync();
            documents.Add(new Document
            {
                SetName = request.SetName,
                Filename = file.FileName,
                Data = data,
                ContentType = file.ContentType
            });
        }

        await context.Documents.AddRangeAsync(documents);
        await context.SaveChangesAsync(cancellationToken);
    }
}