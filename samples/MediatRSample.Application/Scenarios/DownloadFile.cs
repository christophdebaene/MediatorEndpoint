﻿using MediatorEndpoint;
using MediatorEndpoint.Responses;
using MediatR;

namespace Sample.Application.Scenarios;

[Query]
public record DownloadFile(string SetName, string Filename) : IRequest<FileResponse>
{
}
public class DownloadDocumentHandler : IRequestHandler<DownloadFile, FileResponse>
{
    public Task<FileResponse> Handle(DownloadFile request, CancellationToken cancellationToken)
    {
        var document = DocumentSet.Documents.Single(x => x.SetName == request.SetName && x.Filename == request.Filename);
        return Task.FromResult(new FileResponse(document.Filename, document.ContentType, document.Data));
    }
}
