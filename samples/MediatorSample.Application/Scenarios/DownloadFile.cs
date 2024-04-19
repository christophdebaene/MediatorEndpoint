using Mediator;
using MediatorEndpoint.Responses;

namespace Sample.Application.Scenarios;
public record DownloadFile(string SetName, string Filename) : IRequest<FileResponse>
{
}
public class DownloadDocumentHandler : IRequestHandler<DownloadFile, FileResponse>
{
    public ValueTask<FileResponse> Handle(DownloadFile request, CancellationToken cancellationToken)
    {
        var document = DocumentSet.Documents.Single(x => x.SetName == request.SetName && x.Filename == request.Filename);
        return ValueTask.FromResult(new FileResponse(document.Filename, document.ContentType, document.Data));
    }
}
