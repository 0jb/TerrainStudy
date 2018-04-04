using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TerrainEngine
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(VoxelData))]
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(Neighborhood))]
    public class Generator : MonoBehaviour
    {

        private VoxelData voxelDataRef;
        private Neighborhood neighborhoodRef;
        private int v;
        private double t;

        private void OnEnable()
        {
            voxelDataRef = GetComponent<VoxelData>();
            neighborhoodRef = GetComponent<Neighborhood>();
            if (Application.isPlaying)
            {
                StartCoroutine(BuildAll());
            }
        }

        private IEnumerator BuildAll()
        {
            for (int i = 0; i < voxelDataRef.layers.Count; i++)
            {
                if (voxelDataRef.layers.Count > 1)
                {
                    if(i == 0)
                    {
                        BuildLayer(voxelDataRef.layers[0], 0, null, voxelDataRef.layers[1]);
                    }
                    else if (i + 1 < voxelDataRef.layers.Count)
                    {
                        BuildLayer(voxelDataRef.layers[i], i, voxelDataRef.layers[i - 1], voxelDataRef.layers[i + 1]);
                    }
                    else if (i == voxelDataRef.layers.Count - 1)
                    {
                        BuildLayer(voxelDataRef.layers[i], i, voxelDataRef.layers[i - 1], null);
                    }                  

                    
                }
                else if (voxelDataRef.layers.Count == 1)
                {
                    BuildLayer(voxelDataRef.layers[i], i);
                }
                yield return null;
            }
            
            gameObject.SetActive(true);
            voxelDataRef.DestroyAll();
            UpdateMeshCollider();
        }

        private void CombineAll(List<Voxel.MeshPerAngle> Origin)
        {
            CombineInstance[] combine = new CombineInstance[Origin.Count];
            combine[0].mesh = GetComponent<MeshFilter>().sharedMesh;
            combine[0].transform = transform.localToWorldMatrix;
            for (int i = 0; i  < Origin.Count; i++)
            {
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

        private void BuildLayer(Texture2D source, float height, Texture2D downstairs = null, Texture2D upstairs = null)
        {
            int gridX = source.width;
            int gridY = source.height;


            for (int x = 0; x < gridX; x++)
            {
                for (int y = 0; y < gridY; y++)
                {
                    Voxel currentCell = voxelDataRef.GetVoxel(source.GetPixel(x, y));
                    if (currentCell != null)
                    {
                        currentCell = Instantiate(currentCell, transform);
                        currentCell._meshPerAngle = neighborhoodRef.WallsToBeKept(source, downstairs, upstairs, currentCell, x, y);
                        currentCell.transform.position = new Vector3(x, height, y);
                    }                    
                }
            }
            CombineAll(GetVoxelMeshFilters());
            
        }
        
        private void ClearGeneratedMesh()
        {
            transform.GetComponent<MeshFilter>().sharedMesh = new Mesh();
        }


        private void UpdateMeshCollider()
        {
            if(GetComponent<MeshCollider>() != null)
            {
                GetComponent<MeshCollider>().sharedMesh = GetComponent<MeshFilter>().sharedMesh;
            }
        }

        #if UNITY_EDITOR
        public float DebugGenerateDelay = 0f;
        [ContextMenu("Generate")]
        private void ContextGenerate()
        {
            ClearGeneratedMesh();
            voxelDataRef.DestroyAll();
            v = 0; t = UnityEditor.EditorApplication.timeSinceStartup;
            UnityEditor.EditorApplication.update += EditorUpdate;
        }

        [ContextMenu("Clear")]
        private void ContextClear()
        {
            ClearGeneratedMesh();
            voxelDataRef.DestroyAll();
        }

        
        private void EditorUpdate()
        {
            if(v < voxelDataRef.layers.Count)
            {
                if(UnityEditor.EditorApplication.timeSinceStartup - t >= DebugGenerateDelay)
                {
                    t += DebugGenerateDelay;
                    //BuildLayer(voxelDataRef.layers[v], v);
                    if (voxelDataRef.layers.Count > 1)
                    {
                        if (v == 0)
                        {
                            BuildLayer(voxelDataRef.layers[0], 0, null, voxelDataRef.layers[1]);
                        }
                        else if (v + 1 < voxelDataRef.layers.Count)
                        {
                            BuildLayer(voxelDataRef.layers[v], v, voxelDataRef.layers[v - 1], voxelDataRef.layers[v + 1]);
                        }
                        else if (v == voxelDataRef.layers.Count - 1)
                        {
                            BuildLayer(voxelDataRef.layers[v], v, voxelDataRef.layers[v - 1], null);
                        }


                    }
                    else if (voxelDataRef.layers.Count == 1)
                    {
                        BuildLayer(voxelDataRef.layers[v], v);
                    }
                    v++;

                }
            }
            else
            {
                UpdateMeshCollider();
                gameObject.SetActive(true);
                voxelDataRef.DestroyAll();
                UnityEditor.EditorApplication.update -= EditorUpdate;
            }
        }
        #endif
    }
}