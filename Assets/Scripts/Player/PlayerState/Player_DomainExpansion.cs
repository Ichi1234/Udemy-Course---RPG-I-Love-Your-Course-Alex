using UnityEngine;

public class Player_DomainExpansion : PlayerState
{
    private Vector2 originalPosition;
    private float originalGravity;
    private float maxDistanceToGoUp;

    private bool isLevitating;
    private bool createdDomain;

    public Player_DomainExpansion(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        originalPosition = player.transform.position;
        originalGravity = rb.gravityScale;
        maxDistanceToGoUp = GetAvaliableRiseDistance();

        player.health.SetCanTakeDamage(false);
        player.SetVelocity(0, player.riseSpeed);
    }

    public override void Update()
    {
        base.Update();

        if (Vector2.Distance(originalPosition, player.transform.position) >= maxDistanceToGoUp && !isLevitating)
        {
            Levitate();
        }

        if (isLevitating)
        {
            skillManager.domainExpansion.DoSpellCasting();

            if (stateTimer < 0)
            {
                rb.gravityScale = originalGravity;
                isLevitating = false;
                stateMachine.ChangeState(player.fallState);
            }
        }
    }
    public override void Exit()
    {
        base.Exit();

        player.health.SetCanTakeDamage(true);
        createdDomain = false;
    }

    private void Levitate()
    {
        isLevitating = true;
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 0;

        stateTimer = skillManager.domainExpansion.GetDomainDuration();

        if (!createdDomain)
        {
            createdDomain = true;
            skillManager.domainExpansion.CreateDomain();
        }
    }


    private float GetAvaliableRiseDistance()
    {
        RaycastHit2D hit = Physics2D.Raycast(player.transform.position, Vector2.up, player.riseMaxDistance, player.whatIsGround);

        return hit.collider != null ? hit.distance - 1 : player.riseMaxDistance;
    }
}
