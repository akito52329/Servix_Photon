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
    [SerializeField] GeneratReply generatReply;//生成するオブジェクトの親
    [SerializeField] GameObject parent;//生成するオブジェクトの親
    [SerializeField] List<ObjectData> objectDatas = new List<ObjectData>();
    [SerializeField] List<int> decks = new List<int>();
    [SerializeField] List<GameObject> genePos = new List<GameObject>();//生成場所
    [SerializeField] int kindMax = 3;//一枚あたりの枚数



    public int[] clickObject = new int[6];
    private int[] _yourClickObject;
    public int[] yourClickObject
    {
        get { return _yourClickObject; }
        set
        {
            Debug.Log("6464");
            _yourClickObject = value;
            Generation(false);
        }
    }
    [SerializeField] GameObject numInitialPos;//ナンバーの位置
    [SerializeField] GameObject cardInitialPos;//初期位置



    private void Awake()
    {
        for(int i = 0; i < objectDatas.Count; i++)
        {
            for(int k = 0; k < kindMax; k++)
            {
                decks.Add(i);
            }
        }

        if (PhotonNetwork.IsMasterClient)
        {
            // 実際の処理
            Shuffle();


        }      
    }


    public void FastGeneration()//カード生成
    {
        var genePosCount = 0;
        genePosCount = genePos.Count;
        for (int card = 0; card < genePosCount; card++)
        {
           // objectDatas.First().transform.position = genePos[card].transform.position;//10か所に生成
            string name = data.data[decks.First()].Name;
            generatReply.GeneratCards(name, genePos[card].transform, true, card);
            decks.RemoveAt(0);//生成したカードを除外する
        }
    }

    public void Generation(bool first)//カード生成
    {
        var clickObjeCount = 0;
        Debug.Log("来ちゃー");
        List<int> cl = new List<int>();
     
        if (first)//Round１の場合
        {
            cl = clickObject.ToList();
            cl.Sort();
            clickObject = cl.ToArray();
            Debug.Log("my");
            clickObjeCount = clickObject.Length;

            for (int card = 0; card < clickObjeCount; card++)
            {
                string name = data.data[decks.First()].Name;
                generatReply.GeneratCards(name, genePos[clickObject[card]].transform, false, clickObject[card]);
                decks.RemoveAt(0);//生成したカードを除外する
            }
            CardChenge(clickObjeCount, clickObject);
        }
        else
        {
            cl = yourClickObject.ToList();
            cl.Sort();
            _yourClickObject = cl.ToArray();

            // CardChenge();
            clickObjeCount = yourClickObject.Length;

            for (int card = 0; card < clickObjeCount; card++)
            {
                string name = data.data[decks.First()].Name;
                generatReply.GeneratCards(name, genePos[yourClickObject[card]].transform, false, yourClickObject[card]);
                decks.RemoveAt(0);//生成したカードを除外する
            }
            CardChenge(clickObjeCount, yourClickObject);
        }
         generatReply.Numbers();


    }

        public void Shuffle() // デッキをシャッフルする
    {
        decks = decks.OrderBy(shuffle => Guid.NewGuid()).ToList();
    }


    private void CardChenge(int ob ,int[] objects)
    {
        List<ObjectData> obd = new List<ObjectData>();
        foreach(ObjectData o in generatReply.gameObject.GetComponentsInChildren<ObjectData>())
        {
            obd.Add(o);
        }

        for(int card = 0; card < ob; card++)//クリックした分だけチェンジ
        {
            GameObject o = obd.Find(n => n.GetPostionNumber() == objects[card]).gameObject;
            PhotonNetwork.Destroy(o);
           // clickObject[card].transform.position = cardInitialPos.transform.position;
            getCard.numberUi[card].transform.position = numInitialPos.transform.position;
        }
     /*   gameDirector.chenge = true;
        photonView.RPC(nameof(GiveChenge), RpcTarget.Others, gameDirector.chenge);*/
    }

}
