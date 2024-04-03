using System.Collections.Generic;

namespace MediatorEndpoint;
public interface IFileRequest
{
    IReadOnlyList<IFile> Files { get; set; }
}
