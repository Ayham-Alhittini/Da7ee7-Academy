﻿namespace Da7ee7_Academy.Extensions
{
    public static class EnvironmentExtension
    {
        public static string GetUrlRoot(this IWebHostEnvironment env)
        {
            return env.IsDevelopment() ? "https://localhost:7124" : "http://da7ee7-001-site1.dtempurl.com";
        }
    }
}
