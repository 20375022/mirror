using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Mirror;
using Mirror.Discovery;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct SendHostReadyData : NetworkMessage
{
    /// <summary>
    /// ホストが準備できたかどうか
    /// </summary>
    public bool IsHostReady;
}

[Serializable]
public struct SendPlayerCountData : NetworkMessage
{
    /// <summary>
    /// 参加プレイヤーの数
    /// </summary>
    public int PlayerCount;
}

/// <summary>
/// サーバー検索、接続
/// </summary>
public class CustomNetworkDiscovery : NetworkDiscovery
{
    [SerializeField] private Button _multiPlayButton;
    [SerializeField] private Button _backButton;
    [SerializeField] private Button _playButton;
    [SerializeField] private Text _playerCountText;
    [SerializeField] private Text _connectionStateText;
    //SceneのアトリビュートはMirrorに用意されている便利機能
    //Inspectorでシーンを参照してコード内で文字列として使用できる
    [SerializeField,Scene] private string _gameSceneName;

    private ServerResponse _discoveredServer;
    private CancellationTokenSource _cancellationTokenSource;

    private const int CONNECT_INTERVAL_TIME = 2;
    private const int WAIT_TIME = 2;
    private const int CONNECT_TRY_COUNT = 1;

    private const string CONNECTION_STATUS_CLIENT_WAITING = "ホストが開始するのを待っています...";
    private const string CONNECTION_STATUS_HOST_WAITING = "他のプレイヤーを待っています...";
    private const string CONNECTION_STATUS_SUCCESS = "成功！";

    private bool _isHostReady;
    private NetworkManager _networkManager;

    private void OnDestroy()
    {
        //シーン遷移などで破棄されたタイミングで検索をやめる
        StopDiscovery();
    }

    private void Awake()
    {
        //データ受信の準備
 //       NetworkClient.RegisterHandler<SendHostReadyData>(ReceivedReadyInfo);
 //       NetworkClient.RegisterHandler<SendPlayerCountData>(ReceivedPlayerCountInfo);

        //サーバー見つけたらこれが呼ばれる
        OnServerFound.AddListener(serverResponse =>
        {
            //見つけたサーバーを辞書に登録
            _discoveredServer = serverResponse;
            Debug.Log("ServerFound");
        });

        //サーバーの検索＆接続開始
        _multiPlayButton.onClick.AddListener(() =>
        {
            Debug.Log("Search Connection");
            _backButton.transform.gameObject.SetActive(true);
            _multiPlayButton.transform.gameObject.SetActive(false);

            //接続を試みる
            _cancellationTokenSource = new CancellationTokenSource();
            CancellationToken token = _cancellationTokenSource.Token;
            TryConnectAsync(token).Forget();
        });

        //最初の画面に戻る
        _backButton.onClick.AddListener(() =>
        {
            Debug.Log("Cancel");
            // 接続する以外非表示にする
            _backButton.transform.gameObject.SetActive(false);
            _playButton.transform.gameObject.SetActive(false);
            _playerCountText.transform.gameObject.SetActive(false);
            _connectionStateText.transform.gameObject.SetActive(false);
            _multiPlayButton.transform.gameObject.SetActive(true);

            //サーバーから抜ける
            //サーバーの検索停止
            StopDiscovery();
            NetworkManager.singleton.StopHost();

            //非同期処理止める
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();

        });

        //ホスト側にのみ表示されるボタン プレイボタン押下で準備完了とする
        _playButton.onClick.AddListener(() =>
        {
            Debug.Log("Ready Ok");
            //各クライアントにフラグデータを送る
            SendHostReadyData sendData = new SendHostReadyData() {IsHostReady = true};
            NetworkServer.SendToAll(sendData);
 
            //シーン遷移
            _networkManager.ServerChangeScene(_gameSceneName);

            _playButton.transform.gameObject.SetActive(false);
        });
    }

    /// <summary>
    /// サーバーから受け取ったデータを各クライアントで使う
    /// </summary>
    /// <param name="conn">コネクション情報　関数内で使ってないけど必要みたい</param>
    /// <param name="receivedData">受け取ったデータ</param>
    private void ReceivedReadyInfo(NetworkConnection conn, SendHostReadyData receivedData)
    {
        //ローカルのフラグに反映
        _isHostReady = receivedData.IsHostReady;
    }

    /// <summary>
    /// サーバーから受け取ったデータを各クライアントで使う
    /// </summary>
    /// <param name="conn">コネクション情報　関数内で使ってないけど必要みたい</param>
    /// <param name="receivedData">受け取ったデータ</param>
    private void ReceivedPlayerCountInfo(NetworkConnection conn, SendPlayerCountData receivedData)
    {
        if (_playButton == null) return;

        Debug.Log("送られている");
        _playerCountText.text = receivedData.PlayerCount + "/" + _networkManager.maxConnections;
    }


    /// <summary>
    /// 接続を試みる
    /// 非同期
    /// </summary>
    private async UniTaskVoid TryConnectAsync(CancellationToken token)
    {
        _networkManager = NetworkManager.singleton;

        int tryCount = 0;

        //サーバーの検索開始
        StartDiscovery();

        //サーバーに接続するまでループ
        while (!_networkManager.isNetworkActive)
        {
            //n秒間隔で実行
            await UniTask.Delay(TimeSpan.FromSeconds(CONNECT_INTERVAL_TIME), cancellationToken: token);

            //サーバー発見した場合
            if (_discoveredServer.uri != null)
            {
                Debug.Log("Start Client");
                //クライアントとして接続開始
                _networkManager.StartClient(_discoveredServer.uri);
                //接続ステータスの文言変更
                _connectionStateText.text =CONNECTION_STATUS_CLIENT_WAITING;
                // ステータスメッセージの表示
                _connectionStateText.transform.gameObject.SetActive(true);

                //サーバーの検索停止
                StopDiscovery();
                //ここでホストの開始フラグを待つ
                await UniTask.WaitUntil(() => _isHostReady, cancellationToken: token);
                //接続ステータスの文言変更
                _connectionStateText.text = CONNECTION_STATUS_SUCCESS;
            }
            //サーバー見つからない場合
            else
            {
                Debug.Log("Try Connect...");

                //接続を試みた回数をカウントアップ
                tryCount++;

                //任意の回数以上接続に試みて失敗した場合は自身がホストになる
                if (tryCount > CONNECT_TRY_COUNT)
                {
                    Debug.Log("Start Host");

                    //ホストになる(サーバー)
                    _networkManager.StartHost();
                    //サーバーあるよーってお知らせする
                    AdvertiseServer();

                    //接続ステータスの文言変更
                    _connectionStateText.text = CONNECTION_STATUS_HOST_WAITING;
                    // ステータスメッセージの表示
                    _connectionStateText.transform.gameObject.SetActive(true);

                    //プレイボタン表示
                    _playButton.gameObject.SetActive(true);

                    //ここでホストの開始フラグを待つ
                    await UniTask.WaitUntil(() => _isHostReady, cancellationToken: token);
                    //接続ステータスの文言変更
                    _connectionStateText.text = CONNECTION_STATUS_SUCCESS;
                    //n秒待つ
                    await UniTask.Delay(TimeSpan.FromSeconds(WAIT_TIME), cancellationToken: token);
                }
            }
        }
    }
}