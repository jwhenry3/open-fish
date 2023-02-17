## OpenFish

The purpose behind this project is to bring an ECS pattern to Unity without the risks of implementing DOTS
and having compatibility issues. This will be considered an Open Source project for anyone to contribute to
as demands increase.
### Required Packages
- [FishNet](https://github.com/FirstGearGames/FishNet)
- [Tri Inspector](https://github.com/codewriter-packages/Tri-Inspector)

### Basic Usage

- SingletonSpawner
    - Provides a quick way to spawn a list of singletons on the server
- PluginManager
    - Register the list of plugins to use for your game, most plugins rely on the Entity plugin, so make sure that is in the list
- Plugin
    - This will allow you to register your singleton systems with the PluginManager in a standardized way

### Entity System
![Bare Entity Prefab](https://raw.githubusercontent.com/jwhenry3/open-fish/master/Examples/Screenshots/Entity.PNG)
![Bare Entity Config](https://raw.githubusercontent.com/jwhenry3/open-fish/master/Examples/Screenshots/EntityConfig.PNG)
- Any objects that want to leverage the entity system must have the Entity component
- Each entity can have either a Type config or an ID config, which is applied in that order
    - Type configs are applied first, and ID configs are applied after for overrides
- EntityConfig is used to specify the required systems before the OnEntityReady fires
  - This event provides a safe way to interact with other systems on an entity once all are available that are required
- There are 2 ways to create your entities
  - Provide a prefab with all the necessary systems attached to it
  - Provide a Type and/or Id so the EntityConfigs can be loaded and all required systems are added at runtime
    - This method will cause slight overhead for each system that has to be added at runtime since they are GameObjects and not simple components


### System Pattern
- Plugin: A Prefab with a Plugin component and Manager component should exist and registered with the PluginManager
  - Plugins will be loaded as global network objects and singletons to allow for server-wide access to anything needed
  - Plugins can have dependencies which reference other plugins and will make sure that all dependencies are met before doing anything
- Manager: Each Plugin has a Manager script which controls how systems are registered with entities
  - Either a component will be located on the entity or a GameObject will be instantiated from a prefab for the system
- System: A system component is what is tied directly to a specific entity in order to control that entity's parameters and behavior
    - Multiple systems can exist on an entity for the same Plugin, and should often be grouped on the same prefab for optimization
    - You can either leave it up to the Manager to load the configs for the entity or provide them manually in the entity prefab
### Preconfigured Entities
![Preconfigured Entity](https://raw.githubusercontent.com/jwhenry3/open-fish/master/Examples/Screenshots/PreconfiguredEntity.PNG)
- Entities that have their systems added to the Entity object do not need an EntityConfig to define their behavior
  - OnEntityReady will fire once the first system is loaded if no config is provided, so you may want to double check the state of the entity if doing things this way
- You can even add systems that are not required to the Entity object and so long as the Plugin is loaded, it can enhance the Entity
- This method also has a performance benefit over dynamically adding systems since it does not require a Network Object per system
- I prefer using this method when creating player based entities or scene entities that are less dynamic than mobs that can have varying behaviors

![Player Entity](https://raw.githubusercontent.com/jwhenry3/open-fish/master/Examples/Screenshots/Player.PNG)
### Troubleshooting Tips
- If your entity is not spawning the physical object or your systems are showing up as disabled
  - This is a sign that not all systems were loaded that were required and OnEntityReady did not fire
  - Verify that the Plugin is loaded in PluginManager, the system is not failing in its AddSystem step on the Entity class