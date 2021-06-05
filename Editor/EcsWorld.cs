// ----------------------------------------------------------------------------
// The MIT License
// UnityEditor integration https://github.com/Leopotam/ecslite-unityeditor
// for LeoECS Lite https://github.com/Leopotam/ecslite
// Copyright (c) 2021 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using UnityEditor;
using UnityEngine;

namespace Leopotam.EcsLite.UnityEditor {
    [CustomEditor (typeof (EcsWorldObserver))]
    sealed class EcsWorldObserverInspector : Editor {
        public override void OnInspectorGUI () {
            // var observer = (Runtime.EcsWorldObserver) target;
            // var stats = observer.GetStats ();
            var guiEnabled = GUI.enabled;
            GUI.enabled = true;
            GUILayout.BeginVertical (GUI.skin.box);
            // EditorGUILayout.LabelField ("Components", stats.Components.ToString ());
            // EditorGUILayout.LabelField ("Filters", stats.Filters.ToString ());
            // EditorGUILayout.LabelField ("Active entities", stats.ActiveEntities.ToString ());
            // EditorGUILayout.LabelField ("Reserved entities", stats.ReservedEntities.ToString ());
            GUILayout.EndVertical ();
            GUI.enabled = guiEnabled;
        }
    }
}