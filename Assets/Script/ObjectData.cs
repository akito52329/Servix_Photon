﻿using System.Collections;
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

    private AudioController audioController;
    private AudioSource se;
    [SerializeField] AudioClip seClip;

    private int maxClick = 6;//クリックの回数
    [SerializeField] int id;
    public string name;//オブジェクトの名前
    public char nameTop;//頭文字
    public char nameEnd;//語尾
    public Color myColor;//オブジェクトの色
    public int myNumber;//オブジェクトの持っている数字
    public int myPosNumber;

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
            roleCheck = GameObject.Find("RoleCheck").GetComponent<RoleCheck>();
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
            foreach(Transform child in GameObject.Find("Parent").gameObject.GetComponentsInChildren<Transform>())
            {
                if(child.name == myNumber.ToString())
                {
                    transform.parent = child;

                    if (!cardGeneration.master)
                    {
                        transform.position = Vector3.zero;
                       // myPosNumber = int.Parse(GetComponentInParent<GameObject>().name);
                    }
                    break;
                }
            }
           
       }

            
        // 
        se = GetComponent<AudioSource>();
        audioController = new AudioController(se, seClip);

        myButton.onClick.AddListener(() => ClickCard());//onClickに押した時の処理を代入
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


    public int GetPostionNumber()
    {
        return myPosNumber;
    }

    public void SetPostionNumber(int num)
    {
        Debug.Log(num);
        myPosNumber = num;
    }

    private void ClickCard()//ボタンが押されたときに
    {



        myButton.interactable = false;

        if (getCard.onClickCount <= cardGeneration.clickObject.Length)
        {
            cardGeneration.clickObject[getCard.onClickCount] = myPosNumber;
            getCard.SelectCard();
        }

        if (roleCheck != null)
        {
            roleCheck.Role(myColor, nameTop, nameEnd, myNumber, name);
        }

        audioController.ChengePlayAudio(true);
    }
}

