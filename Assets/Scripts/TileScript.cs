using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour
{
    public Material baseColor, secondaryColor;

    public void Init(bool isOffset)
    {
        var renderer = gameObject.GetComponent<MeshRenderer>();
        renderer.material = isOffset ? baseColor : secondaryColor;
    }
}
