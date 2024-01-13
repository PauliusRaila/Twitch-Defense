using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Building : MonoBehaviour {

    public int id;
    public int maxPlayerSlots = 4;
    public List<string> playersInside = new List<string>();
    public GameObject playerUI;



    
    private Transform playerCanvas;

    [HideInInspector]
    public Transform entrance;
    [HideInInspector]
    public Transform roofSpawn;


    void Awake () {
        gameObject.name = id.ToString();

        roofSpawn = transform.Find("roofSpawn");
        entrance = transform.Find("entrance");
        playerCanvas = transform.Find("playerBuildingCanvas");

	} 

    public bool checkIfPlayerInside(string playerName) {

        if (playersInside.Contains(playerName))
        {
            return true;
        }
        else
            return false;

    }

    public bool isBuildingFull() {
        if (playersInside.Count >= maxPlayerSlots)
            return true;
        else return false;        
    }

    public void AddPlayerToBuilding(string playerName) {
        playerController player = GameObject.Find(playerName).GetComponent<playerController>();
            

        Debug.Log("AddPlayerToBuilding");
        if (!isBuildingFull()) {
            playersInside.Add(playerName);

            //Adding playerUI to the building.
            if (GameObject.Find(playerName + "UI") != null)
            {
                GameObject UI = GameObject.Find(playerName + "UI");
                Vector3 localPos = UI.transform.localPosition;
                Quaternion localRot = UI.transform.localRotation;
                UI.transform.SetParent(playerCanvas);
                UI.transform.localPosition = localPos;
                UI.transform.localRotation = localRot;
                UI.transform.localScale = Vector3.one;
            }
            else {
                GameObject UI = Instantiate(playerUI);
                UI.name = player.name + "UI";
                Vector3 localPos = UI.transform.localPosition;
                Quaternion localRot = UI.transform.localRotation;
                UI.transform.SetParent(playerCanvas);
                UI.transform.localPosition = localPos;
                UI.transform.localRotation = localRot;
                UI.transform.localScale = Vector3.one;

                string playerNameLimited = playerName.Substring(0, 8);
                UI.transform.Find("playerNickname").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = playerNameLimited;
                UI.transform.Find("level").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = player.classLevel.ToString();
                UI.transform.Find("classImage").GetComponent<Image>().sprite = Resources.Load<Sprite>("ClassImages/" + player.currentClass.ToString());
               
            }

            player.currentBuilding = GetComponent<Building>();
            player.moveToBuilding();

        }


            
        else
            Debug.Log("Building " + gameObject.name + " is full at the moment.");
               
    }

    public void deletePlayerFromBuilding(playerController player) {

        if (player.isPlayerInside)
            player.exitBuilding(entrance);     
     
        //Destroy(GameObject.Find(player.name + "UI"));


        playersInside.Remove(player.name);
        player.currentBuilding = null;
    }




}
