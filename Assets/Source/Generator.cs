using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TerrainEngine
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(VoxelData))]
    [RequireComponent(typeof(MeshFilter))]
    public class Generator : MonoBehaviour
    {
        public bool DebugNow;
        public bool Clear;

        private VoxelData voxelDataRef;

        private void OnEnable()
        {
            voxelDataRef = GetComponent<VoxelData>();
        }

        private void BuildAll()
        {
            for (int i = 0; i < voxelDataRef.layers.Count; i++)
            {
                BuildLayer(voxelDataRef.layers[i], i);
            }
        }

        private void CombineAll(MeshFilter[] Origin)
        {
            CombineInstance[] combine = new CombineInstance[Origin.Length];
            for(int i = 0; i < Origin.Length; i++)
            {
                combine[i].mesh = Origin[i].sharedMesh;
                combine[i].transform = Origin[i].transform.localToWorldMatrix;
                Origin[i].gameObject.SetActive(false);
            }
            transform.GetComponent<MeshFilter>().sharedMesh = new Mesh();
            transform.GetComponent<MeshFilter>().sharedMesh.CombineMeshes(combine);
        }

        private void BuildLayer(Texture2D source, float height)
        {
            int gridX = source.width;
            int gridY = source.height;

            for (int x = 0; x <= gridX; x++)
            {
                for (int y = 0; y <= gridY; y++)
                {
                    GameObject currentCell;
                    currentCell = Instantiate(voxelDataRef.GetVoxel(source.GetPixel(x, y)));
                    currentCell.transform.position = new Vector3(x, height, y);
                    currentCell.transform.parent = transform;
                }
            }
            CombineAll(GetComponentsInChildren<MeshFilter>());
            voxelDataRef.DestroyAll();
        }
        
        private void ClearGeneratedMesh()
        {
            transform.GetComponent<MeshFilter>().sharedMesh = new Mesh();
        }

        private void Update()
        {
            if (Clear)
            {
                ClearGeneratedMesh();
                voxelDataRef.DestroyAll();
                Clear = false;
            }

            if (DebugNow)
            {
                voxelDataRef.DestroyAll();
                BuildAll();
                DebugNow = false;
            }
        }
    }
}