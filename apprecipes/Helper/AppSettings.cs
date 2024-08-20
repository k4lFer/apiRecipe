using apprecipes.DataTransferObject.OtherObject;

namespace apprecipes.Helper
{
    public class AppSettings
    {
        public static DtoAppSettings dtoAppSettings;

        public static void Init()
        {
            dtoAppSettings = new DtoAppSettings();

            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Development.json");
            IConfigurationRoot configuration = builder.Build();

            dtoAppSettings.ConnetionStringMariaDB = configuration["ConnectionStrings:ConnetionStringMariaDb"];
            dtoAppSettings.OriginAudience = configuration["Authentication:Jwt:Audience"];
            dtoAppSettings.OriginIssuer = configuration["Authentication:Jwt:Issuer"];
            dtoAppSettings.AccessJwtSecret = configuration["Authentication:Jwt:AccessTokenSecret"];
            dtoAppSettings.RefreshJwtSecret = configuration["Authentication:Jwt:RefreshTokenSecret"];
            dtoAppSettings.OriginRequest = configuration["Cors:OriginRequest"];
        }

        public static string GetConnectionStringMariaDb()
        {
            return dtoAppSettings.ConnetionStringMariaDB;
        }

        public static string GetOriginIssuer()
        {
            return dtoAppSettings.OriginIssuer;
        }
        
        public static string GetOriginAudience()
        {
            return dtoAppSettings.OriginAudience;
        }

        public static string GetAccessJwtSecret()
        {
            return dtoAppSettings.AccessJwtSecret;
        }

        public static string GetRefreshJwtSecret()
        {
            return dtoAppSettings.RefreshJwtSecret;
        }
        
        public static string GetOriginRequest()
        {
            return dtoAppSettings.OriginRequest;
        }
    }
}
