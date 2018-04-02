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
        //public Color NeighborColor = new Color(0.0f, 0.0f, 0.0f, 1.0f);
        //private Voxel Neighbor;


        private void OnEnable()
        {
            VoxelDataRef = GetComponent<VoxelData>();
        }

        //public void WallsToBeKept(
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
            //List<Voxel.MeshPerAngle> KeepMyWalls = new List<Voxel.MeshPerAngle>();


            for (int i = 0; i < Home._meshPerAngle.Count; i++)
            {
                switch (Home._meshPerAngle[i].PivotAngle)
                {
                    default:
                        
                        Color NeighborColor;
                        Voxel Neighbor;
                        break;

                        // CHECK TEXTURE BOUNDS
                    case 0:
                        if (Downstairs != null)
                        {
                            NeighborColor = Downstairs.GetPixel(XAdress, YAdress);
                            Neighbor = VoxelDataRef.GetVoxel(NeighborColor);
                            if (!Neighbor.DoIContainThisAngle(1))
                            {
                                //KeepMyWalls.Remove(Home._meshPerAngle[i]);
                            }
                        }                        
                        break;
                    case 1:
                        if (Upstairs != null)
                        {
                            NeighborColor = Upstairs.GetPixel(XAdress, YAdress);
                            Neighbor = VoxelDataRef.GetVoxel(NeighborColor);
                            if (!Neighbor.DoIContainThisAngle(0))
                            {
                                //KeepMyWalls.Remove(Home._meshPerAngle[i]);
                            }
                        }                        
                        break;
                    case 2:
                        if (YAdress - 1 > 0)
                        {
                            Debug.Log("Face 2");
                            Debug.Log("source x "+XAdress);
                            Debug.Log("source y "+YAdress);
                            Debug.Log("neighbor y " + (YAdress - 1));
                            NeighborColor = HomeBlock.GetPixel(XAdress, YAdress - 1);
                            Neighbor = VoxelDataRef.GetVoxel(NeighborColor);

                            // TODO:
                            // COUSIN: TYPES THAT HAVE RELATIONSHIP AND CAN SHARE SEAMS
                            // COUSINS SHOULD BE DETECTED HERE AND INSTANTIATE A SEAM
                            if (Neighbor.type == Home.type)
                            {
                                KeepMyWalls.Remove(Home._meshPerAngle[3]);
                            }
                        }                        
                        break;
                    case 3:
                        if (HomeBlock.width >= XAdress + 1)
                        {
                            NeighborColor = HomeBlock.GetPixel(XAdress + 1, YAdress);
                            Neighbor = VoxelDataRef.GetVoxel(NeighborColor);
                            if (!Neighbor.DoIContainThisAngle(5))
                            {
                                //KeepMyWalls.Remove(Home._meshPerAngle[i]);
                            }
                        }                        
                        break;
                    case 4:
                        if (HomeBlock.height >= YAdress + 1)
                        {
                            NeighborColor = HomeBlock.GetPixel(XAdress, YAdress + 1);
                            Neighbor = VoxelDataRef.GetVoxel(NeighborColor);
                            if (!Neighbor.DoIContainThisAngle(2))
                            {
                                //KeepMyWalls.Remove(Home._meshPerAngle[i]);
                            }
                        }                        
                        break;
                    case 5:
                        if (XAdress - 1 > 0)
                        {
                            NeighborColor = HomeBlock.GetPixel(XAdress - 1, YAdress);
                            Neighbor = VoxelDataRef.GetVoxel(NeighborColor);
                            if (!Neighbor.DoIContainThisAngle(3))
                            {
                                //KeepMyWalls.Remove(Home._meshPerAngle[i]);
                            }
                        }                        
                        break;
                }
            }

            return KeepMyWalls;
        }
    }
}

