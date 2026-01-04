using UnityEngine;

public class SkillObject_FireworkRocketHead : MonoBehaviour
{
    private Animator anim;

    private void Awake()
    {
        gameObject.SetActive(false); 
    }

    public void ActivateAndExplode()
    {
        gameObject.SetActive(true);

    }

    public void DeleteFireworkHead()
    {
        Destroy(gameObject);
    }
}
