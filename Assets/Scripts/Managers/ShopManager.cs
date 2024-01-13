using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ShopManager : MonoBehaviour {

    public static ShopManager instance { get; set; }

    public List<GameObject> weaponList = new List<GameObject>();
    private Transform weaponPos;
    private GameObject shopWeapon;
    private Text price;

    private void Awake()
    {
        weaponPos = transform.Find("weaponPos");
        price = transform.Find("weaponPrice").GetComponent<Text>();
        instance = this;
    }

    public void pickNewWeapon() {
        if (shopWeapon)
            Destroy(shopWeapon);
           
        int randomWep = Random.Range(0, weaponList.Count);
        shopWeapon = Instantiate(weaponList[randomWep]);
        shopWeapon.transform.parent = weaponPos;
        //shopWeapon.name = "shopWeapon";
        shopWeapon.transform.localPosition = Vector3.zero;
        price.text = shopWeapon.GetComponent<shopWeapon>().price + "$";
       
    }






}
