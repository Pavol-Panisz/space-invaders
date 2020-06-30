using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageAnim : MonoBehaviour
{
    SpriteRenderer sr;
    Color baseColor;
    [SerializeField] float damageTime = 0.01f;
    void Start()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
        baseColor = sr.color;
    }

    public void AnimateDamage()
    {
        StopCoroutine(MakeRed());
        StartCoroutine(MakeRed());
    }

    IEnumerator MakeRed()
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(damageTime);
        sr.color = baseColor;
    }


}
