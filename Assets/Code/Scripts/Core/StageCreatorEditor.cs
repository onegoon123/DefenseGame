using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(StageCreator))]
public class StageCreatorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        StageCreator creator = (StageCreator)target;
        if (GUILayout.Button("Create Stage"))
        {
            creator.CreateStage();
        }

    }
}
