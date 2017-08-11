﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SPlayerMove : MonoBehaviour
{
    public JoyStick JoystickScrp = null;
    public Lazer LazerSc;

    public Transform PlayerParentTr;

    public GameObject SkillsGams;
    public GameObject[] HeroFireGams;
    public GameObject ShieldGams;
    public GameObject DestroySkill;
    public GameObject DieGams;
    public GameObject[] LazerGams;
    public GameObject[] Explaying;

    public GameObject BackGround;

    public float fSpeed;

    //public float fTimeCount;

    SpriteRenderer Sr;
    public SpriteRenderer[] HeroFireSr;

    public CapsuleCollider2D Ccol2d;
    bool bBdSkill = false;               //주변 총알 없애기
    bool bSuSkill = false;               //주인공 이속 증가
    bool bMjSkill = false;               //주인공 무적
    bool bBsSkill = false;               //총알 크기 작아짐
    bool bHeroDie = false;
    bool bDmgAccess = false;                    //false일때만 데미지 입음
    bool bSkillSet = false;
    bool bDesCool = false;
    bool bSpeedSkillCheck;
    public bool bTimeCorutinStart = false;

    public bool bCountSetCom;
    public bool[] bSkills;

    float fBdSkillTime;                         //총알 없애는 스킬 지속시간
    float fSuSkill;                             //주인공 이속 증가 지속시간
    float fMjSkill;                             //주인공 무적 지속시간
    float fBsSkillCt;                           //총알 작아지는 스킬 쿨타임


    public int nSkillCount = 2;
    public int[] nSkillNum;
    public int[] SkillCool = new int[2];
    public int[] SkillCoolBackUp = new int[2];

    public Image[] HaveSKillImg;
    public Sprite[] SkillSpr;

    //화면 크기 가로2.5 세로4.5

    // Use this for initialization
    void Start()
    {
        Sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (!SGameMng.I.bPause && !SGameMng.I.bMoveAccess)
        {
            Move();
            TimeCount();
            if (bSkills[0])
            {
                DestroyBullet();
            }
            if (bSkills[1])
            {
                Mujuck();
            }
            if (bSkills[2])
            {
                SpeedUp();
                fSpeed = 5f;
            }
            if (bSkills[3])
            {
                BulletSmall();
            }
        }
        else
        {
            // 일시정지 일때
        }

        if (nSkillCount == 0 || SGameMng.I.bStartCheck)
        {
            SkillsGams.SetActive(false);

            for (int i = 0; i < 5; i++)
            {
                Explaying[i].SetActive(false);
            }

        }
        if (!bDesCool)
        {
            DestroySkill.transform.localPosition = Vector3.zero;
        }
        SKillCoolSet();
    }

    void Move()
    {
        if (!bHeroDie)
        {
            if (!JoystickScrp.bUse)
            {
                fSpeed = 0f;
            }
            else
            {
                if (!bSpeedSkillCheck)
                {
                    fSpeed = 3f;
                }
                PlayerParentTr.localPosition += new Vector3(JoystickScrp.DirVec.x, JoystickScrp.DirVec.y, JoystickScrp.DirVec.z) * fSpeed * Time.deltaTime;
                transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, JoystickScrp.PlayerAngle - 90));
            }
        }

        if (PlayerParentTr.localPosition.x >= 2.5f)
        {
            PlayerParentTr.Translate(Vector2.left * fSpeed * Time.deltaTime);
        }
        if (PlayerParentTr.localPosition.x <= -2.5f)
        {
            PlayerParentTr.Translate(Vector2.right * fSpeed * Time.deltaTime);
        }
        if (PlayerParentTr.localPosition.y >= 4.5f)
        {
            PlayerParentTr.Translate(Vector2.down * fSpeed * Time.deltaTime);
        }
        if (PlayerParentTr.localPosition.y <= -4.5f)
        {
            PlayerParentTr.Translate(Vector2.up * fSpeed * Time.deltaTime);
        }
    }

    void TimeCount()
    {
        if (!bHeroDie && SGameMng.I.bStartCheck && !bTimeCorutinStart)
        {
            StartCoroutine(TimeCountCor());
            bTimeCorutinStart = true;
        }
    }

    IEnumerator TimeCountCor()
    {
        yield return new WaitForSeconds(1f);
        if (!SGameMng.I.bPause)
            SGameMng.I.nTimeCount++;

        StartCoroutine(TimeCountCor());
    }

    public void DestroyBullet()
    {

        if (!bBdSkill)
        {
            if (!bBdSkill)                       //화면 총알 없애기 실행 Z키
            {
                for (int i = 0; i < REnemyMove.v_bullet.Count; i++)
                {
                    if (REnemyMove.v_bullet[i].isColliding)
                    {
                        REnemyMove.v_bullet[i].Sr.color = new Color(255f, 255f, 255f, 0f);
                        REnemyMove.v_bullet[i].bBulletCol = true;
                    }
                }
                bBdSkill = true;
                fBdSkillTime = Time.time;
                LazerGams[0].transform.localPosition = new Vector3(0f, -100f, 0f);
                for (int j = 0; j < 2; j++)                                         //레이저의 X,Y값만 바꿈
                {
                    LazerSc.fPosValue[j] = 100f;
                }
                //bDmgAccess = true;
                DestroySkill.transform.parent = BackGround.transform;
                DestroySkill.SetActive(true);
                bDesCool = true;
            }
        }
        if (bBdSkill)
        {
            if (Time.time > fBdSkillTime + 1f)                     //쿨타임 10초
            {
                bBdSkill = false;
                //bDmgAccess = false;
                bSkills[0] = false;
                DestroySkill.transform.parent = transform;
                bDesCool = false;
            }
        }

    }

    public void Mujuck()
    {
        if (!bMjSkill)                                                  //주인공 무적 실행 X키
        {

            bMjSkill = true;
            fMjSkill = Time.time;
            if (!bHeroDie)
            {
                ShieldGams.SetActive(true);
            }

        }

        if (bMjSkill)                                                       //2초간 무적
        {
            bDmgAccess = true;
            if (Time.time > fMjSkill + 3f)
            {
                bDmgAccess = false;
                ShieldGams.SetActive(false);
                bSkills[1] = false;
                bMjSkill = false;
            }

            //if (Time.time > fMjSkill + 1f)                                  //쿨타임 7초
            //{
            //    bMjSkill = false;
            //}
        }
    }

    public void SpeedUp()
    {
        if (!bSuSkill)                                                  //주인공 이속증가 실행 C키
        {

            bSpeedSkillCheck = true;
            bSuSkill = true;
            fSuSkill = Time.time;
            fSpeed = 5f;
            if (!bHeroDie)
            {
                HeroFireGams[0].SetActive(false);
                HeroFireGams[1].SetActive(true);
            }
        }

        if (bSuSkill)                                                       //3초간 이속 증가
        {
            if (Time.time > fSuSkill + 3f)
            {
                bSpeedSkillCheck = false;
                HeroFireGams[0].SetActive(true);
                HeroFireGams[1].SetActive(false);
                bSuSkill = false;
                bSkills[2] = false;
            }

            //if (Time.time > fSuSkill + 4f)                                  //쿨타임 5초(이속증가 끝난시점)
            //{
            //    bSuSkill = false;
            //}
        }

    }

    public void BulletSmall()
    {
        if (!bBsSkill)
        {
            for (int i = 0; i < REnemyMove.v_bullet.Count; i++)
            {
                if (REnemyMove.v_bullet[i].isColliding)
                    REnemyMove.v_bullet[i].transform.localScale = new Vector2(0.5f, 0.5f);
            }
            bBsSkill = true;
            if (LazerSc.bLazerUsed)
                LazerGams[1].transform.localScale = new Vector3(0.5f, 1f, 1f);

            fBsSkillCt = Time.time;
            bSkills[3] = false;
        }

        if (bBsSkill)
        {
            if (Time.time > fBsSkillCt + 1f)                                //총알 크기 줄이기 쿨타임 15초
            {
                bBsSkill = false;
            }
        }
    }

    IEnumerator SkillCount()
    {
        yield return new WaitForSeconds(0.001f);
        bSkillSet = false;
    }

    void SKillCoolSet()
    {
        if (!bCountSetCom)
        {
            switch (nSkillNum[0])
            {
                case 1:
                    SkillCool[0] = 10;
                    SkillCoolBackUp[0] = 10;
                    break;
                case 2:
                    SkillCool[0] = 7;
                    SkillCoolBackUp[0] = 7;
                    break;
                case 3:
                    SkillCool[0] = 8;
                    SkillCoolBackUp[0] = 8;
                    break;
                case 4:
                    SkillCool[0] = 15;
                    SkillCoolBackUp[0] = 15;
                    break;
            }



            switch (nSkillNum[1])
            {
                case 1:
                    SkillCool[1] = 10;
                    SkillCoolBackUp[1] = 10;
                    bCountSetCom = true;
                    break;
                case 2:
                    SkillCool[1] = 7;
                    SkillCoolBackUp[1] = 7;
                    bCountSetCom = true;
                    break;
                case 3:
                    SkillCool[1] = 8;
                    SkillCoolBackUp[1] = 8;
                    bCountSetCom = true;
                    break;
                case 4:
                    SkillCool[1] = 15;
                    SkillCoolBackUp[1] = 15;
                    bCountSetCom = true;
                    break;

            }

        }
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (!bDmgAccess)
        {
            if (col.tag == ("Bullet") || col.CompareTag("Lazer"))
            {
                bHeroDie = true;
                Sr.enabled = false;
                HeroFireSr[0].enabled = false;
                HeroFireSr[1].enabled = false;
                Ccol2d.enabled = false;
                DieGams.transform.parent = BackGround.transform;
                DieGams.SetActive(true);
            }
        }

        if (nSkillCount == 2 && !bSkillSet)
        {
            for (int i = 0; i < 4; i++)
            {
                if (col.CompareTag(("Skill" + i.ToString())))
                {
                    HaveSKillImg[0].sprite = SkillSpr[i];
                    bSkillSet = true;
                    nSkillNum[0] = i + 1;
                    Explaying[i].SetActive(false);
                    nSkillCount--;
                    StartCoroutine(SkillCount());
                }
            }
        }
        if (nSkillCount == 1 && !bSkillSet)
        {
            for (int i = 0; i < 4; i++)
            {
                if (col.CompareTag(("Skill" + i.ToString())))
                {
                    HaveSKillImg[1].sprite = SkillSpr[i];
                    bSkillSet = true;
                    nSkillNum[1] = i + 1;
                    Explaying[i].SetActive(false);
                    nSkillCount--;
                }
            }
        }
    }
}