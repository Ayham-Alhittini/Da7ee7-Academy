namespace Da7ee7_Academy.Extensions
{
    public static class EnvironmentExtension
    {
        public static string GetUrlRoot(this IWebHostEnvironment env)
        {
            return env.IsDevelopment() ? "https://localhost:7124" : "production url";
        }
    }
}
