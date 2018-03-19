using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Generator : MonoBehaviour
{
    [SerializeField]
    private List<Texture2D> _layers = new List<Texture2D>();

    [SerializeField]
    private List<ColorMappedVoxel> _voxel = new List<ColorMappedVoxel>();

    private List<GameObject> _buffer = new List<GameObject>();

    public bool DebugNow;
    public bool Clear;

    [System.Serializable]
    public class ColorMappedVoxel
    {
        public Color ColorMap;
        public GameObject Target;

        public ColorMappedVoxel(Color _ColorMap, GameObject _Target)
        {
            ColorMap = _ColorMap;
            Target = _Target;
        }

        public GameObject GetVoxelByColor(Color Selector)
        {
            if(ColorMap == Selector)
            {
                return Target;
            }

            return null;
        }
    }

    private GameObject GetVoxel(Color Selector)
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

    private void BuildAll()
    {
        for (int i = 0; i < _layers.Count; i ++)
        {
            BuildLayer(_layers[i], i);
        }
    }

    private void BuildLayer (Texture2D source, float height)
    {
        int gridX = source.width;
        int gridY = source.height;

        for (int x = 0; x <= gridX; x++)
        {
            for(int y = 0; y <= gridY; y++)
            {
                GameObject currentCell;
                currentCell = Instantiate(GetVoxel( source.GetPixel(x,y) ) );
                currentCell.transform.position = new Vector3(x, height, y);
                currentCell.transform.parent = transform;
                _buffer.Add(currentCell);
            }
        }
    }
    
    private void DestroyAll()
    {
        for (int i = 0; i < _buffer.Count; i++)
        {
            DestroyImmediate(_buffer[i]);
        }
        _buffer.Clear();
    }

    private void Update()
    {
        if (Clear)
        {
            DestroyAll();
            Clear = false;
        }

        if (DebugNow)
        {
            DestroyAll();
            BuildAll();
            DebugNow = false;
        }
    }
}
