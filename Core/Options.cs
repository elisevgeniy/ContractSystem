namespace ContractSystem.Core
{
    public static class Options
    {
        public static string ConnectionString {
            get {
                return Environment.GetEnvironmentVariable("ConnectionString") ?? throw new Exception("Not found environment variable \"ConnectionString\"");
            }
        }
    }
}
