using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TerrainEngine
{
    [ExecuteInEditMode]
    public class LayerMapGenerator : MonoBehaviour
    {
        [SerializeField]
        private ProceduralMaterial _substanceMaterial;
        [SerializeField]
        private string _parameterName;
        [SerializeField]
        private string _textureName;

        private VoxelData VoxelDataRef;

        [SerializeField]
        private float DebugGenerateDelay = 0f;

        private double t;

        private void OnEnable()
        {
            VoxelDataRef = GetComponent<VoxelData>();
        }


        public IEnumerator GetProceduralLayer(float TotalHeight)
        {
            
            for (int i = 0; i < TotalHeight; i++)
            {
                if (UnityEditor.EditorApplication.timeSinceStartup - t >= DebugGenerateDelay)

                {
                    float res = (float)i / (float)TotalHeight;
                    _substanceMaterial.isReadable = true;
                    _substanceMaterial.SetProceduralFloat(_parameterName, res);
                    _substanceMaterial.RebuildTextures();
                    yield return null;
                    ProceduralTexture substanceTexture = _substanceMaterial.GetGeneratedTexture(_textureName);
                    Texture2D newTex = new Texture2D(substanceTexture.width, substanceTexture.height);
                    newTex.SetPixels32(substanceTexture.GetPixels32(0, 0, substanceTexture.width, substanceTexture.height));
                    newTex.Apply();
                    VoxelDataRef.layers.Add(newTex);
                }

                t += DebugGenerateDelay;
            }


            UnityEditor.EditorApplication.update -= VoxelDataRef.FeedTextureBuffer;
        }

    }
}

