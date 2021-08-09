using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGrid : MonoBehaviour
{
    public int width;
    public int height;

    public float tileSize;

    public GameObject[,] gridTiles;

    public enum Direction { Down, Right, Up, Left };

    private void Start()
    {
        CreateGrid();
    }

    private void CreateGrid()
    {
        gridTiles = new GameObject[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                gridTiles[x, y] = null;
            }
        }
    }

    public Vector3 GetWorldPos(int x, int y)
    {
        return new Vector3(x, 0, y) * tileSize;
    }

    public Vector2 GetGridPos(Vector3 pos)
    {
        return new Vector2(Mathf.Floor(pos.x * tileSize), Mathf.Floor(pos.z * tileSize));
    }

    public GameObject GetValue(int x, int y)
    {
        if (TileOnGrid(x, y))
        {
            return gridTiles[x, y];
        }

        return null;
    }

    public void SetValue(int x, int y, GameObject value)
    {
        if (TileOnGrid(x, y))
        {
            gridTiles[x, y] = value;
        }
    }

    public bool TileOnGrid(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return true;
        }

        return false;
    }

    public float GetRotationAngle(Direction dir)
    {
        if (dir == Direction.Down)
        {
            return 0;
        }
        else if (dir == Direction.Right)
        {
            return 270;
        }
        else if (dir == Direction.Up)
        {
            return 180;
        }
        else if (dir == Direction.Left)
        {
            return 90;
        }

        return 0;
    }
}
