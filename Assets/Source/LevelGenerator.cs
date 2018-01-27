using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private Texture2D _level = null;

    [Space]
    [SerializeField] private GameObject _walkableTilePrefab     = null;
    [SerializeField] private GameObject _notWalkableTilePrefab  = null;


    public void GenerateLevel()
    {
        for (int x = 0; x < _level.width; ++x)
        {
            for (int y = 0; y < _level.height; ++y)
            {
                GameObject go = GameObject.Instantiate(
                    (_level.GetPixel(x, y) == Color.white ? _walkableTilePrefab : _notWalkableTilePrefab),
                    new Vector3(x, 0f, y), Quaternion.identity
                );
                go.name = "TILE_" + x.ToString() + "_" + y.ToString();
            }
        }
    }
}
