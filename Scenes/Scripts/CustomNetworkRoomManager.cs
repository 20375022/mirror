using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror
{

    public class CustomNetworkRoomManager : NetworkRoomManager
    {
        public virtual void OnRoomStartHost()
        {
        }
 
        public virtual void OnRoomStopHost()
        {
        }
     
        public virtual void OnRoomStartServer()
        {
        }
     
        public virtual void OnRoomServerConnect(NetworkConnection conn)
        {
        }
    
        public virtual void OnRoomServerDisconnect(NetworkConnection conn)
        {
        }
  
        public virtual void OnRoomServerSceneChanged(string sceneName)
        {
        }
    
        public virtual GameObject OnRoomServerCreateRoomPlayer(NetworkConnection conn)
        {
            return null;
        }
    
        public virtual GameObject OnRoomServerCreateGamePlayer(NetworkConnection conn)
        {
            return null;
        }
  
        public virtual bool OnRoomServerSceneLoadedForPlayer(GameObject roomPlayer, GameObject gamePlayer)
        {
            return true;
        }
    
        public virtual void OnRoomServerPlayersReady()
        {
            ServerChangeScene(GameplayScene);
        }

        public virtual void OnRoomClientEnter()
        {
        }

        public virtual void OnRoomClientExit()
        {
        }

        public virtual void OnRoomClientConnect(NetworkConnection conn)
        {
        }

        public virtual void OnRoomClientDisconnect(NetworkConnection conn)
        {
        }

        public virtual void OnRoomStartClient()
        {
        }

        public virtual void OnRoomStopClient()
        {
        }

        public virtual void OnRoomClientSceneChanged(NetworkConnection conn)
        {
        }

        public virtual void OnRoomClientAddPlayerFailed()
        {
        }
    }
}