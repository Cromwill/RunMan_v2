using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(MapElementPool))]
public class MapElementPoolEditor : Editor
{
    private SerializedProperty nonDestroyObj;
    private SerializedProperty destroyObjects;
    private SerializedProperty tiles;
    private SerializedProperty tilesCount;
    private SerializedProperty startTilesCount;

    public override void OnInspectorGUI()
    {
        MapElementPool poolTarget = target as MapElementPool;

        nonDestroyObj = serializedObject.FindProperty("NonDestroyObjects");
        destroyObjects = serializedObject.FindProperty("DestroyObjects");
        tiles = serializedObject.FindProperty("Tiles");
        tilesCount = serializedObject.FindProperty("TilesCount");
        startTilesCount = serializedObject.FindProperty("StartTilesCount");

        EditorGUILayout.PropertyField(nonDestroyObj);
        EditorGUILayout.PropertyField(destroyObjects);
        EditorGUILayout.PropertyField(tiles);
        EditorGUILayout.PropertyField(tilesCount);
        EditorGUILayout.PropertyField(startTilesCount);

        serializedObject.ApplyModifiedProperties();

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Generate")) poolTarget.StartGeneratePool();

        EditorGUILayout.EndHorizontal();

        if (GUI.changed)
        {
            EditorUtility.SetDirty(poolTarget);
            EditorSceneManager.MarkSceneDirty(poolTarget.gameObject.scene);
        }
    }
}
