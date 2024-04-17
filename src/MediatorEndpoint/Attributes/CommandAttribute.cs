using System;

namespace MediatorEndpoint;

public class CommandAttribute : RequestKindAttribute
{
    public CommandAttribute() : base(RequestKind.Command)
    {
    }
    public static bool Is(Type type) => Get(type) == RequestKind.Command;
}
