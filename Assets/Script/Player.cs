using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] int initialLife = 10;
    private int life;
    private int score;

    private void Start()
    {
        life = initialLife;
    }

    private void AddScore(int point)
    {
        score += point;
    }

    private void Damage(int damage) 
    {
        life -= damage;
    }

}
