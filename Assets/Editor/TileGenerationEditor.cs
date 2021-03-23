using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TileGeneration))]
public class TileGenerationEditor : Editor
{
    protected virtual void OnSceneGUI()
    {
        var t = (TileGeneration)target;

        EditorGUI.BeginChangeCheck();

        t.EnemiesSpawnDot = Handles.PositionHandle(t.transform.position + t.EnemiesSpawnDot, Quaternion.identity) - t.transform.position;

        for (int i = 0; i < t.NotDestroyObjectPositions.Length; i++)
        {
            t.NotDestroyObjectPositions[i] = Handles.PositionHandle(t.transform.position + t.NotDestroyObjectPositions[i], Quaternion.identity) - t.transform.position;
        }

        for (int i = 0; i < t.DestroyPositions.Length; i++)
        {
            t.DestroyPositions[i] = Handles.PositionHandle(t.transform.position + t.DestroyPositions[i], Quaternion.identity) - t.transform.position;
        }

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(t, "Change points");
        }
    }
}
