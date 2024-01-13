using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entrance : MonoBehaviour {
    public float distanceToEnter = 0.6f;

    Building parentBuilding;


    private void Awake()
    {
        parentBuilding = GetComponentInParent<Building>();
        InvokeRepeating("checkIfPlayerWantsToEnter", 0f, 0.5f);
    }

    void checkIfPlayerWantsToEnter() {

        if (GameObject.FindGameObjectWithTag("Player") != null) {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

            foreach (GameObject playerObject in players)
            {
                float distanceToPlayer = Vector3.Distance(playerObject.transform.position, transform.position);

                if (distanceToPlayer <= distanceToEnter)
                {
                    playerController player = playerObject.GetComponent<playerController>();

                    if (parentBuilding.checkIfPlayerInside(player.name))
                    {
                        if (!player.isPlayerInside)
                            player.enterBuilding(parentBuilding.roofSpawn);
                            
                    }

                }

            }
        }       

    }

}
