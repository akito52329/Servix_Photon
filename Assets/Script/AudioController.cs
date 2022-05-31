using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioSource clickAudio;
    public AudioSource roundAudio;
    public AudioSource winAudio;
    public AudioSource loseAudio;
    public AudioSource decisionAudio;
    public AudioSource searchAudio;
    public AudioSource fadeAudio;

    public void DecisionAudio(bool on)//選択音
    {
        if(on)
        {
            decisionAudio.Play();
        }
        else
        {
            fadeAudio.Play();
        }

    }

    public void SearchAudio()
    {
        searchAudio.Play();
    }

    public void ClickAudio()//クリック音
    {
        clickAudio.Play();
    }

    public void RoundAudio()//ラウンド表示時音
    {
        roundAudio.Play();
    }

    public void WinAudio()//勝利時音
    {
        winAudio.Play();
    }

    public void LoseAudio()//敗北時音
    {
       loseAudio.Play();
    }

}
