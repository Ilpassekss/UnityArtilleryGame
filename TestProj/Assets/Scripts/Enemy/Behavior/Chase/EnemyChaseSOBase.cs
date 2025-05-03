using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseSOBase : ScriptableObject
{
    protected Transform target;
    protected Transform transform;
    protected GameObject gameObject;
    protected EnemyController enemyController;

    public virtual void Init(GameObject gameObject, EnemyController enemyController)
    {
        target = PlayerManager.instance.player.transform;
        
        this.gameObject = gameObject;
        transform = gameObject.transform;
        this.enemyController = enemyController;
    }

    public virtual void Enter() {}
    public virtual void Exit() {}
    public virtual void Update() {}
    public virtual void AnimationTriggerEvent(EnemyController.AnimationTriggerType triggerType) {}
    public virtual void Reset() {}
    public virtual void DrawGizmo() {}
}
