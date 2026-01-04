using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class Entity_VFX : MonoBehaviour
{
    protected SpriteRenderer sr;
    private Entity entity;

    [Header("On Damage VFX")]
    [SerializeField] private Material onDamageMaterial;
    [SerializeField] private float onDamageVfxDuration = .15f;
    private Material originalMaterial;
    private Coroutine onDamageVfxCoroutine;

    [Header("On Doing Damage VFX")]
    [SerializeField] private GameObject hitVfx;
    [SerializeField] private Color hitVfxColor = Color.white;
    [SerializeField] private GameObject critHitVfx;

    [Header("Elemental Colors")]
    [SerializeField] private Color chillVfx = Color.cyan;
    [SerializeField] private Color burnVfx = Color.red;
    [SerializeField] private Color shockVfx = Color.yellow;

    private Color originalHitVfxColor;

    private void Awake()
    {
        entity = GetComponent<Entity>();
        sr = GetComponentInChildren<SpriteRenderer>();
        originalMaterial = sr.material;
        originalHitVfxColor = hitVfxColor;

    }

    public void PlayOnStatusVfx(float duration, ElementTypes element)
    {
        if (element == ElementTypes.Ice)
        {
            StartCoroutine(PlayerStatusVfxCo(duration, chillVfx));
        }

        if (element == ElementTypes.Fire)
        {
            StartCoroutine(PlayerStatusVfxCo(duration, burnVfx));
        }

        if (element == ElementTypes.Lightning)
        {
            StartCoroutine(PlayerStatusVfxCo(duration, shockVfx));
        }
    }

    public void StopAllVfx()
    {
        StopAllCoroutines();
        sr.color = Color.white;
        sr.material = originalMaterial;
    }

    private IEnumerator PlayerStatusVfxCo(float duration, Color effectColor)
    {
        float tickInterval = .25f;
        float timeHasPassed = 0;

        Color lightColor = effectColor * 1.2f;
        Color darkColor = effectColor * .8f;

        bool toggle = false;

        while (timeHasPassed < duration)
        {
            sr.color = toggle ? lightColor : darkColor;
            toggle = !toggle;

            yield return new WaitForSeconds(tickInterval);
            timeHasPassed += tickInterval;
        }

        sr.color = Color.white;
    }

    public Color GetElementColor(ElementTypes element)
    {
        switch (element)
        {
            case ElementTypes.Ice:
                return chillVfx;
            case ElementTypes.Fire:
                return burnVfx;
            case ElementTypes.Lightning:
                return shockVfx;
            default:
                return Color.white;
        }
    }

    public void CreateOnHitVFX(Transform target, bool isCrit, ElementTypes element)
    {
        GameObject hitPrefab = isCrit ? critHitVfx : hitVfx;
        GameObject vfx = Instantiate(hitPrefab, target.position, Quaternion.identity);

        if (!isCrit)
        {
            vfx.GetComponentInChildren<SpriteRenderer>().color = GetElementColor(element);
        }

        if (entity.facingDirection == -1 && isCrit)
        {
            vfx.transform.Rotate(0 , 180, 0);
        }
    }

    public void PlayOnDamageVfx()
    {
        if (onDamageVfxCoroutine != null)
        {
            StopCoroutine(OnDamageVfxCo());
        }
        onDamageVfxCoroutine = StartCoroutine(OnDamageVfxCo());
    }

    private IEnumerator OnDamageVfxCo()
    {
        sr.material = onDamageMaterial;

        yield return new WaitForSeconds(onDamageVfxDuration);

        sr.material = originalMaterial;
    }

}
