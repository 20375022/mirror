// vis2k: GUILayout instead of spacey += ...; removed Update hotkeys to avoid
// confusion if someone accidentally presses one.
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.UI;

namespace Mirror
{
    /// <summary>Shows NetworkManager controls in a GUI at runtime.</summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("Network/NetworkManagerHUD")]
    [RequireComponent(typeof(NetworkManager))]
    [HelpURL("https://mirror-networking.gitbook.io/docs/components/network-manager-hud")]
    public class NetworkManagerHUD : MonoBehaviour
    {
        NetworkManager manager;

        public int offsetX;
        public int offsetY;
        public GameObject IPado;

        public GameObject matching;
        public GameObject connected;

        void Awake()
        {
            matching.SetActive(true);
            connected.SetActive(false);
            manager = GetComponent<NetworkManager>();
        }

        void Update()
        {
            if (!NetworkClient.isConnected && !NetworkServer.active)
            {
                Debug.Log("接続前");
            }
            else
            {
                if (NetworkServer.active && NetworkClient.active)
                {
                    connected.transform.Find("IP").gameObject.SetActive(true);
                    connected.transform.Find("StatusMessage").GetComponent<Text>().text = "他のプレイヤーを待っています";
                }
                // client only
                else if (NetworkClient.isConnected)
                {
                    connected.transform.Find("StatusMessage").GetComponent<Text>().text = "ホストの開始を待っています";
                }
            }

            // client ready
            if (NetworkClient.isConnected && !NetworkClient.ready)
            {
            }
            else if (NetworkClient.isConnected)
            {
            }
            else if (!NetworkClient.ready)
            {
            }
        }

        public void OnClickServer()
        {
            if (!NetworkClient.active)
            {
                manager.StartHost();
                matching.SetActive(false);
                connected.SetActive(true);
                connected.transform.Find("StopServer").gameObject.SetActive(true);
            }
        }

        public void OnClickClient()
        {
            if (!NetworkClient.active)
            {
                manager.networkAddress = IPado.transform.Find("Text").gameObject.GetComponent<Text>().text;
                Debug.Log(IPado.transform.Find("Text").gameObject.GetComponent<Text>().text);
                manager.StartClient();
                matching.SetActive(false);
                connected.SetActive(true);
                connected.transform.Find("StopClient").gameObject.SetActive(true);
                connected.transform.Find("StatusMessage").GetComponent<Text>().text = "接続中...";
            }
        }

        public void OnClickStopServer()
        {
            matching.SetActive(true);
            connected.SetActive(false);
            connected.transform.Find("StopServer").gameObject.SetActive(false);
            connected.transform.Find("StopClient").gameObject.SetActive(false);
            connected.transform.Find("IP").gameObject.SetActive(false);
            connected.transform.Find("Ready").gameObject.SetActive(false);
            manager.StopHost();
        }

        public void OnClickStopClient()
        {
            matching.SetActive(true);
            connected.SetActive(false);
            connected.transform.Find("StopServer").gameObject.SetActive(false);
            connected.transform.Find("StopClient").gameObject.SetActive(false);
            connected.transform.Find("Ready").gameObject.SetActive(false);
            manager.StopClient();
        }
        
        public void Onclickre()
        {
            NetworkClient.Ready();
        }


        /*       void OnGUI()
               {
                   GUILayout.BeginArea(new Rect(10 + offsetX, 40 + offsetY, 215, 9999));
                   if (!NetworkClient.isConnected && !NetworkServer.active)
                   {
                       Debug.Log("接続前");
                       StartButtons();
                   }
                   else
                   {
                       StatusLabels();
                   }

                   // client ready
                   if (NetworkClient.isConnected && !NetworkClient.ready)
                   {
                       if (GUILayout.Button("Client Ready"))
                       {
                           ClientScene.Ready();
                           if (NetworkClient.localPlayer == null)
                           {
                               NetworkClient.AddPlayer();
                           }
                       }
                   }

                   StopButtons();

                   GUILayout.EndArea();
               }*/


        void StartButtons()
        {
            if (!NetworkClient.active)
            {
                // Server + Client
                if (Application.platform != RuntimePlatform.WebGLPlayer)
                {
                    if (GUILayout.Button("Host (Server + Client)"))
                    {
                        manager.StartHost();
                    }
                }

                // Client + IP
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Client"))
                {
                    manager.StartClient();
                }
                manager.networkAddress = GUILayout.TextField(manager.networkAddress);
                GUILayout.EndHorizontal();

                // Server Only
                /*                if (Application.platform == RuntimePlatform.WebGLPlayer)
                                {
                                    // cant be a server in webgl build
                                    GUILayout.Box("(  WebGL cannot be server  )");
                                }
                                else
                                {
                                    if (GUILayout.Button("Server Only")) manager.StartServer();
                                }*/
            }
            else
            {
                // Connecting
                GUILayout.Label($"Connecting to {manager.networkAddress}..");
                if (GUILayout.Button("Cancel Connection Attempt"))
                {
                    manager.StopClient();
                }
            }
        }

        void StatusLabels()
        {
            // host mode
            // display separately because this always confused people:
            //   Server: ...
            //   Client: ...
            if (NetworkServer.active && NetworkClient.active)
            {
                GUILayout.Label(manager.networkAddress);
            }
            // server only
            else if (NetworkServer.active)
            {
                GUILayout.Label($"<b>Server</b>: running via {Transport.activeTransport}");
            }
            // client only
            else if (NetworkClient.isConnected)
            {
                GUILayout.Label($"<b>Client</b>: connected to {manager.networkAddress} via {Transport.activeTransport}");
            }
        }

        void StopButtons()
        {
            // stop host if host mode
            if (NetworkServer.active && NetworkClient.isConnected)
            {
                if (GUILayout.Button("Stop Host"))
                {
                    manager.StopHost();
                }
            }
            // stop client if client-only
            else if (NetworkClient.isConnected)
            {
                if (GUILayout.Button("Stop Client"))
                {
                    manager.StopClient();
                }
            }
            // stop server if server-only
            else if (NetworkServer.active)
            {
                if (GUILayout.Button("Stop Server"))
                {
                    manager.StopServer();
                }
            }
        }
    }
}








