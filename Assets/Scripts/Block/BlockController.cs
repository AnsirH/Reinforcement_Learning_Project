using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        MoveBlockForDeltaTime();
    }

    private void MoveBlockForDeltaTime()
    {
        if (blockManager.BlocksTrf.Count == 0) return;

        for (int i = 0; i < blockManager.BlocksTrf.Count; i++)
        {
            blockManager.BlocksTrf[i].Translate(Vector3.back * blockManager.BlockMoveSpeed * Time.deltaTime);

            if (blockManager.BlocksTrf[i].transform.localPosition.z < -(blockManager.BlocksTrf[i].transform.lossyScale.z * 0.5f))
            {
                GameObject removedBlock = blockManager.BlocksTrf[i].gameObject;
                blockManager.BlocksTrf.Remove(removedBlock.transform);
                Destroy(removedBlock);
            }
        }
    }

    public BlockManager blockManager;
}
