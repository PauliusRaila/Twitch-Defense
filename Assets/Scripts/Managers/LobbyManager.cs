using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;



public class LobbyManager : MonoBehaviour {

    public float StartTimer = 60f;
    private float countdown;
    public Text timerText;
    public int totalVotes = 0;
    public int[] mapVotes;
    public int mapCount = 0;
    public List<GameObject> Maps = new List<GameObject>();

    public static LobbyManager instance { get; set; }
    // Use this for initialization
    void Awake () {
        if (instance == null)
            instance = this;

        countdown = StartTimer;       
        GameObject[] maps = GameObject.FindGameObjectsWithTag("map");
        foreach (GameObject map in maps)
        {
            //Maps.Add(map);
            mapCount += 1;
        }

        mapVotes = new int[mapCount];

        for (int i = 1; i <= mapCount; i++) {
            Maps.Add(GameObject.Find("map_" + i));
        }

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator startCountdownToNewGame()
    {
        for (int i = (int)StartTimer; i > 0; i--)
        {
            countdown--;
            timerText.text = countdown.ToString();

            yield return new WaitForSeconds(1f);
        }

        if (countdown == 0)
        {
            int highest = -1;
            int mapIndex = 0;
            List<int> favoriteMaps = new List<int>();
            
            for (int i = 0; i < mapVotes.Length; i++)
            {
                if (mapVotes[i] == highest) {
                    Debug.Log("Map: " + (i + 1) + " added to favoriteMaps");
                    favoriteMaps.Add(i + 1);
                }
                else if (mapVotes[i] > highest && mapVotes[i] > 0) {                  
                   // mapIndex = i + 1;
                    highest = mapVotes[i];
                    favoriteMaps = new List<int>();
                    Debug.Log("We found new map with more votes. Clearing favoriteMaps.");
                    favoriteMaps.Add(i + 1);

                    Debug.Log("Map: " + (i + 1) + " added to favoriteMaps");
                }                              

            }

            mapIndex = favoriteMaps[Random.Range(0, favoriteMaps.Count)];

            startGame(mapIndex);

        }
    }

    public void AddVote(int map) {
        if (map - 1 <= mapVotes.Length) {
            totalVotes++;
            mapVotes[map - 1] = mapVotes[map - 1] + 1;
            Debug.Log("Vote added to map: " + map + " || " + "Map " + map + " have " + mapVotes[map - 1] + " votes");
            Maps[map - 1].transform.Find("vote_amount").GetComponent<Text>().text = mapVotes[map - 1].ToString();

        }

        if (totalVotes == 1) {
            StartCoroutine(startCountdownToNewGame());
        }

    }

    public void startGame(int mapIndex) {
        Debug.Log("Starting map " + mapIndex);
        Application.LoadLevel("map_" + mapIndex);
    }


}
