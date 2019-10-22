using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamage : MonoBehaviour
{
    public GameObject explode;
    public float dmg;
    private EnemyScript enemyScript;
    private Shooting shooting;
    InventoryScript inventory;

    void Awake()
    {
        dmg = FindObjectOfType<Shooting>().gunDamage;
    }

    private void Update()
    {
        dmg = FindObjectOfType<Shooting>().gunDamage;

    }



    private void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == "Enemy")
        {
            col.gameObject.GetComponent<EnemyScript>().TakeDmg(dmg);
        }
        GameObject die = Instantiate(explode, transform.position, transform.rotation);
        Destroy(die, 2);

        gameObject.SetActive(false);
    }



}
