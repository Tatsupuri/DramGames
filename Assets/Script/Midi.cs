using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class Midi : MonoBehaviour
{
    public static Midi instance;

    [DllImport("__Internal")]
    private static extern int FindMIDI();

    [SerializeField] Text text;

    void Awake()
    {

		if (instance != null) 
        {
			Destroy(this.gameObject);
		}
         else if (instance == null)
        {
			instance = this;
		}

		DontDestroyOnLoad (this.gameObject);
    }

    
    void Start()
    {
        
    }

    
    void Update()
    {
        int num = FindMIDI();
        text.text = "Detected MIDI Source:" + num.ToString();

        if(num == 1 && SceneManager.GetActiveScene().name　== "Setup")
        {
            SceneManager.LoadScene ("bdTest");
        }
        
    }
}
