using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TerrainEngine
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(LayerMapGenerator))]
    public class VoxelData : MonoBehaviour
    {
        [SerializeField]
        private List<Texture2D> _layers = new List<Texture2D>();
        public List<Texture2D> layers
        {
            get { return _layers; }
            set
            {
                _layers = value;
            }
        }

        [SerializeField]
        private List<ColorMappedVoxel> _voxel = new List<ColorMappedVoxel>();
        public List<ColorMappedVoxel> voxel { get { return _voxel; } }
        
        [SerializeField]
        private int _height;
        [SerializeField]
        private bool _generateProcedural;

        private LayerMapGenerator LayerMapGeneratorRef;

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

        private void OnEnable()
        {
            LayerMapGeneratorRef = GetComponent<LayerMapGenerator>();
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
        
        private void FeedTextureBuffer()
        {
            _layers.Clear();
            StartCoroutine(LayerMapGeneratorRef.GetProceduralLayer(_height));

            //for (int a = 0; a < _height; a++)
            //{
            //    float res = (float)a / (float)_height;

            //    _layers.Add(LayerMapGeneratorRef.GetProceduralLayer(res));
            //}
        }

        private void FeedBuffer()
        {
            _buffer.Clear();
            for (int i = 0; i < transform.childCount; i++)
            {
                _buffer.Add(transform.GetChild(i).gameObject);
            }
        }

        public Voxel GetVoxel (Color Selector)
        {
            for (int i = 0; i < _voxel.Count; i++)
            {
                if (_voxel[i].GetVoxelByColor(Selector))
                {
                    Debug.Log(Selector);

                    if (_voxel[i].Target == null)
                    {
                        Debug.LogErrorFormat("GetVoxel(): The target of the ColorMappedVoxel is null for color = {0}", Selector);
                    }

                    return _voxel[i].Target;
                }
            }

            Debug.LogErrorFormat("GetVoxel(): No voxel found for color = {0}", Selector);

            return null;
        }

        private void Update()
        {
            if (_generateProcedural)
            {
                FeedTextureBuffer();
                _generateProcedural = false;
            }
        }

    }
}

