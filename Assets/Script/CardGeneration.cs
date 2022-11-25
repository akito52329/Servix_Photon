using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;

public class CardGeneration : MonoBehaviourPunCallbacks
{
    [SerializeField] GameDirector gameDirector;
    [SerializeField] GetCard getCard;
    [SerializeField] Data data;
    [SerializeField] TextMove textMove;
    [SerializeField] GameObject parent;//生成するオブジェクトの親
    [SerializeField] List<ObjectData> objectDatas = new List<ObjectData>();
    [SerializeField] List<int> decks = new List<int>();
    [SerializeField] List<GameObject> genePos = new List<GameObject>();//生成場所
    [SerializeField] int kindMax = 3;//一枚あたりの枚数

    public GameObject[] clickObject = new GameObject[6];
    [SerializeField] GameObject numInitialPos;//ナンバーの位置
    [SerializeField] GameObject cardInitialPos;//初期位置
    private List<GameObject> _disPlayObject = new List<GameObject>();


    public List<GameObject> _genPlayObjects = new List<GameObject>();
    public List<GameObject> genPlayObject
    {
        get { return _genPlayObjects; }
        set
        {
            _genPlayObjects = value;
            Debug.Log(23445667);
            Generation(false);
        }
    }



    private void Awake()
    {
        for(int i = 0; i < objectDatas.Count; i++)
        {
            for(int k = 0; k < kindMax; k++)
            {
                decks.Add(i);
            }
        }
        // Shuffle();

    }

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            // 実際の処理
            Shuffle();

        }

        
    }


    public void Generation(bool first)//カード生成
    {
        Debug.Log("来ちゃー");
        if (first)//Round１の場合
        {
            var genePosCount = 0;
            genePosCount = genePos.Count;
            for (int card = 0; card < genePosCount; card++)
            {
                objectDatas.First().transform.position = genePos[card].transform.position;//10か所に生成
                string name = data.data[decks.First()].Name;
                Debug.Log(name);
                GameObject geneCard = PhotonNetwork.Instantiate(name, genePos[card].transform.position, Quaternion.identity);
                geneCard.transform.parent = parent.transform;
                decks.RemoveAt(0);//生成したカードを除外する
            }
         /*   gameDirector.chenge = true;
            photonView.RPC(nameof(GiveChenge), RpcTarget.Others, gameDirector.chenge);*/
        }
        else
        {

            // CardChenge();
            var clickObjeCount = genPlayObject.Count;

            for (int card = 0; card < clickObjeCount; card++)
            {
                string name = data.data[decks.First()].Name;
                GameObject geneCard = PhotonNetwork.Instantiate(name, genPlayObject[card].transform.position, Quaternion.identity);
                geneCard.transform.parent = parent.transform;
                decks.RemoveAt(0);//生成したカードを除外する
            }
            CardChenge();
        }
       // photonView.RPC(nameof(GiveDeck), RpcTarget.Others, decks.ToArray());



    }


    public void Shuffle() // デッキをシャッフルする
    {
        decks = decks.OrderBy(shuffle => Guid.NewGuid()).ToList();
        photonView.RPC(nameof(GiveDeck), RpcTarget.Others, decks);
    }


    public void CardChenge()
    {
        Debug.Log("fhfhfgjhgfj");
        for(int card = 0; card < genPlayObject.Count; card++)//クリックした分だけチェンジ
        {
            PhotonNetwork.Destroy(genPlayObject[card].gameObject);
           // clickObject[card].transform.position = cardInitialPos.transform.position;
            getCard.numberUi[card].transform.position = numInitialPos.transform.position;
        }

     /*   gameDirector.chenge = true;
        photonView.RPC(nameof(GiveChenge), RpcTarget.Others, gameDirector.chenge);*/
    }

    [PunRPC]
    public void GiveDeck(int[] list)
    {
        decks = list.ToList();
    }

    [PunRPC]
    public void GiveChenge(bool b)
    {
        gameDirector.chenge = b;
    }

}
