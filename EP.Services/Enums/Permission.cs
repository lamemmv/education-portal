using System;

namespace EP.Services.Enums
{
    [Flags]
    public enum Permission
    {
        None = 0,
        Read = 1,
        Host = 2,
        Upload = 4,
        All = Read | Host | Upload
    }
}
