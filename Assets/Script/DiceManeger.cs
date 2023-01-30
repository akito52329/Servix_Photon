using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using DG.Tweening;

public class DiceManeger : MonoBehaviourPunCallbacks
{
    [SerializeField] List<Vector2> pos;
    // Start is called before the first frame update
    void Start()
    {
        GameObject d = PhotonNetwork.Instantiate("Dice", Vector3.zero, Quaternion.identity);
        RectTransform r = d.GetComponent<RectTransform>();
        r.transform.parent = GameObject.Find("DiceManeger").transform;
        if(PhotonNetwork.IsMasterClient)
        {
            r.anchoredPosition = pos[0];
            d.gameObject.name = "Master";
        }
        else
        {
            r.anchoredPosition = pos[1];
            d.gameObject.name = "Guest";
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
