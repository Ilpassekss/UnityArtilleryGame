using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "Enemy Chase Simple", menuName = "Scriptable Object/Enemies/Chase Simple")]
public class EnemyChaseSimple : EnemyChaseSOBase
{
    public float speed = 10f;
    public float stopDistance = 2f;

    NavMeshAgent agent;

    public override void Init(GameObject gameObject, EnemyController enemyController)
    {
        base.Init(gameObject, enemyController);

        agent = gameObject.GetComponent<NavMeshAgent>();
        agent.stoppingDistance = stopDistance;
        agent.speed = speed;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        agent.SetDestination(target.position);

        float distance = Vector3.Distance(target.position, transform.position);
        if (distance <= stopDistance)
        {
            enemyController.stateMachine.ChangeState(enemyController.attackState);
        }
    }

    public override void AnimationTriggerEvent(EnemyController.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }

    public override void DrawGizmo()
    {
        base.DrawGizmo();

        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(transform.position, stopDistance);
    }
}
