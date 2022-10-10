using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapPainter : MonoBehaviour
{
    [SerializeField]
    private Tile[] groundTiles, foregroundTiles;

    [SerializeField]
    private Tile colliderTile;

    [SerializeField]
    private Tilemap backgroundTilemap, colliderTilemap;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PaintTiles(Vector3Int gridMovementAreaLowerLimit, Vector3Int gridMovementAreaUpperLimit, TILE_TYPE tileType)
    {
        Tile groundTile, foregroundTile;

        groundTile = groundTiles[(int)tileType];
        foregroundTile = foregroundTiles[(int)tileType];

        for (int x = gridMovementAreaLowerLimit.x; x < gridMovementAreaUpperLimit.x; ++x)
        {
            for (int y = gridMovementAreaLowerLimit.y; y < gridMovementAreaUpperLimit.y; ++y)
            {
                if (y == gridMovementAreaUpperLimit.y - 1)
                {
                    backgroundTilemap.SetTile(new Vector3Int(x, y, 0), foregroundTile);
                }
                else
                {
                    backgroundTilemap.SetTile(new Vector3Int(x, y, 0), groundTile);
                }
            }
        }
    }
}

public enum TILE_TYPE   
{
    GUILD1,
    GUILD2,
    GUILD3,
    GUILD4,
    NO_TILE_TYPE
}
