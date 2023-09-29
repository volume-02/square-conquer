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

    private void Start()
    {
        renderer = gameObject.GetComponent<Renderer>();
        Paint();
    }

    private void Paint()
    {
        if(renderer == null)
        {
            return;
        }
        switch (state)
        {
            case TileState.Regular:
                renderer.material = isOffset ? baseMaterial : secondaryMaterial;
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
        state = newState;
        Paint();
    }
}
