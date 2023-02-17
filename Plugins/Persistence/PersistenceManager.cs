using System;
using System.Collections.Generic;
using OpenFish.Plugins.Entities;

namespace OpenFish.Plugins.Persistence
{
    public class PersistenceManager : EntitySystemManager<PersistenceSystem>
    {
        public readonly Dictionary<string, PersistenceTransport> TransportsById = new();
        public string CurrentTransport;
    }
}