using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public int width = 30;
    public int height = 30;

    public new GameObject camera;
    public GameObject tilePrefab;
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
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                var spawnedTile = Instantiate(tilePrefab, new Vector3(x, 0, z), Quaternion.identity);

                spawnedTile.name = $"Tile: {x}, {z}";

                var isOffset = (x % 2 == 0 && z % 2 != 0) || (x % 2 != 0 && z % 2 == 0);

                var TileScript = spawnedTile.GetComponent<TileScript>();

                TileScript.Init(isOffset);
            }
        }


        camera.transform.position = new Vector3(width / 2, 26.5f, height / 2 - 0.5f);

    }
}
