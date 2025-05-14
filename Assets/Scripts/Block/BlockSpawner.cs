using NUnit.Framework.Constraints;
using System.Collections;
using System.Linq;
using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    /// <summary>
    /// 블록 스폰을 시작합니다.
    /// </summary>
    public void StartBlockSpawn()
    {
        // 초기 스폰 위치 설정
        spawnPoint = new Vector3(0, -1, maxBlockLength * 0.5f);

        CreateBlock(maxBlockLength, spawnPoint);

        GameObject secondBlock = CreateBlock(maxBlockLength, spawnPoint + Vector3.forward * (blockSpawnDistance + maxBlockLength));

        StartCoroutine(SpawnBlockCoroutine(secondBlock.transform));
    }

    /// <summary>
    /// 블록 스폰을 중지합니다.
    /// </summary>
    public void StopSpawn()
    {
        StopAllCoroutines();
        foreach (Transform block in blockManager.BlocksTrf.ToList())
        {
            Destroy(block.gameObject);
        }
        blockManager.BlocksTrf.Clear();
    }

    private IEnumerator SpawnBlockCoroutine(Transform prevBlock)
    {
        int blockLength = Random.Range(minBlockLenght, maxBlockLength);

        GameObject newBlock = CreateBlock(blockLength, prevBlock.localPosition + Vector3.forward * (blockSpawnDistance + (prevBlock.localScale.z + blockLength) * 0.5f));

        float spawnDelay = (blockLength + blockSpawnDistance) / blockManager.BlockMoveSpeed;
        yield return new WaitForSeconds(spawnDelay);

        StartCoroutine(SpawnBlockCoroutine(newBlock.transform));
    }

    private GameObject CreateBlock(int blockLength, Vector3 position)
    {        
        GameObject newBlock = Instantiate(blockPrefab, Vector3.zero, Quaternion.identity, transform);

        newBlock.transform.localPosition = position;

        Vector3 blockScale = newBlock.transform.lossyScale;
        blockScale.z = blockLength;
        newBlock.transform.localScale = blockScale;

        blockManager.BlocksTrf.Add(newBlock.transform);

        return newBlock;
    }

    public BlockManager blockManager;

    public GameObject blockPrefab;

    [SerializeField]
    private float blockSpawnDistance = 7.0f;

    [SerializeField]
    int maxBlockLength;
    [SerializeField]
    int minBlockLenght;

    private Vector3 spawnPoint;
}
