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
    /// actions�� ��Ÿ���� ��:
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
            // �÷��̾��� ��ġ
            sensor.AddObservation(transform.localPosition.y);
            // ���� ����� ��ġ
            sensor.AddObservation(steppingBlockTrf.localPosition.z);
            // ���� ����� ũ��
            sensor.AddObservation(steppingBlockTrf.lossyScale.z);

            // ���� ����� ��ġ
            sensor.AddObservation(nextBlockTrf.localPosition.z);
            // ���� ����� ũ��
            sensor.AddObservation(nextBlockTrf.lossyScale.z);
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        // Ű���� �Է��� ���� jump �׼��� �����մϴ�.
        var discreteActions = actionsOut.DiscreteActions;
        discreteActions.Clear();
        discreteActions[0] = Input.GetKey(KeyCode.Space) ? 1 : 0;
    }

    private void ResetSpawn()
    {
        // �÷��̾� ��ġ ����
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.localPosition = new Vector3(0, 0, 0);

        // ��� �Ŵ��� �ʱ�ȭ
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
            // �ٸ� ��Ͽ� �浹���� ��
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
            // �÷��̾ �ٴڿ� �������� ��
            AddReward(-0.2f);
            ResetSpawn();
        }
    }
}
