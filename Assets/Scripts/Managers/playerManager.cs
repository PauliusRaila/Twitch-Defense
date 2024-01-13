using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerManager : MonoBehaviour {

    public static playerManager instance { get; set; }
    public Transform spawnPoint;
    public GameObject playerPrefab;
    public List<Sprite> classImages = new List<Sprite>();

    void Awake () {
        instance = this;
	}

    public void spawnNewPlayer(string userName , string className)
    {
        GameObject[] playerBuildings = GameObject.FindGameObjectsWithTag("playerBuilding");
        GameObject newPlayer;
        newPlayer = Instantiate(playerPrefab);
        newPlayer.name = userName;
        
        newPlayer.transform.position = spawnPoint.position;
        newPlayer.GetComponent<playerController>().playerName = userName;
        newPlayer.GetComponent<playerController>().changeClass(className);


        foreach (GameObject building in playerBuildings)
        {
            Building b = building.GetComponent<Building>();

            if (!b.isBuildingFull())
            {
                b.AddPlayerToBuilding(newPlayer.name);
                newPlayer.GetComponent<playerController>().enterBuilding(b.roofSpawn);
                break;
            }

        }
    }


}
