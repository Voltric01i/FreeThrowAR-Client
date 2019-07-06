using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.IO;
using System.Text;
using System.Net.Sockets;
using System.Threading.Tasks;
using MiniJSON;
using UnityEngine.UI;
using UnityEngine.Events;

/// <summary>
/// サーバーに接続してデータのやり取りをする
/// 使うときはコールバックをoverrideする
/// </summary>


public enum GameState
{
    Matching,
    Ready,
    Playing,
    Finish,
    Reset
}

public class ClientNetwork : MonoBehaviour
{
    private TcpClient connection;
    GameState currentState;

    int playerValue = 0;

    public int port = 30000;
    public string serverIP = "192.168.0.200";
    public InputField input;

    public GameObject throwBall;
    ThrowBall tB;
    //デバッグ用
    public Text statusText;
    public Text textReceivedMessage;
    // public String input;
    private string receivedMessage = "";
    
    // Start is called before the first frame update
    void Start()
    {
        currentState = GameState.Reset;
        input.text = PlayerPrefs.GetString ("ServerIP", "192.168.200.1");
        InputText();
        tB = throwBall.GetComponent<ThrowBall>();
        // serverIP = input;
    }

    public void InputText()
    {
        serverIP = input.text;
        PlayerPrefs.SetString ("ServerIP", input.text);
        PlayerPrefs.Save ();
    }

    // Update is called once per frame
    void Update()
    {
        // デバッグ用
        // statusText.text = "State: " + currentState.ToString();
        // textReceivedMessage.text = "Received: " + receivedMessage;

    }

    void Quit()
    {

        connection.Close();
    }


// ネットワーク

    private void Connect(string address, int port)
    {
        Task.Run(() =>
        {
            // サーバーに接続
            connection = new TcpClient(address, port);
            var stream = connection.GetStream();
            var reader = new StreamReader(stream, Encoding.UTF8);
            Debug.Log("Connect:" + connection.Client.RemoteEndPoint);

            // 接続が切れるまで読み込み
            while (connection.Connected)
            { 
                while (!reader.EndOfStream)
                {
                    string str = reader.ReadLine();
                    Debug.Log("MessageReceived: " + str);
                    OnMessage(str);
                    receivedMessage = str;

                }

                if (connection.Client.Poll(1000, SelectMode.SelectRead) && (connection.Client.Available == 0))
                {
                    Debug.Log("Disconnect: " + connection.Client.RemoteEndPoint);
                    connection.Close();
                    GameReset();
                    break;
                }
                
            }

        });

        
    }


    // パケットを受け取ったら実行
    private void OnMessage(string str)
    {
        IDictionary msgJson = null;
        var msgName = "";

        try
        { 
            msgJson = (IDictionary)Json.Deserialize(str);
            msgName = msgJson["name"].ToString();
        }
        catch (Exception e)
        {
            Debug.Log("before if:" + e);
        }
        
        if (msgName == "joined")
        {
            try
            {
                // サーバーに誰かが接続した時
                int msgValue = int.Parse(msgJson["value"].ToString());
                OnPlayerJoined(msgValue);
            }
            catch (Exception e)
            {
                Debug.Log("joined:" + e);
            }
        }
        else if (msgName == "start" && currentState == GameState.Ready)
        {
            // ゲームスタートが通知された時
            GameStart();
            OnGameStart();
        }
        else if(msgName == "ball" && currentState == GameState.Playing)
        {
            // 他クライアントでボールが投げられた時
            float posx = float.Parse(msgJson["posx"].ToString());
            float posy = float.Parse(msgJson["posy"].ToString());
            float posz = float.Parse(msgJson["posz"].ToString());
            float wayx = float.Parse(msgJson["wayx"].ToString());
            float wayy = float.Parse(msgJson["wayy"].ToString());
            float wayz = float.Parse(msgJson["wayz"].ToString());

            Vector3 pos = new Vector3(posx, posy, posz);
            Vector3 way = new Vector3(wayx, wayy, wayz);
            Debug.Log("State: Ball Receved");
            OnReceiveBallData(pos, way);
        }
        else if (msgName == "ranking" && currentState == GameState.Finish)
        {
            var pointList = (IList)msgJson["value"];
            OnReceiveRankingData(pointList);
        }

    }

    protected void SendMessageToServer(string msg)
    {
        msg += "\n"; //改行を追加
        var body = Encoding.UTF8.GetBytes(msg);
        if (connection.Connected)
        {
            connection.GetStream().Write(body, 0, body.Length);
            Debug.Log("MessageSend: " + msg);
        }

    }
    




// 状態遷移メソッド

    private void GameMatching()
    {
        Debug.Log("State: Matching");
        currentState = GameState.Matching;
    }

    private void GameReady()
    {
        Debug.Log("State: Ready");
        currentState = GameState.Ready;
    }

    private void GameStart()
    {
        Debug.Log("State: Playing");
        currentState = GameState.Playing;

    }

    private void GameFinish()
    {
        Debug.Log("State: Finish");
        currentState = GameState.Finish;

    }

    private void GameReset()
    {
        Debug.Log("State: Reset");
        currentState = GameState.Reset;
    }





 // パブリックメソッドとコールバック
    public void ConnectToServer()
    {
        if (currentState == GameState.Reset)
        {
            GameMatching();
            Connect(serverIP, port);
        }
    }

    public GameState getStatus(){
        return currentState;
    }

    public int getPlayerValue(){
        return playerValue;
    }

    // ゲーム開始準備完了
    public void GameStartReady()
    {
        if (currentState == GameState.Matching)
        {
            GameReady();
            SendMessageToServer("{\"name\":\"start\"}");
        }
    }

    private void OnPlayerJoined(int playerCount)
    {
        playerValue = playerCount;
    }

    private void OnGameStart()
    {
         
    }

    public void SendBallData(Vector3 pos, Vector3 way)
    {
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("name", "ball");
        dict.Add("posx", pos.x);
        dict.Add("posy", pos.y);
        dict.Add("posz", pos.z);
        dict.Add("wayx", way.x);
        dict.Add("wayy", way.y);
        dict.Add("wayz", way.z);
        string json = Json.Serialize(dict);
        SendMessageToServer(json);
        

    }

    private void OnReceiveBallData(Vector3 pos, Vector3 way)
    {
        var ball =  tB.getBall();
        
        tB.throwRecevedBall(pos,way);
    }

    // ゲーム終了時に得点を送信
    public void sendPointData(int point)
    {
        if (currentState == GameState.Playing)
        {
            GameFinish();
            SendMessageToServer("{\"name\":\"finish\",\"value\":" + point + "}");
        }
    }

    protected virtual void OnReceiveRankingData(IList pointList)
    {
        
    }

    public void sendGameReset()
    {
        GameReset();
        SendMessageToServer("{\"name\":\"reset\"}");
    }
    

    // デバッグメソッド
    public void testBalldata1()
    {
        SendBallData(new Vector3(0.1111f, 0.2222f, 0.4424431f), new Vector3(3.444f, 3.555f, 3.666f));
    }

    public void testBalldata2()
    {
        SendBallData(new Vector3(0.1234f, 0.724367f, 0.8262f), new Vector3(3.444f, 3.555f, 3.666f));
    }

}
