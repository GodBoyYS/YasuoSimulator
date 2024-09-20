using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RSkill : Skill
{
    private bool ifEnabled;
    private float animationLength;
    private AudioSource audioSource;
    [SerializeField] private List<AudioClip> attackClipList;

    public bool IfEnabled => ifEnabled;
    public float AnimationLength => animationLength;

    private void Start()
    {
        ifEnabled = true;
        animationNames = new List<string>() {
            "yasuo_RSkill"
        };

        animationLength = 1.33f;
        coolDownTime = 15f;
        audioSource = GetComponent<AudioSource>();
    }


    public void Release(Animator animator, PlayerInput skillCaller)
    {
        if (ifEnabled == true)
        {
            StartCoroutine(AnimationOverFlag(skillCaller));
            StartCoroutine(CoolDown());
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

    private IEnumerator AnimationOverFlag(PlayerInput skillCaller)
    {
        yield return new WaitForSeconds(animationLength);
        skillCaller.IfAQWERed = false;
        Debug.Log("R技能释放完毕");
    }
}
