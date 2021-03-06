﻿using UnityEngine;
using System.Collections;
using System.Xml.Serialization;
using System.IO;
using System.Text;

public class GameController : MonoBehaviour {

    public Character playerData = null, enemyData = null;

    public CharacterStats player;
    public CharacterStats enemy;

    public bool backendDataLoaded = false;
    public bool isGameStarted = false;
    public bool isGameFinished = false;

    public bool isPaused = false;

    public string errorMsg = "";
    public string loadingMsg = "";

	// Use this for initialization
	void Start () {
        Application.ExternalCall("requestFight");
	}
	
	// Update is called once per frame
	void Update () {
        if (isGameStarted == false)
        {
            CharacterStats[] characterStats = FindObjectsOfType(typeof(CharacterStats)) as CharacterStats[];

            if (characterStats.Length != 2)
            {
                if (backendDataLoaded == true)
                {
                    loadingMsg = "Waiting for other player to connect ...";
                }

                return;
            }
            else
            {
                if ( characterStats[0].GetComponent<PhotonView>().isMine )
                {
                    player = characterStats[0];
                    enemy = characterStats[1];
                } else {
                    player = characterStats[1];
                    enemy = characterStats[0];
                }

                Debug.Log("Loading player spellcasting data");

                player.LoadFromData(playerData);
                player.GetComponent<PhotonCharacterSpellcasting>().LoadSpells(playerData);

                Debug.Log("Loading enemy spellcasting data");

                enemy.LoadFromData(enemyData);
                enemy.GetComponent<PhotonCharacterSpellcasting>().LoadSpells(enemyData);


                if (player.health > 0)
                {
                    isGameStarted = true;
                }
            }
        }
        else if( isGameStarted == true )
        {
            if (player.health <= 0 || enemy == null || enemy.health <= 0)
            {
                isGameFinished = true;

                PauseGame();
            }
        }

	}

    public void PauseGame()
    {
        player.GetComponent<MouseController>().Disable();
        player.GetComponent<HumanPlayerController>().enabled = false;

        isPaused = true;
    }

    public void UnpauseGame()
    {
        player.GetComponent<MouseController>().Enable();
        player.GetComponent<HumanPlayerController>().enabled = true;

        isPaused = false;
    }

    public void SetPlayer(string data)
    {
        var serializer = new XmlSerializer(typeof(Character));
        var stream = new MemoryStream(Encoding.ASCII.GetBytes(data));
        playerData = serializer.Deserialize(stream) as Character;
        stream.Close();
    }

    public void SetEnemy(string data)
    {
        var serializer = new XmlSerializer(typeof(Character));
        var stream = new MemoryStream(Encoding.ASCII.GetBytes(data));
        enemyData = serializer.Deserialize(stream) as Character;
        stream.Close();
    }

    public void StartGame(string roomname)
    {
        GetComponent<MMOArenaPhotonConnector>().Connect(roomname);
        backendDataLoaded = true;
    }
}
