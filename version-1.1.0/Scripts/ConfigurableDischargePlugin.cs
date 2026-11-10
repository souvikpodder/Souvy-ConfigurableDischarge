using System.Security.Permissions;
using HarmonyLib;
using Timberborn.ModManagerScene;

#pragma warning disable CS0618
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
#pragma warning restore CS0618

namespace ConfigurableDischarge
{
    public class ConfigurableDischargePlugin : IModStarter
    {
        public const string ModId = "Souvy.ConfigurableDischarge";
        public const string HarmonyId = "com.souvy.configurabledischarge";

        public void StartMod(IModEnvironment modEnvironment)
        {
            new Harmony(HarmonyId).PatchAll();
            Settings.ConfigurableDischargeSettingsOwner.UpdateValues();
        }
    }
}
