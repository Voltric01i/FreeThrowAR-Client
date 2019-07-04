using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject GameStartBoard;
    public GameObject ConnectingCanvas;
    public GameObject WaitUntilGameStartCanvas;
    public GameObject ScoreBoardCanvas;
    public GameObject ResultCanvas;
    
    public GameObject NetworkController;

    public Text ConnectedMember;
    public Text ConnectedStatus;

    public UnityEvent onGameStarted;

    float connectingTimer = 0;
    bool connectingFlag = false;
    ClientNetwork cn;

    // Start is called before the first frame update

    void Start()
    {
        setBoard(GameStartBoard);
        cn = NetworkController.GetComponent<ClientNetwork>();
    }
    // Update is called once per frame
    void Update()
    {
        // if(connectingFlag){
        //     connectingTimer += Time.deltaTime; 
        // }
        // if(connectingTimer >= 5){
        //     connectingFlag = false;
        //     connectingTimer = 0;
        //     cn.GameStartReady();
        // }
    }
    
    public void OnGameStartButtonPressed(){
        setBoard(ConnectingCanvas);
        cn.ConnectToServer();
    }
    public void OnPlayerJoined(int playerCount){
        ConnectedMember.text = "参加人数 : " + playerCount + " 人";
    }

    public void OnReadyButtonPressed(){
        setBoard(WaitUntilGameStartCanvas);
        cn.GameStartReady();
    }

    public void OnGameStart(){
        Debug.Log("ゲーム開始！！！");    
        setBoard(ScoreBoardCanvas);
        onGameStarted.Invoke();
    }


    public void OnGameTimeEnded(int score){
        setBoard(ResultCanvas);
        cn.sendPointData(score);
    }

    public void OnReceiveRankingData(IList pointList){
        
    }

    void setBoard(GameObject ActiveBoard){
        GameStartBoard.SetActive(false);
        ConnectingCanvas.SetActive(false);
        WaitUntilGameStartCanvas.SetActive(false);
        ScoreBoardCanvas.SetActive(false);
        ResultCanvas.SetActive(false);

        ActiveBoard.SetActive(true);
    }
}
