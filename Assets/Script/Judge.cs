using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Judge : MonoBehaviour
{

    //private GameObject justNote;
    public Text text;

    private void NoteJudge(string str)
    {
        text.text = str;

        foreach (GameObject justNote in GameObject.FindGameObjectsWithTag("now"))
        {      
            if(justNote.GetComponent<Note>().lineNumber == Line(str))//MIDIからのトーン情報から参照
            {
                justNote.GetComponent<Note>().Success();
            }
        }
    }

    private int Line(string tone)
    {
        if(tone == "36")//バスドラム
        {
            return 4;
        }
        else if(tone == "38")//スネア
        {
            return 3;
        }
        else if(tone == "42")//クローズドハイハット
        {
            return 2;
        }

        return 0;
    }
}
