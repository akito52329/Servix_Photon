using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class FinalScoreText : MonoBehaviour
{
    [SerializeField] ScoreText scoreText;
    [SerializeField] AudioController audio;
    [SerializeField] Text myTotalScoreText;//自分のスコア
    [SerializeField] Text yourTotalScoreText;//相手ののスコア
    [SerializeField] Text resultText;//勝敗
    [SerializeField] Text detailText;//スコアの詳細

    [SerializeField] Color winColor;//勝敗
    [SerializeField] Color loseColor;//敗北
    [SerializeField] Color drawColor;//引き分け
    [SerializeField] float displaySpeed = 1f;//表示スピード
    private int _finalScore = 0;//最終スコア
    public int finalScore
    {
        get { return _finalScore; }
        set
        {
            if( _finalScore != value )
            {
                _finalScore += value;
                myTotalScoreText.text = _finalScore.ToString();
            }
        }
    }

    private int _finalYourScore = 0;//相手の最終スコア
    public int finalYourScore
    {
        get { return _finalYourScore; }
        set
        {
            if (_finalYourScore != value)
            {
                _finalYourScore = value;
                yourTotalScoreText.text = _finalYourScore.ToString();
                ScaleUp();
            }
        }
    }


    private int time = 0;//残り時間
    private int timeScore = 0;//タイムスコア
    [SerializeField] int proportion;//割合
    [SerializeField] int firstBonus;//ボーナス

    private string dominance = "勝利";
    private string inferiority = "敗北";
    private string evenly = "引き分け";

    [SerializeField] RectTransform rectTransform;

    private void Start()
    {
        
    }



    public void FinalScore(int score, float time, bool first)//最終スコア
    {
        Debug.Log(888);
        this.time = (int)time;
        timeScore = this.time / proportion;
        if (!first)
        {
            firstBonus = 0;
        }
        detailText.text =　"ゲームスコア：" + score + 
           "\nタイムボーナス：" + this.time + "/" + proportion + " = " + timeScore;
        finalScore = score + timeScore;
    }

    public void GameScore()//最終スコア表示
    {
        Debug.Log($"{finalScore} vs {finalYourScore}");

        if (finalScore > finalYourScore)//勝ったとき
        {
            resultText.color = winColor;
            resultText.text = dominance;
            audio.WinAudio();
        }
        else if(finalScore < finalYourScore)//負けたとき
        {
            resultText.color = loseColor;
            resultText.text = inferiority;
            audio.LoseAudio();
        }
        else if (finalScore == finalYourScore)//スコアが同じ
        {
            resultText.color = drawColor;
            resultText.text = evenly;
        }

    }

    public void ScaleUp()
    {
        GameScore();
        rectTransform.DOScale(Vector3.one, displaySpeed);
    }
}
