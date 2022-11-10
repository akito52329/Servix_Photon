using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;

public class CardGeneration : MonoBehaviourPunCallbacks
{
    [SerializeField] GetCard getCard;
    [SerializeField] Data data;

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
            photonView.RPC(nameof(GiveDeck), RpcTarget.OthersBuffered, decks);
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
        }
        else
        {
            
            CardChenge();
            var clickObjeCount = clickObject.Length;

            for (int card = 0; card <= clickObjeCount - 1; card++)
            {
                string name = data.data[decks.First()].Name;
                GameObject geneCard = PhotonNetwork.Instantiate(name, clickObject[card].transform.position, Quaternion.identity);
                geneCard.transform.parent = parent.transform;
                decks.RemoveAt(0);//生成したカードを除外する
            }
        }
        photonView.RPC(nameof(GiveDeck), RpcTarget.OthersBuffered, decks);
    }


    public void Shuffle() // デッキをシャッフルする
    {
        decks = decks.OrderBy(shuffle => Guid.NewGuid()).ToList();
    }


    public void CardChenge()
    {

        for(int card = 0; card < clickObject.Length; card++)//クリックした分だけチェンジ
        {
            PhotonNetwork.Destroy(clickObject[card].gameObject);
           // clickObject[card].transform.position = cardInitialPos.transform.position;
            getCard.numberUi[card].transform.position = numInitialPos.transform.position;
        }
    }

    [PunRPC]
    public void GiveDeck(List<int> list)//相手側に自分の最終スコアを渡す
    {
        decks = list;
    }
}
