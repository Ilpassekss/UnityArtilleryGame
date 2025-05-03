using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public enum AnimationTriggerType
    {
        EnemyDamaged,

    }

    [Header("Scriptable Objects")]
    #region ScriptableObject Variables

    [SerializeField] EnemyChaseSOBase enemyChaseBase;
    [SerializeField] EnemyAttackSOBase enemyAttackBase;

    public EnemyChaseSOBase enemyChaseBaseInstance { get; set; }
    public EnemyAttackSOBase enemyAttackBaseInstance { get; set; }

    #endregion

    #region State Machine Variables

    public EnemyStateMachine stateMachine { get; set; }
    public EnemyChaseState chaseState { get; set; }
    public EnemyAttackState attackState { get; set; }

    #endregion

    void Awake()
    {
        enemyChaseBaseInstance = Instantiate(enemyChaseBase);
        enemyAttackBaseInstance = Instantiate(enemyAttackBase);

        stateMachine = new EnemyStateMachine();
        chaseState = new EnemyChaseState(this, stateMachine);
        attackState = new EnemyAttackState(this, stateMachine);
    }

    void Start()
    {
        stateMachine.Init(chaseState);
        enemyChaseBaseInstance.Init(gameObject, this);
        enemyAttackBaseInstance.Init(gameObject, this);
    }

    void FixedUpdate()
    {
        stateMachine.currentEnemyState.Update();
    }

    void OnDrawGizmosSelected()
    {
        if (enemyChaseBaseInstance)
            enemyChaseBaseInstance.DrawGizmo();

        if (enemyAttackBaseInstance)
            enemyAttackBaseInstance.DrawGizmo();
    }

    // void Chase()
    // {
    //     agent.SetDestination(target.position);
    // }

    // void FaceTarget()
    // {

    // }

    // void Attack()
    // {
    //     if (!attacked)
    //     {
    //         Vector3 dir = (target.position - transform.position).normalized;

    //         // Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
    //         // rb.AddForce(dir * 32f, ForceMode.Impulse);

    //         attacked = true;
    //         Invoke(nameof(ResetAttack), enemyData.attackCooldown);
    //     }
    // }

    // void ResetAttack()
    // {
    //     attacked = false;
    // }
}
