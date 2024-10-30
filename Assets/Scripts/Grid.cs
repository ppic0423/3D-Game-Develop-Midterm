using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [Header("맵 설정")]
    [SerializeField] MapShape mapShape = MapShape.Rectangle; // 맵 모양
    [SerializeField] int mapWidth; // 맵 넓이
    [SerializeField] int mapHeight; // 맵 높이

    [Header("육각형 설정")]
    [SerializeField] HexOrientation hexOrientation = HexOrientation.Flat;
    [SerializeField] float hexRadius = 1; // 헥사곤 넓이
    [SerializeField] Material hexMaterial;

    [Header("생성 옵션")]
    [SerializeField] bool addColliders = true;
    [SerializeField] bool drawOutlines = true;
    [SerializeField] Material lineMaterial;
    [SerializeField] Vector3Int[] excludedCoordinates = null; 

    // 내부 변수
    Dictionary<string, Tile> grid = new Dictionary<string, Tile>();
    Mesh hexMesh = null;
    CubeIndex[] directions = new CubeIndex[]
    {
        new CubeIndex(1, -1, 0),
        new CubeIndex(1, 0, -1),
        new CubeIndex(0, 1, -1),
        new CubeIndex(-1, 1, 0),
        new CubeIndex(-1, 0, 1),
        new CubeIndex(0, -1, 1)
    };

    #region Getters and Setters
    public Dictionary<string, Tile> Tiles
    {
        get { return grid; }
    }
    #endregion

    private void Awake()
    {
        GenerateGrid();
    }

    // 그리드 생성
    public void GenerateGrid()
    {
        //Generating a new grid, clear any remants and initialise values
        ClearGrid();
        GetMesh();

        //Generate the grid shape
        switch (mapShape)
        {
            case MapShape.Hexagon:
                GenHexShape();
                break;

            case MapShape.Rectangle:
                GenRectShape();
                break;

            case MapShape.Parrallelogram:
                GenParrallShape();
                break;

            case MapShape.Triangle:
                GenTriShape();
                break;

            default:
                break;
        }
    }
    // 그리드 제거
    public void ClearGrid()
    {
        UnityEngine.Debug.Log("Clearing grid...");
        foreach (var tile in grid)
            DestroyImmediate(tile.Value.gameObject, false);

        grid.Clear();
    }

    private void GetMesh()
    {
        hexMesh = null;
        Tile.GetHexMesh(hexRadius, hexOrientation, ref hexMesh);
    }
    #region 해당 좌표에 있는 타일 반환
    public Tile TileAt(CubeIndex index)
    {
        if (grid.ContainsKey(index.ToString()))
            return grid[index.ToString()];
        return null;
    }
    public Tile TileAt(int x, int y, int z)
    {
        return TileAt(new CubeIndex(x, y, z));
    }
    public Tile TileAt(int x, int z)
    {
        return TileAt(new CubeIndex(x, z));
    }
    #endregion
    #region 근접한 타일 반환
    public List<Tile> Neighbours(Tile tile)
    {
        List<Tile> ret = new List<Tile>();

        if (tile == null)
            return ret;

        CubeIndex o;

        for (int i = 0; i < 6; i++)
        {
            o = tile.index + directions[i];
            if (grid.ContainsKey(o.ToString()))
                ret.Add(grid[o.ToString()]);
        }
        return ret;
    }
    public List<Tile> Neighbours(CubeIndex index)
    {
        return Neighbours(TileAt(index));
    }
    public List<Tile> Neighbours(int x, int y, int z)
    {
        return Neighbours(TileAt(x, y, z));
    }
    public List<Tile> Neighbours(int x, int z)
    {
        return Neighbours(TileAt(x, z));
    }
    #endregion
    #region 주어진 타일 범위 내 타일 반환
    public List<Tile> TilesInRange(Tile center, int range)
    {
        List<Tile> ret = new List<Tile>();
        CubeIndex o;

        for (int dx = -range; dx <= range; dx++)
        {
            for (int dy = Mathf.Max(-range, -dx - range); dy <= Mathf.Min(range, -dx + range); dy++)
            {
                o = new CubeIndex(dx, dy, -dx - dy) + center.index;
                if (grid.ContainsKey(o.ToString()))
                    ret.Add(grid[o.ToString()]);
            }
        }
        return ret;
    }
    public List<Tile> TilesInRange(CubeIndex index, int range)
    {
        return TilesInRange(TileAt(index), range);
    }
    public List<Tile> TilesInRange(int x, int y, int z, int range)
    {
        return TilesInRange(TileAt(x, y, z), range);
    }
    public List<Tile> TilesInRange(int x, int z, int range)
    {
        return TilesInRange(TileAt(x, z), range);
    }
    #endregion
    #region 두 타일의 범위 반환
    public int Distance(CubeIndex a, CubeIndex b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y) + Mathf.Abs(a.z - b.z);
    }
    public int Distance(Tile a, Tile b)
    {
        return Distance(a.index, b.index);
    }
    #endregion
    #region 모양에 따른 육각형 생성
    private void GenHexShape()
    {
        UnityEngine.Debug.Log("Generating hexagonal shaped grid...");

        Tile tile;
        Vector3 pos = Vector3.zero;

        int mapSize = Mathf.Max(mapWidth, mapHeight);

        for (int q = -mapSize; q <= mapSize; q++)
        {
            int r1 = Mathf.Max(-mapSize, -q - mapSize);
            int r2 = Mathf.Min(mapSize, -q + mapSize);
            for (int r = r1; r <= r2; r++)
            {
                // 제외 대상인지 확인
                if (CheckExcluded(q, r))
                {
                    continue;
                }

                switch (hexOrientation)
                {
                    case HexOrientation.Flat:
                        pos.x = hexRadius * 3.0f / 2.0f * q;
                        pos.z = hexRadius * Mathf.Sqrt(3.0f) * (r + q / 2.0f);
                        break;

                    case HexOrientation.Pointy:
                        pos.x = hexRadius * Mathf.Sqrt(3.0f) * (q + r / 2.0f);
                        pos.z = hexRadius * 3.0f / 2.0f * r;
                        break;
                }

                tile = CreateHexGO(pos, ("Hex[" + q + "," + r + "," + (-q - r).ToString() + "]"));
                tile.index = new CubeIndex(q, r, -q - r);
                grid.Add(tile.index.ToString(), tile);
            }
        }
    }
    private void GenRectShape()
    {
        UnityEngine.Debug.Log("Generating rectangular shaped grid...");

        Tile tile;
        Vector3 pos = Vector3.zero;

        switch (hexOrientation)
        {
            case HexOrientation.Flat:
                for (int q = 0; q < mapWidth; q++)
                {
                    int qOff = q >> 1;
                    for (int r = -qOff; r < mapHeight - qOff; r++)
                    {
                        // 제외 대상인지 확인
                        if (CheckExcluded(q, r))
                        {
                            continue;
                        }

                        pos.x = hexRadius * 3.0f / 2.0f * q;
                        pos.z = hexRadius * Mathf.Sqrt(3.0f) * (r + q / 2.0f);
                        tile = CreateHexGO(pos, ("Hex[" + q + "," + r + "," + (-q - r).ToString() + "]"));
                        tile.index = new CubeIndex(q, r, -q - r);
                        grid.Add(tile.index.ToString(), tile);
                    }
                }
                break;

            case HexOrientation.Pointy:
                for (int r = 0; r < mapHeight; r++)
                {
                    int rOff = r >> 1;
                    for (int q = -rOff; q < mapWidth - rOff; q++)
                    {
                        // 제외 대상인지 확인
                        if (CheckExcluded(q, r))
                        {
                            continue;
                        }

                        pos.x = hexRadius * Mathf.Sqrt(3.0f) * (q + r / 2.0f);
                        pos.z = hexRadius * 3.0f / 2.0f * r;

                        tile = CreateHexGO(pos, ("Hex[" + q + "," + r + "," + (-q - r).ToString() + "]"));
                        tile.index = new CubeIndex(q, r, -q - r);
                        grid.Add(tile.index.ToString(), tile);
                    }
                }
                break;
        }
    }
    private void GenParrallShape()
    {
        UnityEngine.Debug.Log("Generating parrellelogram shaped grid...");

        Tile tile;
        Vector3 pos = Vector3.zero;

        for (int q = 0; q <= mapWidth; q++)
        {
            for (int r = 0; r <= mapHeight; r++)
            {
                // 제외 대상인지 확인
                if (CheckExcluded(q, r))
                {
                    continue;
                }

                switch (hexOrientation)
                {
                    case HexOrientation.Flat:
                        pos.x = hexRadius * 3.0f / 2.0f * q;
                        pos.z = hexRadius * Mathf.Sqrt(3.0f) * (r + q / 2.0f);
                        break;

                    case HexOrientation.Pointy:
                        pos.x = hexRadius * Mathf.Sqrt(3.0f) * (q + r / 2.0f);
                        pos.z = hexRadius * 3.0f / 2.0f * r;
                        break;
                }

                tile = CreateHexGO(pos, ("Hex[" + q + "," + r + "," + (-q - r).ToString() + "]"));
                tile.index = new CubeIndex(q, r, -q - r);
                grid.Add(tile.index.ToString(), tile);
            }
        }
    }
    private void GenTriShape()
    {
        UnityEngine.Debug.Log("Generating triangular shaped grid...");

        Tile tile;
        Vector3 pos = Vector3.zero;

        int mapSize = Mathf.Max(mapWidth, mapHeight);

        for (int q = 0; q <= mapSize; q++)
        {
            for (int r = 0; r <= mapSize - q; r++)
            {
                // 제외 대상인지 확인
                if (CheckExcluded(q, r))
                {
                    continue;
                }

                switch (hexOrientation)
                {
                    case HexOrientation.Flat:
                        pos.x = hexRadius * 3.0f / 2.0f * q;
                        pos.z = hexRadius * Mathf.Sqrt(3.0f) * (r + q / 2.0f);
                        break;

                    case HexOrientation.Pointy:
                        pos.x = hexRadius * Mathf.Sqrt(3.0f) * (q + r / 2.0f);
                        pos.z = hexRadius * 3.0f / 2.0f * r;
                        break;
                }

                tile = CreateHexGO(pos, ("Hex[" + q + "," + r + "," + (-q - r).ToString() + "]"));
                tile.index = new CubeIndex(q, r, -q - r);
                grid.Add(tile.index.ToString(), tile);
            }
        }
    }
    #endregion

    private bool CheckExcluded(int x, int y)
    {
        foreach (Vector3Int vector in excludedCoordinates)
        {
            if (vector.x == x && vector.y == y && vector.z == -x - y)
            {
                return true;
            }
        }

        return false;
    }
    // 헥사곤 생성
    private Tile CreateHexGO(Vector3 postion, string name)
    {
        GameObject go = new GameObject(name, typeof(MeshFilter), typeof(MeshRenderer), typeof(Tile));

        if (addColliders)
        {
            go.AddComponent<MeshCollider>();
        }
        if (drawOutlines)
        {
            go.AddComponent<LineRenderer>();
        }

        go.layer = (int)Define.Layer.Cell;
        go.transform.position = postion + transform.position;
        go.transform.parent = this.transform;

        Tile tile = go.GetComponent<Tile>();
        MeshFilter filter = go.GetComponent<MeshFilter>();
        MeshRenderer render = go.GetComponent<MeshRenderer>();

        tile.parentGrid = this;

        filter.sharedMesh = hexMesh;

        // render.material = (hexMaterial) ? hexMaterial : UnityEditor.AssetDatabase.GetBuiltinExtraResource<Material>("Default-Diffuse.mat");
        render.material = hexMaterial;

        if (addColliders)
        {
            MeshCollider col = go.GetComponent<MeshCollider>();
            col.sharedMesh = hexMesh;
        }

        if (drawOutlines)
        {
            LineRenderer lines = go.GetComponent<LineRenderer>();
            lines.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
            lines.receiveShadows = false;

            lines.startWidth = 0.1f;
            lines.endWidth = 0.1f;
            lines.startColor = Color.black;
            lines.endColor = Color.black;
            lines.material = lineMaterial;

            lines.positionCount = 7;

            for (int vert = 0; vert <= 6; vert++)
            {
                Vector3 linePos = Tile.Corner(tile.transform.position, hexRadius, vert, hexOrientation);
                lines.SetPosition(vert, new Vector3(linePos.x, linePos.y + 0.05f, linePos.z));
            }
        }

        return tile;
    }
}

[System.Serializable]
public enum MapShape
{
    Rectangle,
    Hexagon,
    Parrallelogram,
    Triangle
}
[System.Serializable]
public enum HexOrientation
{
    Pointy,
    Flat
}