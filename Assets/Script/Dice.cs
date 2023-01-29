using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using DG.Tweening;

public class Dice : MonoBehaviourPunCallbacks
{
    public enum DiceState { Standby, Roll, Wait, Finish}
    private DiceState _diceState = DiceState.Standby;
    public DiceState diceState
    {
        get { return _diceState; }
        set
        {
            if(value != _diceState)
            {
                _diceState = value;
                ChengeState();
            }
            
        }
    }

    [SerializeField] List<Sprite> diceSprites;
    [SerializeField] RectTransform diceRect;
    [SerializeField] Image diceImage;
    [SerializeField] Image yourDiceImage;
    [SerializeField] float maxUp = 400f;
    [SerializeField] float upSpeed = 2f;
    [SerializeField] GameObject afterThenButton;
    [SerializeField] List<Button> buttons;
    private int _number;
    public int number
    {
        get { return _number; }
        set
        {
            _number = value;
            photonView.RPC(nameof(YourChengeSprit), RpcTarget.Others, _number);
        }
    }
    private int _yourNumber;
    public int yourNumber
    {
        get { return _yourNumber; }
        set
        {
            _yourNumber = value;
            youCheck = true;
            
        }
    }

    
    private int _finishNumber;
    public int finishNumber
    {
        get { return _finishNumber; }
        set
        {
            _finishNumber = value;
            photonView.RPC(nameof(YourFinish), RpcTarget.Others, finishNumber);
            
        }
    }
    private bool start;

    bool check = false;
    bool youCheck = false;
   
    
    public void ChengeState()
    {
        switch(diceState)
        {
            case DiceState.Standby:
                if (PhotonNetwork.IsMasterClient)
                {
                    buttons[0].gameObject.SetActive(true);
                }
                else
                {
                    buttons[1].gameObject.SetActive(true);
                }
                break;
            case DiceState.Roll:
                StartCoroutine(DiceRoll());
               
                break;
            case DiceState.Wait:
                check = true;
                finishNumber = int.Parse(diceImage.sprite.name);
                break;
            case DiceState.Finish:
               
                Judge();
                break;
        }    
    }

    void Start()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            buttons[1].gameObject.SetActive(false);
        }
        else
        {
            buttons[0].gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if(diceState == DiceState.Roll)
        {
            int num = UnityEngine.Random.Range(0, 6);
            diceImage.sprite = diceSprites[num];
            number = num;
        }

        if (diceState == DiceState.Wait)
        {
            if (youCheck && youCheck == check)
            {
                diceState = DiceState.Finish;
            }
        }

    }

    public void OnDiceRoll(Button b)
    {
        b.gameObject.SetActive(false);
        diceState = DiceState.Roll;
    }

    IEnumerator DiceRoll()
    {
        diceRect.DOAnchorPosY(maxUp, upSpeed).OnComplete(() => diceRect.DOAnchorPosY(0, upSpeed));
        yield return new WaitForSeconds(5);

        diceState = DiceState.Wait;
    }

    [PunRPC]
    public void YourChengeSprit(int num)
    {
        yourDiceImage.sprite = diceSprites[num];
    }

    [PunRPC]
    public void YourFinish(int n)
    {
        yourNumber = n;
    }

    public void Judge()
    {
        Debug.Log($"{yourNumber} vs {finishNumber}");

        if (yourNumber < finishNumber)
        {
            afterThenButton.SetActive(true);
        }
        else if(yourNumber == finishNumber)
        {
            diceState = DiceState.Standby;
        }
        else
        {
            afterThenButton.SetActive(false);
        }
    }
}
