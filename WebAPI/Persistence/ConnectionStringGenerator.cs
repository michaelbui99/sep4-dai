public class ConnectionStringGenerator
{
    public static string GetConnectionStringFromEnvironment()
    {
        var dbName = System.Environment.GetEnvironmentVariable("RDS_DB_NAME");

        var username = System.Environment.GetEnvironmentVariable("RDS_USERNAME");
        var password = System.Environment.GetEnvironmentVariable("RDS_PASSWORD");
        var hostname = System.Environment.GetEnvironmentVariable("RDS_HOSTNAME");
        var port = System.Environment.GetEnvironmentVariable("RDS_PORT");

        return
            $"Data Source= {hostname},{port};" +
            $"Initial Catalog= {dbName};" +
            $"User ID={username};" +
            $"Password = {password}";
    }
    
    
    public static string GetConnectionStringFromEnvironmentDataWareHouse()
    {
        var dwhName = System.Environment.GetEnvironmentVariable("RDS_DWH_NAME");

        var username = System.Environment.GetEnvironmentVariable("RDS_USERNAME");
        var password = System.Environment.GetEnvironmentVariable("RDS_PASSWORD");
        var hostname = System.Environment.GetEnvironmentVariable("RDS_HOSTNAME");
        var port = System.Environment.GetEnvironmentVariable("RDS_PORT");

        return                     
            $"Data Source= {hostname},{port};" +
            $"Initial Catalog= {dwhName};" +
            $"User ID={username};" +
            $"Password = {password}";
    }
}