using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SearchList : MonoBehaviour
{
    [SerializeField] Dropdown[] dropdowns;//検索項目
                                              //0:色
                                              //1:数字
                                              //2:語尾or頭文字
                                              //3:文字
    [SerializeField] ObjectData[] objectDatas;
    [SerializeField] Scrollbar scrollbar;
    private List<GameObject> resultObject = new List<GameObject>();
    [SerializeField] RectTransform content;
    private const int one = 1;
    public void OnSearch()//検索ボタン
    {

        if (content.gameObject.transform.childCount != 0)//前回の検索履歴あるか
        {
            foreach (Transform child in content.gameObject.transform)
            {
                Destroy(child.gameObject);
            }
        }

        for (int o = 0; o <= objectDatas.Length - one; o++)
        {
            if (ColorCheck(SearchColor(), objectDatas[o].myColor) /*色が同じものを検索*/
                && NumberCheck(objectDatas[o].myNumber) /*数字持ちかを検索*/
                && NameCheck(objectDatas[o].nameTop, objectDatas[o].nameEnd))//名前が同じものを検索
            {
                resultObject.Add(objectDatas[o].gameObject); 
            }
        }
        GeneratingCard(resultObject.Count);
    }

    private void GeneratingCard(int count)//検索結果表示
    {
        for (int r = 0; r < count; r++)
        {
            Instantiate(resultObject.First(), content);
            resultObject.RemoveAt(0);
        } 
        scrollbar.value = one;
    }


    /// <summary>
    /// 名前検索
    /// </summary>
    /// <param name="nameTop">カードの頭文字</param>
    /// <param name="nameEnd">カードの語尾</param>
    /// <returns></returns>
    private bool NameCheck(char nameTop, char nameEnd)
    {
        var searchName = dropdowns[3].options[dropdowns[3].value].text;
        switch (dropdowns[2].options[dropdowns[2].value].text)
        {
            case "頭文字":
                switch (searchName)
                {
                    case "選択なし":
                        break;
                    default:
                        return searchName[0] == nameTop;
                }
                break;
            case "語尾":
                switch (searchName)
                {
                    case "選択なし":
                        break;
                    default:
                        return searchName[0] == nameEnd;
                }
                break;
        }
        return true;
    }

    /// <summary>
    /// 色検索
    /// </summary>
    /// <param name="searchColor">指定された色</param>
    /// <param name="objectColor">カードの色</param>
    /// <returns></returns>
    private bool ColorCheck(Color searchColor, Color objectColor)
    {
        if (searchColor == Color.clear)
        {
            return true;
        }
        else
        {
            return searchColor == objectColor;
        }
    }

    /// <summary>
    /// 数字検索
    /// </summary>
    /// <param name="objectNum">カードの持っている数字</param>
    /// <returns></returns>
    private bool NumberCheck(int objectNum)
    {
        var noNum = 10;
        switch (dropdowns[1].options[dropdowns[1].value].text)
        {
            case "あり":
                return noNum != objectNum;
            case "なし":
                return noNum == objectNum;
        }
        return true;
    }

    /// <summary>
    /// 色のドロップダウン
    /// </summary>
    /// <param name="colorDropdown">色用ドロップダウン</param>
    /// <returns></returns>
    private Color SearchColor()
    {
        switch (dropdowns[0].options[dropdowns[0].value].text)
        {
            case "赤色":
                return Color.red;
            case "青色":
                return Color.blue;
            case "緑色":
                return Color.green;
            case "水色":
                return Color.cyan;
            case "黄色":
                return Color.yellow;
            case "紫色":
                return Color.magenta;
            case "白色":
                return Color.white;
            case "黒色":
                return Color.black;
            default:
                break;
        }
        return Color.clear;
    }
}

