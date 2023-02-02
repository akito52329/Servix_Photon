using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RoleCheck : MonoBehaviour
{
    [SerializeField] GetCard getCard;

    public string[] nameObject = new string[6];
    public Color[] colorObject = new Color[6];
    public char[] nameTopObject = new char[6];
    public char[] nameEndObject = new char[6];
    public int[] myNumberObject = new int[6];
    [SerializeField] private int scoreStandard = 100;//スコア１つ当たりの倍率
    private int oneCount = 1;//増加量

    //色役
    private bool threeColorkind = false;
    public int colorRole = 0;   
    private int _colorCount = 0;
    public int colorCount
    {
        get
        {
            return _colorCount;
        }
        set
        {
            if (_colorCount != value)
            {
                _colorCount = value;

                if (threeColorkind)
                {
                    if (_colorCount <= 2)
                    {
                        colorRole += roleMax;
                        _colorCount -= roleMax;
                    }

                }
                else
                {
                    _colorCount = 0;
                }

            }
            threeColorkind = false;
        }
    }

    //しりとり役
    private char shiritoriOut;
    private string endHiragana = "ん";
    private int _shiritoriCount = 0;
    public int shiritoriCount
    {
        get
        {
            return _shiritoriCount;
        }
        set
        {
            _shiritoriCount = value;
        }
    }

    //ラッキー役(日付)
    private DateTime nowDay;
    private int dayCount = 0;

    //同種役
    private int twoKind = 0;
    private int twoLuckyKind = 0;
    private int threeKind = 0;

    [SerializeField] private int toiCountMax = 3;
    private int _toiCount;
    public int toiCount
    {
        get
        {
            return _toiCount;
        }
        set
        {
            if (_toiCount != value)
            {
                _toiCount = value;

                if (_toiCount == toiCountMax)
                {
                    twoLuckyKind = oneCount;
                    _toiCount = 0;
                }
                else
                {
                    twoKind += oneCount;
                }
            }
        }
    }

    private int _objectCount;
    public int objectCount
    {
        get
        {
            return _objectCount;
        }
        set
        {
            _objectCount = value;

            if(_objectCount == 2)
            {
                threeKind += roleMax;
                _objectCount = 0;
            }
        }
    }

    //階段役
    private int stepLuckyCounter;
    private int stepLuckyCount;
    private int _stepCount;
    public int stepCount
    {
        get
        {
            return _stepCount;
        }
        set
        {
            _stepCount = value;
        }
    }

    //役倍率
    [SerializeField] int luckyRoleMax = 10;
    [SerializeField] int roleMax = 1;
    [SerializeField] int twoRoleMax = 2;
    void Start()
    {
        shiritoriOut = endHiragana[0];
    }

    //選択したカードの各データ取得
    public void Role(Color color, char top, char end, int num, string name)
    {
        colorObject[getCard.onClickCount] = color;
        nameTopObject[getCard.onClickCount] = top;
        nameEndObject[getCard.onClickCount] = end;
        myNumberObject[getCard.onClickCount] = num;
        nameObject[getCard.onClickCount] = name;
        getCard.onClickCount ++;
    }

    public void RoleColor()//色役の計算
    {
        for (int n = 0; n < colorObject.Length - 1; n++)
        {
            if (colorObject[n] == colorObject[n + 1])
            {
                colorCount += roleMax;
                threeColorkind = true;
            }
            else
            {
                colorCount = 0;
                threeColorkind = false;
            }
        }
    }

    public void RoleShiritori()//しりとり役の計算
    {
        for (int n = 0; n < nameTopObject.Length - 1; n++)
        {
            if (nameEndObject[n] != shiritoriOut)
            {
                if (nameEndObject[n] == nameTopObject[n + 1])
                {
                    shiritoriCount += roleMax;
                }
            }
            else
            {
                shiritoriCount -= roleMax;
            }
        }
    }

    public void RoleDay()
    {
        nowDay = DateTime.Now;
        for (int n = 0; n < myNumberObject.Length - 3; n++)
        { 
            switch(myNumberObject[n])
            {
                case 0://月の十の位
                    if (myNumberObject[n + 1] != 10 && myNumberObject[n + 1] != 0)//月の一の位
                    {
                        if (MonthCheck(n))//月が今日と等しいか
                        {
                            switch (myNumberObject[n + 2])//日の十の位
                            {
                                case 0:
                                case 1:
                                case 2:
                                case 3:
                                    if(DayCheck(n + 2))
                                    {
                                        dayCount += luckyRoleMax;
                                    }
                                    break;
                                default:
                                    break;

                            }
                        }
                    }
                    break;
                case 1://月の十の位
                    switch (myNumberObject[n + 1])//月の一の位
                    {
                        case 0:
                        case 1:
                        case 2:
                            if (MonthCheck(n))//月が今日と等しいか
                            {
                                switch (myNumberObject[n + 2])//日の十の位
                                {
                                    case 0:
                                    case 1:
                                    case 2:
                                    case 3:
                                        if (DayCheck(n + 2))
                                        {
                                            dayCount += luckyRoleMax;
                                        }
                                        break;
                                    default:
                                        break;
                                }
                            }
                            break;
                        default:
                            break;
                    }
                    break;

                case 2:
                    if(YearCheck(n))
                    {
                        dayCount += luckyRoleMax;
                    }
                    break;
                default:
                    break;
            }
        }
    }//ラッキー（日付）役の計算

    public void RoleKind()//同種役の計算
    {
        for (int n = 0; n < nameObject.Length - 1; n++)
        {
            if (nameObject[n] == nameObject[n + 1])
            {
                objectCount += 1;
            }
            else
            {
                if(objectCount == oneCount)
                {
                    objectCount = 0;
                }
            }
        }
    }

    public void ToiKind()//対種役の計算
    {
        for (int n = 0; n < nameObject.Length - 1; n++)
        {
            if (nameObject[n] == nameObject[n + 1])
            {
                toiCount += oneCount;
                n++;
            }
        }
    }

    public void RoleStep()//階段役の計算
    {
        for (int n = 0; n < myNumberObject.Length - 2; n++)
        {
            if (myNumberObject[n] != 10 && myNumberObject[n + 1] != 10 && myNumberObject[n + 2] != 10)
            {
                if (myNumberObject[n] == myNumberObject[n + 1] - 1 && myNumberObject[n] == myNumberObject[n + 2] - 2)
                {
                    stepCount += oneCount;

                    if (stepLuckyCounter == 1)
                    {
                        stepLuckyCount += oneCount;
                        stepLuckyCounter = 0;
                    }
                    else
                    {
                        stepLuckyCounter++;
                    }
                }
            }

        }
    }

    /// <summary>
    /// 年がひとしいか
    /// </summary>
    /// <param name="n">選択されたオブジェクトのナンバー</param>
    /// <returns></returns>
    private bool YearCheck(int n)
    {
        return myNumberObject[n] * 1000 + myNumberObject[n + 1] * 100 + myNumberObject[n + 2] * 10 + myNumberObject[n + 3] == nowDay.Year;
    }

    /// <summary>
    /// 月が等しいか
    /// </summary>
    /// <param name="n">選択されたオブジェクトのナンバー</param>
    /// <returns></returns>
    private bool MonthCheck(int n)
    {
        return myNumberObject[n] * 10 + myNumberObject[n + 1] == nowDay.Month;
    }

    /// <summary>
    /// 日が等しいか
    /// </summary>
    /// <param name="n">選択されたオブジェクト２つ先のナンバー</param>
    /// <returns></returns>
    private bool DayCheck(int n)
    {
        return myNumberObject[n] * 10 + myNumberObject[n + 1] == nowDay.Day;
    }

    /// <summary>
    /// 各役表示
    /// 0:色役
    /// 1:しりとり役
    /// 2:同種役
    /// 3:階段役
    /// 4:ラッキー役
    /// 5:4STEP役
    /// 6:二種役:
    /// 7:対種役
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    public string SetResult(int num)
    {
        switch (num)
        {
            case 0:
                return colorRole.ToString();
            case 1:
                return shiritoriCount.ToString();
                    case 2:
                return threeKind.ToString();
            case 3:
                return stepCount.ToString();
            case 4:
                return (dayCount / luckyRoleMax).ToString();
            case 5:
                return stepLuckyCount.ToString();
            case 6:
                return twoKind.ToString();
            case 7:
                return twoLuckyKind.ToString();
            default:
                return "";
        }
    }

    public int SetScore()
    {
        return ((twoKind + twoLuckyKind * luckyRoleMax
            + colorRole + threeKind * twoRoleMax + shiritoriCount
            + stepLuckyCount * luckyRoleMax + stepCount * twoRoleMax
            + dayCount) * scoreStandard);
    }

    public void Calculate()
    {
        RoleColor();
        RoleShiritori();
        RoleDay();
        RoleKind();
        RoleStep();
        ToiKind();
    }


    public void RoleReset()//役リセット
    {
        stepCount = 0;
        stepLuckyCount = 0;
        dayCount = 0;
        shiritoriCount = 0;
        threeKind = 0;
        colorRole = 0;
        twoLuckyKind = 0;
        twoKind = 0;
        toiCount = 0;
        colorCount = 0;
    }
}
