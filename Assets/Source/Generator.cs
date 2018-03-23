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

        private GameObject neighborCells;

        private GameObject currentCell;

        public enum voxelDirections
        {
            North,
            East,
            South,
            West,
        }

        struct DataCoordinate
        {
            public int x;
            public int y;

            public DataCoordinate(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }

        static DataCoordinate[] pixelOffset =
        {
            new DataCoordinate (0,1),
            new DataCoordinate (1,0),
            new DataCoordinate (0,-1),
            new DataCoordinate (-1,0)
            };

        private static voxelDirections dir;
        DataCoordinate offsetToCheck = pixelOffset[(int)dir];

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

            Color colNeighbor;
            Color colCell;
            int gridX = source.width;
            int gridY = source.height;

            for (int x = 0; x < gridX; x++)
            {
                for (int y = 0; y < gridY; y++)
                {
                    if (voxelDataRef.GetVoxel(source.GetPixel(x, y)) != null)
                    {
                        colCell = source.GetPixel(x,y);
                        
                        for (int n = 0; n < pixelOffset.Length; n++)
                    {


                        colNeighbor = source.GetPixel(x + pixelOffset[n].x, y + pixelOffset[n].y);

                        if (colNeighbor == Color.black)
                        {
                            
                            neighborCells = Instantiate(voxelDataRef.showNeighbors);
                            neighborCells.transform.position = new Vector3(x + pixelOffset[n].x, height, y + pixelOffset[n].y);
                            
                            neighborCells.transform.parent = transform;
                           
                        }

                    }

                        currentCell = Instantiate(voxelDataRef.GetVoxel(colCell));
                        currentCell.transform.position = new Vector3(x, height, y);
                        currentCell.transform.parent = transform;
                    }

                    


                    //is there a cleaner way to fetch neighbor pixels in one go?
                    //or not use so many loops at once?

                    


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

