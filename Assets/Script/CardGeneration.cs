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

    public GameObject[] clickObject = new GameObject[6];
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
                generatReply.GeneratCards(name, genePos[card].transform, true);
                decks.RemoveAt(0);//生成したカードを除外する
            }
         /*   gameDirector.chenge = true;
            photonView.RPC(nameof(GiveChenge), RpcTarget.Others, gameDirector.chenge);*/
        }
        else
        {

            // CardChenge();
            var clickObjeCount = clickObject.Length;

            for (int card = 0; card < clickObjeCount; card++)
            {
                string name = data.data[decks.First()].Name;
                generatReply.GeneratCards(name, clickObject[card].transform, false);
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
        for(int card = 0; card < clickObject.Length; card++)//クリックした分だけチェンジ
        {
            PhotonNetwork.Destroy(clickObject[card].gameObject);
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

}
