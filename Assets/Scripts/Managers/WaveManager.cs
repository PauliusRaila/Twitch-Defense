using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour
{


    public Text waveText;
    public Text highscoreText;
    public Text nextWaveInText;
    



    public static WaveManager instance { get; set; }
    public List<GameObject> enemyPrefabs = new List<GameObject>();
    public List<GameObject> bossPrefabs = new List<GameObject>();
    public List<Transform> enemySpawnPositions = new List<Transform>();
    List<int> upcomingSpawnPoints = new List<int>();   //  Declare list
    // List<KeyValuePair<double>> enemyPropabilities = new List<KeyValuePair<double>>();

    private int openSpawnPoints;
    private int typesToSpawn;

    public int currentWave;
    public int totalEnemiesAlive;
    public int difficulty;

    public int shelterCount;

    //public int[] enemyTypeAlive;

    public int enemiesToSpawn = 2;


    public float timeAfterWave = 20f;
    private float countdown;

    public int nextWave;

    //BOSS STUFF
    private bool isBossSpawned = false;
    public int wavesTillBoss = 9;
    public Text bossWaveAlert;
    public bool bossWave = false;
    private int bossSpawnPoints;

    void Awake()
    {
        if (instance == null)
            instance = this;

        GameObject[] enemySpawnPoints = GameObject.FindGameObjectsWithTag("enemySpawnPoint");
        foreach (GameObject point in enemySpawnPoints) {
            enemySpawnPositions.Add(point.transform);
        }


      
    }

    void Start()
    {
        StartGame();
    }

    public void StartGame()
    {       
        currentWave = 0;
        nextWave = currentWave + 1;
        openSpawnPoints = 1;
        difficulty = 1;
        countdown = timeAfterWave;


        prepareSpawnPoints();

        StartCoroutine(startCountdownToWave());
    }

    public void EndWave()
    {
        if (bossWave)
        {
            isBossSpawned = false;
            bossWave = false;
            wavesTillBoss = 9;
        }
        else {
            wavesTillBoss -= 1;
            Debug.Log("Wave Till Boss: " + wavesTillBoss);
        }
               
        if (wavesTillBoss == 0)
        {
            Debug.Log("BOSS wave incoming !");
            bossWave = true;
            bossWaveAlert.gameObject.SetActive(true);

        }

            if (currentWave / openSpawnPoints == 5 && openSpawnPoints < 5)
            {

                openSpawnPoints += 1;

                enemiesToSpawn = enemiesToSpawn / openSpawnPoints;
            }


            countdown = timeAfterWave;
            prepareSpawnPoints();

            if(wavesTillBoss == 0)
            StartCoroutine(startCountdownToWave());
            else
            StartCoroutine(startCountdownToWave());
        
            Debug.Log("End Wave");
        

        
    }                     
    

    IEnumerator startCountdownToWave() {


        for (int i = (int)timeAfterWave; i > 0; i--) {
           // Debug.Log("Countdown: " + countdown);
            countdown--;
            nextWaveInText.text = "Next wave in: " + countdown;

            yield return new WaitForSeconds(1f);
        }

        if (countdown == 0)
        {

            currentWave = nextWave;
            nextWave = currentWave + 1;
           
            Debug.Log("Starting wave " + currentWave);
            startWave(currentWave);
        }
    }

    private void prepareSpawnPoints() {
        upcomingSpawnPoints = new List<int>();
        List<int> list = new List<int>();


        for (int i = 0; i < enemySpawnPositions.Count; i++)
        {
            list.Add(i);
        }


        if (bossWave)
        {
 
            bossSpawnPoints = bossPrefabs[Random.Range(0, bossPrefabs.Count - 1)].GetComponent<BossNPC>().bossSpawnPositions;

            for (int i = 0; i < bossSpawnPoints; i++)
            {
                int index = Random.Range(0, list.Count - 1);
                upcomingSpawnPoints.Add(list[index]);
                enemySpawnPositions[list[index]].Find("skull").gameObject.SetActive(true);
                list.RemoveAt(index);

            }

        }
        else {

            for (int i = 0; i < openSpawnPoints; i++)
            {
                int index = Random.Range(0, list.Count - 1);
                upcomingSpawnPoints.Add(list[index]);
                enemySpawnPositions[list[index]].Find("skull").gameObject.SetActive(true);
                list.RemoveAt(index);

            }
        }


        

    }

    public void startWave(int wave) {
        GameObject[] skulls = GameObject.FindGameObjectsWithTag("spawnPointIndicator");
        foreach (GameObject skull in skulls)
        {
            if(skull.active)
            skull.SetActive(false);
        }

        waveText.text = "Wave: " + currentWave;

        if(enemiesToSpawn < 15)
        enemiesToSpawn += 1;

        StartCoroutine(spawnEnemies());


    }


    IEnumerator spawnEnemies()
    {
        if (bossWave)
        {

            for (int a = 0; a < bossSpawnPoints; a++)
            {
                //int index = Random.Range(0, upcomingSpawnPoints.Count - 1);    //  Pick random element from the list
                int spawnPoint = upcomingSpawnPoints[a];    //  i = the number that was randomly picked
                Debug.Log("spawnPoint: " + spawnPoint);
                //upcomingSpawnPoints.RemoveAt(a);   //  Remove chosen element

                if (bossWave)
                {
                    if (!isBossSpawned)
                    {
                        spawnBoss(spawnPoint, Random.Range(0, bossPrefabs.Count - 1));

                    }
                    else
                    {

                    }
                }
            }



        }
        else {

            for (int a = 0; a < openSpawnPoints; a++)
            {
                //int index = Random.Range(0, upcomingSpawnPoints.Count - 1);    //  Pick random element from the list
                int spawnPoint = upcomingSpawnPoints[a];    //  i = the number that was randomly picked
                Debug.Log("spawnPoint: " + spawnPoint);
                //upcomingSpawnPoints.RemoveAt(a);   //  Remove chosen element             

                for (int b = 0; b < enemiesToSpawn; b++)
                {
                    float R = Random.value * 100;
                    // Debug.Log(R);
                    float cumulative = 0.0f;


                    for (int i = 0; i < enemyPrefabs.Count; i++)
                    {

                        cumulative += enemyPrefabs[i].GetComponent<EnemyNPC>().chanceToSpawn;
                        Debug.Log(cumulative);
                        if (R < cumulative)
                        {
                           // Debug.Log("spawn");
                            spawnEnemy(spawnPoint, i);
                            break;

                        }

                        yield return new WaitForSeconds(0.05f);
                    }
                }
            }
        }

    }

    private void spawnEnemy(int spawnPoint ,int enemyType) {          
                Instantiate(enemyPrefabs[enemyType], enemySpawnPositions[spawnPoint].position, enemySpawnPositions[spawnPoint].rotation);
                totalEnemiesAlive++;
       
    }

    private void spawnBoss(int spawnPoint, int bossType)
    {
        Instantiate(bossPrefabs[bossType], enemySpawnPositions[spawnPoint].position, enemySpawnPositions[spawnPoint].rotation);
        isBossSpawned = true;
        totalEnemiesAlive++;
    }

    public void checkWaveStatus() {
       if (totalEnemiesAlive == 0) {
            EndWave();
       }

       
    }

    public void GameOver()
    {
        Debug.Log("Game Over!");
        Application.LoadLevel("map_selection");
    }

}
