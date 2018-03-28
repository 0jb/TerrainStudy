using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Voxel : MonoBehaviour
{
	public enum VoxelType
    {
        Empty,
        Generic_Brick
    }
    
    [System.Serializable]
    public class MeshPerAngle
    {
        public Vector3 PivotAngle;
        public MeshFilter Mesh;

        public MeshPerAngle(Vector3 _PivotAngle, MeshFilter _Mesh)
        {
            PivotAngle = _PivotAngle;
            Mesh = _Mesh;
        }
    }
    
    [SerializeField]
    private VoxelType _type;
    [SerializeField]
    public List<MeshPerAngle> _meshPerAngle;

    public bool PopulateNow;

    private void FillVoxelMeshAngleList()
    {
        _meshPerAngle.Clear();

        for (int i = 0; i < transform.childCount; i++)
        {
            _meshPerAngle.Add(new MeshPerAngle(transform.GetChild(i).transform.localEulerAngles, transform.GetChild(i).GetComponent<MeshFilter>()));
        }
    }

    private void Update()
    {
        if (PopulateNow)
        {
            FillVoxelMeshAngleList();
            PopulateNow = false;
        }
    }
}
