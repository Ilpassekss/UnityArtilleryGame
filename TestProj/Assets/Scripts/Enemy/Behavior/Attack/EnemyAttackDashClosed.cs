using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Attack Dash Closed", menuName = "Scriptable Object/Enemies/Attack Dash Closed")]
public class EnemyAttackDashClosed : EnemyAttackSOBase
{
    public float cooldown = 2f;
    public float exitDistance = 10f;
    public float dashPower = 40f;

    float attackTimeCounter = 0f;
    bool attacked = false;
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

        if (Vector3.Distance(target.position, transform.position) > exitDistance)
        {
            ResetAttack();
            enemyController.stateMachine.ChangeState(enemyController.chaseState);
        }

        FaceTarget();
        Attack();

        attackTimeCounter += Time.fixedDeltaTime;
    }

    public override void AnimationTriggerEvent(EnemyController.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }

    public override void Reset()
    {
        base.Reset();
    }

    public override void DrawGizmo()
    {
        base.DrawGizmo();

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, exitDistance);
    }

    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.fixedDeltaTime * 3f);
    }

    void Attack()
    {
        if (attackTimeCounter >= cooldown)
        {
            ResetAttack();
        }

        if (!attacked)
        {
            attacked = true;
            
            Vector3 dir = (target.position - transform.position).normalized;
            gameObject.GetComponent<Rigidbody>().AddForce(dir * dashPower, ForceMode.Impulse);
        }
    }

    void ResetAttack()
    {
        attackTimeCounter = 0f;
        attacked = false;
    }
}
