using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootEffect : MonoBehaviour
{
    private Animator shootEffectAnim;
    private bool shootSign;
    // Start is called before the first frame update
    void Start()
    {
        shootEffectAnim = GetComponent<Animator>();
        Shoot();
    }
    public void Shoot()
    {
        Debug.Log($"shootEffectAnim : {shootEffectAnim}");
        if (shootEffectAnim == null)
        {
            shootEffectAnim = GetComponent<Animator>();
        }
        gameObject.SetActive(true);
        shootEffectAnim.Play("ShootEffect");
        Invoke("EffectEnd",0.5f);
    }

    private void EffectEnd()
    {
        gameObject.SetActive(false);
    }
}
