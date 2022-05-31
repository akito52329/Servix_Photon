using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;

public class CardGeneration : MonoBehaviour
{
    [SerializeField] GetCard getCard;

    [SerializeField] List<GameObject> deck = new List<GameObject>();
    [SerializeField] List<GameObject> genePos = new List<GameObject>();//生成場所

    public GameObject[] clickObject = new GameObject[6];
    [SerializeField] GameObject numInitialPos;//ナンバーの位置
    [SerializeField] GameObject cardInitialPos;//初期位置

    public void Generation(bool first)//カード生成
    {
       
        if (first)//Round１の場合
        {　
            var genePosCount = 0;
            genePosCount = genePos.Count;
            for (int card = 0; card < genePosCount; card++)
            {
                deck.First().transform.position = genePos[card].transform.position;//10か所に生成
                deck.RemoveAt(0);//生成したカードを除外する
            }
        }
        else
        {
            var clickObjeCount = clickObject.Length;

            for (int card = 0; card <= clickObjeCount - 1; card++)
            {
                deck.First().transform.position = clickObject[card].transform.position;//１度クリックされた場所に生成
                deck.RemoveAt(0);//生成したカードを除外する
            }
        }
    }


    public void Shuffle() // デッキをシャッフルする
    {
        deck = deck.OrderBy(shuffle => Guid.NewGuid()).ToList();
    }


    public void CardChenge()
    {
        for(int card = 0; card < clickObject.Length; card++)//クリックした分だけチェンジ
        {
            clickObject[card].transform.position = cardInitialPos.transform.position;
            getCard.numberUi[card].transform.position = numInitialPos.transform.position;
        }
    }

}
