using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    private int[,,] board; // 3D array to represent the game board
    public TMP_Text boardText;
    public TMP_Text stateText;
    public GameObject reverseButton;
    public GameObject arrow0;
    public GameObject arrow1;
    public GameObject arrow2;
    public GameObject arrow3;
    public GameObject arrow4;
    private bool gameFinished;
    private string previousMove;
    private GameObject previousPiece;
    private GameObject[] pieceButtons;

    void Start()
    {
        InitializeGame();
        DisplayBoard();
    }

    void Update()
    {
        if (gameFinished)
        {
            reverseButton.SetActive(false);
        }
        if (GameObject.Find("Player").GetComponent<PlayerController>().GetTotalTurn() == 1)
        {
            reverseButton.SetActive(true);
        }
    }

    // Constructor to initialize the game board
    public void InitializeGame()
    {
        // Initialize the board with empty (-1) values
        board = new int[5, 5, 5];
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                for (int k = 0; k < 5; k++)
                {
                    board[i, j, k] = -1;
                }
            }
        }

        Renderer rend;
        rend = arrow0.GetComponent<Renderer>();
        rend.material.color = Color.white;

        rend = arrow1.GetComponent<Renderer>();
        rend.material.color = Color.white;

        rend = arrow2.GetComponent<Renderer>();
        rend.material.color = Color.white;

        rend = arrow3.GetComponent<Renderer>();
        rend.material.color = Color.white;

        rend = arrow4.GetComponent<Renderer>();
        rend.material.color = Color.white;

        // remove existing buttons
        pieceButtons = GameObject.FindGameObjectsWithTag("Piece");
        if (pieceButtons.Length > 0)
        {
            foreach (GameObject piece in pieceButtons)
            {
                Destroy(piece);
            }
        }
        
        reverseButton.SetActive(false);
        gameFinished = false;
        stateText.text = "";
        GameObject.Find("Player").GetComponent<PlayerController>().SetCanSpawn(true);
        GameObject.Find("Player").GetComponent<PlayerController>().SetPlayerTurn(0);
        GameObject.Find("Player").GetComponent<PlayerController>().SetTotalTurn(0);
        GameObject.Find("Player").GetComponent<PlayerController>().ChangeMaterialColor(0);
    }

    // Function to display the current state of the board in a TextMeshProUGUI element
    public void DisplayBoard()
    {
        string display = "";

        for (int level = 0; level < 5; level++)
        {
            display += $"L E V E L  {level + 1} :\n";
            for (int row = 0; row < 5; row++)
            {
                for (int col = 0; col < 5; col++)
                {
                    if (board[row, col, level] == -1)
                        display += " - ";
                    else
                        display += $" {board[row, col, level]} ";
                }
                display += "\n";
            }
            display += "\n";
        }

        // Update the TextMeshProUGUI element with the board representation
        boardText.text = display;
    }

    // Function to make a move and update the board
    public void MakeMove(int row, int col, int level, int player)
    {
        // Update the board with the player's move
        board[row, col, level - 1] = player;
        // Debug.Log(row + "-" + col + "-" + (level - 1));
        DisplayBoard();
        GameObject.Find("SoundEffects").GetComponent<AudioManager>().MakeMove();
        CheckWinCondition();
        previousMove = row.ToString() + col.ToString() + (level-1).ToString();
        reverseButton.SetActive(true);
    }

    public void ReverseMove()
    {
        int row = 0;
        int col = 0;
        int level = 0;
        int count = 0;
        foreach (char c in previousMove)
        {
            int number = c - '0';
            if (count == 0)
            {
                row = number;
            }
            else if (count == 1)
            {
                col = number;
            }
            else {
                level = number;
            }
            count++;
        }
        board[row, col, level] = -1;
        DisplayBoard();
        
        PlayerController player = GameObject.Find("Player").GetComponent<PlayerController>();
        int currPlayer = player.GetPlayerTurn();
        int currTurn = player.GetTotalTurn();
        player.SetPlayerTurn(Math.Abs(currPlayer - 1));
        player.SetTotalTurn(currTurn - 1);
        player.ChangeMaterialColor(player.GetPlayerTurn());
        gameFinished = false;
        Destroy(previousPiece);
        reverseButton.SetActive(false);
    }

    public void SetPrevPiece(GameObject piece)
    {
        previousPiece = piece;
    }


    // Function to check for a win condition
    private void CheckWinCondition()
    {
        int currentPlayer = GameObject.Find("Player").GetComponent<PlayerController>().GetPlayerTurn();

        // win
        if (WinDetected())
        {
            string winner;
            if (currentPlayer + 1 == 1)
            {
                winner = "BLACK";
            }
            else {
                winner = "WHITE";
            }
            gameFinished = true;
            reverseButton.SetActive(false);
            GameObject.Find("SoundEffects").GetComponent<AudioManager>().Win();
            stateText.text = winner + " won !";
        }
        else {
            if (GameObject.Find("Player").GetComponent<PlayerController>().GetTotalTurn() >= 64)
            {
                gameFinished = true;
                reverseButton.SetActive(false);
                stateText.text = "Tie !";
            }
        }
    }
    // Function to reset the game
    public void ResetGame()
    {
        InitializeGame();
    }

    private bool WinDetected()
    {
        // Horizontal, vertical, and diagonal wins
        for (int level = 0; level < 5; level++)
        {
            // Horizontal win
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    if (board[i, j, level] != -1 &&
                        board[i, j, level] == board[i, j + 1, level] &&
                        board[i, j, level] == board[i, j + 2, level] &&
                        board[i, j, level] == board[i, j + 3, level])
                    {
                        WinArrow(j);
                        WinArrow(j+1);
                        WinArrow(j+2);
                        WinArrow(j+3);
                        return true;
                    }
                }
            }

            // Vertical win
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (board[i, j, level] != -1 &&
                        board[i, j, level] == board[i + 1, j, level] &&
                        board[i, j, level] == board[i + 2, j, level] &&
                        board[i, j, level] == board[i + 3, j, level])
                    {
                        WinArrow(j);
                        return true;
                    }
                }
            }

            // Diagonal (left to right) win
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    if (board[i, j, level] != -1 &&
                        board[i, j, level] == board[i + 1, j + 1, level] &&
                        board[i, j, level] == board[i + 2, j + 2, level] &&
                        board[i, j, level] == board[i + 3, j + 3, level])
                    {
                        WinArrow(j);
                        WinArrow(j+1);
                        WinArrow(j+2);
                        WinArrow(j+3);
                        return true;
                    }
                }
            }

            // Diagonal (right to left) win
            for (int i = 0; i < 2; i++)
            {
                for (int j = 3; j < 5; j++)
                {
                    if (board[i, j, level] != -1 &&
                        board[i, j, level] == board[i + 1, j - 1, level] &&
                        board[i, j, level] == board[i + 2, j - 2, level] &&
                        board[i, j, level] == board[i + 3, j - 3, level])
                    {
                        WinArrow(j);
                        WinArrow(j-1);
                        WinArrow(j-2);
                        WinArrow(j-3);
                        return true;
                    }
                }
            }
        }

        // Check stacking 4 pieces vertically (level-wise)
        for (int row = 0; row < 5; row++)
        {
            for (int col = 0; col < 5; col++)
            {
                for (int level = 0; level < 2; level++)
                {
                    if (board[row, col, level] != -1 &&
                        board[row, col, level] == board[row, col, level + 1] &&
                        board[row, col, level] == board[row, col, level + 2] &&
                        board[row, col, level] == board[row, col, level + 3])
                    {
                        WinArrow(col);
                        return true;
                    }
                }
            }
        }

        ////// Check diagonals (top-left to bottom-right) for ascending order win
        for (int level = 0; level < 2; level++)
        {
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    if (board[i, j, level] != -1 &&
                        board[i, j, level] == board[i + 1, j + 1, level + 1] &&
                        board[i, j, level] == board[i + 2, j + 2, level + 2] &&
                        board[i, j, level] == board[i + 3, j + 3, level + 3])
                    {
                        WinArrow(j);
                        WinArrow(j+1);
                        WinArrow(j+2);
                        WinArrow(j+3);
                        return true;
                    }
                }
            }
        }

        ////// Check diagonals (bottom-left to top-right) for ascending order win
        for (int level = 0; level <= 1; level++)
        {
            for (int i = 3; i < 5; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    if (board[i, j, level] != -1 &&
                        board[i, j, level] == board[i - 1, j + 1, level + 1] &&
                        board[i, j, level] == board[i - 2, j + 2, level + 2] &&
                        board[i, j, level] == board[i - 3, j + 3, level + 3])
                    {
                        WinArrow(j);
                        WinArrow(j+1);
                        WinArrow(j+2);
                        WinArrow(j+3);
                        return true;
                    }
                }
            }
        }
        
        ////// Check diagonals (bottom-right to top-left) for ascending order win
        for (int level = 0; level <= 1; level++)
        {
            for (int i = 3; i < 5; i++)
            {
                for (int j = 3; j < 5; j++)
                {
                    if (board[i, j, level] != -1 &&
                        board[i, j, level] == board[i - 1, j - 1, level + 1] &&
                        board[i, j, level] == board[i - 2, j - 2, level + 2] &&
                        board[i, j, level] == board[i - 3, j - 3, level + 3])
                    {
                        WinArrow(j);
                        WinArrow(j-1);
                        WinArrow(j-2);
                        WinArrow(j-3);
                        return true;
                    }
                }
            }
        }

        // Check diagonals (top-right to bottom-left) for ascending order win
        for (int level = 0; level < 2; level++)
        {
            for (int i = 0; i < 2; i++)
            {
                for (int j = 3; j < 5; j++)
                {
                    if (i + 3 < 5 && j - 3 >= 0 &&
                        board[i, j, level] != -1 &&
                        board[i, j, level] == board[i + 1, j - 1, level + 1] &&
                        board[i, j, level] == board[i + 2, j - 2, level + 2] &&
                        board[i, j, level] == board[i + 3, j - 3, level + 3])
                    {
                        WinArrow(j);
                        WinArrow(j-1);
                        WinArrow(j-2);
                        WinArrow(j-3);
                        return true;
                    }
                }
            }
        }

        ////// Check diagonals (bottom-up) for ascending order win
        for (int level = 0; level <= 1; level++)
        {
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (board[i, j, level] != -1 &&
                        board[i, j, level] == board[i + 1, j, level + 1] &&
                        board[i, j, level] == board[i + 2, j, level + 2] &&
                        board[i, j, level] == board[i + 3, j, level + 3])
                    {
                        WinArrow(j);
                        return true;
                    }
                }
            }
        }

        ////// Check diagonals (top-down) for ascending order win
        for (int level = 0; level <= 1; level++)
        {
            for (int i = 3; i < 5; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (board[i, j, level] != -1 &&
                        board[i, j, level] == board[i - 1, j, level + 1] &&
                        board[i, j, level] == board[i - 2, j, level + 2] &&
                        board[i, j, level] == board[i - 3, j, level + 3])
                    {
                        WinArrow(j);
                        return true;
                    }
                }
            }
        }

        ////// Check diagonals (left-right) for ascending order win
        for (int level = 0; level <= 1; level++)
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    if (board[i, j, level] != -1 &&
                        board[i, j, level] == board[i, j + 1, level + 1] &&
                        board[i, j, level] == board[i, j + 2, level + 2] &&
                        board[i, j, level] == board[i, j + 3, level + 3])
                    {
                        WinArrow(j);
                        WinArrow(j+1);
                        WinArrow(j+2);
                        WinArrow(j+3);
                        return true;
                    }
                }
            }
        }

        ////// Check diagonals (right-left) for ascending order win
        for (int level = 0; level <= 1; level++)
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 3; j < 5; j++)
                {
                    if (board[i, j, level] != -1 &&
                        board[i, j, level] == board[i, j - 1, level + 1] &&
                        board[i, j, level] == board[i, j - 2, level + 2] &&
                        board[i, j, level] == board[i, j - 3, level + 3])
                    {
                        WinArrow(j);
                        WinArrow(j-1);
                        WinArrow(j-2);
                        WinArrow(j-3);
                        return true;
                    }
                }
            }
        }
        return false; // No win detected
    }

    private void WinArrow(int row)
    {
        Renderer rend;
        switch(row)
        {
            case 0:
                rend = arrow0.GetComponent<Renderer>();
                rend.material.color = Color.red;
                break;
            case 1:
                rend = arrow1.GetComponent<Renderer>();
                rend.material.color = Color.yellow;
                break;
            case 2:
                rend = arrow2.GetComponent<Renderer>();
                rend.material.color = Color.green;
                break;
            case 3:
                rend = arrow3.GetComponent<Renderer>();
                rend.material.color = Color.cyan;
                break;
            case 4:
                rend = arrow4.GetComponent<Renderer>();
                rend.material.color = new Color(219/ 255f, 3/ 255f, 252/ 255f);
                break;
        }
    }

    public bool IsGameFinished()
    {
        return gameFinished;
    }

}
