using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using Photon.Pun;

public class GeneratReply : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject p;
    [SerializeField] CardGeneration cardGeneration;
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

    public void GeneratCards(string name, Transform transform, bool check, int num)
    {
         GameObject card = PhotonNetwork.Instantiate(name, transform.position, Quaternion.identity);
         card.transform.parent = gameObject.transform;
         card.GetComponent<ObjectData>().SetPostionNumber(num);
         fast = check;
         counter++;
       /* GameObject card = PhotonNetwork.Instantiate(name, Vector3.zero, Quaternion.identity);
        card.transform.parent = transform;
        card.transform.localPosition = Vector3.zero;
        card.GetComponent<ObjectData>().SetPostionNumber(num);
        fast = check;
        counter++;*/
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


    public void Numbers()
    {
        /*List<ObjectData> od = new List<ObjectData>();
        foreach (var k in GetComponentsInChildren<GameObject>())
        {
            od.Add(k.GetComponentInChildren<ObjectData>());
        }
        Debug.Log(od.Count);
        int[] nums = new int[od.Count];
        for(int i = 0; i < od.Count; i++)
        {

            nums[i] = od[i].GetPostionNumber();
        }

        photonView.RPC(nameof(SetNumber), RpcTarget.OthersBuffered, nums);*/
        ObjectData[] od = GetComponentsInChildren<ObjectData>();
        int[] nums = new int[od.Length];
        for (int i = 0; i < od.Length; i++)
        {

            nums[i] = od[i].GetPostionNumber();
        }

        photonView.RPC(nameof(SetNumber), RpcTarget.OthersBuffered, nums);
    }

    [PunRPC]
    public void SetNumber(int[] n)
    {
        //cardGeneration.geneNum = n;
        StartCoroutine(L(n));
        /*ObjectData[] od = GetComponentsInChildren<ObjectData>();
        Debug.Log(od.Length);
        for (int j = 0; j < od.Length; j++)
        {
            Debug.Log($"{od[j].name} , {n[j]}");
            od[j].SetPostionNumber(n[j]);
        }*/
    }

    IEnumerator L(int[] n)
    {
        while (true)
        {

            yield return new WaitUntil(() => GetComponentsInChildren<ObjectData>().Length == 10);

            break;
        }

        ObjectData[] od = GetComponentsInChildren<ObjectData>();


        for (int j = 0; j < od.Length; j++)
        {
            od[j].SetPostionNumber(n[j]);
        }
    }
}
