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
        if (BlockManager.Instance.BlocksTrf.Count == 0) return;

        for (int i = 0; i < BlockManager.Instance.BlocksTrf.Count; i++)
        {
            BlockManager.Instance.BlocksTrf[i].Translate(Vector3.back * BlockManager.Instance.BlockMoveSpeed * Time.deltaTime);

            if (BlockManager.Instance.BlocksTrf[i].transform.position.z < -(BlockManager.Instance.BlocksTrf[i].transform.lossyScale.z * 0.5f))
            {
                GameObject removedBlock = BlockManager.Instance.BlocksTrf[i].gameObject;
                BlockManager.Instance.BlocksTrf.Remove(removedBlock.transform);
                Destroy(removedBlock);
            }
        }
    }
}
