using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewRskill : Skill
{
    private bool ifEnabled;
    private float animationLength;
    private AudioSource audioSource;
    [SerializeField] private List<AudioClip> attackClipList;

    public bool IfEnabled => ifEnabled;
    public float SkillLength => animationLength;
    public float CoolDownTime => coolDownTime;

    private void Start()
    {
        ifEnabled = true;
        animationNames = new List<string>() {
            "yasuo_spell4"
        };

        animationLength = 1.33f;
        coolDownTime = 15f;
        audioSource = GetComponent<AudioSource>();
    }


    public void Release(Animator animator)
    {
        if (ifEnabled == true)
        {
            StartCoroutine(AnimationOverFlag());
            StartCoroutine(CoolDown());

            animator.SetBool("RUN", false);
            animator.Play(animationNames[0]);

            int audioClipIndex = Random.Range(0, attackClipList.Count);
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

   // private IEnumerator AnimationOverFlag(PlayerInput skillCaller)
    private IEnumerator AnimationOverFlag()
    {
        yield return new WaitForSeconds(animationLength);
        //skillCaller.IfAQWERed = false;
        Debug.Log("R技能释放完毕");
    }
}
