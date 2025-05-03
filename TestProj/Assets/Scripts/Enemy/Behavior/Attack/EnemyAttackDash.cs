using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Attack Dash", menuName = "Scriptable Object/Enemies/Attack Dash")]
public class EnemyAttackDash : EnemyAttackSOBase
{
    [SerializeField] Cooldown cooldown;
    public float exitDistance = 10f;
    public float dashPower = 40f;

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
            enemyController.stateMachine.ChangeState(enemyController.chaseState);
        }

        FaceTarget();
        Attack();
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
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.fixedDeltaTime * 5f);
    }

    void Attack()
    {
        if (cooldown.IsCoolingDown) return;

        Vector3 dir = (target.position - transform.position).normalized;
        gameObject.GetComponent<Rigidbody>().AddForce(dir * dashPower, ForceMode.Impulse);

        cooldown.StartCooldown();
    }
}
