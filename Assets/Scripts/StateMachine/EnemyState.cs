using UnityEngine;

public class EnemyState : EntityState
{
    protected Enemy enemy;
    public EnemyState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(stateMachine, animBoolName)
    {
        this.enemy = enemy;

        rb = enemy.rb;
        anim = enemy.anim;
        stats = enemy.stats;
    }

    public override void Update()
    {
        base.Update();

        
    }

    public override void UpdateAnimationParameters()
    {
        base.UpdateAnimationParameters();

        float battleAnimSpeedMultiplier = enemy.battleMoveSpeed / enemy.moveSpeed;

        anim.SetFloat("battleAnimSpeedMultipiler", battleAnimSpeedMultiplier);
        anim.SetFloat("moveAnimSpeedMultipiler", enemy.moveAnimSpeedMultipiler);
        anim.SetFloat("xVelocity", rb.linearVelocity.x);
    }
}
