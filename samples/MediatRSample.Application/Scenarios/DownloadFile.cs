using MediatorEndpoint;
using MediatorEndpoint.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Sample.Application.Scenarios;

[Query]
public record DownloadFile(string SetName, string Filename) : IRequest<FileResponse>
{
}
public class DownloadDocumentHandler(ApplicationContext context) : IRequestHandler<DownloadFile, FileResponse>
{
    public async Task<FileResponse> Handle(DownloadFile request, CancellationToken cancellationToken)
    {
        var document = await context.Documents.SingleAsync(x => x.SetName == request.SetName && x.Filename == request.Filename);
        return new FileResponse(document.Filename, document.ContentType, document.Data);
    }
}
