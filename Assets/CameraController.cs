using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraController : MonoBehaviour
{
    private const float extentsToSizeRatio = 1.7222f; // Magic!
    
    private Vector3 startPosition; // I assume we might want it, idk
    
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        var tilemapManager = FindObjectOfType<TileMapManager>();
        var tilemapBounds = tilemapManager.GetTilemapBounds();

        GetComponent<Camera>().orthographicSize =
            extentsToSizeRatio * Math.Max(tilemapBounds.extents.x, tilemapBounds.extents.y);
    }

}
