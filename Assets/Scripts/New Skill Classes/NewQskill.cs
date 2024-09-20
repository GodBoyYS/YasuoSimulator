using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewQSkill : Skill
{
    private bool ifEnabled;
    [SerializeField] private List<AudioClip> qskillFisrtClips;
    [SerializeField] private List<AudioClip> qskillSecondClips;
    [SerializeField] private List<AudioClip> qskillThirdClips;
    public List<List<AudioClip>> skillClips;
    private AudioSource audioSource;

    private List<string> skillAnimationNames;
    string eqAnimation = "yasuo_spell1_dash";

    [SerializeField] private List<float> skillAnimationLength;
    private float skillLength;

    private static int SkillReleaseCnter = 0;
    private LineRenderer lineRenderer;


    public bool IfEnabled => ifEnabled;
    public float SkillLength => skillLength;

    public float CoolDownTime => coolDownTime;

    private void Start()
    {
        ifEnabled = true;
        skillClips = new List<List<AudioClip>>() {
            qskillFisrtClips,
            qskillSecondClips,
            qskillThirdClips
        };

        skillAnimationNames = new List<string>()
        {
            "yasuo_spell1a",
            "yasuo_spell1b",
            "yasuo_spell1c",
        };

        

        audioSource = gameObject.GetComponent<AudioSource>();
        coolDownTime = 2f;
        skillLength = skillAnimationLength[0];

        lineRenderer = gameObject.GetComponent<LineRenderer>();
    }

    private void Update()
    {

    }


    public void Release(Animator animator, int releaseFalg)
    {
        // flag == 0, normal; flag == 1, eq
        Debug.Log("释放Q技能");
        if (ifEnabled == true)
        {
            //if (skillCaller.IfESkilling == true)
            //{
            //    Debug.Log("EQ释放");
            //    animator.SetBool("EQ", true);
            //}

            if(releaseFalg == 0)
            {
                string curAnimationName = skillAnimationNames[SkillReleaseCnter];
                animator.Play(curAnimationName);
            }
            else if(releaseFalg == 1)
            {
                animator.CrossFade(eqAnimation, 0.25f);
            }

            animator.SetBool("RUN", false);
            StartCoroutine(CoolDown());

            int index = Random.Range(0, skillClips[SkillReleaseCnter].Count);
            audioSource.clip = skillClips[SkillReleaseCnter][index];
            audioSource.Play();

            //SkillRay(rayLauncher);
            SkillReleaseCnter += 1;
            if (SkillReleaseCnter >= 3)
            {
                SkillReleaseCnter = 0;
            }
        }
    }

    private IEnumerator CoolDown()
    {
        ifEnabled = false;
        yield return new WaitForSeconds(coolDownTime);
        ifEnabled = true;
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
