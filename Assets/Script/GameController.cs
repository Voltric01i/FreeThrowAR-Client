using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameController : MonoBehaviour
{
    public GameObject GameStartBoard;
    public GameObject ConnectingCanvas;
    public GameObject ScoreBoardCanvas;
    public GameObject ResultCanvas;

    public UnityEvent onGameStarted;

    float connectingTimer = 0;
    bool connectingFlag = false;
    // Start is called before the first frame update

    void Start()
    {
        setBoard(GameStartBoard);
    }
    // Update is called once per frame
    void Update()
    {
        if(connectingFlag){
            connectingTimer += Time.deltaTime; 
        }
        if(connectingTimer >= 5){
            connectingFlag = false;
            connectingTimer = 0;
            onGameStart();
        }
    }

    public void onPushedGameStartButton(){
        setBoard(ConnectingCanvas);
        connectingFlag = true;
    }

    public void onGameTimeEnded(){
        setBoard(ResultCanvas);
    }

    public void onGameStart(){
        onGameStarted.Invoke();
        setBoard(ScoreBoardCanvas);
    }

    void setBoard(GameObject ActiveBoard){
        GameStartBoard.SetActive(false);
        ConnectingCanvas.SetActive(false);
        ScoreBoardCanvas.SetActive(false);
        ResultCanvas.SetActive(false);

        ActiveBoard.SetActive(true);
    }
}
