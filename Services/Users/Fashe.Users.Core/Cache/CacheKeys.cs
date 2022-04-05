using System;

namespace Fashe.Users.Core.Cache
{
    public static class CacheKeys
    {
        public static string UserKey(Guid userId) => $"user:{userId}";
    }
}
