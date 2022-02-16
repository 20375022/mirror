// vis2k: GUILayout instead of spacey += ...; removed Update hotkeys to avoid
// confusion if someone accidentally presses one.
using UnityEngine;
using UnityEngine.UI;

namespace Mirror
{
    /// <summary>Shows NetworkManager controls in a GUI at runtime.</summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("Network/NetworkManagerHUD")]
    [RequireComponent(typeof(NetworkManager))]
    [HelpURL("https://mirror-networking.gitbook.io/docs/components/network-manager-hud")]
    public class NetHUD : MonoBehaviour
    {
        NetworkManager manager;

        public GameObject ServerBtn;    //ServerButton
        public GameObject ClientBtn;    //ClientButton
        public GameObject Inputfield;   //Inputfield
        public GameObject Enteradress;  //Text欄

        const int server = 1;
        const int client = 2;
        const int end = 3;
        static int connectedF = 0;


        void Awake()
        {
            manager = GetComponent<NetworkManager>();
        }

        void Update()
        {
            if (!NetworkClient.active && connectedF != server && connectedF != client)
            {
                ClientBtn.SetActive(true);
                ServerBtn.SetActive(true);
                Inputfield.SetActive(true);
                if (connectedF == end)
                {
                    connectedF = 0;
                }
            }
            if (connectedF == server)
            {
                StartMyServer();
            }
            if (connectedF == client)
            {
                Debug.Log("クライアント開始");
                StartMyClient();
            }
        }

        //ボタンでサーバーを選択
        public void Selserver()
        {
            StartAll();
            connectedF = server;
        }

        //ボタンでクライアントを選択
        public void Selclient()
        {
            if (Enteradress.GetComponent<Text>().text.Length < 9) return;
            StartAll();
            connectedF = client;
        }

        //サーバー開始
        void StartMyServer()
        {
            if (CharactorManager.playerType != PlayerType.none)
            {
                manager.StartHost();
                connectedF = end;
            }
        }

        //クライアント接続
        void StartMyClient()
        {
            if (CharactorManager.playerType != PlayerType.none)
            {
                manager.StartClient();
                Debug.Log(manager.networkAddress);
                connectedF = end;
            }
        }

        void StartAll()
        {
            manager.networkAddress = Enteradress.GetComponent<Text>().text;
            ClientBtn.SetActive(false);
            ServerBtn.SetActive(false);
            Inputfield.SetActive(false);
        }
    }
}


