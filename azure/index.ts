import * as pulumi from "@pulumi/pulumi";
import * as azure from "@pulumi/azure";

function stackNamed(name: string): string {
  return `${name}-${pulumi.getStack()}`;
}

// Create an Azure Resource Group
const resourceGroup = new azure.core.ResourceGroup(stackNamed('parkingLot'));

// Create Azure SQL database
const sqlUsername = process.env.SQL_USERNAME || 'sa';
const sqlPassword = process.env.SQL_PASSWORD || 'My-Secret-123';

const sqlServer = new azure.sql.SqlServer(stackNamed('ParkingLotServer'), {
  resourceGroupName: resourceGroup.name,
  administratorLogin: sqlUsername,
  administratorLoginPassword: sqlPassword,
  version: '12.0'
});

const firewallRule = new azure.sql.FirewallRule(stackNamed('AllowAllIPs'), {
  resourceGroupName: resourceGroup.name,
  serverName: sqlServer.name,
  startIpAddress: '0.0.0.0',
  endIpAddress: '0.0.0.0'
});

const database = new azure.sql.Database(stackNamed('ParkingLotDb'), {
  resourceGroupName: resourceGroup.name,
  serverName: sqlServer.name,
  requestedServiceObjectiveName: 'S0'
});

const connectionString = `Server=tcp:${sqlServer.name}.database.windows.net,1433;Database=${database.name};User ID=${sqlUsername};Password=${sqlPassword};Encrypt=true;Connection Timeout=30;`

// Create app service
const appServicePlan = new azure.appservice.Plan(stackNamed('ParkingLotPlan'), {
  resourceGroupName: resourceGroup.name,
  sku: {
    tier: 'Free',
    size: 'F1'
  }
});

const appService = new azure.appservice.AppService(stackNamed('ParkingAPI'), {
  appServicePlanId: appServicePlan.id,
  resourceGroupName: resourceGroup.name,

  connectionStrings: [
    {
      name: "ParkingLotDb",
      value: connectionString,
      type: 'SQLAzure'
    }
  ]
});

export const endpoint = pulumi.interpolate`https://${appService.defaultSiteHostname}`;
