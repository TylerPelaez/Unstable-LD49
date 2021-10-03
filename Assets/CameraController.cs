using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraController : MonoBehaviour
{
    private const float extentsToSizeRatio = 2.21428f; // Magic!
    
    private Vector3 startPosition; // I assume we might want it, idk
    
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        var tilemapManager = FindObjectOfType<TileMapManager>();
        var tilemapBounds = tilemapManager.GetTilemapBounds();

        var largerBound = Math.Max(tilemapBounds.extents.x, tilemapBounds.extents.y);
        
        GetComponent<Camera>().orthographicSize =
            extentsToSizeRatio * largerBound;

        var offset = largerBound * 2.0f % 2f == 0 ? new Vector3(0f, -1.5f, -10f) : new Vector3(-0.5f, -1.5f, -10f);

        transform.position = tilemapBounds.center + offset;
    }

}
