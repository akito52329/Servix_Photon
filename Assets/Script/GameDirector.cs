using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using DG.Tweening;

public class GameDirector : MonoBehaviourPunCallbacks
{
    public enum GameState { Standby, Leave, Round, InGame, Score, Set, Wait, Finish }

    [SerializeField] CardGeneration cardGeneration;
    [SerializeField] TextMove textMove;
    [SerializeField] ScorePanel scorePanel;
    [SerializeField] FinalScoreText finalScoreText;
    [SerializeField] RoleCheck roleCheck;
    [SerializeField] GetCard getCard;
    [SerializeField] ScoreText scoreTextCo;
    [SerializeField] Timer timer;

    [SerializeField] GameObject leavePanel;
    [SerializeField] GameObject waitPanel;
    [SerializeField] GameObject bottonsParent;

    private int maxRound = 10;
    bool precedence = false;

    private bool final = false;//終わったか
    private bool yourFinal = false;//相手が終わったか
    private bool first = false;//先に終了したか
    public static bool online = true;

    private bool _chenge = false;
    public bool chenge
    {
        get { return _chenge; }
        set
        {
            _chenge = value;
           // ChengeInteractable(textMove.round == 1);
        }
    }

    private GameState _loadState = GameState.Standby;
    static public GameState loadState//ゲームステート
    {
        get { return gameDire._loadState; }
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

        precedence = PhotonNetwork.IsMasterClient;

        loadState = GameState.Round;
        maxRound = textMove.GetRound();
    }

    void Update()
    {
        if(final == yourFinal && final)//2人とも終了したか
        {
            loadState = GameState.Finish;
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
                timer.ChengeCountTime(false);
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

                photonView.RPC(nameof(GiveRound), RpcTarget.Others, textMove.round);
                CardGeneration();
                break;
            case GameState.InGame:
                timer.ChengeCountTime(true);
                textMove.gameObject.SetActive(false);

                //  Operation();
                precedence = !precedence;
                break;
            case GameState.Score:
                timer.ChengeCountTime(false);
                scorePanel.gameObject.SetActive(true);
                scoreTextCo.RoleScore();
                scorePanel.ScaleUp();

                /*      if((float)textMove.round / 2 == textMove.round / 2)//偶数
                      {
                          photonView.RPC("YourScore", RpcTarget.Others, scoreTextCo.totalScore);
                      }*/
                photonView.RPC(nameof(YourScore), RpcTarget.Others, scoreTextCo.totalScore);
                textMove.round++;


                break;
            case GameState.Wait:
                final = true;
                if(final != yourFinal)//先に終了
                {
                    waitPanel.SetActive(true);
                    first = true;                    
                    photonView.RPC(nameof(FinalCheck), RpcTarget.Others, final);
                    photonView.RPC(nameof(YourScore), RpcTarget.Others, scoreTextCo.totalScore);
                    if (!PhotonNetwork.InRoom)
                    {
                        yourFinal = true;
                    }
                }
                else//最後に終了
                {
                    first = false;
                    photonView.RPC(nameof(FinalCheck), RpcTarget.Others, final);
                }
                break;
            case GameState.Finish:
                waitPanel.SetActive(false);
                photonView.RPC(nameof(YourTotalScore), RpcTarget.Others, finalScoreText.FinalScore(scoreTextCo.totalScore, timer.SetTime(), first));
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


    void CardGeneration()//カード生成
    {
        if (textMove.round == 1)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                cardGeneration.Generation(true);
            }
        }
        else
        {
            if (PhotonNetwork.IsMasterClient)
            {

                cardGeneration.Generation(false);
            }
            else
            {
                Debug.Log("popopop");
                photonView.RPC(nameof(GiveDisplay), RpcTarget.MasterClient, cardGeneration.clickObject);
            }
        }
    }

    public void ChengeInteractable()//ボタンを押せるようにする
    {
        Debug.Log(10000000);
       /*bool checke = 
            PhotonNetwork.IsMasterClient ? 
            (float)textMove.round / 2 != textMove.round / 2 
            : (float)textMove.round / 2 == textMove.round / 2;*/
        foreach (Button b in bottonsParent.GetComponentsInChildren<Button>())
        {
            b.interactable = precedence;
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

    [PunRPC]
    public void GiveRound(int r)
    {
        textMove.round = r;
        loadState = GameState.Round;
    }


    [PunRPC]
    public void GiveState(GameState state)
    {
        loadState = state;
    }

    [PunRPC]
    public void GiveDisplay(GameObject[] list)//相手側に自分の最終スコアを渡す
    {
       cardGeneration.clickObject = list;
       cardGeneration.Generation(false);
    }
}
