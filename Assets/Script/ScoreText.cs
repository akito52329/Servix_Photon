using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ScoreText : MonoBehaviour
{
    [SerializeField] GameDirector.GameState finishState;

    [SerializeField] RoleCheck roleCheck;

    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI myTotalScoreText;
    public TextMeshProUGUI yourTotalScoreText;

    //各役表示
    [SerializeField] TextMeshProUGUI stepText;
    [SerializeField] TextMeshProUGUI luckyStepText;
    [SerializeField] TextMeshProUGUI luckyText;
    [SerializeField] TextMeshProUGUI shiritoriText;
    [SerializeField] TextMeshProUGUI kindText;
    [SerializeField] TextMeshProUGUI twokindText;
    [SerializeField] TextMeshProUGUI toikindText;
    [SerializeField] TextMeshProUGUI colorText;


    [SerializeField] private int scoreStandard = 100;//スコア１つ当たりの倍率
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
        stepText.text = roleCheck.stepCount.ToString();
        luckyStepText.text = roleCheck.stepLuckyCount.ToString();
        luckyText.text = (roleCheck.dayCount / roleCheck.luckyRoleMax).ToString();
        shiritoriText.text = roleCheck.shiritoriCount.ToString();
        kindText.text = roleCheck.threeKind.ToString();
        toikindText.text = roleCheck.twoLuckyKind.ToString();
        twokindText.text = roleCheck.twoKind.ToString();
        colorText.text = roleCheck.colorRole.ToString();

        //ラウンド１回分のスコア
        score = (roleCheck.twoKind + roleCheck.twoLuckyKind * roleCheck.luckyRoleMax 
            + roleCheck.colorRole + roleCheck.threeKind * roleCheck.twoRoleMax + roleCheck.shiritoriCount 
            + roleCheck.stepLuckyCount * roleCheck.luckyRoleMax + roleCheck.stepCount * roleCheck.twoRoleMax 
            + roleCheck.dayCount) * scoreStandard;

        scoreText.text = _score.ToString();
    }


    public void TotalScore()//スコア表示
    {
        myTotalScoreText.text = "あなたのスコア:" + totalScore.ToString();       
    }
}
