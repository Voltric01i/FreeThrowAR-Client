using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject ConnectingCanvas;
    public GameObject WaitUntilGameStartCanvas;
    public GameObject ScoreBoardCanvas;

    public GameObject ThrowButton;

    public GameObject StartReadyButton;
    
    public GameObject NetworkController;

    public Text ConnectedMember;
    public Text scoreValue;

    public UnityEvent onGameStarted;

    float connectingTimer = 0;
    bool connectingFlag = true;
    GameState currentState = GameState.Reset;
    ClientNetwork cn;

    // Start is called before the first frame update

    void Start()
    {
        cn = NetworkController.GetComponent<ClientNetwork>();
        
    }
    // Update is called once per frame
    void Update()
    {

        if(connectingFlag){
            connectingTimer++;
        }
        if(connectingTimer >= 1 * 60){
            connectingFlag = false;
            OnGameStartButtonPressed();
            connectingTimer = 0;
        }

        var nowStatus = cn.getStatus();
        switch(nowStatus){
            case GameState.Matching:
                var value = cn.getPlayerValue();
                ConnectedMember.text = value + "人";
            break;

            case GameState.Ready:
            break;

            case GameState.Playing:
                if(currentState != cn.getStatus()){
                    OnGameStart();
                }
            break;

            case GameState.Finish:
            break;

            case GameState.Reset:
            break;
        }
        currentState = nowStatus;


    }
    
    public void OnGameStartButtonPressed(){
        setBoard(ConnectingCanvas);
        setButton(StartReadyButton);
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
        setBoard(ScoreBoardCanvas);
        setButton(ThrowButton);
        Debug.Log("setBoard(ScoreBoardCanvas);");
        onGameStarted.Invoke();
    }


    public void OnGameTimeEnded(){
        var score = 0;
        int.TryParse(scoreValue.text,out score);
        cn.sendPointData(score);
    }

    public void resetServer(){
        cn.sendGameReset();
    }

    public void OnReceiveRankingData(IList pointList){
        
    }

    void setBoard(GameObject ActiveBoard){
        ConnectingCanvas.SetActive(false);
        WaitUntilGameStartCanvas.SetActive(false);
        ScoreBoardCanvas.SetActive(false);

        ActiveBoard.SetActive(true);
    }
    void setButton(GameObject ActiveButton){
        ThrowButton.SetActive(false);
        StartReadyButton.SetActive(false);

        ActiveButton.SetActive(true);
    }
}
