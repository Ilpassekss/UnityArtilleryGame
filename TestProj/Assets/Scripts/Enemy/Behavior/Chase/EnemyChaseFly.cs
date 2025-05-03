using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "Enemy Chase Fly", menuName = "Scriptable Object/Enemies/Chase Fly")]
public class EnemyChaseFly : EnemyChaseSOBase
{
    public float speed = 10f;
    public float stopDistance = 10f;

    public override void Init(GameObject gameObject, EnemyController enemyController)
    {
        base.Init(gameObject, enemyController);
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

        FaceTarget();
        Vector3 dir = (target.position - transform.position).normalized;
        transform.position += dir * speed * Time.fixedDeltaTime;

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

    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.fixedDeltaTime * 10f);
    }
}
