using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class NetworkConnection : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject startPanel;
    [SerializeField] GameObject waitPanel;
    [SerializeField] GameObject multiPanel;
    [SerializeField] GameObject button;
    [SerializeField] RectTransform content;


    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();

    }
    private void Update()
    {
        if (PhotonNetwork.InRoom)//ルームで待機しているとき
        {
            GameStart();
        }
    }
    public void PlayNumber(bool solo)//マルチかソロにするか
    {
        if (solo)
        {
            GameDirector.online = false;
            SceneManager.LoadScene("GameScene");
        }
        else
        {
            PhotonNetwork.JoinLobby();
            multiPanel.SetActive(true);
            startPanel.SetActive(false);
        }
    }

    public void RamdomRoom()//ランダム入室
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public void CreateRoom(TMP_InputField inputField)//ルームを作成する
    {
        string roomName = "";
        roomName = inputField.text;//入力されたルーム名を代入

        var roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;//最大人数を設定

        PhotonNetwork.CreateRoom(roomName, roomOptions);//ルーム名と最大人数を設定してルーム作成
    }

    public void GameStart()//ゲームスタート
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            PhotonNetwork.IsMessageQueueRunning = false;
            PhotonNetwork.CurrentRoom.IsOpen = false;//ルーム入室を禁止
            SceneManager.LoadScene("GameScene");
        }
    }

    public void BackTitle(bool title)
    {
        if (title)
        {
            multiPanel.SetActive(false);
            startPanel.SetActive(true);
            PhotonNetwork.LeaveLobby();
        }
    }

    public void RoomUpdates()//戻す
    {
        SceneManager.LoadScene("TitleScene");
    }

    public override void OnJoinedRoom()
    {
        multiPanel.SetActive(false);
        waitPanel.SetActive(true);
        GameStart();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo r in roomList)
        {
            //プレイヤーが存在しているルーム
            if (r.PlayerCount > 0)
            {
                RoomButtonCreate(r);
            }
            else
            {
                RoomButtonDelete(r);
            }
        }
    }


    public void RoomButtonCreate(RoomInfo r)
    {
        //すでに存在していたのなら情報の更新
        if (gameObject.transform.Find(r.Name))
        {
            RoomInfoUpdate(gameObject.transform.Find(r.Name).gameObject, r);
        }
        //新しく作られたルームならばボタンの作成
        else
        {
            var roomButton = Instantiate(button);
            roomButton.transform.SetParent(content, false);
            RoomInfoUpdate(roomButton, r);          
            //生成したボタンの名前を作成するルームの名前にする
            roomButton.name = r.Name;
            roomButton.GetComponent<Button>().onClick.AddListener(() => OnClickButton(r.Name));
        }
    }

    public void OnClickButton(string name)//生成したボタンに与えられる
    {
        PhotonNetwork.JoinRoom(name);
    }

    public void RoomInfoUpdate(GameObject button, RoomInfo info)
    {
        foreach (Text t in button.GetComponentsInChildren<Text>())
        {
            if (t.name == "RoomName")
            {
                t.text = info.Name;
            }
            else if (t.name == "Number")
            {
                t.text = info.PlayerCount.ToString() + "/" + info.MaxPlayers.ToString();
            }
        }
    }

    public void RoomButtonDelete(RoomInfo r)
    {
        //ボタンが存在すれば削除
        if (gameObject.transform.Find(r.Name))
        {
            Destroy(gameObject.transform.Find(r.Name).gameObject);
        }
    }
}



