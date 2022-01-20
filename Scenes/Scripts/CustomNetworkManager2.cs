using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 接続にまつわるいろいろ
/// </summary>
public class CustomNetworkManager2 : NetworkManager
{
    [SerializeField, Scene] private string _titleScene;
    [SerializeField, Scene] private string _mainScene;

    private Transform _playerTransform;
    private Material _playerMaterial;

    /// <summary>
    /// プレイヤー入室時にサーバー側が実行
    /// </summary>
    /// <param name="conn">接続されたプレイヤーのコネクション</param>
    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        Debug.Log("Add Player");

        //タイトルシーンでのみ実行
        if (_titleScene.Contains(SceneManager.GetActiveScene().name))
        {
            //接続中の人数表記を変える
            SendPlayerCountData sendData = new SendPlayerCountData() { PlayerCount = NetworkServer.connections.Count };
            NetworkServer.SendToAll(sendData);
        }

        //メインシーンでのみ実行
        if (_mainScene.Contains(SceneManager.GetActiveScene().name))
        {
            Debug.Log("Spawn Player");
            //プレイヤー生成
            GameObject player = Instantiate(playerPrefab);
            //今立ち上げているサーバーにプレイヤーを追加登録
            NetworkServer.AddPlayerForConnection(conn, player);
        }
    }

    /// <summary>
    /// 各プレイヤー退室時にサーバー側が実行
    /// </summary>
    /// <param name="conn">切れたコネクション</param>
    public override void OnServerDisconnect(NetworkConnection conn)
    {
        //接続中の人数表記を変える
        SendPlayerCountData sendData = new SendPlayerCountData() { PlayerCount = NetworkServer.connections.Count };
        NetworkServer.SendToAll(sendData);
        Debug.Log("Anyone Disconnect");
        base.OnServerDisconnect(conn);
    }

    /// <summary>
    /// サーバーとの接続が切れた時にクライアント側で呼ばれる
    /// </summary>
    public override void OnStopClient()
    {
        SceneManager.LoadScene(_titleScene);
        Debug.Log("Disconnect");
        base.OnStopClient();
    }
}