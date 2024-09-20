using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QSkill : Skill
{
    private bool ifEnabled;
    [SerializeField] private List<AudioClip> qskillFisrtClips;
    [SerializeField] private List<AudioClip> qskillSecondClips;
    [SerializeField] private List<AudioClip> qskillThirdClips;
    public List<List<AudioClip>> skillClips;
    private AudioSource audioSource;

    [SerializeField] private List<float> skillAnimationLength;
    private float skillLength;

    private static int SkillRealeaseCnter = 0;
    private LineRenderer lineRenderer;


    public bool IfEnabled => ifEnabled;
    public float SkillLength => skillLength;

    private void Start()
    {
        ifEnabled = true;
        animationNames = new List<string>() {
            "QSkillFirst",
            "QSkillSecond",
            "QSkillThird"
        };

        skillClips = new List<List<AudioClip>>() {
            qskillFisrtClips,
            qskillSecondClips,
            qskillThirdClips};

        audioSource = gameObject.GetComponent<AudioSource>();
        coolDownTime = 2f;
        skillLength = skillAnimationLength[0];

        lineRenderer = gameObject.GetComponent<LineRenderer>();
    }

    private void Update()
    {
        skillLength = skillAnimationLength[SkillRealeaseCnter];
    }


    public void Release(Animator animator, PlayerInput skillCaller, GameObject rayLauncher)
    {
        if(ifEnabled == true) {
            if (skillCaller.IfESkilling == true)
            {
                Debug.Log("EQ释放");
                animator.SetBool("EQ", true);
            }
            else
            {
                animator.Play(animationNames[SkillRealeaseCnter]);
            }
            StartCoroutine(AnimationOverFlag(skillCaller));
            StartCoroutine(CoolDown());
            
            int index = Random.Range(0, skillClips[SkillRealeaseCnter].Count);
            audioSource.clip = skillClips[SkillRealeaseCnter][index];
            audioSource.Play();
            
            SkillRay(rayLauncher);
            SkillRealeaseCnter += 1;
            if (SkillRealeaseCnter >= 3)
            {
                SkillRealeaseCnter = 0;
            }
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
        yield return new WaitForSeconds(skillAnimationLength[SkillRealeaseCnter]);
        skillCaller.IfAQWERed = false;
        Debug.Log("Q技能释放完毕");
    }

    void SkillRay(GameObject skillCaller)
    {
        float rayLength = 300f; // 射线的长度
        float rayWidth = 80f;   // 射线的宽度

        lineRenderer.startWidth = rayWidth;
        lineRenderer.endWidth = rayWidth;
        lineRenderer.positionCount = 2;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default")); // 为了简单可见，使用一个简单的材质
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;

        Vector3 startPoint = skillCaller.transform.position;
        Vector3 direction = skillCaller.transform.forward;
        Vector3 endPoint = startPoint + direction * rayLength;

        // 绘制射线
        lineRenderer.SetPosition(0, startPoint);
        lineRenderer.SetPosition(1, endPoint);

    }
}
