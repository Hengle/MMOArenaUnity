﻿using UnityEngine;
using System.Collections;

public class RoomNameFetcher : AbstractHttpFetcher
{
    public string actionName = "/getFight";

    RequestFetcher requestFetcher = null;
    GameController gameController = null;

    public string roomName = "";
    public string enemyId = "";

    public bool requestSent = false;
    public bool dataReceived = false;

    // Use this for initialization
    void Start()
    {
        requestFetcher = GetComponent<RequestFetcher>();
        gameController = GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        if ( requestFetcher != null && !requestSent && requestFetcher.requestId !=  "")
        {
            requestSent = true;
            gameController.loadingMsg = "Waiting for the fight ...";

            receiveCallback += UpdateRequestId;
            this.absoluteAddress = gameController.matchmakerAddress + actionName + "/" + requestFetcher.requestId;
            this.Fetch();
        }
    }

    void UpdateRequestId(string data)
    {
        Debug.Log("Received data about pending fight");

        // parse output
        string state = SimpleJSON.JSON.Parse(data)["state"]["fightState"];

        // if state is scheduled, then setup roomName
        if (state == "PREPARED")
        {
            roomName = SimpleJSON.JSON.Parse(data)["state"]["fightId"];
            enemyId = SimpleJSON.JSON.Parse(data)["state"]["enemyId"];

            dataReceived = true;
        }
        else
        {
            this.Fetch();
        }
    }
}
