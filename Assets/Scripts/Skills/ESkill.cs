using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESkill : Skill
{
    private bool ifEnabled;
    private float animationLength;
    private AudioSource audioSource;
    [SerializeField] private List<AudioClip> attackClipList;

    public float SkillLength => animationLength;

    private void Start()
    {
        ifEnabled = true;
        animationNames = new List<string>() {
            "yasuo_ESkill"
        };
        animationLength = 1f;
        coolDownTime = 0.5f;
        audioSource = GetComponent<AudioSource>();
    }


    public void Release(Animator animator, PlayerInput skillCaller)
    {
        if (ifEnabled == true)
        {
            animator.SetBool("EQ", false);
            StartCoroutine(AnimationOverFlag(skillCaller)); 
            StartCoroutine(CoolDown());
            int audioClipIndex = Random.Range(0, attackClipList.Count);
            animator.Play(animationNames[0]);
            audioSource.clip = attackClipList[audioClipIndex];
            audioSource.Play();
        }
    }

    private IEnumerator CoolDown()
    {
        ifEnabled = false;
        yield return new WaitForSeconds(coolDownTime);
        ifEnabled = true;
    }

    private IEnumerator AnimationOverFlag(PlayerInput skillCaller)
    {
        yield return new WaitForSeconds(animationLength);
        skillCaller.IfAQWERed = false;
        Debug.Log("E技能释放完毕");
    }
}
