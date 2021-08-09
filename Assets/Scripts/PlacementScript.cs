using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementScript : MonoBehaviour
{
    public BuildingGrid buildingGrid;
    public LayerMask placeableLayer;

    public RoadType[] roadTypes;

    public BuildingGrid.Direction dir;

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, 100, placeableLayer))
        {
            Vector2 gridPos = buildingGrid.GetGridPos(hitInfo.point);

            int x = (int)gridPos.x;
            int y = (int)gridPos.y;

            Vector3 placePos = buildingGrid.GetWorldPos(x, y) + new Vector3(buildingGrid.tileSize, 0, buildingGrid.tileSize) * 0.5f;

            if (Input.GetMouseButtonDown(0))
            {
                PlaceObject(placePos, gridPos);
            }
            else if (Input.GetMouseButtonDown(1))
            {
                DestroyObject(gridPos);
            }
        }
    }

    private void PlaceObject(Vector3 placePos, Vector2 gridPos)
    {
        int x = (int)gridPos.x;
        int y = (int)gridPos.y;

        if (!buildingGrid.TileOnGrid(x, y))
        {
            return;
        }

        if (buildingGrid.GetValue(x, y) == null)
        {
            GameObject objectToPlace = GetRoadType(x, y);
            Place(placePos, Quaternion.Euler(0, buildingGrid.GetRotationAngle(dir), 0), gridPos, objectToPlace);

            for (int i = 1; i < 5; i++)
            {
                Vector2 checkPos;

                switch (i)
                {
                    default:
                    case 1:
                        // down
                        checkPos = new Vector2(x, y - 1);
                        break;
                    case 2:
                        // right
                        checkPos = new Vector2(x + 1, y);
                        break;
                    case 3:
                        // up
                        checkPos = new Vector2(x, y + 1);
                        break;
                    case 4:
                        // left
                        checkPos = new Vector2(x - 1, y);
                        break;
                }

                int checkX = (int)checkPos.x;
                int checkY = (int)checkPos.y;

                if (buildingGrid.GetValue(checkX, checkY) != null)
                {
                    if (buildingGrid.GetValue(checkX, checkY).name.Contains("Road"))
                    {
                        Vector3 thisPlacePos = buildingGrid.GetWorldPos(checkX, checkY) + new Vector3(buildingGrid.tileSize, 0, buildingGrid.tileSize) * 0.5f;
                        GameObject thisObjectToPlace = GetRoadType(checkX, checkY);

                        Place(thisPlacePos, Quaternion.Euler(0, buildingGrid.GetRotationAngle(dir), 0), checkPos, thisObjectToPlace);
                    }
                }
            }
        }
    }

    private void DestroyObject(Vector2 gridPos)
    {
        int x = (int)gridPos.x;
        int y = (int)gridPos.y;

        if (!buildingGrid.TileOnGrid(x, y))
        {
            return;
        }

        if (buildingGrid.GetValue(x, y) != null)
        {
            Destroy(buildingGrid.gridTiles[x, y]);
            buildingGrid.SetValue(x, y, null);

            for (int i = 1; i < 5; i++)
            {
                Vector2 checkPos;

                switch (i)
                {
                    default:
                    case 1:
                        // down
                        checkPos = new Vector2(x, y - 1);
                        break;
                    case 2:
                        // right
                        checkPos = new Vector2(x + 1, y);
                        break;
                    case 3:
                        // up
                        checkPos = new Vector2(x, y + 1);
                        break;
                    case 4:
                        // left
                        checkPos = new Vector2(x - 1, y);
                        break;
                }

                int checkX = (int)checkPos.x;
                int checkY = (int)checkPos.y;

                if (buildingGrid.GetValue(checkX, checkY) != null)
                {
                    if (buildingGrid.GetValue(checkX, checkY).name.Contains("Road"))
                    {
                        Vector3 thisPlacePos = buildingGrid.GetWorldPos(checkX, checkY) + new Vector3(buildingGrid.tileSize, 0, buildingGrid.tileSize) * 0.5f;
                        GameObject thisObjectToPlace = GetRoadType(checkX, checkY);

                        Place(thisPlacePos, Quaternion.Euler(0, buildingGrid.GetRotationAngle(dir), 0), checkPos, thisObjectToPlace);
                    }
                }
            }
        }
    }

    private void Place(Vector3 placePos, Quaternion placeRot, Vector2 gridPos, GameObject objectToPlace)
    {
        int x = (int)gridPos.x;
        int y = (int)gridPos.y;

        if (!buildingGrid.TileOnGrid(x, y))
        {
            return;
        }

        if (buildingGrid.GetValue(x, y) != null)
        {
            Destroy(buildingGrid.GetValue(x, y));
            buildingGrid.SetValue(x, y, null);
        }

        GameObject placedObject = Instantiate(objectToPlace, placePos, placeRot);
        buildingGrid.SetValue(x, y, placedObject);
    }

    private GameObject GetRoadType(int x, int y)
    {
        bool down = buildingGrid.GetValue(x, y - 1) != null;
        bool right = buildingGrid.GetValue(x + 1, y) != null;
        bool up = buildingGrid.GetValue(x, y + 1) != null;
        bool left = buildingGrid.GetValue(x - 1, y) != null;

        foreach (RoadType road in roadTypes)
        {
            if (road.down == down && road.right == right && road.up == up && road.left == left)
            {
                dir = BuildingGrid.Direction.Down;
                return road.roadPrefab;
            }
            if (road.down == right && road.right == up && road.up == left && road.left == down)
            {
                dir = BuildingGrid.Direction.Right;
                return road.roadPrefab;
            }
            if (road.down == up && road.right == left && road.up == down && road.left == right)
            {
                dir = BuildingGrid.Direction.Up;
                return road.roadPrefab;
            }
            if (road.down == left && road.right == down && road.up == right && road.left == up)
            {
                dir = BuildingGrid.Direction.Left;
                return road.roadPrefab;
            }
        }

        return null;
    }

    [System.Serializable]
    public class RoadType
    {
        public GameObject roadPrefab;
        public bool down;
        public bool right;
        public bool up;
        public bool left;
    }
}
