using Bindito.Core;

namespace ConfigurableDischarge.Settings
{
    [Context("MainMenu")]
    [Context("Game")]
    public class ConfigurableDischargeSettingConfigurator : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
            containerDefinition.Bind<ConfigurableDischargeSettingsOwner>().AsSingleton();
        }
    }
}
