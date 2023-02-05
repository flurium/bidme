namespace Web.Helpers
{
    public static class EnvName
    {
        /// <summary>
        /// Global Postgres Supabase database.
        /// Connection string from appsettings.json
        /// </summary>
        public static string Development => "Development";

        /// <summary>
        /// Global Postgres Supabase database.
        /// Connection string from Enviroment Variables
        /// </summary>
        public static string Production => "Production";
    }
}