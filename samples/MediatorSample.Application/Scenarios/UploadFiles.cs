using Mediator;
using MediatorEndpoint;
using System.Text.Json.Serialization;

namespace Sample.Application.Scenarios;

public record UploadFiles : ICommand, IFileRequest
{
    public string SetName { get; init; }

    [JsonIgnore]
    public IReadOnlyList<IFile> Files { get; set; }
}
public class UploadFilesHandler : ICommandHandler<UploadFiles>
{
    public async ValueTask<Unit> Handle(UploadFiles request, CancellationToken cancellationToken)
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

        DocumentSet.Documents.AddRange(documents);

        return Unit.Value;
    }
}