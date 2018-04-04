using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TerrainEngine
{
    [RequireComponent(typeof(TerrainEngine.VoxelData))]
    [ExecuteInEditMode]
    public class Neighborhood : MonoBehaviour
    {
        private VoxelData VoxelDataRef;

        private void OnEnable()
        {
            VoxelDataRef = GetComponent<VoxelData>();
        }
        
        public List<Voxel.MeshPerAngle> WallsToBeKept(
            Texture2D HomeBlock,
            Texture2D Downstairs,
            Texture2D Upstairs,
            Voxel Home,
            int XAdress,
            int YAdress
            )
        {
            List<Voxel.MeshPerAngle> KeepMyWalls = Home._meshPerAngle ;

            for (int i = 0; i < Home._meshPerAngle.Count; i++)
            {
                Color NeighborColor;
                Voxel Neighbor;
                switch (Home._meshPerAngle[i].PivotAngle)
                {
                    case 0:
                        if (Downstairs != null)
                        {
                            NeighborColor = Downstairs.GetPixel(XAdress, YAdress);
                            Neighbor = VoxelDataRef.GetVoxel(NeighborColor);
                            if (Neighbor.type == Home.type)
                            {
                                KeepMyWalls[i].CandidateForExclusion = true;
                            }
                        }                        
                        break;
                    case 1:
                        if (Upstairs != null)
                        {
                            NeighborColor = Upstairs.GetPixel(XAdress, YAdress);
                            Neighbor = VoxelDataRef.GetVoxel(NeighborColor);
                            if (Neighbor.type == Home.type)
                            {
                                KeepMyWalls[i].CandidateForExclusion = true;
                            }
                        }                        
                        break;
                    case 2:
                        if (XAdress - 1 >= 0)
                        {
                            NeighborColor = HomeBlock.GetPixel(XAdress -1, YAdress);
                            Neighbor = VoxelDataRef.GetVoxel(NeighborColor);
                            if (Neighbor.type == Home.type)
                            {
                                KeepMyWalls[i].CandidateForExclusion = true;
                            }
                        }                        
                        break;
                    case 3:
                        if (YAdress - 1 >= 0)
                        {                            
                            NeighborColor = HomeBlock.GetPixel(XAdress, YAdress - 1);
                            Neighbor = VoxelDataRef.GetVoxel(NeighborColor);                            
                            if (Neighbor.type == Home.type)
                            {
                                KeepMyWalls[i].CandidateForExclusion = true;
                            }
                        }                        
                        break;
                    case 4:
                        if (HomeBlock.width > XAdress + 1)
                        {
                            NeighborColor = HomeBlock.GetPixel(XAdress + 1, YAdress);
                            Neighbor = VoxelDataRef.GetVoxel(NeighborColor);
                            if (Neighbor.type == Home.type)
                            {
                               KeepMyWalls[i].CandidateForExclusion = true;
                            }
                        }                        
                        break;
                    case 5:
                        if (HomeBlock.height > YAdress + 1)
                        {
                            NeighborColor = HomeBlock.GetPixel(XAdress, YAdress + 1);
                            Neighbor = VoxelDataRef.GetVoxel(NeighborColor);
                            if (Neighbor.type == Home.type)
                            {
                                KeepMyWalls[i].CandidateForExclusion = true;
                            }
                        }                        
                        break;
                }
            }

            int listLength = KeepMyWalls.Count;

            for (int i = 0; i < listLength; i++)
            {
                if (KeepMyWalls[i].CandidateForExclusion)
                {
                    KeepMyWalls.RemoveAt(i);
                    i--;
                    listLength--;
                }
            }

            return KeepMyWalls;
        }
    }
}

