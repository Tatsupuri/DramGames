using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Klak.Timeline.Midi;
//using Klak.Timeline.Midi.MidiFileAssetImporter;

public class NoteGenerator : MonoBehaviour
{
    [SerializeField] string file;//Pathの直打ち
    public bool toConsole;

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
    private float runTimeTick;
    [SerializeField] private float timeOffset = 3;

    public bool audio;

    [SerializeField] private List<int> midiEventData;

    

    void Start()
    {
        MidiSet();
        AudioSpeed();
        time = -timeOffset;
        runTimeTick = -timeOffset * ratio;

        audio = false;
    }

    // Update is called once per frame
    void Update()
    {
        //time += Time.deltaTime;
        runTimeTick += Time.deltaTime * ratio;

        if (index < numOfData)//Indexがデータの総量を超えてたら何もしない
        {

            while (runTimeTick >= midiEventSet[index].time && index < numOfData) //以下の処理で同時刻のノートをすべて発生させる
            {
                if (index == 0)
                {
                    Init();
                }

                    if (midiEventSet[index].status == 255 && midiEventSet[index].data1 == 81)//テンポ情報かどうかの判定 
                    {
                        tempo = midiEventSet[index].data2;
                        //AudioSpeed();
                    }
                    else if (midiEventSet[index].status == 153) //発音情報かどうか？
                    // && midiEventData.Contains(midiEventSet[index].data2)
                    //(midiEventSet[index].data2 == 64 || midiEventSet[index].data2 == 80 || midiEventSet[index].data2 == 90 || midiEventSet[index].data2 == 94 || midiEventSet[index].data2 == 100 || midiEventSet[index].data2 == 106 || midiEventSet[index].data2 == 110 || midiEventSet[index].data2 == 112 || midiEventSet[index].data2 == 114 || midiEventSet[index].data2 == 127)
                {
                    NoteSet(midiEventSet[index].data1);
                    }

                index += 1;
            }

            AudioSpeed();
        }
        else
        {
            if (runTimeTick > duration + timeOffset * ratio)
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

            if (toConsole)
            {
                foreach (MidiEvent midiEvent in midiEventSet)
                {
                    Debug.Log(midiEvent.time + "," + midiEvent.status + "," + midiEvent.data1 + "," + (uint)midiEvent.data2);
                }
            }
        }
        else
        {
            Debug.LogError("In this program, # of track must be 1");
        }
    }

    void AudioSpeed()
    {
        qndt = 60f / tempo;
        ratio = tpqn / qndt; //Second to Ticks
    }

    void NoteSet(int tone)
    {
        if (tone == 51 || tone == 59 || tone == 47)//ride, low mid
        {
            GameObject note = Instantiate(notePrefab, new Vector3(3.0f, 0, startPoint), Quaternion.identity);
            note.GetComponent<Note>().lineNumber = 5;
        }
        else if (tone == 35 || tone == 36 || tone == 43)//bass, high floor
        {
            GameObject note = Instantiate(notePrefab, new Vector3(1.5f, 0, startPoint), Quaternion.identity);
            note.GetComponent<Note>().lineNumber = 4;
        }
        else if (tone == 38 || tone == 40)//snare
        {
            GameObject note = Instantiate(notePrefab, new Vector3(0, 0, startPoint), Quaternion.identity);
            note.GetComponent<Note>().lineNumber = 3;
        }
        else if (tone == 42) //closed high hat
        {
            GameObject note = Instantiate(notePrefab, new Vector3(-1.5f, 0, startPoint), Quaternion.identity);
            note.GetComponent<Note>().lineNumber = 2;
        }
        else if (tone == 49 || tone == 57 || tone == 46 || tone == 44) //crash, open high hat, pedal high hat
        {
            GameObject note = Instantiate(notePrefab, new Vector3(-3.0f, 0, startPoint), Quaternion.identity);
            note.GetComponent<Note>().lineNumber = 1;
        }

    }

    void Init() 
    {
        GameObject init = Instantiate(notePrefab, new Vector3(0, 0, startPoint), Quaternion.identity);
        init.GetComponent<Note>().initial = true;
        init.GetComponent<MeshRenderer>().enabled = false;
    }
}
