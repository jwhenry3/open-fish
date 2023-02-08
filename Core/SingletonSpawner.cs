using System;
using System.Collections.Generic;
using FishNet;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Transporting;
using UnityEngine;

namespace OpenFish.Core
{
    public class SingletonSpawner : MonoBehaviour
    {
        public List<NetworkObject> Singletons = new();

        private NetworkManager _networkManager;

        private void Awake()
        {
            _networkManager = InstanceFinder.NetworkManager;
            _networkManager.ServerManager.OnServerConnectionState += OnServerConnection;
        }

        void OnServerConnection(ServerConnectionStateArgs args)
        {
            if (args.ConnectionState != LocalConnectionState.Started) return;
            foreach (NetworkObject singleton in Singletons)
            {
                var instance = _networkManager.GetPooledInstantiated(singleton, true);
                _networkManager.ServerManager.Spawn(instance);
            }
        }
    }
}