# UnityEditor integration for LeoEcsLite C# Entity Component System framework
Unity editor integration for [LeoECS Lite](https://github.com/Leopotam/ecslite).

> Tested on unity 2020.3 (dependent on it) and contains assembly definition for compiling to separate assembly file for performance reason.

> **Important!** Don't forget that this module developed to work only inside unity editor and under `UNITY_EDITOR` definition.

# Table of content
* [Socials](#socials)
* [Installation](#installation)
    * [As unity module](#as-unity-module)
    * [As source](#as-source)
* [Integration](#integration)
    * [From code](#from-code)
    * [From UI](#from-ui)
* [License](#license)

# Socials
[![discord](https://img.shields.io/discord/404358247621853185.svg?label=enter%20to%20discord%20server&style=for-the-badge&logo=discord)](https://discord.gg/5GZVde6)

# Installation

## As unity module
This repository can be installed as unity module directly from git url. In this way new line should be added to `Packages/manifest.json`:
```
"com.leopotam.ecslite.unityeditor": "https://github.com/Leopotam/ecslite-unityeditor.git",
```
By default last released version will be used. If you need trunk / developing version then `develop` name of branch should be added after hash:
```
"com.leopotam.ecslite.unityeditor": "https://github.com/Leopotam/ecslite-unityeditor.git#develop",
```

## As source
If you can't / don't want to use unity modules, code can be cloned or downloaded as archive from `releases` page.

# Integration

## From code
```csharp
// ecs-startup code:
void Start () {        
    _world = new EcsWorld ();
    _systems = new EcsSystems (_world);
#if UNITY_EDITOR
    Leopotam.EcsLite.UnityEditor.EcsWorldObserver.Create (_world);
#endif
    _systems
        .Add (new TestSystem1 ())
        .Init ();
}
```

## From UI
Some code can be generated automatically from unity editor main menu "Assets / Create / LeoECS Lite". 

# License
The software is released under the terms of the [MIT license](./LICENSE.md).

No personal support or any guarantees.
