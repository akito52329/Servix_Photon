using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ScoreText : MonoBehaviour
{
    [SerializeField] GameDirector.GameState finishState;

    [SerializeField] RoleCheck roleCheck;

    [SerializeField] Text scoreText;
    [SerializeField] Text myTotalScoreText;
    public Text yourTotalScoreText;


    /// <summary>
    /// 各役表示
    /// 0:色役
    /// 1:しりとり役
    /// 2:同種役
    /// 3:階段役
    /// 4:ラッキー役
    /// 5:4STEP役
    /// 6:二種役:
    /// 7:対種役
    /// </summary>
    [SerializeField] List<Text> roleTexts;


    public int totalScore;
    public int yourScore = 0;
    private int _score = 0;//スコア取得
    public int score
    {
        get
        {
            return _score;
        }
        set
        {
            _score = value;
            totalScore += _score;
            TotalScore();
        }

    }

    void Start()
    {
        TotalScore();
    }

    public void RoleScore()
    {
        //各役のカウント
        for(int r = 0; r< roleTexts.Count; r++)
        {
            roleTexts[r].text = roleCheck.SetResult(r);
        }


        //ラウンド１回分のスコア
        score = roleCheck.SetScore();

        scoreText.text = _score.ToString();
    }





    public void TotalScore()//スコア表示
    {
        myTotalScoreText.text = "あなたのスコア:" + totalScore.ToString();       
    }
}
