using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public int width = 30;
    public int height = 30;

    public new GameObject camera;
    public GameObject tilePrefab;
    public Material fillColor;

    private TileScript[][] tiles;
    // Start is called before the first frame update
    void Start()
    {
        GenerateGrid();
    }

    // Update is called once per frame
    void Update()
    {

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

                tileScript.Init(isOffset);
                tiles[x][z] = tileScript;
            }
        }


        camera.transform.position = new Vector3(width / 2, 26.5f, height / 2 - 0.5f);

    }

    public void FillTile(int x, int z)
    {
        tiles[x][z].Fill();
    }
}
