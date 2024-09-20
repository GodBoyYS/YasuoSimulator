using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WSkill : Skill
{
    [SerializeField] private GameObject windWall;
    private bool ifEnabled;

    private AudioSource audioSource;
    [SerializeField] private List<AudioClip> attackClipList;
    private float animationLength;

    public float SkillLength => animationLength;


    private void Start()
    {
        ifEnabled = true;
        coolDownTime = 10f;
        animationLength = 0.667f;
        animationNames = new List<string>() {
            "yasuo_WSkill"
        };

        audioSource = GetComponent<AudioSource>();
    }

    public void Release(Animator animator, PlayerInput skillCaller, GameObject hero)
    {
        if(ifEnabled == true)
        {
            StartCoroutine(AnimationOverFlag(skillCaller));
            StartCoroutine(CoolDown());
            windWall.transform.position = hero.transform.position;
            windWall.SetActive(true);
            StartCoroutine(WindWallKeeper());
            animator.Play(animationNames[0]);
            int audioClipIndex = Random.Range(0, attackClipList.Count);
            audioSource.clip = attackClipList[audioClipIndex];
            audioSource.Play();
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        }
    }

    private IEnumerator WindWallKeeper()
    {
        yield return new WaitForSeconds(3);
        windWall.SetActive(false);
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
        Debug.Log("W技能释放完毕");
    }
}
