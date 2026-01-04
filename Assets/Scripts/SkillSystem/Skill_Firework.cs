using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class FireworkTail
{
    public Vector3 lastSpawnPos;
    public Coroutine coroutine;
    public List<GameObject> fireworks = new List<GameObject>();
    public GameObject lastFirework;

    public SkillObject_FireworkRocketHead rocket;
}

public class Skill_Firework : Skill_Base
{
    [SerializeField] private GameObject fireworkRocketPrefab;
    [SerializeField] private GameObject fireworkPrefab;
    [SerializeField] private int fireworkCount = 10;
    [SerializeField] private float spawnInterval = 0.1f;

    private List<FireworkTail> activeTails = new List<FireworkTail>();


    public override void TryUseSkill()
    {
        base.TryUseSkill();

        FireworkTail newTail = new FireworkTail();
        newTail.lastSpawnPos = transform.position;

        newTail.coroutine = StartCoroutine(FireworkTailCoroutine(newTail));
        activeTails.Add(newTail);
    }

    private IEnumerator FireworkTailCoroutine(FireworkTail tail)
    {
        Vector3 spawnPos = tail.lastSpawnPos;

        GameObject firstFirework = Instantiate(
            fireworkPrefab,
            spawnPos,
            Quaternion.identity
        );

        SkillObject_FireworkTails fw = firstFirework.GetComponent<SkillObject_FireworkTails>();

        tail.fireworks.Add(firstFirework);
        tail.lastFirework = firstFirework;

        Vector3 topPosition =
            spawnPos + Vector3.up * (fw.MoveSpeed * fw.LifeTime);


        Color headColor = Random.ColorHSV(0f, 1f, 0.9f, 1f, 1f, 1.5f);
        Color tailColor = headColor * 0.8f;

        GameObject rocketGO = Instantiate(
            fireworkRocketPrefab,
            topPosition,
            Quaternion.identity
        );

        rocketGO.GetComponentInChildren<SpriteRenderer>().color = headColor;

        tail.rocket = rocketGO.GetComponent<SkillObject_FireworkRocketHead>();


        for (int i = 1; i < fireworkCount; i++)
        {
            GameObject firework = Instantiate(
                fireworkPrefab,
                spawnPos,
                Quaternion.identity
            );

            firework.GetComponentInChildren<SpriteRenderer>().color = tailColor;
            tail.fireworks.Add(firework);
            tail.lastFirework = firework;

            yield return new WaitForSeconds(spawnInterval);
        }

        yield return new WaitForSeconds(fw.LifeTime);

        if (tail.rocket != null)
            tail.rocket.ActivateAndExplode();

        activeTails.Remove(tail);
    }


}
