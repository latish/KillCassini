// Guids.cs
// MUST match guids.h
using System;

namespace LatishSehgal.KillCassini
{
    static class GuidList
    {
        public const string guidKillCassiniPkgString = "6ffccb42-5c12-4632-82d8-41d3349e8ba8";
        public const string guidKillCassiniCmdSetString = "b4d7892f-9be2-4189-9c2b-5fa540244816";

        public static readonly Guid guidKillCassiniCmdSet = new Guid(guidKillCassiniCmdSetString);
    };
}