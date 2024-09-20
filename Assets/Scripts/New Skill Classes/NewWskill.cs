using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewWskill : Skill
{
    [SerializeField] private GameObject windWall;
    private bool ifEnabled;

    private AudioSource audioSource;
    [SerializeField] private List<AudioClip> skillClipList;
    private float animationLength;

    public float SkillLength => animationLength;
    public float CoolDownTime => coolDownTime;

    public bool IfEnabled => ifEnabled;


    private void Start()
    {
        ifEnabled = true;
        coolDownTime = 10f;
        animationLength = 0.667f;
        animationNames = new List<string>() {
            "yasuo_spell2"
        };

        audioSource = GetComponent<AudioSource>();
    }

    public void Release(Animator animator, NewPlayerInput skillCaller, GameObject hero)
    {
        if (ifEnabled == true)
        {
            StartCoroutine(CoolDown());
            windWall.transform.position = hero.transform.position;
            windWall.SetActive(true);
            StartCoroutine(WindWallKeeper());

            animator.SetBool("RUN", false);
            animator.Play(animationNames[0]);
            
            audioSource.clip = skillClipList[0];
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

}
