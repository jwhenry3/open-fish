using System;
using UnityEngine;

namespace OpenFish.Plugins.Persistence
{
    public class PersistenceTransport : MonoBehaviour
    {
        public PersistenceManager Manager;
        
        public string Id;

        private void Awake()
        {
            Manager.TransportsById[Id] = this;
        }
        
        protected virtual void Reset()
        {
            if (!Manager)
                Manager = GetComponent<PersistenceManager>();
        }
        
        
        public bool Persist<T>(string collection, string entityId, T data)
        {
            return true;
        }

        public T Retrieve<T>(string collection, string entityId)
        {
            return default(T);
        }
    }
}