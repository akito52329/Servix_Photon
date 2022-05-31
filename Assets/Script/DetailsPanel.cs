using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DetailsPanel: MonoBehaviour
{
    [SerializeField] AudioController audio;
    [SerializeField] GameObject rule;//ルール表示
    [SerializeField] GameObject role;//役表示
    [SerializeField] GameObject[] roleObject;//役表示
    [SerializeField] float displaySpeed = 1f;//表示スピード

    private int _roleNumber;//役ナンバー取得用
    public int roleNumber
    {
        set
        {
            if(_roleNumber != value)
            {
                roleObject[_roleNumber].SetActive(false);//変更前非表示
                _roleNumber = value;
                roleObject[_roleNumber].SetActive(true);//変更後表示
            }
        }
    }

    public void OnMenu(RectTransform rectTransform)//表示
    {
        rectTransform.DOScale(Vector3.one, displaySpeed);
    }

    public void OffMenu(RectTransform rectTransform)//非表示
    {
        rectTransform.DOScale(Vector3.zero, displaySpeed);
    }

    public void RoleRuleChenge(bool chenge)//役とルール表示の切り替え
    {
        role.SetActive(!chenge);
        rule.SetActive(chenge);
    }

    public void OnRolePanel(int num)//役ボタン
    {
        roleNumber = num;
    }
}
