using UnityEngine;

public class SkillObject_FireworkTails : SkillObject_Base
{
    private AudioSource audioSource;
    public float MoveSpeed => moveSpeed;
    public float LifeTime => lifeTime;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float lifeTime = 1f;

    private float timer;

    private void Update()
    {
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;

        timer += Time.deltaTime;
        if (timer >= lifeTime)
            Destroy(gameObject);
    }
}
