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
    public int dayCount = 0;

    //同種役
    public int twoKind = 0;
    public int twoLuckyKind = 0;
    public int threeKind = 0;

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
            Debug.Log(threeKind);
            Debug.Log(_objectCount);
        }
    }

    //階段役
    public int stepLuckyCount;
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

            if(_stepCount >= 4)
            {
                stepLuckyCount += oneCount;
                _stepCount = 0;
            }
        }
    }

    //役倍率
    public int luckyRoleMax = 10;
    public int roleMax = 1;
    public int twoRoleMax = 2;
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
        getCard.onClickCount += 1;
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

}
