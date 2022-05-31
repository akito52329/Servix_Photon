using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class TextMove : MonoBehaviour
{
    [SerializeField] GameDirector.GameState nextState;
    [SerializeField] AudioController audio;
    [SerializeField] private float centerPos = 0;
    [SerializeField] TextMeshProUGUI roundText;
    [SerializeField] TextMeshProUGUI roundDisplayText;

    [SerializeField] private int finalRound = 10;
    private int _round = 1;
    public int round
    {
        get
        {
            return _round;
        }
        set
        {
            if (_round != value)
            {
                _round = value;
                
                if(round <= finalRound)
                {
                    RoundText();
                }
            }
        }
    }
    public RectTransform rectTransform;

    void Start()
    {
        RoundText();
    }

    public void TextMoving()//テキスト移動処理
    {
        audio.RoundAudio();
        rectTransform.DOAnchorPosX(centerPos, 2f).SetLoops(2, LoopType.Incremental)
            .OnComplete(() => GameDirector.loadState = nextState).Play();
    }

    public void RoundText()//ラウンド表示
    {
        if(round < finalRound)
        {
            roundText.text = "ラウンド " + _round.ToString();
            roundDisplayText.text = "ラウンド " + _round.ToString();
        }
        else
        {
            roundText.text = "ファイナル ラウンド";
            roundDisplayText.text = "ファイナル ラウンド";
        }
    }

    public int GetRound()
    {
        return finalRound;
    }
}
