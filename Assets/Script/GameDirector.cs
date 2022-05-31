using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using DG.Tweening;

public class GameDirector : MonoBehaviourPunCallbacks
{
    public enum GameState {Standby,Leave, Round, InGame, Score, Set, Wait, Finish}

    [SerializeField] CardGeneration cardGeneration;
    [SerializeField] TextMove textMove;
    [SerializeField] ScorePanel scorePanel;
    [SerializeField] FinalScoreText finalScoreText;
    [SerializeField] RoleCheck roleCheck;
    [SerializeField] GetCard getCard;
    [SerializeField] ScoreText scoreTextCo;

    [SerializeField] GameObject leavePanel;
    [SerializeField] GameObject waitPanel;
    [SerializeField] Button[] buttons;//ボタン取得

    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] private float maxTime = 300;//時間の最大時間
    [SerializeField] private float minTime = 0;
    private int maxRound = 10;

    private bool countTime = false;//制限時間
    private bool final = false;//終わったか
    private bool yourFinal = false;//相手が終わったか
    private bool first = false;//先に終了したか
    public static bool online = true;

    private GameState _loadState = GameState.Standby;
    static public GameState loadState//ゲームステート
    {
        set
        {
            if (gameDire._loadState != value)
            {
                gameDire._loadState = value;
                gameDire.OnGameState();
            }
        }
    }

    // シングルトン用の参照
    static GameDirector gameDire = null;

    private void Awake()
    {
        if (gameDire == null)
        {
            gameDire = this;
        }
        else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        if (online)//ネットワークにつながっているかどうか
        {
            PhotonNetwork.IsMessageQueueRunning = true;
        }

        loadState = GameState.Round;
        maxRound = textMove.GetRound();
        cardGeneration.Shuffle();
    }

    void Update()
    {
        if(final == yourFinal && final)//2人とも終了したか
        {
            loadState = GameState.Finish;
        }

        if (countTime)
        {
            if (maxTime > minTime)
            {
                maxTime -= Time.deltaTime;
                timerText.text = maxTime.ToString("f0");
            }
            else
            {
               loadState = GameState.Wait;
            }
        }
    }

    void OnGameState()//ゲームのステート
    {
        switch (gameDire._loadState)
        {
            case GameState.Standby:
                break;
            case GameState.Leave:
                leavePanel.SetActive(true);
                countTime = false;
                PhotonNetwork.LeaveRoom();
                break;
            case GameState.Round:
                if (textMove.round <= maxRound)
                {
                    textMove.gameObject.SetActive(true);
                    scorePanel.gameObject.SetActive(false);
                    finalScoreText.gameObject.SetActive(false);
                    RoleReset();
                    textMove.TextMoving();
                    getCard.onClickCount = 0;
                }
                else
                {
                    loadState = GameState.Wait;
                }
                break;
            case GameState.InGame:
                countTime = true;
                textMove.gameObject.SetActive(false);
                Operation();
                CardGeneration();
                break;
            case GameState.Score:
                countTime = false;
                scorePanel.gameObject.SetActive(true);
                scoreTextCo.RoleScore();
                scorePanel.ScaleUp();
                if((float)textMove.round / 2 == textMove.round / 2)//偶数
                {
                    photonView.RPC("YourScore", RpcTarget.Others, scoreTextCo.totalScore);
                }
                textMove.round++;
                break;
            case GameState.Wait:
                final = true;
                if(final != yourFinal)//先に終了
                {
                    waitPanel.SetActive(true);
                    first = true;                    
                    photonView.RPC("FinalCheck", RpcTarget.Others, final);
                    photonView.RPC("YourScore", RpcTarget.Others, scoreTextCo.totalScore);
                    if (!PhotonNetwork.InRoom)
                    {
                        yourFinal = true;
                    }
                }
                else//最後に終了
                {
                    first = false;
                    photonView.RPC("FinalCheck", RpcTarget.Others, final);
                }
                break;
            case GameState.Finish:
                waitPanel.SetActive(false);
                photonView.RPC("YourTotalScore", RpcTarget.Others, finalScoreText.FinalScore(scoreTextCo.totalScore, maxTime, first));
                finalScoreText.gameObject.SetActive(true);
                break;
        }
    }
    
    public override void OnPlayerLeftRoom(Player otherPlayer)//相手が切断したら
    {
        if (_loadState != GameState.Finish)
        {
            loadState = GameState.Leave;
        }
    }

    public void LeaveRoom()//退出ボタン
    {
        if(online)
        {
            PhotonNetwork.LeaveRoom();
        }
        SceneManager.LoadScene("TitleScene");
    }

    void Operation()//表示非表示
    {
        if (textMove.round <= maxRound)
        {
            ButtonTrue();
        }
        else
        {           
            ButtonFalse();
        }
    }

    void CardGeneration()//カード生成
    {
        cardGeneration.Generation(textMove.round == 1);
        cardGeneration.CardChenge();
    }

    void ButtonTrue()//ボタンを押せるようにする
    {
        for(int b = 0; b <= buttons.Length - 1; b++)
        {
            buttons[b].interactable = true;
        }
    }

    void ButtonFalse()//ボタンを押せなくする
    {
        for (int b = 0; b <= buttons.Length - 1; b++)
        {
            buttons[b].interactable = false;
        }
    }

    void RoleReset()//役リセット
    {
        roleCheck.stepCount = 0;
        roleCheck.stepLuckyCount = 0;
        roleCheck.dayCount = 0;
        roleCheck.shiritoriCount = 0;
        roleCheck.threeKind = 0;
        roleCheck.colorRole = 0;
        roleCheck.twoLuckyKind = 0;
        roleCheck.twoKind = 0;
        roleCheck.toiCount = 0;
        roleCheck.colorCount = 0;
    }

    [PunRPC]
    public void YourTotalScore(int s)//相手側に自分の最終スコアを渡す
    {
        finalScoreText.finalYourScore = s;
    }

    [PunRPC]
    public void YourScore(int s)//相手側に自分のスコアを渡す
    {
        scoreTextCo.yourScore = s;
        scoreTextCo.yourTotalScoreText.text = "あいてのスコア:" + s.ToString();
    }

    [PunRPC]
    public void FinalCheck(bool f)//自分が先に終わったことを伝える
    {
        yourFinal = f;
    }
}
