using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ObjectData : MonoBehaviour
{
    [SerializeField] RoleCheck roleCheck;
    [SerializeField] CardData cardData;
    [SerializeField] GetCard getCard;
    [SerializeField] Button myButton;
    [SerializeField] CardGeneration cardGeneration;
    [SerializeField] AudioController audio;
    [SerializeField] Data data;

    private int maxClick = 6;//クリックの回数
    [SerializeField] int id;
    public string name;//オブジェクトの名前
    public char nameTop;//頭文字
    public char nameEnd;//語尾
    public Color myColor;//オブジェクトの色
    public int myNumber;//オブジェクトの持っている数字

    void Awake()
    {
        name = data.data[id].Name;
        nameTop = name.First();
        nameEnd = name.Last();
        myColor = cardData.CadeDetaColor(name);
        myNumber = cardData.CadeDetaNumber(name);

    }

    private void Start()
    {
        if(roleCheck == null)
        {
            roleCheck = GameObject.Find("Role").GetComponent<RoleCheck>();
        }

        if(getCard == null)
        {
            getCard = GameObject.Find("GetCard").GetComponent<GetCard>();
        }

        if(audio == null)
        {
            audio = GameObject.Find("AudioController").GetComponent<AudioController>();
        }

        if(cardGeneration == null)
        {
            cardGeneration = GameObject.Find("CardGeneration").GetComponent<CardGeneration>();

        }

       if (transform.parent == null)
       {
           transform.parent = GameObject.Find("Parent").transform;
       }

        myButton.onClick.AddListener(() => ClickCard(this.gameObject));//onClickに押した時の処理を代入
    }

    void Update()
    {
        if (getCard != null)
        {
            if (getCard.onClickCount == maxClick)
            {
                if (myButton != null && myButton.interactable != false)
                {
                    CadeFalse();
                }
            }
        }
    }

    void CadeFalse()//ボタンを押せなくする
    {
        if (myButton != null)
        {
            myButton.interactable = false;
        }
    }

    /*public void MyButton()//ボタンを押されたら処理する
    {
        Debug.Log(987565);
        audio.ClickAudio();
        myButton.interactable = false;
        if (roleCheck != null)
        {
            roleCheck.Role(myColor, nameTop, nameEnd, myNumber, name);
        }
    }*/


    private void ClickCard(GameObject card)//ボタンが押されたときに
    {
        audio.ClickAudio();
        myButton.interactable = false;

        if (getCard.onClickCount <= cardGeneration.clickObject.Length)
        {
            cardGeneration.clickObject[getCard.onClickCount] = card;
            getCard.SelectCard();
        }

        if (roleCheck != null)
        {
            roleCheck.Role(myColor, nameTop, nameEnd, myNumber, name);
        }
    }
}

