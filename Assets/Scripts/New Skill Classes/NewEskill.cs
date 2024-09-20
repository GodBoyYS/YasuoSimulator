using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewEskill : Skill
{
    private bool ifEnabled;
    private float animationLength;
    private AudioSource audioSource;
    [SerializeField] private List<AudioClip> attackClipList;

    public float SkillLength => animationLength;
    public float CoolDownTime => coolDownTime;
    public bool IfEnabled => ifEnabled;

    private List<string> EToIdle;
    string eOvertype1;
    string eOvertype2;

    private void Start()
    {
        ifEnabled = true;
        animationNames = new List<string>() {
            "yasuo_spell3"
        };
        animationLength = 1f;
        coolDownTime = 0.5f;
        audioSource = GetComponent<AudioSource>();

        EToIdle = new List<string>() {
            "yasuo_idle1", "yasuo_run1", "yasuo_run2"
        };
        eOvertype1 = "yasuo_idle1";
        eOvertype2 = "yasuo_idle_out";
    }


    public void Release(Animator animator)
    {
        if (ifEnabled == true)
        {
            Echange(animator);
            animator.SetBool("RUN", false);
            animator.Play(animationNames[0]);

            //animator.SetBool("EQ", false);
            //StartCoroutine(AnimationOverFlag(skillCaller));
            StartCoroutine(CoolDown());
            StartCoroutine(AnimationOverFlag()); 
            int audioClipIndex = Random.Range(0, attackClipList.Count);
            audioSource.clip = attackClipList[audioClipIndex];
            audioSource.Play();
        }
    }

    private void Echange(Animator animator)
    {   // 用来检测E技能该往哪个状态切换
        // 能够让E技能在收刀状态下切换，唯二的情况就是idle以及run，其余的，通通进入idle_out
        // 那么，E技能应该也单独拿出来
        // Debug.Log("释放了E技能");
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        for (int i = 0; i < EToIdle.Count; i++)
        {
            if (stateInfo.shortNameHash == Animator.StringToHash(EToIdle[i]))
            {
                //targetAnimation = eOvertype1;
                animator.SetInteger("EOVERTYPE", 1);
                return;
            }
        }

        //targetAnimation = eOvertype2;
        animator.SetInteger("EOVERTYPE", 2);
    }

    private IEnumerator CoolDown()
    {
        ifEnabled = false;
        yield return new WaitForSeconds(coolDownTime);
        ifEnabled = true;
    }

    //private IEnumerator AnimationOverFlag(PlayerInput skillCaller)
    private IEnumerator AnimationOverFlag()
    {
        yield return new WaitForSeconds(animationLength);
        //skillCaller.IfAQWERed = false;
        Debug.Log("E技能释放完毕");
    }
}
