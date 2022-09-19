using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Oxide.Core;
namespace Carbon.Plugins
{
    [Info("CarbonEmptyLowFPS", "JonDPugh", "1.0.0")]
    [Description("Lower the server FPS and save CPU usage when its empty")]
    public class CarbonEmptyLowFPS : CarbonPlugin
    {
        private Configuration config;
        public class Configuration
        {
            [JsonProperty(PropertyName = "FPS limit when empty")]
            public int FPSE { get; set; } = 10;
            [JsonProperty(PropertyName = "FPS limit when not empty")]
            public int FPS { get; set; } = 250;
            [JsonProperty(PropertyName = "Log changes to rcon")]
            public bool LogChanges { get; set; } = true;
        }
        protected override void LoadConfig()
        {
            base.LoadConfig();
            try
            {
                config = Config.ReadObject<Configuration>();
                if (config == null)
                    throw new JsonException();
            }
            catch
            {
                string configPath = $"{Interface.Oxide.ConfigDirectory}{Path.DirectorySeparatorChar}{Name}";
                PrintError($"Could not load a valid configuration file, creating a new configuration file at {configPath}.json");
                Config.WriteObject(config, false, $"{configPath}_invalid.json");
                LoadDefaultConfig();
            }
            SaveConfig();
        }
        protected override void LoadDefaultConfig() => config = new Configuration();
        protected override void SaveConfig() => Config.WriteObject(config);
        private void OnServerInitialized() => Playercheck();
        private void OnPlayerConnected(BasePlayer player) => Playercheck();
        private void OnPlayerDisconnected(BasePlayer player, string reason) => Playercheck();
        private void Unload() => Playercheck();
        private void Playercheck()
        {
            if (BasePlayer.activePlayerList.Count() == 0)
            {
                if (config.LogChanges)
                    Puts($"Server is empty, setting FPS limit to " + config.FPSE);
                UnityEngine.Application.targetFrameRate = config.FPSE;
            }
            else
            {
                if (config.LogChanges)
                    Puts($"Server is no longer empty, setting FPS limit to " + config.FPS);
                UnityEngine.Application.targetFrameRate = config.FPS;
            }
        }
    }
}