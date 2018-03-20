using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TerrainEngine
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(VoxelData))]
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
        }           

        private void Update()
        {
            if (Clear)
            {
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

