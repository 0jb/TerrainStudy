using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TerrainEngine;

[CustomEditor(typeof(Generator))]
public class GeneratorEditor : Editor {

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Generator _generator = (Generator)target;

        if(GUILayout.Button("Generate Voxels", GUILayout.Height(40)))
        {
            _generator.ContextGenerate();
        }

		if(GUILayout.Button("Clear", GUILayout.Height(20)))
        {
            _generator.ContextClear();
        }
        
            

    }
}
