> use Microsoft.Extensions.Configuration - this code existed to provide a backwards maintainable simulation for full framrwork usage

# ConMan

Load configs from App/Web.config (AppSettings), Environment Variables and a dedicated named json file 
living in %APPDATA%\Development\Settings\{ConfigName}.json

## Usage

**ConMan** can be used eather from as a static set of apis living at `ConMan.Settings.GetSetting(...)` 
or can be configured in DI by instanciating a singleton of `SettingsManager` and passing it around.

### Using `Settings`
#### Configure providers 
```c#
Settings.Configure(s=>{
    //settings are resolved from providers in the order they are registerd
    s.AddDevelopmentSettings("YourSharedProviderName") //setttigns in here
     .AddEnvironmentVariables() // will beat env vars
     .AddFromConfig(); // app.config will be last
});
```

#### Reading Settings
```c#
var value = Settings.GetSetting("Setting.Path.Here");
```


### Using `SettingsManager`
#### Configure providers 
```c#
var manager = new SettingsManager();

//settings are resolved from providers in the order they are registerd
manager.AddDevelopmentSettings("YourSharedProviderName") //setttigns in here
    .AddEnvironmentVariables() // will beat env vars
    .AddFromConfig(); // app.config will be last
```

#### Reading Settings
```c#
// manager instance from previous step 
var value = manager.GetSetting("Setting.Path.Here");
```

### Providers

#### Development Setting Provider

Development settings are stored in a JSON file in a well know location per developer outside of source control, 
this can be used to store secrets that must stay out of source control (passwords etcs).

This file can be located at `%APPDATA%\Development\Settings\{ConfigName}.json`  with `{ConfigName}` replace with 
the value passed into `.AddDevelopmentSettings(...)`.

The path of the setting is converted into reading values from a JSON object structure so `Path.To.Variable` would
expect a JSON file with a minimal structure of :
```JSON
{
    'Path' : {
        'To' : {
            'Variable' : 'value'
        }
    }
}

```


#### Environment Variables Provider

Settings are read out of the Environment Variables availible to the current process.

The path of the settings are converted into a normalized formate for Environment variables such that dots are 
replaces with underscores and the phrase is converted to uppercase. i.e. `Path.To.Variable` becomes `PATH_TO_VARIABLE`.


#### Config Provider

The config provider reads values fro the underlying .net `ConfigurationManager.AppSettings[...]`, such that it will read
from app.config and web.config appSettings.

The path the the settings into a formate more normally used in appSettings such that dots are replaces with colons.
i.e. `Path.To.Variable` will read from app settings `<add key="Path:To:Variable" value="value" />`.    
