using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TerrainEngine;

[CustomEditor(typeof(VoxelData))]
public class VoxelDataEditor : Editor {

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        VoxelData _voxelData = (VoxelData)target;

        if(GUILayout.Button("Generate Procedural Textures", GUILayout.Height(30)))
        {
            UnityEditor.EditorApplication.update += _voxelData.FeedTextureBuffer;
        }

        
            

    }

}
