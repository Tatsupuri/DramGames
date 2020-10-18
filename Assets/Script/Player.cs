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

    public void AddScore(int point)
    {
        score += point;
    }

    public void Damage(int damage) 
    {
        life -= damage;
    }

    public int Life() 
    {
        return life;
    }

    public int Score()
    {
        return score;
    }

}
