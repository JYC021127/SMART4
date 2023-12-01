using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiatePiece : MonoBehaviour
{
    [SerializeField] GameObject blackPiece;
    [SerializeField] GameObject whitePiece;
    public ParticleSystem particle;

    public void Spawn()
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
        pieceTransform.parent = transform;
        pieceTransform.position = new Vector3(transform.position.x, transform.position.y + 6.45f, transform.position.z);
    }
}
