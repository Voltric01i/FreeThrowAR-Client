using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{

    public GameObject menuUI;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ItemMenuEnabler()
    {
        menuUI.SetActive(true);
    }

    public void ItemMenuDisabler()
    {
        menuUI.SetActive(false);
    }
}
