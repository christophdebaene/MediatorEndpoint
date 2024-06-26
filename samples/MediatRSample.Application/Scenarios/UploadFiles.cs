﻿using MediatorEndpoint;
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
public class UploadFilesHandler : IRequestHandler<UploadFiles>
{
    public Task Handle(UploadFiles request, CancellationToken cancellationToken)
    {
        var documents = new List<Document>();
        foreach (var file in request.Files)
        {
            var data = file.ReadAsBytes();
            documents.Add(new Document
            {
                SetName = request.SetName,
                Filename = file.FileName,
                Data = data,
                ContentType = file.ContentType
            });
        }

        DocumentSet.Documents.AddRange(documents);
        return Task.CompletedTask;
    }
}