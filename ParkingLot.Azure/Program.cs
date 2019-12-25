using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Pulumi;
using Pulumi.Azure.AppService;
using Pulumi.Azure.AppService.Inputs;
using Pulumi.Azure.Core;
using Pulumi.Azure.Sql;

namespace ParkingLot.Azure
{
    class Program
    {
#pragma warning disable CS8618
        private static string _sqlUsername;
        private static string _sqlPassword;

        private static Task<int> Main()
        {
            return Deployment.RunAsync(() =>
            {
                _sqlUsername = Environment.GetEnvironmentVariable("SQL_USERNAME") ?? "sa";
                _sqlPassword = Environment.GetEnvironmentVariable("SQL_PASSWORD") ?? "My-Secret-123";

                // Create an Azure Resource Group
                var resourceGroup = new ResourceGroup(StackNamed("parkingLot"));

                // Create an Azure SQL Server
                var connectionString = ConfigureSqlServer(resourceGroup);
                
                // Create App Service
                var endpoint = ConfigureAppService(resourceGroup, connectionString);

                // Export the connection string for the storage account
                return new Dictionary<string, object?>
                {
                    { "connectionString", connectionString },
                    { "endpoint", endpoint }
                };
            });
        }

        private static string StackNamed(string resourceName) => $"{Deployment.Instance.StackName}-{resourceName}";

        private static string ConfigureSqlServer(ResourceGroup resourceGroup)
        {
            var server = new SqlServer(StackNamed("ParkingLotServer"), new SqlServerArgs
            {
                ResourceGroupName = resourceGroup.Name,
                AdministratorLogin = _sqlUsername,
                AdministratorLoginPassword = _sqlPassword,
                Version = "12.0"
            });

            var firewallRule = new FirewallRule(StackNamed("AllowAllIPs"), new FirewallRuleArgs
            {
                ResourceGroupName = resourceGroup.Name,
                ServerName = server.Name,
                StartIpAddress = "0.0.0.0",
                EndIpAddress = "0.0.0.0"
            });

            var database = new Database(StackNamed("ParkingLotDb"), new DatabaseArgs
            {
                ResourceGroupName = resourceGroup.Name,
                ServerName = server.Name,
                RequestedServiceObjectiveName = "S0"
            });

            return $"Server=tcp:${server.Name}.database.windows.net,1433;Database=${database.Name};User ID=${_sqlUsername};Password=${_sqlPassword};Encrypt=true;Connection Timeout=30;";
        }

        private static Output<string> ConfigureAppService(ResourceGroup resourceGroup, string connectionString)
        {
            var appServicePlan = new Plan(StackNamed("ParkingLotPlan"), new PlanArgs
            {
                ResourceGroupName = resourceGroup.Name,
                Sku = new PlanSkuArgs
                {
                    Tier = "Free",
                    Size = "F1"
                }
            });
            
            var appService = new AppService(StackNamed("ParkingAPI"), new AppServiceArgs
            {
                AppServicePlanId = appServicePlan.Id,
                ResourceGroupName = resourceGroup.Name,
                ConnectionStrings = new []
                {
                    new AppServiceConnectionStringsArgs
                    {
                        Name = "ParkingLotDb",
                        Value = connectionString,
                        Type = "SQLAzure"
                    }
                }
            });

            return Output.Format($"https://{appService.DefaultSiteHostname}");
        }
    }
}
