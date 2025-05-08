using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    private void Awake()
    {
        if (instance != null)
        {
            if (instance != this)
                Destroy(this);
        }
        else
            instance = this;
    }

    static BlockManager instance;
    public static BlockManager Instance => instance;

    private List<Transform> blocksTrf = new();
    public List<Transform> BlocksTrf => blocksTrf;

    [SerializeField] private float blockMoveSpeed = 10.0f;
    public float BlockMoveSpeed => blockMoveSpeed;
}
