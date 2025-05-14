using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    public void Initialize()
    {
        blockSpawner = GetComponentInChildren<BlockSpawner>();

        if (blockSpawner == null)
        {
            Debug.LogError("BlockSpawner not found in children.");
            return;
        }
        else
        {
            blockSpawner.StopSpawn();
            blockSpawner.StartBlockSpawn();
        }
    }

    public Transform GetNextBlock(Transform blockTrf)
    {
        for (int i = 0; i < blocksTrf.Count; i++)
        {
            if (blocksTrf[i] == blockTrf)
            {
                if (i + 1 < blocksTrf.Count)
                {
                    return blocksTrf[i + 1];
                }
                else
                {
                    Debug.LogError("No next block found.");
                    return null;
                }
            }
        }
        Debug.LogError("Block not found in BlocksTrf.");
        return null;
    }

    private List<Transform> blocksTrf = new();
    public List<Transform> BlocksTrf => blocksTrf;    

    [SerializeField] private float blockMoveSpeed = 10.0f;
    public float BlockMoveSpeed => blockMoveSpeed;

    private BlockSpawner blockSpawner;
}
