using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    float dmg;

    private void Start()
    {
        dmg = EnemyScript.instance.enemyDamage;
    }

    private void Update()
    {


    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            StartCoroutine(DealDamage());
        }
    }

    IEnumerator DealDamage()
    {
        PlayerMove.instance.TakeDmg(dmg);
        yield return new WaitForSeconds(0.5f);
        PlayerMove.instance.TakeDmg(dmg);
    }

}
