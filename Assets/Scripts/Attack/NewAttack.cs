using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewAttack : Skill
{
    [SerializeField] private GameObject attackRangeIndicator;
    [SerializeField] private float attackSpeed;
    private bool ifEnabled;
    private bool ifAttackContinuos;
    private int attackAnimationIndex;
    private float attackCd;

    private AudioSource audioSource;
    [SerializeField] private List<AudioClip> attackClipList;

    public bool IfAttackContinuos
    {
        get { return ifAttackContinuos; }
        set { ifAttackContinuos = value; }
    }

    public bool IfEnabled => ifEnabled;


    private void Start()
    {
        animationNames = new List<string>() {
            "yasuo_attack1",
            "yasuo_attack2",
            "yasuo_attack3",
            "yasuo_attack4"};

        ifEnabled = true;
        attackAnimationIndex = 0;
        ifAttackContinuos = false;
        attackCd = 1 / attackSpeed;

        // component
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.A))
        {
            attackRangeIndicator.SetActive(false);
        }
    }
    public void Release(Animator animator)
    {
        attackRangeIndicator.SetActive(true);
        string targetAttackAnimation = animationNames[attackAnimationIndex++];
        animator.Play(targetAttackAnimation); // play animation

        if(attackAnimationIndex > 3)
        {
            attackAnimationIndex = 0;
        }

        int audioClipIndex = Random.Range(0, attackClipList.Count);
        audioSource.clip = attackClipList[audioClipIndex];
        audioSource.Play();

        StartCoroutine(CoolDownw(attackCd));
    }

    private IEnumerator CoolDownw(float cd)
    {
        ifEnabled = false;
        yield return new WaitForSeconds(cd);
        ifEnabled = true;
        Debug.Log("∆’π• Õ∑≈ÕÍ±œ");
    }

}
