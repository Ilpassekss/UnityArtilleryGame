using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Attack Straight Projectile", menuName = "Scriptable Object/Enemies/Attack Straight Projectile")]
public class EnemyAttackStraightProjectile : EnemyAttackSOBase
{
    [SerializeField] Cooldown cooldown;
    public float exitDistance = 16f;
    [SerializeField] GameObject projectile;

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
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.fixedDeltaTime * 3f);
    }

    void Attack()
    {
        if (cooldown.IsCoolingDown) return;

        Vector3 dir = (target.position - transform.position).normalized;
        Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
        rb.AddForce(dir * 32f, ForceMode.Impulse);

        cooldown.StartCooldown();
    }
}
