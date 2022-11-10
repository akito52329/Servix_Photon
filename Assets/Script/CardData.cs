using System;
using System.Collections.Generic;
using UnityEngine;

public class CardData : MonoBehaviour
{
    [SerializeField]
    Color[] colorCode = { Color.black, Color.white, Color.red, Color.blue, Color.green, Color.cyan, Color.magenta, Color.yellow, Color.clear };//�J���[�R�[�h

    [SerializeField]
    int[] numericCode = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };//�����R�[�h

    public Color CadeDetaColor(string name)
    {
        switch (name)
        {
            case "���炷":
            case "����":
            case "�����Ђ�":
                return colorCode[0];

            case "�ɂ�Ƃ�":
            case "�€":
            case "���イ�ɂイ":
                return colorCode[1];

            case "���":
            case "����":
            case "�Ƃ܂�":
            case "�΂�":
            case "������":
            case "��":
            case "���߂ڂ�":
                return colorCode[2];

            case "��":
            case "�Ȃ�":
            case "���邩":
            case "��������":
                return colorCode[3];

            case "���":
            case "��":
            case "���[��":
                return colorCode[4];

            case "�ꂢ":
            case "�낭":
            case "��ނ�":
                return colorCode[5];

            case "�͂�":
            case "���イ":
            case "�Ȃ�":
                return colorCode[6];

            case "����":
            case "�ׂɂ΂�":
            case "�Ђ܂��":
                return colorCode[7];

            default:
                return colorCode[8];
        }
    }

    public int CadeDetaNumber(string name)
    {
        switch (name)
        {
            case "�ꂢ":
                return numericCode[0];
            case "����":
                return numericCode[1];
            case "��":
                return numericCode[2];
            case "����":
                return numericCode[3];
            case "���":
                return numericCode[4];
            case "��":
                return numericCode[5];
            case "�낭":
                return numericCode[6];
            case "�Ȃ�":
                return numericCode[7];
            case "�͂�":
                return numericCode[8];
            case "���イ":
                return numericCode[9];
            default:
                return numericCode[10];
        }
    }
}
