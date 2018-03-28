using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TerrainEngine
{
    [ExecuteInEditMode]
    public class VoxelData : MonoBehaviour
    {
        [SerializeField]
        private List<Texture2D> _layers = new List<Texture2D>();
        public List<Texture2D> layers { get { return _layers; } }

        [SerializeField]
        private List<ColorMappedVoxel> _voxel = new List<ColorMappedVoxel>();
        public List<ColorMappedVoxel> voxel { get { return _voxel; } }

        private List<GameObject> _buffer = new List<GameObject>();

        [System.Serializable]
        public class ColorMappedVoxel
        {
            public Color ColorMap;
            public Voxel Target;

            public ColorMappedVoxel(Color _ColorMap, Voxel _Target)
            {
                ColorMap = _ColorMap;
                Target = _Target;
            }

            public Voxel GetVoxelByColor(Color Selector)
            {
                if (ColorMap == Selector)
                {

                    return Target;
                    
                }

                return null;
            }
        }

        public void DestroyAll()
        {
            FeedBuffer();
            for (int i = 0; i < _buffer.Count; i++)
            {
                DestroyImmediate(_buffer[i]);
            }
            _buffer.Clear();
        }

        private void FeedBuffer()
        {
            _buffer.Clear();
            for (int i = 0; i < transform.childCount; i++)
            {
                _buffer.Add(transform.GetChild(i).gameObject);
            }
        }

        public Voxel GetVoxel(Color Selector)
        {
            for (int i = 0; i < _voxel.Count; i++)
            {
                if (_voxel[i].GetVoxelByColor(Selector))
                {
                    return _voxel[i].Target;
                }
            }
            return null;
        }
        
    }
}

