using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField]
    private GameObject _level;

    private float _currentZ = 20.0f;

    public string Surfaces = "Assets/Prefabs/Surfaces";
    private List<GameObject> _surfacePrefabs;

    public string Obstacles = "Assets/Prefabs/Obstacles";
    private List<GameObject> _obstaclePrefabs;

    private bool _nextGenPending = false;

    [SerializeField]
    private PlayerController _player;


    // Start is called before the first frame update
    void Start()
    {
        // Get all prefabs
        _surfacePrefabs = GetAllPrefabs(Surfaces);
        _obstaclePrefabs = GetAllPrefabs(Obstacles);

        //  Generate Next 10 surfaces
        GenerateSurface(10);
    }

    // Update is called once per frame
    void Update()
    {
        if ((_player.transform.position.z > (_currentZ - 50f)) && _nextGenPending)
        {
            GenerateSurface(10);
            _nextGenPending = false;
        }

        if(_player.transform.position.z > _currentZ - 60f)
        {
            _nextGenPending = true;
        }

    }

    void GenerateSurface(int count)
    {
        // Get all prefabs
        _surfacePrefabs = GetAllPrefabs(Surfaces);

        // Place 10 prefabs
        for (int i = 0; i < count; i++)
        {
            // Generate one surface
            foreach (var surface in _surfacePrefabs)
            {
                //GenerateObstacles(_currentZ + 2.5f);
                GenerateObstacles(_currentZ + 7.5f);
                Instantiate(surface, new Vector3(0f, -0.8f, _currentZ), Quaternion.identity);
                _currentZ += 10f;
            }
        }
    }

    void GenerateObstacles(float zPos)
    {
        int numPlaced = 0;
        bool left = (Random.value) > 0.5f;
        if(left)
        {
            PlaceObstacle(-_player.LaneDistance, zPos);
            numPlaced++;
        }

        bool center = (Random.value) > 0.5f;
        if (center)
        {
            PlaceObstacle(0f, zPos);
            numPlaced++;
        }
        if (numPlaced == 2)
            return;
        bool right = (Random.value) > 0.5f;
        if (right)
        {
            PlaceObstacle(_player.LaneDistance, zPos);
        }

    }

    void PlaceObstacle(float xPos, float zPos)
    {
        var index = Random.Range(0, _obstaclePrefabs.Count);
        Instantiate(_obstaclePrefabs[index], new Vector3(xPos, 0f, zPos), Quaternion.identity);
    }

    #region DYNAMIC PREFAB ACQUISITION

    private List<GameObject> GetAllPrefabs(string path)
    {
        string[] foldersToSearch = { path };
        var allPrefabs =  GetAssets<GameObject>(foldersToSearch, "t:prefab");
        return allPrefabs;
    }

    public static List<T> GetAssets<T>(string[] _foldersToSearch, string _filter) where T : UnityEngine.Object
    {
        string[] guids = AssetDatabase.FindAssets(_filter, _foldersToSearch);
        List<T> a = new List<T>();
        for (int i = 0; i < guids.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            a.Add(AssetDatabase.LoadAssetAtPath<T>(path));
        }
        return a;
    }

    #endregion 
}
