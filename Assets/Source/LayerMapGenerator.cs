using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TerrainEngine
{
    [ExecuteInEditMode]
    public class LayerMapGenerator : MonoBehaviour
    {
        private ProceduralMaterial _substanceMaterial;
        private float _parameterValue;
        private string _parameterName;
        private string _textureName;

        private void UpdateTexture(float ParameterValue)
        {
            _substanceMaterial.SetProceduralFloat(_parameterName, ParameterValue);
            _substanceMaterial.GetGeneratedTexture(_textureName);
            //Texture2D newTex;
            //newTex.SetPixels()


        }

    }
}

