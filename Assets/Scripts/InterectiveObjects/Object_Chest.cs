using UnityEngine;

public class Object_Chest : MonoBehaviour, IDamagable
{
    private Animator anim => GetComponentInChildren<Animator>();
    private Rigidbody2D rb => GetComponentInChildren<Rigidbody2D>();

    private Entity_VFX fx => GetComponent<Entity_VFX>();

    [Header("Open Details")]
    [SerializeField] private Vector2 knockback;

    public bool TakeDamage(float damage, float elementalDamage, ElementTypes element, Transform damageDealer)
    {
        fx.PlayOnDamageVfx();

        anim.SetBool("chestOpen", true);

        rb.linearVelocity = knockback;

        rb.angularVelocity = Random.Range(-200f, 200f);

        return true;
    }
}
