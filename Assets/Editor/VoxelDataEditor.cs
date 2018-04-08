using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TerrainEngine;

[CustomEditor(typeof(VoxelData))]
public class VoxelDataEditor : Editor
{

    private LayerMapGenerator layerMapGeneratorRef;
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        VoxelData _voxelData = (VoxelData)target;

        if (GUILayout.Button("Generate Procedural Textures", GUILayout.Height(30)))
        {
            UnityEditor.EditorApplication.update += EditorGenerateProceduralTex;
        }
    }

    public void EditorGenerateProceduralTex(float TotalHeight)
    {
        for (int i = 0; i < TotalHeight; i++)
        {
            layerMapGeneratorRef.GenerateProceduralTexture(i, TotalHeight);
            layerMapGeneratorRef.GenerateTexture2D();
        }
        UnityEditor.EditorApplication.update -= EditorGenerateProceduralTex(float);
    }

}
