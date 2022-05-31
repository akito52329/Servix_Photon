using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectData : MonoBehaviour
{
    [SerializeField] RoleCheck roleCheck;
    [SerializeField] CardData cardData;
    [SerializeField] GetCard getCard;
    [SerializeField] Button myButton;
    [SerializeField] AudioController audio;

    private int maxClick = 6;//クリックの回数
    public string name;//オブジェクトの名前
    public char nameTop;//頭文字
    public char nameEnd;//語尾
    public Color myColor;//オブジェクトの色
    public int myNumber;//オブジェクトの持っている数字

    void Awake()
    {
        name = gameObject.name;
        nameTop = name[0];
        nameEnd = name[gameObject.name.Length - 1];
        myColor = cardData.CadeDetaColor(name);
        myNumber = cardData.CadeDetaNumber(name);
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

    public void MyButton()//ボタンを押されたら処理する
    {
        audio.ClickAudio();
        if (roleCheck != null)
        {
            roleCheck.Role(myColor, nameTop, nameEnd, myNumber, name);
        }
    }
}

