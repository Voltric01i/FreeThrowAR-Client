using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadResultatGameEnded : MonoBehaviour
{
    // Start is called before the first frame update
    public Text resultText;
    void Start()
    {
        var result = GoalChecker.getResult();
        resultText.text = result + "";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

