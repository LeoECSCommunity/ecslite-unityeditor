// ----------------------------------------------------------------------------
// The MIT License
// Unity integration https://github.com/Leopotam/ecs-unityintegration
// for ECS framework https://github.com/Leopotam/ecs
// Copyright (c) 2017-2021 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEngine;

// ReSharper disable UnusedMethodReturnValue.Global
// ReSharper disable InconsistentNaming

namespace Leopotam.EcsLite.UnityEditor {
    public static class EditorHelpers {
        public static string GetCleanGenericTypeName (Type type) {
            if (!type.IsGenericType) {
                return type.Name;
            }
            var constraints = "";
            foreach (var constraint in type.GetGenericArguments ()) {
                constraints += constraints.Length > 0 ? $", {GetCleanGenericTypeName (constraint)}" : constraint.Name;
            }
            return $"{type.Name.Substring (0, type.Name.LastIndexOf ("`", StringComparison.Ordinal))}<{constraints}>";
        }
    }

    public sealed class EcsEntityObserver : MonoBehaviour {
        public EcsWorld World;
        public int Entity;
    }

    // public sealed class EcsSystemsObserver : MonoBehaviour, IEcsSystemsDebugListener {
    //     EcsSystems _systems;
    //
    //     public static GameObject Create (EcsSystems systems) {
    //         if (systems == null) { throw new ArgumentNullException (nameof (systems)); }
    //         var go = new GameObject (systems.Name != null ? $"[ECS-SYSTEMS {systems.Name}]" : "[ECS-SYSTEMS]");
    //         DontDestroyOnLoad (go);
    //         go.hideFlags = HideFlags.NotEditable;
    //         var observer = go.AddComponent<EcsSystemsObserver> ();
    //         observer._systems = systems;
    //         systems.AddDebugListener (observer);
    //         return go;
    //     }
    //
    //     public EcsSystems GetSystems () {
    //         return _systems;
    //     }
    //
    //     void OnDestroy () {
    //         if (_systems != null) {
    //             _systems.RemoveDebugListener (this);
    //             _systems = null;
    //         }
    //     }
    //
    //     void IEcsSystemsDebugListener.OnSystemsDestroyed (EcsSystems systems) {
    //         // for immediate unregistering this MonoBehaviour from ECS.
    //         OnDestroy ();
    //         // for delayed destroying GameObject.
    //         Destroy (gameObject);
    //     }
    // }

    public sealed class EcsWorldObserver : MonoBehaviour, IEcsWorldEventListener {
        EcsWorld _world;
        public readonly Dictionary<int, GameObject> EntityGameObjects = new Dictionary<int, GameObject> (1024);
        static Type[] _componentTypesCache = new Type[32];

        Transform _entitiesRoot;
        // Transform _filtersRoot;
        
        public static GameObject Create (EcsWorld world, string name = null) {
            if (world == null) { throw new ArgumentNullException (nameof (world)); }
            var go = new GameObject (name != null ? $"[ECS-WORLD {name}]" : "[ECS-WORLD]");
            DontDestroyOnLoad (go);
            go.hideFlags = HideFlags.NotEditable;
            var observer = go.AddComponent<EcsWorldObserver> ();
            observer._world = world;
            var worldTr = observer.transform;
            // entities root.
            observer._entitiesRoot = new GameObject ("Entities").transform;
            observer._entitiesRoot.gameObject.hideFlags = HideFlags.NotEditable;
            observer._entitiesRoot.SetParent (worldTr, false);
            // filters root.
            // observer._filtersRoot = new GameObject ("Filters").transform;
            // observer._filtersRoot.gameObject.hideFlags = HideFlags.NotEditable;
            // observer._filtersRoot.SetParent (worldTr, false);
            // subscription to events. 
            world.AddEventListener (observer);
            return go;
        }

        // public EcsWorldStats GetStats () {
        //     return _world.GetStats ();
        // }

        public void OnEntityCreated (int entity) {
            if (!EntityGameObjects.TryGetValue (entity, out var go)) {
                go = new GameObject ();
                go.transform.SetParent (_entitiesRoot, false);
                go.hideFlags = HideFlags.NotEditable;
                var unityEntity = go.AddComponent<EcsEntityObserver> ();
                unityEntity.World = _world;
                unityEntity.Entity = entity;
                EntityGameObjects[entity] = go;
                UpdateEntityName (entity, false);
            } else {
                // need to update cached entity generation.
                go.GetComponent<EcsEntityObserver> ().Entity = entity;
            }
            go.SetActive (true);
        }

        public void OnEntityChanged (int entity) {
            UpdateEntityName (entity, true);
        }

        public void OnEntityDestroyed (int entity) {
            if (!EntityGameObjects.TryGetValue (entity, out var go)) {
                throw new Exception ("Unity visualization not exists, looks like a bug");
            }
            UpdateEntityName (entity, false);
            go.SetActive (false);
        }

        public void OnFilterCreated (EcsFilter filter) {
            // var go = new GameObject ();
            // go.transform.SetParent (_filtersRoot);
            // go.hideFlags = HideFlags.NotEditable;
            // var observer = go.AddComponent<EcsFilterObserver> ();
            // observer.World = this;
            // observer.Filter = filter;
            //
            // // included components.
            // var goName = $"Inc<{filter.IncludedTypes[0].Name}";
            // for (var i = 1; i < filter.IncludedTypes.Length; i++) {
            //     goName += $",{filter.IncludedTypes[i].Name}";
            // }
            // goName += ">";
            // // excluded components.
            // if (filter.ExcludedTypes != null) {
            //     goName += $".Exc<{filter.ExcludedTypes[0].Name}";
            //     for (var i = 1; i < filter.ExcludedTypes.Length; i++) {
            //         goName += $",{filter.ExcludedTypes[i].Name}";
            //     }
            //     goName += ">";
            // }
            // go.name = goName;
        }

        public void OnWorldResized (int newSize) { }

        public void OnWorldDestroyed (EcsWorld world) {
            // for immediate unregistering this MonoBehaviour from ECS.
            OnDestroy ();
            // for delayed destroying GameObject.
            Destroy (gameObject);
        }

        void UpdateEntityName (int entity, bool requestComponents) {
            var entityName = entity.ToString ("X8");
            if (_world.GetEntityGen (entity) > 0 && requestComponents) {
                var count = _world.GetComponentTypes (entity, ref _componentTypesCache);
                for (var i = 0; i < count; i++) {
                    entityName = $"{entityName}:{EditorHelpers.GetCleanGenericTypeName (_componentTypesCache[i])}";
                    _componentTypesCache[i] = null;
                }
            }
            EntityGameObjects[entity].name = entityName;
        }

        void OnDestroy () {
            if (_world != null) {
                _world.RemoveEventListener (this);
                _world = null;
            }
        }
    }

    // public sealed class EcsFilterObserver : MonoBehaviour {
    //     public EcsWorldObserver World;
    //     public EcsFilter Filter;
    // }
}
#endif