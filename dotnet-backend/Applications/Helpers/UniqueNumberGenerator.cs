using Domain.Entities;

namespace Applications.Helpers
{
    public static class UniqueNumberGenerator
    {
        public static string GenerateUniqueNumber<T>(this T entity) 
            => Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
    }
}
