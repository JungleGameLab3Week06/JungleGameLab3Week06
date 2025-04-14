using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_ControlCanvas : MonoBehaviour
{
    [SerializeField] Image[] btnImage; // Content as Flame, Water, Ground, Lightning

    void Start()
    {
        // UIManager Should Catch this class and connect events for Modularization
    }

    // Update is called once per frame
    public void MakePressBtnFX(int index)
    {
        CanvasGroup _canvasGroup = btnImage[index].GetComponent<CanvasGroup>();
        if(_canvasGroup){
            StartCoroutine(ShowPressBtnFX(_canvasGroup));
        }
    }

    private IEnumerator ShowPressBtnFX(CanvasGroup cg)
    {
        cg.alpha = 0.6f;

        yield return new WaitForSeconds(0.25f);
        cg.alpha = 1;
    }
}
