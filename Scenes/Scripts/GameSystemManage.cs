using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameSystemManage : NetworkBehaviour
{
    public enum GameMode
    {
        LOBBY = 0,
        GAME,
        RESULT
    }

    public int gameMode;
    [SyncVar]
    public int countPlayer;
    [SyncVar]
    public int readyPlayer;

    // Start is called before the first frame update
    void Start()
    {
        gameMode = (int)GameMode.LOBBY;
        readyPlayer = 0;
        countPlayer = 4;
    }

    // Update is called once per frame
    void Update()
    {
        switch (gameMode)
        {
            case (int)GameMode.LOBBY:

                Debug.Log("Gamemode = Lobby");
                break;

            case (int)GameMode.GAME:
                Debug.Log("Gamemode = Game");
                break;

            case (int)GameMode.RESULT:
                Debug.Log("Gamemode = Result");
                break;
        }
    }


    [ServerCallback]
    void ReadyInc()
    {
        readyPlayer++;
    }

    [ClientCallback]
    public void ReadySync()
    {
        ReadyInc();
    }

}
