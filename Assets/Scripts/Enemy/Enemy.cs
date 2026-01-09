using System.Collections;
using UnityEngine;

public class Enemy : Entity
{
    public Entity_Stats stats { get; private set; }

    public Enemy_Health health { get; private set; }
    public Enemy_IdleState idleState;
    public Enemy_MoveState moveState;
    public Enemy_AttackState attackState;
    public Enemy_BattleState battleState;
    public Enemy_DeadState deathState;
    public Enemy_StunState stunnedState;

    [Header("Battle Details")]
    public float battleMoveSpeed = 3;
    public float attackDistance = 2;
    public float battleTimeDuration = 5;
    public float lastTimeWasInBattle;
    public float inGameTime;
    public float minRetreatDistance = 1;
    public Vector2 retreatVelocity;

    [Header("Stunned State details")]
    public float stunnedDuration = 1;
    public Vector2 stunedVelocity = new Vector2(7, 7);
    protected bool canBeStunned;

    [Header("Movement Details")]
    public float idleTime = 2;
    public float moveSpeed = 1.4f;
    [Range(0,2)]
    public float moveAnimSpeedMultipiler = 1;

    [Header("Player Dettection")]
    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private Transform playerCheck;
    [SerializeField] private float playerCheckDistance = 10;
    public Transform player { get; private set; }

    public float actitiveSlowMultiplier { get; private set; } = 1;

    public float GetMoveSpeed() => moveSpeed * actitiveSlowMultiplier;
    public float GetBattleMoveSpeed() => battleMoveSpeed * actitiveSlowMultiplier;

    protected override void Awake()
    {
        base.Awake();
        stats = GetComponent<Entity_Stats>();
        health = GetComponent<Enemy_Health>();
    }

    protected override IEnumerator SlowDownEntityCo(float duration, float slowMultiplier)
    {
        actitiveSlowMultiplier = 1 - slowMultiplier;
        anim.speed *= actitiveSlowMultiplier;

        yield return new WaitForSeconds(duration);

    }

    public override void StopSlowdown()
    {
        actitiveSlowMultiplier = 1;
        anim.speed = 1;
        base.StopSlowdown();
    }

    public void EnableCounterWindow(bool enable) => canBeStunned = enable; 

    public override void EntityDeath()
    {
        base.EntityDeath();

        stateMachine.ChangeState(deathState);
    }

    private void HandlePlayerDeath()
    {
        stateMachine.ChangeState(idleState);
    }

    public Transform GetPlayerReference()
    {
        if (!player)
        {
            player = PlayerDetected().transform;
        }

        return player;
    }

    public void TryEnterBattleState(Transform player)
    {
        if (stateMachine.currentState == battleState || stateMachine.currentState == attackState)
        {
            return;
        }
        this.player = player;
        stateMachine.ChangeState(battleState);
    }

    protected override void Update()
    {
        base.Update();

        inGameTime = Time.time;
    }


    public RaycastHit2D PlayerDetected()
    {
        RaycastHit2D hit = Physics2D.Raycast(playerCheck.position, Vector2.right * facingDirection, playerCheckDistance, whatIsPlayer | whatIsGround);

        if (hit.collider == null || hit.collider.gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            return default;
        }
        return hit;
    }
    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(playerCheck.position, new Vector3(playerCheck.position.x + (facingDirection * playerCheckDistance), playerCheck.position.y));
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(playerCheck.position, new Vector3(playerCheck.position.x + (facingDirection * attackDistance), playerCheck.position.y));
        Gizmos.color = Color.green;
        Gizmos.DrawLine(playerCheck.position, new Vector3(playerCheck.position.x + (facingDirection * minRetreatDistance), playerCheck.position.y));

    }

    private void OnEnable()
    {
        Player.OnPlayerDeath += HandlePlayerDeath;
    }

    private void OnDisable()
    {
        Player.OnPlayerDeath -= HandlePlayerDeath;
    }
}
