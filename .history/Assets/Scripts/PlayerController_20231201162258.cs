using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    private int playerTurn;
    private int totalTurn;
    private Game game;
    [SerializeField] GameObject blackPiece;
    [SerializeField] GameObject whitePiece;
    public GameObject particle;
    [SerializeField] GameObject player1;
    [SerializeField] GameObject player2;
    public Color newColor = Color.yellow;
    public Color originalColor = Color.white;

    private bool startTiming;
    private float remainingTime;
    private float stepTime = 1f;

    private bool startTiming1;
    private float remainingTime1;
    private float stepTime1 = 0.4f;
    private bool canSpawn = true;

    
    // Start is called before the first frame update
    void Start()
    {
        game = GameObject.Find("Player").GetComponent<Game>();
        playerTurn = 0;
        Renderer rend1 = player1.transform.GetChild(0).GetComponent<Renderer>();
        rend1.material.color = newColor;
    }

    public void closedRestart()
    {
        startTiming = true;
        remainingTime1 = stepTime1;
        canSpawn = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (startTiming == true)
        {
            if (remainingTime > 0)
            {
                remainingTime -= Time.deltaTime;
            }
            else
            {
                canSpawn = true;
                startTiming = false;
                remainingTime = stepTime;
            }
        }

        if (startTiming1 == true)
        {
            if (remainingTime1 > 0)
            {
                remainingTime1 -= Time.deltaTime;
            }
            else
            {
                canSpawn = true;
                startTiming1 = false;
                remainingTime1 = stepTime1;
            }
        }

    }

    public void setStartTiming(bool start)
    {
        startTiming = start;
    }

    public bool getStartTiming()
    {
        return startTiming;
    }

    public void setRemainingTime(float time)
    {
        remainingTime = time;
    }


    public bool getCanSpawn()
    {
        return canSpawn;
    }

    public void SetCanSpawn(bool can)
    {
        canSpawn = can;
    }

    public void ChangeMaterialColor(int playerTurn)
    {
        Renderer rend1 = player1.transform.GetChild(0).GetComponent<Renderer>();
        Renderer rend2 = player2.transform.GetChild(0).GetComponent<Renderer>();
        // white
        if (playerTurn == 0)
        {
            if (rend1 != null)
            {
                rend1.material.color = newColor;
            }
            if (rend2 != null)
            {
                rend2.material.color = originalColor;
            }
        }
        // black
        else {
            if (rend1 != null)
            {
                rend1.material.color = originalColor;
            }
            if (rend2 != null)
            {
                rend2.material.color = newColor;
            }
        }
    }

    public int GetPlayerTurn()
    {
        return playerTurn;
    }

    public void SetPlayerTurn(int turn)
    {
        playerTurn = turn;
    }
    
    public void UpdatePlayerTurn()
    {
        playerTurn = Math.Abs(playerTurn - 1);
        ChangeMaterialColor(playerTurn);
    }

    public void Spawn(Vector3 pos, GameObject parent)
    {
        GameObject piece;
        int playerTurn = GameObject.Find("Player").GetComponent<PlayerController>().GetPlayerTurn();
        // white
        if (playerTurn == 0)
        {
            piece = whitePiece;
        }
        // black
        else {
            piece = blackPiece;
        }

        GameObject.Find("Player").GetComponent<PlayerController>().UpdatePlayerTurn();
        // creating a piece
        var pieceTransform = Instantiate(piece).transform;
        pieceTransform.parent = parent.transform;
        pieceTransform.position = new Vector3(pos.x, pos.y + 6.45f, pos.z);
        game.SetPrevPiece(pieceTransform.gameObject);

        // particle effect
        Quaternion rotation = Quaternion.Euler(new Vector3(90, 0, 90));
        Vector3 position = new Vector3(0, 0.5, 0);
        GameObject newParticle = Instantiate(particle, parent.transform.position + position, rotation);
        Destroy(newParticle, 1.5f);

        totalTurn += 1;
    }

    public int GetTotalTurn()
    {
        return totalTurn;
    }

    public void SetTotalTurn(int turn)
    {
        totalTurn = turn;
    }
}
