﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SCount : MonoBehaviour
{
    public Lazer LazerSc;
    public Text CountText = null;
    public SPlayerMove HeroSc;
    int nCount = 10;
    public GameObject PauseBtnGams;
    bool bLazerCorutineUse = false;

    // Update is called once per frame
    void Update()
    {
        if (SGameMng.I.TimeCtrl((int)E_TIME.E_COUNT, 1f) && nCount > 0)
        {
            nCount--;
            CountText.text = nCount.ToString();
        }

        if (nCount <= 0)
        {
            SGameMng.I.bStartCheck = true;
            HeroSc.bCountSetCom = true;
            CountText.text = "START!";
            StartCoroutine(TextEnable());
            if (!bLazerCorutineUse)
            {
                StartCoroutine(LazerSc.LazerUse());
                bLazerCorutineUse = true;
            }
        }
    }

    IEnumerator TextEnable()
    {
        yield return new WaitForSeconds(1f);
        CountText.enabled = false;
        PauseBtnGams.SetActive(true);
    }
}