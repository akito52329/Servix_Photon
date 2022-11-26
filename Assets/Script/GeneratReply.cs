using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class GeneratReply : MonoBehaviourPunCallbacks
{
    [SerializeField] GameDirector director;
    [SerializeField] int fastCount = 10;
    [SerializeField] int afterCount = 6;
    bool fast = true;

    private int _counter;
    public int counter
    {
        get { return _counter; }
        set
        {
            _counter = value;
            if (fast)
            {
                OnInteractable(fastCount == _counter);
            }
            else
            {
                OnInteractable(afterCount == _counter);
            }
        }
    }
    [SerializeField] GameObject textParent;
    private int rpc = 1;

    private void Update()
    {
        if(GameDirector.loadState == GameDirector.GameState.Round)
        {
            director.ChengeInteractable();
        }
    }

    public void GeneratCards(string name, Transform transform, bool check)
    {
        GameObject card = PhotonNetwork.Instantiate(name, transform.position, Quaternion.identity);
        card.transform.parent = gameObject.transform;
        fast = check;
        counter++;
    } 

    private void OnInteractable(bool countCheck)
    {
        if(countCheck)
        {
            director.ChengeInteractable();
            _counter = 0;
            photonView.RPC(nameof(Interactable), RpcTarget.Others, rpc++);
        }
    }

    [PunRPC]
    public void Interactable(int c)
    {
        rpc = c;
        director.ChengeInteractable();
    }
}
