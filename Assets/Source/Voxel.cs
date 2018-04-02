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
        public int PivotAngle;
        public MeshFilter Mesh;
        public bool CandidateForExclusion;

        public MeshPerAngle(int _PivotAngle, MeshFilter _Mesh)
        {
            PivotAngle = _PivotAngle;
            Mesh = _Mesh;
        }
    }
    
    [SerializeField]
    private List<Quaternion> _QuaternionIntRef;
    [SerializeField]
    private VoxelType _type;
    public VoxelType type { get { return _type; } }
    [SerializeField]
    public List<MeshPerAngle> _meshPerAngle;

    public bool PopulateNow;
    public bool DebugAngle;
    public int Angle;

    private void FillVoxelMeshAngleList()
    {
        _meshPerAngle.Clear();

        for (int i = 0; i < transform.childCount; i++)
        {
            _meshPerAngle.Add(new MeshPerAngle(QuaternionToInt(transform.GetChild(i).transform.rotation), 
                                                transform.GetChild(i).GetComponent<MeshFilter>()));
        }
    }

    private int QuaternionToInt(Quaternion QuaternionRef)
    {
        for (int i = 0; i < _QuaternionIntRef.Count; i++)
        {
            if (Quaternion.Angle(QuaternionRef, _QuaternionIntRef[i]) < 0.1f &&
                Quaternion.Angle(QuaternionRef, _QuaternionIntRef[i]) > -0.1f)
            {
                return i;
            }
        }
        return 666;
    }

    public bool DoIContainThisAngle (int TargetAngle)
    {
        for (int i = 0; i < _meshPerAngle.Count; i++)
        {
            if (TargetAngle == _meshPerAngle[i].PivotAngle)
            {
                _meshPerAngle[i].CandidateForExclusion = true;
                return true;
            }
        }

        return false;
    }

    private void Update()
    {
        if (PopulateNow)
        {
            FillVoxelMeshAngleList();
            PopulateNow = false;
        }

        if (DebugAngle)
        {
            Debug.Log(DoIContainThisAngle(Angle) );
        }
    }
}
