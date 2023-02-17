using System.Linq;
using OpenFish.Plugins.Entities;

namespace OpenFish.Plugins.Persistence
{
    public class PersistenceSystem : EntitySystem
    {
        public PersistenceManager Manager => NetworkManager.GetInstance<PersistenceManager>();
        
        public bool Persist<T>(string collection, string entityId, T data)
        {
            if (!Manager.TransportsById.ContainsKey(Manager.CurrentTransport)) return false;
            var transport = Manager.TransportsById[Manager.CurrentTransport];

            return transport.Persist(collection, entityId, data);
        }

        public T Retrieve<T>(string collection, string entityId)
        {
            if (!Manager.TransportsById.ContainsKey(Manager.CurrentTransport)) return default(T);
            var transport = Manager.TransportsById[Manager.CurrentTransport];
            return transport.Retrieve<T>(collection, entityId);
        }
    }
}