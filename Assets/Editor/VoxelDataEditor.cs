using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TerrainEngine;
using System.Threading;

[CustomEditor(typeof(VoxelData))]
public class VoxelDataEditor : Editor
{
    private float TotalHeight;
    private LayerMapGenerator _layerMapGenerator;
    private VoxelData _voxelData;
    private Thread generatorThread;
    //private VoxelData voxelDataRef;

    private void OnEnable()
    {
        if (_voxelData == null) 
        {
            generatorThread = new Thread(THREADINEDITOR);
            _voxelData = (VoxelData)target;
            if (_layerMapGenerator == null) _layerMapGenerator = _voxelData.GetComponent<LayerMapGenerator>();
        }
    }
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        if (GUILayout.Button("Generate Procedural Textures", GUILayout.Height(30)))
        {
            generatorThread.Start();
            //UnityEditor.EditorApplication.update += EditorGenerateProceduralTex;
        }
    }

    public void THREADINEDITOR()
    {
        _voxelData.layers.Clear(); 

        TotalHeight = _voxelData._height;
        
        for (int i = 0; i < TotalHeight; i++)
        {            
            _layerMapGenerator.GenerateProceduralTexture(i, TotalHeight);
            System.Threading.Thread.Sleep(100);
            _layerMapGenerator.GenerateTexture2D();
        }

        generatorThread.Abort();
    }

    public void EditorGenerateProceduralTex()
    {        
        
        UnityEditor.EditorApplication.update -= EditorGenerateProceduralTex;
    }

}
