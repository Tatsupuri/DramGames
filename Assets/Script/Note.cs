using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    public bool initial = false;
    public int lineNumber;

    [SerializeField] float speed = 0.05f;
    [SerializeField] float deadPoint = 0;

    [SerializeField] float mag = 1.2f;

    [SerializeField] Material defaultMat;
    [SerializeField] Material touchedMat;

    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.GetComponent<MeshRenderer>().material = defaultMat;
        Debug.Log(lineNumber);
        player = GameObject.FindGameObjectWithTag("Player");
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

    private void OnTriggerEnter(Collider other)
    {
         if (other.gameObject.tag == "Bar")
        {
            this.gameObject.GetComponent<MeshRenderer>().material = touchedMat;
            transform.localScale = new Vector3(mag, mag, mag);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Bar") 
        {
            //if(Judge(lineNumber))//バーに触れているとき、入力があったかをJudgeして、あれば破壊して得点を与える。
            if(lineNumber == 3)
            {
                Destroy(this.gameObject);
                player.GetComponent<Player>().AddScore(1);             
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Bar")
        {
            this.gameObject.GetComponent<MeshRenderer>().material = defaultMat;
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            //この段階でまだ破壊されていない時はミス。プレイヤーにダメージ。
            player.GetComponent<Player>().Damage(1);
        }
    }
    
    private void Dead() //このObjectがバーに触れているときにこのメソッドが呼ばれると得点。
    {
        
    }
}
