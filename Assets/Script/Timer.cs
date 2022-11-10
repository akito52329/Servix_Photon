using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] GameDirector.GameState waitState;
    private bool countTime = false;//§ŒÀŽžŠÔ
    [SerializeField] Text timerText;
    [SerializeField] private float maxTime = 180;//ŽžŠÔ‚ÌÅ‘åŽžŠÔ
    [SerializeField] private float minTime = 0;

    // Update is called once per frame
    void Update()
    {
        if (countTime)
        {
            if (maxTime > minTime)
            {
                maxTime -= Time.deltaTime;
                timerText.text = maxTime.ToString("f0");
            }
            else
            {
                GameDirector.loadState = waitState;
            }
        }
    }

    public void ChengeCountTime(bool checke)
    {
        countTime = checke;
    }

    public float SetTime()
    {
        return maxTime;
    }
}
