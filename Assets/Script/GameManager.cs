using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Text score;
    [SerializeField] private Text life;

    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }


    void Update()
    {
        score.text = "Score:" + player.GetComponent<Player>().Score().ToString();
        life.text = "Life:" + player.GetComponent<Player>().Life().ToString();
    }
}
