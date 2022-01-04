// ----------------------------------------------------------------------------
// The MIT License
// UnityEditor integration https://github.com/Leopotam/ecslite-unityeditor
// for LeoECS Lite https://github.com/Leopotam/ecslite
// Copyright (c) 2021-2022 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using UnityEditor;
using UnityEngine;

namespace Leopotam.EcsLite.UnityEditor.Inspectors {
    sealed class BoundsInspector : EcsComponentInspectorTyped<Bounds> {
        public override bool OnGuiTyped (string label, ref Bounds value, EcsWorld world, int entityId) {
            var newValue = EditorGUILayout.BoundsField (label, value);
            if (newValue == value) { return false; }
            value = newValue;
            return true;
        }
    }
}