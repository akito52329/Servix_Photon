using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardData : MonoBehaviour
{
    [SerializeField]
    Color[] colorCode = { Color.black, Color.white, Color.red, Color.blue, Color.green, Color.cyan, Color.magenta, Color.yellow, Color.clear };//カラーコード

    [SerializeField]
    int[] numericCode = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };//数字コード

    public Color CadeDetaColor(string name)
    {
        switch (name)
        {
            case "からす":
            case "ごま":
            case "こうひい":
                return colorCode[0];

            case "にわとり":
            case "やぎ":
            case "ぎゅうにゅう":
                return colorCode[1];

            case "りんご":
            case "いち":
            case "とまと":
            case "ばら":
            case "すいか":
            case "ひ":
            case "うめぼし":
                return colorCode[2];

            case "に":
            case "なな":
            case "いるか":
            case "あじさい":
                return colorCode[3];

            case "よん":
            case "ご":
            case "ごーや":
                return colorCode[4];

            case "れい":
            case "ろく":
            case "らむね":
                return colorCode[5];

            case "はち":
            case "きゅう":
            case "なす":
                return colorCode[6];

            case "さん":
            case "べにばな":
            case "ひまわり":
                return colorCode[7];

            default:
                return colorCode[8];
        }
    }

    public int CadeDetaNumber(string name)
    {
        switch (name)
        {
            case "れい":
                return numericCode[0];
            case "いち":
                return numericCode[1];
            case "に":
                return numericCode[2];
            case "さん":
                return numericCode[3];
            case "よん":
                return numericCode[4];
            case "ご":
                return numericCode[5];
            case "ろく":
                return numericCode[6];         
            case "なな":
                return numericCode[7];
            case "はち":
                return numericCode[8];
            case "きゅう":
                return numericCode[9];
            default:
                return numericCode[10];
        }
    }
}
