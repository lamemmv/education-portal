using Microsoft.AspNetCore.Authorization;

namespace EP.API.Infrastructure
{
    public sealed class MinimumAgeRequirement : IAuthorizationRequirement
    {
        public MinimumAgeRequirement(int age)
        {
            Age = age;
        }

        public int Age { get; }
    }
}