using System.Collections.Generic;
using FishNet.Object;
using JetBrains.Annotations;

namespace OpenFish.Core
{
    public class PluginManager : NetworkBehaviour
    {
        public List<Plugin> PluginList;
        public readonly Dictionary<string, Plugin> Plugins = new();

        public override void OnStartNetwork()
        {
            base.OnStartNetwork();
            base.NetworkManager.RegisterInstance(this);
        }

        public override void OnStartServer()
        {
            base.OnStartServer();
            
            foreach (Plugin plugin in PluginList)
            {
                if (Plugins.ContainsKey(plugin.Name)) continue;
                NetworkObject nob = NetworkManager.GetPooledInstantiated(plugin.gameObject, true);
                ServerManager.Spawn(nob);
                Plugins.Add(plugin.Name, nob.GetComponent<Plugin>());
            }

            foreach (var kvp in Plugins)
                kvp.Value.enabled = IsPluginReady(kvp.Key);
        }

        private bool AreDependenciesRegistered(string pluginName, [CanBeNull] List<string> iterated = null)
        {
            if (!Plugins.ContainsKey(pluginName))
                return false;
            Plugin plugin = Plugins[pluginName];
            iterated = iterated ?? new();
            iterated.Add(pluginName);
            foreach (string dep in plugin.Dependencies)
            {
                if (iterated.Contains(dep)) // skip dependencies already checked
                    continue;
                if (!Plugins.ContainsKey(dep))
                    return false;
                if (!AreDependenciesRegistered(dep, iterated))
                    return false;
            }

            return true;
        }

        public bool IsPluginReady(string pluginName)
        {
            return AreDependenciesRegistered(pluginName);
        }
    }
}