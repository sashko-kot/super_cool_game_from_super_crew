using UnityEngine;

public class Tile : MonoBehaviour
{

    public enum TileType
    {
        Neutral,
        Danger,
        Reward,
        Shop,
        Item,
        Event,
        Forge
    }

    [SerializeField] private TileType tileType;

    private Tile previous { get; set; }
    private Tile next { get; set; }

    void Start()
    {
        Transform myTransform = GetComponent<Transform>();
    }

    void Update()
    {
        
    }

    public TileType getTileType() { return tileType; }
}
