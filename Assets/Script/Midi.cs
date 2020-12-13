using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class Midi : MonoBehaviour
{
    //public static Midi instance;//シングルトン

    // [DllImport("__Internal")]
    // private static extern int FindMIDI();

    // [DllImport("__Internal")]
    // private static extern bool MIDIConnection();

    // [DllImport("__Internal")]
    // private static extern uint GetTone();

    // [DllImport("__Internal")]
    // private static extern uint GetTime();

    [DllImport("__Internal")]
    private static extern bool GetMIDI();

    [SerializeField] Text numLog;
    [SerializeField] Text connectionLog;

    private int num = 0;
    private bool isConnection = false;

    public uint tone;
    public uint time;

    // void Awake()
    // {

	// 	if (instance != null) 
    //     {
	// 		Destroy(this.gameObject);
	// 	}
    //      else if (instance == null)
    //     {
	// 		instance = this;
	// 	}

	// 	DontDestroyOnLoad (this.gameObject);
    // }
    
    void Update()
    {
        MIDISet();
        //numLog.text = "Detected MIDI Source : " + num.ToString();
        numLog.text = "Title : ";
        connectionLog.text = "Connection : " + isConnection.ToString();

        //tone = GetTone();
        //time = GetTime();
    }

    public void NewGame()
    {
        //if(num == 1 && isConnection)
        if(isConnection)
        {
            SceneManager.LoadScene ("bdTest");
        }
        
    }

    private void MIDISet()
    {
        //num = FindMIDI();
        //isConnection = MIDIConnection();
        isConnection = GetMIDI();
    }
}
