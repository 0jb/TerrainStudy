﻿using System.Collections;
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
            Application.runInBackground = true;
        }

        private IEnumerator BuildAll()
        {
            for (int i = 0; i < voxelDataRef.layers.Count; i++)
            {
                BuildLayer(voxelDataRef.layers[i], i);
                yield return null;
            }
            
            gameObject.SetActive(true);
        }

        private void CombineAll(List<Voxel.MeshPerAngle> Origin)
        {
            CombineInstance[] combine = new CombineInstance[Origin.Count + 1];
            for(int i = 0; i < Origin.Count; i++)
            {
                if(i == 0)
                {
                    combine[0].mesh = GetComponent<MeshFilter>().sharedMesh;
                    combine[0].transform = transform.localToWorldMatrix;
                    continue;
                }

                combine[i].mesh = Origin[i].Mesh.sharedMesh;
                combine[i].transform = Origin[i].Mesh.transform.localToWorldMatrix;
                Origin[i].Mesh.gameObject.SetActive(false);
            }
            transform.GetComponent<MeshFilter>().sharedMesh = new Mesh();
            transform.GetComponent<MeshFilter>().sharedMesh.CombineMeshes(combine);
        }

        private List<Voxel.MeshPerAngle> GetVoxelMeshFilters()
        {
            List<Voxel.MeshPerAngle> allVoxelMeshFilters = new List<Voxel.MeshPerAngle>();

            for (int i = 0; i < transform.childCount; i++)
            {
                allVoxelMeshFilters.AddRange(transform.GetChild(i).GetComponent<Voxel>()._meshPerAngle);
            }

            return allVoxelMeshFilters;
        }

        private void BuildLayer(Texture2D source, float height)
        {
            int gridX = source.width;
            int gridY = source.height;

            for (int x = 0; x <= gridX; x++)
            {
                for (int y = 0; y <= gridY; y++)
                {
                    Voxel currentCell;
                    currentCell = Instantiate(voxelDataRef.GetVoxel(source.GetPixel(x, y)));
                    currentCell.transform.position = new Vector3(x, height, y);
                    currentCell.transform.parent = transform;
                }
            }
            CombineAll(GetVoxelMeshFilters());
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
                StartCoroutine(BuildAll());
                DebugNow = false;
            }
        }
    }
}