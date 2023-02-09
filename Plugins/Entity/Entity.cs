using System;
using System.Collections.Generic;
using FishNet.Object;
using FishNet.Object.Synchronizing;

namespace OpenFish.Plugins.Entity
{
    public class EntityDataChange
    {
        private object previous;
        public event Action<object, object> OnChange;

        public void Trigger(object value)
        {
            OnChange?.Invoke(previous, value);
            previous = value;
        }
    }

    public class Entity : NetworkBehaviour
    {
        public readonly Dictionary<string, EntityDataChange> Changes = new();
        [SyncObject]
        public readonly SyncDictionary<string, object> Data = new();

        public override void OnStartNetwork()
        {
            base.OnStartNetwork(); 
            Data.OnChange += OnDataChange;
        }

        private void OnDataChange(SyncDictionaryOperation op, string key, object next, bool asServer)
        {
            CreateChangeHandle(key);
            Changes[key].Trigger(next);
        }
        
        public T GetData<T>(string pluginName, string property)
        {
            CreateChangeHandle(pluginName + '.' + property);
            
            if (!Data.ContainsKey(pluginName + '.' + property)) return default(T);
            
            return Data[pluginName + '.' + property] is T data ? data : default(T);
        }

        public void SetData(string pluginName, string property, object value)
        {
            if (!IsServer) return;
            CreateChangeHandle(pluginName + '.' + property);
            Data[pluginName + '.' + property] = value;
        }

        public void Subscribe(string pluginName, string property, Action<object, object> callback)
        {
            CreateChangeHandle(pluginName + '.' + property);
            Changes[pluginName + '.' + property].OnChange += callback;
        }

        public void Unsubscribe(string pluginName, string property, Action<object, object> callback)
        {
            CreateChangeHandle(pluginName + '.' + property);
            Changes[pluginName + '.' + property].OnChange -= callback;
        }

        private void CreateChangeHandle(string key)
        {
            if (!Changes.ContainsKey(key))
                Changes[key] = new EntityDataChange();
        }
    }
}