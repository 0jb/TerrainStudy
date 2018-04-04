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

        private VoxelData voxelDataRef;

        private void OnEnable()
        {
            voxelDataRef = GetComponent<VoxelData>();
        }

        private IEnumerator BuildAll()
        {
            for (int i = 0; i < voxelDataRef.layers.Count; i++)
            {
                BuildLayer(voxelDataRef.layers[i], i);
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

        private void BuildLayer(Texture2D source, float height)
        {
            int gridX = source.width;
            int gridY = source.height;

            for (int x = 0; x < gridX; x++)
            {
                for (int y = 0; y < gridY; y++)
                {
                    Voxel currentCell = voxelDataRef.GetVoxel(source.GetPixel(x, y));
                    if(currentCell != null)
                    {
                        currentCell = Instantiate(currentCell);
                        currentCell.transform.position = new Vector3(x, height, y);
                        currentCell.transform.parent = transform;
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

        private void Start()
        {
            StartCoroutine(BuildAll());     
        }

        #if UNITY_EDITOR
        public float DebugGenerateDelay = 0f;
        [ContextMenu("Generate")]
        private void ContextGenerate()
        {
            ClearGeneratedMesh();
            voxelDataRef.DestroyAll();
            i = 0; t = UnityEditor.EditorApplication.timeSinceStartup;
            UnityEditor.EditorApplication.update += EditorUpdate;
        }

        [ContextMenu("Clear")]
        private void ContextClear()
        {
            ClearGeneratedMesh();
            voxelDataRef.DestroyAll();
        }

        int i;
        double t;
        private void EditorUpdate()
        {
            if(i < voxelDataRef.layers.Count)
            {
                if(UnityEditor.EditorApplication.timeSinceStartup - t >= DebugGenerateDelay)
                {
                    t += DebugGenerateDelay;
                    BuildLayer(voxelDataRef.layers[i], i);
                    i++;
                }
            }
            else
            {
                gameObject.SetActive(true);
                voxelDataRef.DestroyAll();
                UnityEditor.EditorApplication.update -= EditorUpdate;
            }
        }
        #endif
    }
}