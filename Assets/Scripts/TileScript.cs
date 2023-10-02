using UnityEngine;

public enum TileState
{
    Regular,
    Trajectory,
    Filled
}

public class TileScript : MonoBehaviour
{
    private new Renderer renderer;

    public bool isOffset { get; set; }
    public TileState state { get; set; } = TileState.Regular;

    public Material baseMaterial, secondaryMaterial, trajectoryMaterial, fillMaterial;

    public Material borderMaterial;

    public int x { get; set; }
    public int z { get; set; }
    public bool isBorder { get; set; }
    public TileScript prevBorder { get; set; }
    public TileScript nextBorder { get; set; }
    public Vector3 direction { get; set; }
    public bool isTurn { get; set; }

    private void Start()
    {
        renderer = gameObject.GetComponent<Renderer>();
        Paint();
    }

    public void Paint()
    {
        if (renderer == null)
        {
            return;
        }
        switch (state)
        {
            case TileState.Regular:
                if (isBorder)
                {
                    renderer.material = borderMaterial;
                }
                else
                {
                    renderer.material = isOffset ? baseMaterial : secondaryMaterial;
                }
                break;
            case TileState.Trajectory:
                renderer.material = trajectoryMaterial;
                break;
            case TileState.Filled:
                renderer.material = fillMaterial;
                break;
        }
    }

    public void ChangeState(TileState newState)
    {
        if (state != TileState.Filled)
        {
            state = newState;
            Paint();
        }
    }

    public void SetTrajectory(Vector3 dir)
    {
        ChangeState(TileState.Trajectory);
        direction = dir;
    }
}
