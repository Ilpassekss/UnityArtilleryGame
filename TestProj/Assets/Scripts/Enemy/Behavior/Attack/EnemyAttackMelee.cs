using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Attack Melee", menuName = "Scriptable Object/Enemies/Attack Melee")]
public class EnemyAttackMelee : EnemyAttackSOBase
{
    [SerializeField] Cooldown cooldown;
    public float exitDistance = 2f;
    public float attackRange = 1f;
    public int attackDamage = 10;
    [SerializeField] LayerMask attackableLayer;
    Transform attackTransform;

    public override void Init(GameObject gameObject, EnemyController enemyController)
    {
        base.Init(gameObject, enemyController);

        attackTransform = transform.Find("AttackHitbox");
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

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackTransform.position, attackRange);
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

        var hits = Physics.SphereCastAll(attackTransform.position, attackRange, transform.right, 0, attackableLayer);

        foreach (var hit in hits)
        {
            Debug.Log(hit.collider.name);
            HealthStats healthStat = hit.collider.GetComponent<PlayerHealthStats>();
            if (healthStat) {
                healthStat?.TakeDamage(attackDamage);
            }
        }

        cooldown.StartCooldown();
    }
}
