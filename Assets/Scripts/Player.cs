using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class Player : Agent
{
    public BlockManager blockManager;

    public float jumpForce = 5.0f;
    // The player's rigidbody component
    private Rigidbody rb;

    private Transform steppingBlockTrf;

    private Transform nextBlockTrf;

    private bool isJumping = false;

    public override void Initialize()
    {
        rb = GetComponent<Rigidbody>();
    }
    // Called when the agent is reset
    public override void OnEpisodeBegin()
    {
        ResetSpawn();
    }

    /// <summary>
    /// Called when the agent receives an action from the neural network.
    /// 
    /// actions가 나타내는 것:
    /// actions.DiscreteActions[0] jump( 1 = true, 0 = false)
    /// </summary>
    /// <param name="actions"></param>
    public override void OnActionReceived(ActionBuffers actions)
    {
        bool jump = actions.DiscreteActions[0] == 1;

        if (jump && !isJumping)
        {

            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isJumping = true;
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        if (steppingBlockTrf == null)
        {
            sensor.AddObservation(new float[5]);
        }
        else
        {
            // 플레이어의 위치
            sensor.AddObservation(transform.localPosition.y);
            // 현재 블록의 위치
            sensor.AddObservation(steppingBlockTrf.localPosition.z);
            // 현재 블록의 크기
            sensor.AddObservation(steppingBlockTrf.lossyScale.z);

            // 다음 블록의 위치
            sensor.AddObservation(nextBlockTrf.localPosition.z);
            // 다음 블록의 크기
            sensor.AddObservation(nextBlockTrf.lossyScale.z);
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        // 키보드 입력을 통해 jump 액션을 결정합니다.
        var discreteActions = actionsOut.DiscreteActions;
        discreteActions.Clear();
        discreteActions[0] = Input.GetKey(KeyCode.Space) ? 1 : 0;
    }

    private void ResetSpawn()
    {
        // 플레이어 위치 설정
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.localPosition = new Vector3(0, 0, 0);

        // 블록 매니저 초기화
        blockManager.Initialize();

        isJumping = false;

        steppingBlockTrf = null;

        nextBlockTrf = null;
    }

    private void OnCollisionEnter(Collision collision)
    {
        isJumping = false;
        rb.linearVelocity = Vector3.zero;

        if (steppingBlockTrf == null)
        {
            steppingBlockTrf = collision.transform;
            nextBlockTrf = blockManager.GetNextBlock(steppingBlockTrf);
        }
        else
        {
            // 다른 블록에 충돌했을 때
            if (collision.transform != steppingBlockTrf)
            {
                steppingBlockTrf = collision.transform;
                nextBlockTrf = blockManager.GetNextBlock(steppingBlockTrf);
                AddReward(0.1f);
            }
        }
    }

    private void Update()
    {
        if (steppingBlockTrf != null)
        {
            Debug.DrawLine(transform.position, steppingBlockTrf.position, Color.green);
            Debug.DrawLine(transform.position, nextBlockTrf.position, Color.blue);
        }

        if (transform.localPosition.y < -3.0f)
        {
            // 플레이어가 바닥에 떨어졌을 때
            AddReward(-0.2f);
            ResetSpawn();
        }
    }
}
