using System;

namespace EP.Services.Enums
{
    [Flags]
    public enum Permission
    {
        None = 0,
        Read = 1,
        Host = 2,
        Publish = 4,
        Upload = 8,
        All = Read | Host | Publish | Upload
    }
}
