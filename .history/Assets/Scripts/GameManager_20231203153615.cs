using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private PlayerController player;
    private Game game;

    private float stepTime = 1f;
    public GameObject restartPage;


    public TMP_Text ruleInform;
    private bool startTiming;
    private float remainingTime;
    private float showText = 1.5f;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        game = GameObject.Find("Player").GetComponent<Game>();
        //instantiate = GetComponent<InstantiatePiece>();
    }

    void Update()
    {
        if (startTiming)
        {
            if (remainingTime > 0)
            {
                remainingTime -= Time.deltaTime;
            }
            else
            {
                ruleInform.text = "";
                startTiming = false;
                remainingTime = showText;
            }
        }
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0); // Get the first touch
            if (touch.phase == TouchPhase.Ended)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;

                // Raycast to check if the touch hits this object
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject == gameObject)
                    {
                        if (player.getCanSpawn())
                        {
                            if (gameObject.transform.childCount < 5)
                            {
                                if (!restartPage.activeSelf && !game.IsGameFinished())
                                {
                                    player.SetCanSpawn(false);
                                    if (player.getStartTiming() == false)
                                    {
                                        player.setRemainingTime(stepTime);
                                        player.setStartTiming(true);
                                    }
                                    int turn = player.GetPlayerTurn();
                                    player.Spawn(gameObject.transform.position, gameObject);

                                    // update board data to check for win
                                    int row = gameObject.name[0] - '0';
                                    int col = gameObject.name[1] - '0';
                                    game.MakeMove(row, col, gameObject.transform.childCount, turn);
                                }
                            }
                            // can't place
                            else 
                            {
                                if (!restartPage.activeSelf)
                                {
                                    GameObject.Find("SoundEffects").GetComponent<AudioManager>().Error();
                                    ruleInform.text = "Maximum stack 5 pieces, cannot place here";
                                    startTiming = true;
                                    remainingTime = showText;
                                }
                            }
                        }
                    }
                }
            }
        }

        // For laptops (mouse clicks)
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Raycast to check if the mouse click hits this object
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    if (player.getCanSpawn())
                    {
                        if (gameObject.transform.childCount < 5)
                        {
                            if (!restartPage.activeSelf && !game.IsGameFinished())
                            {
                                player.SetCanSpawn(false);
                                if (player.getStartTiming() == false)
                                {
                                    player.setRemainingTime(stepTime);
                                    player.setStartTiming(true);
                                }
                                int turn = player.GetPlayerTurn();
                                player.Spawn(gameObject.transform.position, gameObject);

                                // update board data to check for win
                                int row = gameObject.name[0] - '0';
                                int col = gameObject.name[1] - '0';
                                game.MakeMove(row, col, gameObject.transform.childCount, turn);
                            }
                        }
                        // can't place
                        else 
                        {
                            if (!restartPage.activeSelf) 
                            {
                                GameObject.Find("SoundEffects").GetComponent<AudioManager>().Error();
                                ruleInform.text = "Maximum stack 5 pieces, cannot place here";
                                startTiming = true;
                                remainingTime = showText;
                            }
                        }
                    }
                }
            }
        }
    }
}
