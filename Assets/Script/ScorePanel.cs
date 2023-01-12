using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ScorePanel : MonoBehaviour
{
    [SerializeField] TextMove textMove;
    [SerializeField] GameDirector.GameState nextState;

    [SerializeField] RectTransform rectTransform;
    [SerializeField] private float time = 1;

    void Start()
    {
        rectTransform.localScale = Vector3.zero;
    }

    public void ScaleUp()
    {
        StartCoroutine("Scale");
    }

    IEnumerator Scale()//スコアパネルの表示コルーチン
    {
        rectTransform.DOScale(Vector3.one, 0.5f);
        yield return new WaitForSeconds(time);
        rectTransform.DOScale(Vector3.zero, 1f).OnComplete(() => GameDirector.loadState = nextState).Play();
    }
}
