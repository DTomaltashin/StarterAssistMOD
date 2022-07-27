using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Pickup : MonoBehaviour
{
    public bool isWeapon = false;
    public bool isPickUp = false;
    public bool weaponactive = false;

    [SerializeField] private GameObject text;
    [SerializeField] private GameObject Weapon_Axe;

    private void Start()
    {
        text.SetActive(false);
    }

    void Update()
    {
    }

    private void OnTriggerStay(Collider other)
    {
        if (isWeapon)
        {
            if (other.tag == "Player")
            {
                text.SetActive(true);
                PickWeapon();
            }
        }
        if (isPickUp)
        {
            if (other.tag == "Player")
            {
                text.SetActive(true);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        text.SetActive(false);   
    }

     void PickWeapon()
    {
        if (isWeapon)
        {
            var keyboard = Keyboard.current;
            if (keyboard == null)
                return;
            if (keyboard.fKey.wasPressedThisFrame)
            {
                //Destroy(gameObject);
                Weapon_Axe.SetActive(true);
                weaponactive = true ;          
            }
        }
    }
}
