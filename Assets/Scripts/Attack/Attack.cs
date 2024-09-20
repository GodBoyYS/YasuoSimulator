using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : Skill
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
        if(Input.GetKeyUp(KeyCode.A))
        {
            attackRangeIndicator.SetActive(false);
        }
    }
    public void Release(Animator animator, PlayerInput attackCaller)
    {
        attackRangeIndicator.SetActive(true);
        if (ifEnabled == true)
        {// 需要一个变量来指示，该次普攻有没有被打断
            if (ifAttackContinuos == true)
            {
                attackAnimationIndex += 1;
                if (attackAnimationIndex > 3)
                {
                    attackAnimationIndex = 0;
                }
            }
            else
            {
                attackAnimationIndex = 0;
            }
            string targetAttackAnimation = animationNames[attackAnimationIndex];
            animator.speed = attackSpeed;
            animator.Play(targetAttackAnimation); // play animation
            
            int audioClipIndex = Random.Range(0, attackClipList.Count);
            audioSource.clip = attackClipList[audioClipIndex];
            audioSource.Play();

            StartCoroutine(CoolDownw(attackCd, attackCaller));
        }
        else
        {
            ifAttackContinuos = true;
        }
    }

    private IEnumerator CoolDownw(float cd, PlayerInput attackCaller)
    {
        ifEnabled = false;
        yield return new WaitForSeconds(cd);
        ifEnabled = true;
        attackCaller.IfAQWERed = false;
        Debug.Log("普攻释放完毕");
    }

}
