using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Klak.Timeline.Midi;
//using Klak.Timeline.Midi.MidiFileAssetImporter;

public class NoteGenerator : MonoBehaviour
{
    [SerializeField] string file;//Pathの直打ち

    private MidiEvent[] midiEventSet;
    public GameObject notePrefab;

    [SerializeField] private float tempo = 120;
    private int duration;
    private int tpqn;
    private int numOfData;
    private int bars;

    [SerializeField] float startPoint = 10;

    private int index;
    private float qndt; //Quater note delta time
    private float ratio;
    private float time;
    [SerializeField] private float timeOffset = 3;

    public bool audio;
  

    void Start()
    {
        MidiSet();
        qndt = 60f / tempo;
        ratio = tpqn / qndt; //Second to Ticks
        time = -timeOffset;

        audio = false;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        if (index < numOfData)//Indexがデータの総量を超えてたら何もしない
        {
            //Debug.Log(midiEventSet[i].time + "," + midiEventSet[i].status + "," + midiEventSet[i].data1 + "," + midiEventSet[i].data2);
            //Debug.Log(time * ratio);

            while (time * ratio >= midiEventSet[index].time && index < numOfData) //以下の処理で同時刻のノートをすべて発生させる
            {
                //Debug.Log(time * ratio >= midiEventSet[index].time);
                if (midiEventSet[index].data2 == 80 || midiEventSet[index].data2 == 90 || midiEventSet[index].data2 == 94 ) //発音情報かどうか？
                {
                    NoteSet((float)midiEventSet[index].time / 200f, midiEventSet[index].data1);
                }

                index += 1;
            }
        }
        else
        {
            if (time > duration / ratio + timeOffset) 
            {
                Debug.Log("END");
                Application.Quit();
            }
        }
    }

    void MidiSet() 
    {
        var buffer = File.ReadAllBytes(file);
        var asset = MidiFileDeserializer.Load(buffer);


        if (asset.tracks.Length == 1)
        {
            var track = asset.tracks[0];//Trackの取得

            //曲の基本情報
            //tempo = track.template.tempo;
            duration = (int)track.template.duration;
            tpqn = (int)track.template.ticksPerQuarterNote;
            numOfData = track.template.events.Length;
            bars = (duration + tpqn * 4 - 1) / (tpqn * 4);

            Debug.Log("Tempo = " + tempo);
            Debug.Log("Duratoin = " + duration);
            Debug.Log("Ticks/QuarterNote = " + tpqn);
            Debug.Log(bars + " bars");
            Debug.Log("# of data = " + numOfData);


            midiEventSet = track.template.events;

            foreach (MidiEvent midiEvent in midiEventSet)
            {
               //Debug.Log(midiEvent.time+","+ midiEvent.status + ","+ midiEvent.data1 + ","+ midiEvent.data2);
            }
        }
        else
        {
            Debug.LogError("In this program, # of track must be 1");
        }
    }

    void NoteSet(float time,int tone) 
    {
        if (tone == 51 || tone == 59 || tone == 47)//ride, low mid
        {
            Instantiate(notePrefab, new Vector3(3.0f, 0, startPoint), Quaternion.identity);
        }
        else if (tone == 35 || tone == 36 || tone == 43)//bass, high floor
        {
            Instantiate(notePrefab, new Vector3(1.5f, 0, startPoint), Quaternion.identity);
        }
        else if (tone == 38 || tone == 40)//snare
        {
            Instantiate(notePrefab, new Vector3(0, 0, startPoint), Quaternion.identity);
        }
        else if (tone == 42) //closed high hat
        {
            Instantiate(notePrefab, new Vector3(-1.5f, 0, startPoint), Quaternion.identity);
        }
        else if (tone == 49 || tone == 57 || tone == 46 || tone == 44) //crash, open high hat, pedal high hat
        {
            Instantiate(notePrefab, new Vector3(-3.0f, 0, startPoint), Quaternion.identity);
        }

    }
}
