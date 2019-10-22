using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryScript : MonoBehaviour
{
    public static InventoryScript instance;

    [Header ("Ammo and aid quantity")]
    public int firstAid;
    public int heavyAmmo;
    public int mediumAmmo;
    public int smallAmmo;

    [Header("Weapons")]
    public GameObject sniper;
    public GameObject assaultRifle;
    public GameObject pistol;

    [Header("Other Settings")]
    [SerializeField] private Camera playerCam;
    [SerializeField] private LayerMask layer;
    [SerializeField] private Text itemName;

    public Text ammoHeavy;
    public Text ammoMedium;
    public Text ammoSmall;
    public Text firstAidText;

    public void Awake()
    {
        itemName.text = "";
        instance = this;
    }

    private void Start()
    {

    }

    private void Update()
    {
        ChangeWeapon();
        PickUp();
        ShowAmmoText();
        UseFirstAid();
    }

    public  void AddHeavy(int toAdd)
    {
        heavyAmmo += toAdd;
    }

    public void AddMedium(int toAdd)
    {
        mediumAmmo += toAdd;
    }

    public void AddSmall(int toAdd)
    {
        smallAmmo += toAdd;
    }

    public void AddAid(int toAdd)
    {
        firstAid += toAdd;
    }

    private void ChangeWeapon()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && !Shooting.instance.isReloading)
        {
            sniper.SetActive(true);
            assaultRifle.SetActive(false);
            pistol.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && !Shooting.instance.isReloading)
        {
            assaultRifle.SetActive(true);
            sniper.SetActive(false);
            pistol.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3) && !Shooting.instance.isReloading)
        {
            pistol.SetActive(true);
            sniper.SetActive(false);
            assaultRifle.SetActive(false);
        }
    }

    void PickUp()
    {
        Ray ray = playerCam.ViewportPointToRay(Vector3.one / 2f);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 2f, layer))
        {
            itemName.text = "Press E to pick up " + hit.collider.name;
            if (Input.GetKeyDown(KeyCode.E))
            {
                //Pick up the item
                try { 
                hit.collider.GetComponent<ItemScript>().PickUp();
                }
                catch(Exception e)
                {
                    Debug.LogError("Nothing to pick up!");
                }
            }
        }
        else
            itemName.text = "";
    }

    public void UseFirstAid()
    {
        if (Input.GetKeyDown(KeyCode.F) && firstAid > 0 && PlayerMove.instance.healthPoints < 100)
        {
            PlayerMove.instance.HealPlayer(33f);
            firstAid--;
        }
    }

    void ShowAmmoText()
    {
            ammoHeavy.text = "" + heavyAmmo;
            ammoMedium.text = "" + mediumAmmo;
            ammoSmall.text = "" + smallAmmo;
            firstAidText.text = "" + firstAid;

    }

}
