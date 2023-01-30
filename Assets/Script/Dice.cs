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
            if( _diceState != value )
            {
                _diceState = value;
                ChengeState();
            }
            
        }
    }

    public Dice dice;
    [SerializeField] List<Sprite> diceSprites;
    [SerializeField] RectTransform diceRect;
    [SerializeField] Image diceImage;
    [SerializeField] Image yourDiceImage;
    [SerializeField] float maxUp = 400f;
    [SerializeField] float upSpeed = 2f;
    [SerializeField] GameObject afterThenButton;
    [SerializeField] Button button;
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

    public int yourNumber;
    
    private int _finishNumber;
    public int finishNumber
    {
        get { return _finishNumber; }
        set
        {
            _finishNumber = value;
            photonView.RPC(nameof(YourFinish), RpcTarget.Others, _finishNumber);
        }
    }
    private bool start;

    public bool _check;
    bool check
    {
        get { return _check; }
        set
        {
            _check = value;
            diceState = DiceState.Wait;
        }
    }
    bool youCheck = false;


    public void ChengeState()
    {
        switch (diceState)
        {
            case DiceState.Standby:
                button.gameObject.SetActive(true);
                check = false;
                youCheck = false;
                break;
            case DiceState.Roll:
                diceRect.DOAnchorPosY(maxUp, upSpeed).OnComplete(() => diceRect.DOAnchorPosY(0, upSpeed).OnComplete(() => check = true));

                break;
            case DiceState.Wait:
                finishNumber = int.Parse(diceImage.sprite.name);
                if (youCheck == check)
                {
                    photonView.RPC(nameof(YourCheck), RpcTarget.Others, true);
                    diceState = DiceState.Finish;
                }
                else
                {
                    photonView.RPC(nameof(YourCheck), RpcTarget.Others, true);
                }
                break;
            case DiceState.Finish:

                Judge();
                break;
        }
    }

    void Start()
    {
    /*    if(PhotonNetwork.IsMasterClient)
        {
            buttons[1].gameObject.SetActive(false);
        }
        else
        {
            buttons[0].gameObject.SetActive(false);
        }*/

        if(transform.parent == null)
        {
            button.gameObject.SetActive(false);
            transform.parent = GameObject.Find("DiceManeger").transform;
        }


        afterThenButton = GameObject.Find("firstThen");

        StartCoroutine(GetStart());
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

            if (dice.GetCheck() == check)
            {
                diceState = DiceState.Finish;
            }
        }

    }

    IEnumerator GetStart()
    {
        yield return new WaitForSeconds(2);

        dice = GameObject.Find("Dice(Clone)").GetComponent<Dice>();
    }

    public bool GetCheck()
    {
        return youCheck;
    }

    public int GetYourNumber()
    {
        return yourNumber;
    }

    public void OnDiceRoll(Button b)
    {
        b.gameObject.SetActive(false);
        diceState = DiceState.Roll;
    }

    [PunRPC]
    public void YourChengeSprit(int num)
    {
        yourDiceImage.sprite = diceSprites[num];
        yourNumber = num;
    }

    [PunRPC]
    public void YourCheck(bool c)
    {
        youCheck = c;
    }

    [PunRPC]
    public void YourFinish(int n)
    {
        Debug.Log(n);
        yourNumber = n;
    }

    private void Judge()
    {

        Debug.Log($"{yourNumber} vs {finishNumber}");

        if (dice.GetYourNumber() < finishNumber)
        {
            afterThenButton.transform.DOScaleX(1, 0.5f);
        }
        else if(dice.GetYourNumber() == finishNumber)
        {
            diceState = DiceState.Standby;
        }
    }
}
