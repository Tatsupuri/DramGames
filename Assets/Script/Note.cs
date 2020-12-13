using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.VFX;
using UnityEngine.VFX;

public class Note : MonoBehaviour
{
    public bool initial = false;
    public int lineNumber;

    [SerializeField] float speed = 0.05f;
    [SerializeField] float deadPoint = 0;

    [SerializeField] float mag = 1.2f;

    [SerializeField] Material defaultMat;
    [SerializeField] Material touchedMat;

    private Transform VFXs;
    private VisualEffect VFX;



    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.GetComponent<MeshRenderer>().material = defaultMat;
        player = GameObject.FindGameObjectWithTag("Player");
        VFXs = GameObject.FindGameObjectWithTag("VFX").transform;
        VFX = VFXs.GetChild(lineNumber - 1).GetComponent<VisualEffect>();
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Translate(0, 0, -speed);

        if (initial && this.transform.position.z < 0 && !GameObject.FindGameObjectWithTag("GameController").GetComponent<NoteGenerator>().audio)
        {//ノートがバーについたときまだ音楽が始まっていなかったら、つまり最初のノートの時音楽を開始する

            GameObject.FindGameObjectWithTag("GameController").GetComponent<AudioSource>().Play();
            GameObject.FindGameObjectWithTag("GameController").GetComponent<NoteGenerator>().audio = true;
        }
            

        if (this.transform.position.z < deadPoint)//プレハブを破壊
        {
            Destroy(this.gameObject);
        }
    }

    public void Success()
    {
        VFX.SendEvent("OnPlay");
        //player.GetComponent<Player>().AddScore(1);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
         if (other.gameObject.tag == "Bar")
        {
            this.gameObject.tag = "now";
            this.gameObject.GetComponent<MeshRenderer>().material = touchedMat;
            transform.localScale = new Vector3(mag, mag, mag);
        }
    }

    // private void OnTriggerStay(Collider other)
    // {
    //     if (other.gameObject.tag == "Bar") 
    //     {
    //         //if(Judge(lineNumber))//バーに触れているとき、入力があったかをJudgeして、あれば破壊して得点を与える。
    //         if(ToneToLine() == lineNumber)
    //         {
    //             VFX.SendEvent("OnPlay");
    //             player.GetComponent<Player>().AddScore(1);
    //             Destroy(this.gameObject);
    //         }

    //     }
    // }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Bar" && !initial)
        {
            this.gameObject.tag = "note";
            this.gameObject.GetComponent<MeshRenderer>().material = defaultMat;
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            //この段階でまだ破壊されていない時はミス。プレイヤーにダメージ。
            player.GetComponent<Player>().Damage(1);
        }
    }

    private void OnDestroy()
    {
        if(this.gameObject.tag == "now")
        {
            player.GetComponent<Player>().AddScore(1);
        }
    }
    // private int ToneToLine()
    // {
    //     var tone = Midi.instance.tone;

    //     if(tone == 36)//バスドラム
    //     {
    //         return 4;
    //     }
    //     else if(tone == 38)//スネア
    //     {
    //         return 3;
    //     }
    //     else if(tone == 42)//クローズドハイハット
    //     {
    //         return 2;
    //     }

    //     return 0;
    // }

}
