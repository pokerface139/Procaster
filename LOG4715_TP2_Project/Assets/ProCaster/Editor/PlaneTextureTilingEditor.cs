using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlaneTextureTiling))]
public class PlaneTextureTilingEditor : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        PlaneTextureTiling script = (PlaneTextureTiling)target;
        if (GUILayout.Button("Apply"))
        {
            script.UpdateTiling();
        }
    }

}
