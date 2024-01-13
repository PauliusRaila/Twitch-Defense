using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Net.Sockets;
using System.ComponentModel;
using System.IO;

public class TwitchChat : MonoBehaviour {

    private TcpClient twitchClient;
    private StreamReader reader;
    private StreamWriter writer;
   

    public string username, password, channelName;
    //public Text chatBox;

    public static TwitchChat instance { get; set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else {
            Destroy(this);
        }
        
    }

    // Use this for initialization
    void Start () {
        Connect();
	}
	
	// Update is called once per frame
	void Update () {
        if (!twitchClient.Connected) {
            Debug.Log("Twitch Client disconnected, trying to reconnect.");
            Connect();
        }

        ReadChat();
	}

    private void Connect()
    {
        twitchClient = new TcpClient("irc.chat.twitch.tv", 6667);
        reader = new StreamReader(twitchClient.GetStream());
        writer = new StreamWriter(twitchClient.GetStream());

        writer.WriteLine("PASS " + password);
        writer.WriteLine("NICK " + username);
        writer.WriteLine("USER " + username + " 8 * :" + username);
        writer.WriteLine("JOIN #" + channelName);
        writer.Flush();
    }

    private void ReadChat() {
        if (twitchClient.Available > 0) {
            var message = reader.ReadLine();
            print(message);

            if (message.Contains("PRIVMSG")) {
                var splitPoint = message.IndexOf("!", 1);
                var chatName = message.Substring(0, splitPoint);
                chatName = chatName.Substring(1);

                splitPoint = message.IndexOf(":", 1);
                message = message.Substring(splitPoint + 1);
                print(message);
                //print(String.Format("{0}: {1}", chatName, message));
                
                //chatBox.text = chatBox.text + "\n" + String.Format("{0}: {1}", chatName, message);

                GameInputs(chatName, message);
            }

        }
    }

    private void GameInputs(string userName, string ChatInput)
    {
        //Spawn new player.
        var splitPoint = ChatInput.IndexOf("!", 1);
        string command = ChatInput.Substring(0, 1).ToLower();

        //Debug.Log("Command begins with: " + command);
        //Debug.Log(ChatInput.Substring(splitPoint + 2).ToLower());
        if (command == "!" && ChatInput != "!")
        {
            if (Application.loadedLevelName != "map_selection")
            {
                int buildingID;
                

                if (!GameObject.Find(userName)) {
                    
                    switch (ChatInput.Substring(splitPoint + 2))
                    {                  
                        case "sharpshooter":                                             
                            playerManager.instance.spawnNewPlayer(userName, ChatInput.Substring(splitPoint + 2));
                            break;
                        case "scientist":
                            playerManager.instance.spawnNewPlayer(userName, ChatInput.Substring(splitPoint + 2));
                            break;
                        case "soldier":
                            playerManager.instance.spawnNewPlayer(userName, ChatInput.Substring(splitPoint + 2));
                            break;
                        case "demolition":
                            playerManager.instance.spawnNewPlayer(userName, ChatInput.Substring(splitPoint + 2));
                            break;
                    }

                }

                if (GameObject.Find(userName))
                {
                    playerController player = GameObject.Find(userName).GetComponent<playerController>();


                    if (ChatInput.Substring(splitPoint + 2) == "stats")
                    {
                        writer.WriteLine($":{userName}!{userName}@{userName}.tmi.twitch.tv PRIVMSG #{channelName} : {"@" + userName + " gtfo kid"}");
                        writer.Flush();
                        return;
                    }


                    if (ChatInput.Substring(splitPoint + 2) == "shop")
                    {
                        ShopManager.instance.pickNewWeapon();
                        // writer.WriteLine($":{userName}!{userName}@{userName}.tmi.twitch.tv PRIVMSG #{channelName} : {"@" + userName + " gtfo kid"}");
                        // writer.Flush();
                        return;
                    }

                    if (int.TryParse(ChatInput.Substring(splitPoint + 2), out buildingID))
                    {
                           // playerController player = GameObject.Find(userName).GetComponent<playerController>();

                            // int buildingID = int.Parse(ChatInput.Substring(splitPoint + 2));

                            if (GameObject.Find(buildingID.ToString()) != null)
                            {
                                Building building = GameObject.Find(buildingID.ToString()).GetComponent<Building>();


                                if (building.checkIfPlayerInside(userName))
                                {
                                    Debug.Log(userName + " is already inside building " + buildingID);
                                }
                                else
                                {
                                    if (player.currentBuilding != null)
                                        player.currentBuilding.deletePlayerFromBuilding(player);

                                    building.AddPlayerToBuilding(userName);
                                }

                                Debug.Log("Players inside the building: " + building.playersInside.Count);
                            }

                        return;
                    }



                }
                else
                {
                    Debug.Log("Spawn your character first!");
                    return;
                }


            }
            else if (Application.loadedLevelName == "map_selection")
            {
                bool Result;
                int vote;

                if (ChatInput.Length >= 4)
                {
                    if (ChatInput.Substring(splitPoint + 2, 3) == "map")
                    {
                        Result = int.TryParse(ChatInput.Substring(splitPoint + 5), out vote);
                        if (Result && vote >= 1 && vote <= LobbyManager.instance.Maps.Count)
                        {
                            LobbyManager.instance.AddVote(vote);

                        }
                    }

                }

                return;
            }

        }


       
    }
}



