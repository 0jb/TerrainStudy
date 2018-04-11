using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TerrainEngine;

[CustomEditor(typeof(VoxelData))]
public class VoxelDataEditor : Editor
{
    private float TotalHeight;
    private LayerMapGenerator _layerMapGenerator;
    private VoxelData _voxelData;
    //private VoxelData voxelDataRef;

    private void OnEnable()
    {
        if (_voxelData == null) 
        {
            _voxelData = (VoxelData)target;
            if (_layerMapGenerator == null) _layerMapGenerator = _voxelData.GetComponent<LayerMapGenerator>();
        }
    }
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        if (GUILayout.Button("Generate Procedural Textures", GUILayout.Height(30)))
        {
            UnityEditor.EditorApplication.update += EditorGenerateProceduralTex;
        }
    }

    public void EditorGenerateProceduralTex()
    {        
        _voxelData.layers.Clear(); 

        TotalHeight = _voxelData._height;
        
        for (int i = 0; i < TotalHeight; i++)
        {            
            _layerMapGenerator.GenerateProceduralTexture(i, TotalHeight);
            _layerMapGenerator.GenerateTexture2D();
        }
        UnityEditor.EditorApplication.update -= EditorGenerateProceduralTex;
    }

}
