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


        private double t;

        private void OnEnable()
        {
            VoxelDataRef = GetComponent<VoxelData>();
        }

        public void GenerateProceduralTexture(int i, float TotalHeight)
        {
            //Gets input and begins RebuildTextures function for next frame
                float res = (float)i / (float)TotalHeight; 
                _substanceMaterial.isReadable = true;
                _substanceMaterial.SetProceduralFloat(_parameterName, res);
                _substanceMaterial.RebuildTextures();
        }

        public void GenerateTexture2D()
        {
            //Procedural Texture is created and is transformed into a Tex2D;
                ProceduralTexture substanceTexture = _substanceMaterial.GetGeneratedTexture(_textureName);
                Texture2D newTex = new Texture2D(substanceTexture.width, substanceTexture.height);
                newTex.SetPixels32(substanceTexture.GetPixels32(0, 0, substanceTexture.width, substanceTexture.height));
                newTex.Apply();
                VoxelDataRef.layers.Add(newTex);
                
        }
        public IEnumerator GetProceduralLayer(float TotalHeight)
        {

            for (int i = 0; i < TotalHeight; i++)
            {
                GenerateProceduralTexture(i, TotalHeight);
                yield return null;
                GenerateTexture2D();
                
            }
            //UnityEditor.EditorApplication.update -= VoxelDataRef.FeedTextureBuffer;


            
        }

    }
}

