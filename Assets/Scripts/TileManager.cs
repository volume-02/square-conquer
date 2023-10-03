using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public int width = 30;
    public int height = 30;

    public GameObject camera;
    public GameObject tilePrefab;
    public Material fillColor;

    private TileScript[][] tiles;

    private TileScript startTrajectory;
    private TileScript endTrajectory;

    private List<TileScript> trajectory = new List<TileScript>();
    void Start()
    {
        GenerateGrid();
        ChangeTileState(0, 0, TileState.Filled);
        RecalculateBorders();
    }

    private void GenerateGrid()
    {
        tiles = new TileScript[width][];
        for (int x = 0; x < width; x++)
        {
            tiles[x] = new TileScript[height];
            for (int z = 0; z < height; z++)
            {
                var spawnedTile = Instantiate(tilePrefab, new Vector3(x, 0, z), Quaternion.identity);

                spawnedTile.name = $"Tile: {x}, {z}";

                var isOffset = (x % 2 == 0 && z % 2 != 0) || (x % 2 != 0 && z % 2 == 0);

                var tileScript = spawnedTile.GetComponent<TileScript>();
                tileScript.isOffset = isOffset;
                tileScript.x = x;
                tileScript.z = z;
                tiles[x][z] = tileScript;
            }
        }


        //camera.transform.position = new Vector3(width / 2, 26.5f, height / 2 - 0.5f);
    }

    private void RecalculateBorders()
    {
        TileScript firstTs = null;
        TileScript nextTs = null;
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                var tileScript = tiles[x][z];
                tileScript.isBorder = false;
                if (tileScript.state == TileState.Filled)
                {
                    continue;
                }
                if (x == 0 || z == 0 || x == width - 1 || z == height - 1)
                {
                    tileScript.isBorder = true;
                    firstTs = tileScript;
                }
                if ((x - 1 >= 0 && z - 1 >= 0 && tiles[x - 1][z - 1].state == TileState.Filled) ||
                    (z - 1 >= 0 && tiles[x][z - 1].state == TileState.Filled) ||
                    (x + 1 < width && z - 1 >= 0 && tiles[x + 1][z - 1].state == TileState.Filled) ||
                    (x + 1 < width && tiles[x + 1][z].state == TileState.Filled) ||
                    (x + 1 < width && z + 1 < height && tiles[x + 1][z + 1].state == TileState.Filled) ||
                    (z + 1 < height && tiles[x][z + 1].state == TileState.Filled) ||
                    (x - 1 >= 0 && z + 1 < height && tiles[x - 1][z + 1].state == TileState.Filled) ||
                    (x - 1 >= 0 && z - 1 >= 0 && tiles[x - 1][z].state == TileState.Filled))
                {
                    tileScript.isBorder = true;
                    firstTs = tileScript;
                }
                tileScript.Paint();
            }
        }

        while (nextTs != firstTs)
        {
            if (nextTs == null)
            {
                nextTs = firstTs;
            }
            var x = nextTs.x;
            var z = nextTs.z;

            for (var lx = x - 1; lx <= x + 1; lx++)
            {
                var br = false;
                for (var lz = z - 1; lz <= z + 1; lz++)
                {
                    if (lz == z && lx == x)
                    {
                        continue;
                    }
                    var tile = fillTile(nextTs, lx, lz);
                    if (tile != null)
                    {
                        nextTs = tile;
                        br = true;
                        break;
                    }
                }
                if (br)
                {
                    break;
                }
            }
        }
    }

    TileScript fillTile(TileScript nextTs, int x, int z)
    {
        if (x < 0 || x >= width || z < 0 || z >= height)
        {
            return null;
        }
        var tile = tiles[x][z];
        if (tile.prevBorder != null || !tile.isBorder || tile.nextBorder == nextTs)
        {
            return null;
        }
        tile.prevBorder = nextTs;
        nextTs.nextBorder = tile;
        return tile;
    }

    public void ChangeTileState(int x, int z, TileState state)
    {
        if (state == TileState.Trajectory && tiles[x][z].isBorder)
        {
            if (startTrajectory == null)
            {
                startTrajectory = tiles[x][z];
            }
            else
            {
                endTrajectory = tiles[x][z];
            }
        }
        tiles[x][z].ChangeState(state);
    }

    public void SetTrajectory(int x, int z, Vector3 dir, bool prefill = false)
    {
        if (tiles[x][z].isBorder && !prefill)
        {
            if (startTrajectory == null)
            {
                startTrajectory = tiles[x][z];
            }
            else
            {
                endTrajectory = tiles[x][z];
            }
        }
        if (tiles[x][z].state == TileState.Regular)
        {
            trajectory.Add(tiles[x][z]);
        }
        tiles[x][z].SetTrajectory(dir);
    }

    public void FillTiles()
    {

        var directCount = 0;
        var curr = startTrajectory;
        while (curr != endTrajectory && curr != null)
        {
            if (curr.state == TileState.Regular)
            {
                directCount++;
            }
            curr = curr.nextBorder;
        }
        var backCount = 0;
        curr = startTrajectory;
        while (curr != endTrajectory && curr != null)
        {
            if (curr.state == TileState.Regular)
            {
                backCount++;
            }
            curr = curr.prevBorder;
        }

        if (directCount <= backCount)
        {
            curr = endTrajectory;
            while (curr != startTrajectory && curr != null)
            {
                SetTrajectory(curr.x, curr.z, Vector3.right, true);
                curr = curr.prevBorder;
            }

        }
        else
        {
            curr = endTrajectory;
            while (curr != startTrajectory && curr != null)
            {
                SetTrajectory(curr.x, curr.z, Vector3.right, true);
                curr = curr.nextBorder;
            }
        }

        Vector3 dir = Vector3.zero;
        for (int i = 0; i < trajectory.Count; i++)
        {
            var cur = trajectory[i];
            var next = trajectory[(i + 1) % trajectory.Count];
            var dirrr = new Vector3(next.x, 0, next.z) - new Vector3(cur.x, 0, cur.z);
            cur.SetTrajectory(dirrr);
            if (dir != Vector3.zero)
            {
                if (dirrr != dir)
                {
                    cur.isTurn = true;
                }
                else
                {
                    cur.isTurn = false;
                }
            }
            dir = dirrr;
        }
        for (int z = 0; z < height; z++)
        {
            var inBlock = false;
            for (int x = 0; x < width; x++)
            {
                var tileScript = tiles[x][z];
                if (tileScript.state == TileState.Trajectory)
                {
                    ChangeTileState(x, z, TileState.Filled);
                    if ((tileScript.direction == Vector3.forward || tileScript.direction == Vector3.back) && !tileScript.isTurn)
                    {
                        inBlock = !inBlock;
                    }
                }
                if (inBlock)
                {
                    ChangeTileState(x, z, TileState.Filled);
                }
            }
        }

        RecalculateBorders();
        trajectory = new List<TileScript>();
        startTrajectory = null;
        endTrajectory = null;
    }
}
