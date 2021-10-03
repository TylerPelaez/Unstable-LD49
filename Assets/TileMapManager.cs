using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapManager : MonoBehaviour
{
    public GameObject ColliderToCreate;
    
    // Start is called before the first frame update
    void Start()
    {
        var tilemap = GetComponent<Tilemap>();

        var tilemapSize = tilemap.cellBounds;

        Debug.Log(tilemap.localBounds);

        for (int i = tilemapSize.x; i < tilemapSize.xMax; i++)
        {
            for (int j = tilemapSize.y; j < tilemapSize.yMax; j++)
            {
                var currentCell = new Vector3Int(i, j, 0);
                if (!tilemap.HasTile(currentCell))
                {
                    continue;
                }

                var worldPos = tilemap.CellToLocal(currentCell);
                Instantiate(ColliderToCreate, worldPos, Quaternion.identity);
            }
        }
    }

    public Bounds GetTilemapBounds()
    {
        var tilemap = GetComponent<Tilemap>();
        return tilemap.localBounds;
    }
}
