using NUnit.Framework.Constraints;
using System.Collections;
using System.Linq;
using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    private void Awake()
    {
        spawnPoint = new Vector3(0, -1, maxBlockLength * 0.5f);
    }

    void Start()
    {
        GameObject newBlock = CreateBlock(maxBlockLength);
        newBlock.transform.position = new Vector3(0, -1, maxBlockLength * 0.5f);
        BlockManager.Instance.BlocksTrf.Add(newBlock.transform);

        StartCoroutine(SpawnBlockCoroutine());
    }

    private IEnumerator SpawnBlockCoroutine()
    {
        int blockLength = Random.Range(minBlockLenght, maxBlockLength);

        GameObject newBlock = CreateBlock(blockLength);
        newBlock.transform.position = spawnPoint + Vector3.forward * blockLength * 0.5f;
        BlockManager.Instance.BlocksTrf.Add(newBlock.transform);

        float spawnDelay = (blockLength + spawnDelayTerm) / BlockManager.Instance.BlockMoveSpeed;

        yield return new WaitForSeconds(spawnDelay);

        StartCoroutine(SpawnBlockCoroutine());
    }

    private GameObject CreateBlock(int blockLength)
    {        
        GameObject newBlock = Instantiate(blockPrefab, Vector3.zero, Quaternion.identity);
        Vector3 blockScale = newBlock.transform.lossyScale;
        blockScale.z = blockLength;
        newBlock.transform.localScale = blockScale;

        return newBlock;
    }

    public GameObject blockPrefab;

    [SerializeField]
    private float spawnDelayTerm = 1.0f;

    [SerializeField]
    int maxBlockLength;
    [SerializeField]
    int minBlockLenght;

    private Vector3 spawnPoint;
}
