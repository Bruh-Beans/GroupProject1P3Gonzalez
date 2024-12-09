using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    private Button button;
    private GameManager gameManager;

    //there are 3 difficultys... this is the trigger for the buttons so that a difficulty is chosen and a signal is sent to the player controller
    //--- script where the spawn and speed interval changes based on the difficulty
    void Start()
    {
        button = GetComponent<Button>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
}
