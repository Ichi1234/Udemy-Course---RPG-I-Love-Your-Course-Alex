using UnityEngine;

public class Enemy_Skeleton : Enemy, ICounterable
{
    public bool CanbeCounter { get =>  canBeStunned; }

    protected override void Awake()
    {
        base.Awake();

        idleState = new Enemy_IdleState(this, stateMachine, "idle");
        moveState = new Enemy_MoveState(this, stateMachine, "move");
        attackState = new Enemy_AttackState(this, stateMachine, "attack");
        battleState = new Enemy_BattleState(this, stateMachine, "battle");
        deathState = new Enemy_DeadState(this, stateMachine, "idle");
        stunnedState = new Enemy_StunState(this, stateMachine, "stunned");
    }
    [ContextMenu("Stun Enemy")]
    public void HandleCounter()
    {
        if (!CanbeCounter)
        {
            return;
        }
        stateMachine.ChangeState(stunnedState);
    }


}
