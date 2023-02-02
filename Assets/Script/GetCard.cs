using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class GetCard : MonoBehaviour
{
    [SerializeField] GameDirector.GameState nextState;

    [SerializeField] RoleCheck roleCheck;
    [SerializeField] CardGeneration cardGene;

    public GameObject[] numberUi = new GameObject[6];
    private int _onClickCount = 0;//クリック回数
    public int onClickCount
    {
        get
        {
            return _onClickCount;
        }
        set
        {
            _onClickCount = value;
            if (_onClickCount == 6)
            {
                Debug.Log(_onClickCount);
                //それぞれ役の計算
                roleCheck.Calculate();
                GameDirector.loadState = nextState;
            }
        }
    }

    public void SelectCard()
    {
        //numberUi[_onClickCount].transform.position = cardGene.clickObject[_onClickCount].transform.position;
    }

}
