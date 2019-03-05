# Kompair
Compare Umbraco instances and highlight document type or property editor differences.

## Introduction
Koben.Kompair is a plugin for comparing document types, property groups and properties as well as property editors of two separate umbraco instances. 

Users are able to easily compare sites through a Dashboard in the Backoffice.

## Installation

Koben.Kompair will need to be installed on each Umbraco instance you want to perform a compare on.

### Nuget
[![NuGet](https://buildstats.info/nuget/Koben.Kompair)](https://www.nuget.org/packages/Koben.Kompair/)

You can run the following command from within Visual Studio:

    PM> Install-Package Koben.Kompair

### Umbraco Package
https://our.umbraco.com/packages/backoffice-extensions/kompair/

### Manually
[![pipeline status](https://gitlab.com/EwneoN/Koben.Kompair/badges/master/pipeline.svg)](https://gitlab.com/EwneoN/Koben.Kompair/commits/master)
Download the code, compile it and copy the Kompair binary and the App_Plugins folder into your Umbraco website App_Plugin folder.

## Configuration
Once the plugin has been installed you will need to configure some AppSettings in the web config and potentially a json file if Api Key Authentication is used.

Please ensure that any Kompair configurations that are not used are commented out or removed from the web config.

In order to protect the internals of your Umbraco instances, Koben.Kompair uses configuration values to determine what authentication method to use and perform the authentication operations. 

Out of the box, Koben.Kompair will default to the highest Authentication mode which is Certificate base authentication.
Koben.Kompair also provides 2 AppSettings for users to use when working with custom routes or App_Plugins locations.

- Kompair.ApiKey.ClientsConfigPath
  - Only use this if you have a custom location for storing the clients.json file.
- Kompair.GetDocumentTypesForComparisonPath
  - Only use this if your site has custom routing for umbraco API endpoints.

#### No Authentication
No Authentication is exactly what it says, anyone and anything can call the Compare API endpoint. Using this mode in production is strongly discouraged.

In order to disable the Kompair API authentication you will need make the following AppSettings modifications:
- Kompair.AuthenticationMode
  - Update the value to "None" on all Umbraco instances you have installed Kompair on.

#### Api Key Authentication
Api Key is the 2nd highest level of security. A shared key is used to authenticate an API consumer.

In order to use Api Key authentication you will need make the following AppSettings modifications:
- Kompair.AuthenticationMode
  - Update the value to "Key" on all Umbraco instances you have installed Kompair on.
- Kompair.ApiKey.ClientId
  - Update the value to the client id this Umbraco instance will use when making a call to the Compare API endpoint on another Umbraco instance.
  - A matching ClientConfig item in the clients.json file on all Umbraco instances you want to allow this Umbraco instance to be able to make a call to their Compare endpoints. This is how a server knows what client secret to use for a client id.
- Kompair.ApiKey.ClientSecret
  - Update the value to the client secret this Umbraco instance will use when making a call to the Compare API endpoint on another Umbraco instance.
  - A matching ClientConfig item in the clients.json file on all Umbraco instances you want to allow this Umbraco instance to be able to make a call to their Compare endpoints. This is how a server knows what client secret to use for a client id.
- Kompair.ApiKey.ClientsConfigPath
  - Only use this if you have a custom location for storing the clients.json file. It should be a relative path inside the AppDomain base directory. Better support for other locations could be added by a contributer.

Clients will need to be registered on each Umbraco instance in order to be allowed to call the Compare API endpoint. This is done by:
- Adding ClientConfig records to clients.json. This file can be found under App_Plugins\Koben.Kompair\data\clients.json.
- Each record requires a ClientId. This is used when authenticating requests in order to locate the ClientSecret required to perform encryption.
- Each record requires a ClientSecret. This is used when authenticating requests when performing encryption for comparison with the request Authentication header value.
- Each client Umbraco instance will need to have their AppSettings updated with the matching ClientConfig record as per the instructions listed further above.

#### Certificate Authentication
Certificate Authentication is the highest level of security but the one that requires the most work to configure. This method also has not been thoroughly tested on different hosting environments. The certificate must be installed correctly on both client and server machines otherwise authentication will fail.

In order to use Certificate authentication you will need make the following AppSettings modifications:

- Kompair.Certificate.Thumbprint
  - Update the value with the thumbprint of the client certificate to use.
  - This is how the certificate is looked up by client and server.
- Kompair.Certificate.StoreName
  - This is how the certificate is looked up by client and server.
- Kompair.Certificate.StoreLocation
  - This is how the certificate is looked up by client and server.
- Kompair.Certificate.ValidOnly
  - This indicates whether or not to only use valid certificates when adding it to an HTTP request. Defaults to true.

## Known issues

## Planned Features
- Better styling so matching rows are aligned. 
  - This will require some change however due to the potential complexity when both sites have items with a MatchStatus == None.
  - We might need to group items into a table per Match Status to acheive this.
  - Will create a basic mock up for input.
- DI friendly controllers.

## Umbraco Versions
Kompair has been tested with Umbraco versions:
- 7.13.0
- 7.13.1
- 7.10.0
- 7.4.0

More testing on earlier versions is planned and contributers are more than welcome here :)
Testing on new releases will be performed as they come out.

## Changelog
### 0.1.18
- Added gitlab ci config so my gitlab mirror can handle deployments for me.
- Added packaging script so CI can create umbraco packages for me.
- Added base package.xml file
- README work.
- Upped assembly version to 0.1.18.
### 0.1.17
- Split the project into two Separate projects, Koben.Kompair and Koben.Kompair.Dashboard.
- Corrected targetted versions. Koben.Kompair targets Umbraco 7.0.0 whereas Koben.Kompair.Dashboard targets 7.4.0. We want our controllers to work for earliest as possible without changing code as there was a change from IActionResult to IHttpActionResult during early versions of 7.
- Added example projects so users can download code and spin it up to see what the go is.
- Added my own flex box styling utils so styling can be maintained in older versions before the built in flex utils were added.
- README work.
- Upped assembly version to 0.1.17.
### 0.1.16
- Reduced the version of the Umbraco.Core dependency from 7.10.0 to 7.0.0 to accommodate older versions without forcing update.
- README work.
- Upped assembly version to 0.1.16.
### 0.1.15
- Reduced the version of the Umbraco.Core dependency from 7.13.0 to 7.10.0 to accommodate older versions without forcing update.
- README work.
- Upped assembly version to 0.1.15.
### 0.1.14
- Added informational tooltips. These are simple title tooltips.
- README work.
- Upped assembly version to 0.1.14.
### 0.1.13
- Added scroll functionality so users can quickly navigate to a parents children.
- README work.
- Upped assembly version to 0.1.13.
### 0.1.12
- Removed web.config.uninstall.xdt from as it causes AppSettings to be replaced when upgrading versions.
- README work.
- Upped assembly version to 0.1.12.
### 0.1.11
- Styled error message.
- Added better styling to the compare button.
- README work.
- Upped assembly version to 0.1.11.
### 0.1.10
- Added web config to App_Plugins\data\ to stop clients.json file being served via browser.
- README work.
- Upped assembly version to 0.1.10.
### 0.1.9
- Nuspec work.
- README work.
- Upped assembly version to 0.1.9.
### 0.1.8
- Nuspec work.
- README work.
- Upped assembly version to 0.1.8.
### 0.1.7
- Nuspec work.
- README work.
- Upped assembly version to 0.1.7.
### 0.1.6
- Nuspec work.
- README work.
- Upped assembly version to 0.1.6.
### 0.1.5
- Added install and uninstall nuget transforms for dashboard.
- Updated web config transform into install and uninstall transforms.
- Clean up.
- README work.
- Upped assembly version to 0.1.5.
### 0.1.4
- Fixed nuspec issue with web config transform.
- README work.
- Upped assembly version to 0.1.4.
### 0.1.3
- Removed Json.NET dependency from NuSpec as this will come from the UmbracoCms.Core dependency.
- Removed UseImportMethod certificate config values as well as the logic.
- Clean up.
- README work.
- Upped assembly version to 0.1.3.
### 0.1.2
- Added API key authentication method.
- Modified code so the Auth mode is determined via a config value.
- Performed clean up of namespace as old namespace was still in code.
- Update nuspec with framework assembly dependency on System.Web as we are using Umbraco RuntimeCache for storing nonces.
- Added web.config.transform for nuget. This is so Kompair app settings are added to web configs.
- Upped assembly version to 0.1.2.


Handmade by Samuel Butler - 2019 @KobenDigital
