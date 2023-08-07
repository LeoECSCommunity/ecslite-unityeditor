// ----------------------------------------------------------------------------
// The MIT License
// UnityEditor integration https://github.com/Leopotam/ecslite-unityeditor
// for LeoECS Lite https://github.com/Leopotam/ecslite
// Copyright (c) 2021-2022 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using UnityEditor;
using UnityEngine;

namespace Leopotam.EcsLite.UnityEditor.Inspectors
{
	sealed class EcsPackedEntityArrayInspector : EcsComponentInspectorTyped<EcsPackedEntity[]>
	{
		public override bool IsNullAllowed()
		{
			return true;
		}

		public override bool OnGuiTyped(string label, ref EcsPackedEntity[] value, EcsEntityDebugView entityView)
		{
			if (value == null)
			{
				GUILayout.Label($"{label} array is null");
				return true;
			}
			GUILayout.Label($"{label} has {value.Length} entitys:");
			var entity = new EcsPackedEntityInspector();
			for (int i = 0; i < value.Length; i++)
			{
				if (value[i].Unpack(entityView.World, out var unpackedEntity))
				{
					if (GUILayout.Button("Ping entity"))
					{
						EditorGUIUtility.PingObject(entityView.DebugSystem.GetEntityView(unpackedEntity));
					}
				}
				else
				{
					if (value[i].EqualsTo(default))
					{
						EditorGUILayout.SelectableLabel("<Empty entity>", GUILayout.MaxHeight(EditorGUIUtility.singleLineHeight));
					}
					else
					{
						EditorGUILayout.SelectableLabel("<Invalid entity>", GUILayout.MaxHeight(EditorGUIUtility.singleLineHeight));
					}
				}
			}
			EditorGUILayout.Space();
			return true;
		}
	}
}