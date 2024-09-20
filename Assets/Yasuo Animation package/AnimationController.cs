
/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Animator animator;
    private string curAnimationName;

    private Dictionary<string, List<string>> ToMoveAnimations;
    private List<string> ToMoveAnimationsKeys;
    Dictionary<string, string> fromIdleToFast;

    List<string> fromIdleToFastKeys;

    private Dictionary<string, string> ToIdleAnimations;
    private List<string> ToIdleAnimationsKeys;
    private List<string> EToIdle;
    string eOvertype1;
    string eOvertype2;

    private string targetAnimation;
    private bool ifChangeAnimation;
    private float crossFadeTime = 0.25f;
    private bool ifRunning;

    [SerializeField] private float speed;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("RUN", false);
        curAnimationName = "";
        targetAnimation = "";
        ifChangeAnimation = false;
        ifRunning = false;

        ToMoveAnimations = new Dictionary<string, List<string>>() {
            { "yasuo_idle_in", new List<string>{ "yasuo_run1", "yasuo_run2" } },
            {"yasuo_idle_in_sheathed", new List<string>{ "yasuo_run1", "yasuo_run2" } },
            {"yasuo_idle1", new List<string>{ "yasuo_run1", "yasuo_run2" } },
            { "yasuo_idle_out", new List<string>{ "yasuo_run_out_loop" } }
        };
        ToMoveAnimationsKeys = new List<string>(ToMoveAnimations.Keys);

        fromIdleToFast = new Dictionary<string, string>() {
            {  "yasuo_idle1","yasuo_run_fast_in_sheathed"},
            {  "yasuo_run1","yasuo_run_fast_in_sheathed"},
            {  "yasuo_run2","yasuo_run_fast_in_sheathed"},
            {  "yasuo_idle_in_sheathed","yasuo_run_fast_in_sheathed"},
            {  "yasuo_run_out_loop","yasuo_run_fast_in"},
            {  "yasuo_run_out","yasuo_run_fast_in"},
            {  "yasuo_idle_out","yasuo_run_fast_in"},
            {  "yasuo_idle_in","yasuo_run_fast_in"},
        };
        fromIdleToFastKeys = new List<string>(fromIdleToFast.Keys);


        ToIdleAnimations = new Dictionary<string, string>() {
            {"yasuo_run_out_loop", "yasuo_idle_out" },
            {"yasuo_run_out", "yasuo_idle_out" },
            {"yasuo_sheath_run", "yasuo_idle_in_sheathed" },
            { "yasuo_run1", "yasuo_idle_in_sheathed"},
            { "yasuo_run2", "yasuo_idle_in_sheathed"},
            {"yasuo_run_fast_in", "yasuo_idle_out" },
            {"yasuo_run_fast_loop", "yasuo_idle_out" },
        };
        ToIdleAnimationsKeys = new List<string>(ToIdleAnimations.Keys);

        EToIdle = new List<string>() {
            "yasuo_idle1", "yasuo_run1", "yasuo_run2"
        };
        eOvertype1 = "yasuo_idle1";
        eOvertype2 = "yasuo_idle_out";
    }


    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(1))
        {   // �ƶ����԰�ť
            animator.SetBool("RUN", true);
            animator.SetFloat("SPEED", speed);
           
            C1();
            if (ifChangeAnimation)
            {
                animator.CrossFade(targetAnimation, 0.1f);
            }
            ifRunning = true;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {   // ��ֹ���԰�ť
            animator.SetBool("RUN", false);
            
            C2();
            if(ifChangeAnimation)
            {
                animator.CrossFade(targetAnimation, 0.1f);
            }
            ifRunning = false;
        }

        if (Input.GetKeyUp(KeyCode.D))
        {   // ģ�⵽��ָ���ص�
            animator.SetBool("RUN", false);
            
            C3();
            if (ifChangeAnimation)
            {
                animator.CrossFade(targetAnimation, crossFadeTime);
            }
            ifRunning = false;
        }

        if(Input.GetKeyDown(KeyCode.A))
        {
            curAnimationName = $"yasuo_attack{(int) Random.Range(1, 5)}";
            animator.SetBool("RUN", false);
            ifRunning = false;
            animator.Play(curAnimationName);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            char[] indices = { 'a', 'b', 'c' };
            curAnimationName = $"yasuo_spell1{(char)(97 + (int)Random.Range(0, 3))}";
            animator.SetBool("RUN", false);
            ifRunning = false;
            animator.Play(curAnimationName);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            curAnimationName = "yasuo_spell2";
            animator.SetBool("RUN", false);
            ifRunning = false;
            animator.Play(curAnimationName);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Echange();
            curAnimationName = "yasuo_spell3";
            animator.SetBool("RUN", false);
            ifRunning = false;
            animator.Play(curAnimationName);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            curAnimationName = "yasuo_spell4";
            animator.SetBool("RUN", false);
            ifRunning = false;
            animator.Play(curAnimationName);
        }
    }

    private void FixedUpdate()
    {
        
    }

    private void C1()
    {   // ��Ҫ�ƶ�ʱ�����Ķ����ĺ���
        // ��ȡ��ǰ״̬���㼶0�е�״̬��Ϣ
        // �����ٶ�����
        Debug.Log("C1!!!");
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // in sheathed => fast in sheathed

        

        if (speed <= 350)
        {   // normal
            for (int i = 0; i < ToMoveAnimationsKeys.Count; i++)
            {
                if (stateInfo.shortNameHash == Animator.StringToHash(ToMoveAnimationsKeys[i]))
                {
                    Debug.Log($"�Ҽ��ƶ���ʱ���ڽ��� {ToMoveAnimationsKeys[i]} �������л��������ܶ�");
                    curAnimationName = ToMoveAnimationsKeys[i];
                    targetAnimation = ToMoveAnimations[ToMoveAnimationsKeys[i]][(int)Random.Range(0, ToMoveAnimations[ToMoveAnimationsKeys[i]].Count)];
                    ifChangeAnimation = true;
                    return;
                }
            }
        }

        else
        {   // fast
            for(int i = 0; i < fromIdleToFastKeys.Count; i++) { 
                if(stateInfo.shortNameHash == Animator.StringToHash(fromIdleToFastKeys[i]))
                {
                    targetAnimation = fromIdleToFast[fromIdleToFastKeys[i]];
                    ifChangeAnimation = true;
                    return;
                }
            }
        }

        Debug.Log("��������������Ҫ�л���������");
        ifChangeAnimation = false;
    }

    private void C2()
    {   // ����Sʱ����Ҫ���Ķ����ĺ���
        // ��ȡ��ǰ״̬���㼶0�е�״̬��Ϣ
        Debug.Log("C2!!!");
        if(ifRunning == true)
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            for (int i = 0; i < ToIdleAnimationsKeys.Count; i++)
            {
                if (stateInfo.shortNameHash == Animator.StringToHash(ToIdleAnimationsKeys[i]))
                {
                    Debug.Log($"���� S ��ʱ���ڽ��� {ToIdleAnimationsKeys[i]} �������л���վ��");
                    curAnimationName = ToIdleAnimationsKeys[i];
                    targetAnimation = ToIdleAnimations[ToIdleAnimationsKeys[i]];
                    ifChangeAnimation = true;
                    return;
                }
            }
        }
        else
        {
            Debug.Log("���� S ��ʱ�������������������л���վ��");
            ifChangeAnimation = false;
        }
    }

    private void C3()
    {   // �����ƶ��ص�ʱ����Ҫ�л������ĺ���
        Debug.Log("C3!!!");
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // ���״̬�Ƿ�ƥ��
        for (int i = 0; i < ToIdleAnimationsKeys.Count; i++)
        {
            if (stateInfo.shortNameHash == Animator.StringToHash(ToIdleAnimationsKeys[i]))
            {
                Debug.Log($"����Ŀ��ص��ʱ���ڽ��� {ToIdleAnimationsKeys[i]} �������л���վ��");
                curAnimationName = ToIdleAnimationsKeys[i];
                targetAnimation = ToIdleAnimations[ToIdleAnimationsKeys[i]];
                ifChangeAnimation = true;
                return;
            }
        }

        Debug.Log("����Ŀ��ص��ʱ�������������������л���վ��");
        ifChangeAnimation = false;
    }

    private void Echange()
    {   // �������E���ܸ����ĸ�״̬�л�
        // �ܹ���E�������յ�״̬���л���Ψ�����������idle�Լ�run������ģ�ͨͨ����idle_out
        // ��ô��E����Ӧ��Ҳ�����ó���
        Debug.Log("�ͷ���E����");
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        for(int i = 0; i < EToIdle.Count; i++)
        {
            if(stateInfo.shortNameHash == Animator.StringToHash(EToIdle[i]))
            {
                //targetAnimation = eOvertype1;
                animator.SetInteger("EOVERTYPE", 1);
                return;
            }
        }

        //targetAnimation = eOvertype2;
        animator.SetInteger("EOVERTYPE", 2);
    }
}


*/
// version1


// version2 -- + input codes
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// ����ű�ֻ���𶯻����л��������𶯻��ľ��岥��
// ���嶯�����磺���ܡ��չ����赸�ȵ�
// �þ���������ɣ��ýű����������۵���
// director class

public class AnimationController : MonoBehaviour
{
    #region properties about animation
    private Animator animator;
    private string curAnimationName;

    private Dictionary<string, List<string>> ToMoveAnimations;
    private List<string> ToMoveAnimationsKeys;
    Dictionary<string, string> fromIdleToFast;

    List<string> fromIdleToFastKeys;

    private Dictionary<string, string> ToIdleAnimations;
    private List<string> ToIdleAnimationsKeys;
    private List<string> EToIdle;
    string eOvertype1;
    string eOvertype2;

    private string targetAnimation;
    private bool ifChangeAnimation;
    private float crossFadeTime = 0.25f;
    private bool ifRunning;

    [SerializeField] private float speed;
    #endregion

    #region skills
    // for idle or running animations have nothing to do with skills alike actions
    // we're not going to take idle or run into classes
    // firstly we need four skills to maintain the animation and audios
    // every time we launch a skill, use the skill-class to achieve such goal
    [SerializeField] GameObject hero;
    [SerializeField] QSkill _qskill;
    [SerializeField] WSkill _wskill;
    [SerializeField] ESkill _eskill;
    [SerializeField] RSkill _rskill;
    [SerializeField] Attack _attack;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        #region entity for animations
        animator = GetComponent<Animator>();
        animator.SetBool("RUN", false);
        curAnimationName = "";
        targetAnimation = "";
        ifChangeAnimation = false;
        ifRunning = false;

        ToMoveAnimations = new Dictionary<string, List<string>>() {
            { "yasuo_idle_in", new List<string>{ "yasuo_run1", "yasuo_run2" } },
            {"yasuo_idle_in_sheathed", new List<string>{ "yasuo_run1", "yasuo_run2" } },
            {"yasuo_idle1", new List<string>{ "yasuo_run1", "yasuo_run2" } },
            { "yasuo_idle_out", new List<string>{ "yasuo_run_out_loop" } }
        };
        ToMoveAnimationsKeys = new List<string>(ToMoveAnimations.Keys);

        fromIdleToFast = new Dictionary<string, string>() {
            {  "yasuo_idle1","yasuo_run_fast_in_sheathed"},
            {  "yasuo_run1","yasuo_run_fast_in_sheathed"},
            {  "yasuo_run2","yasuo_run_fast_in_sheathed"},
            {  "yasuo_idle_in_sheathed","yasuo_run_fast_in_sheathed"},
            {  "yasuo_run_out_loop","yasuo_run_fast_in"},
            {  "yasuo_run_out","yasuo_run_fast_in"},
            {  "yasuo_idle_out","yasuo_run_fast_in"},
            {  "yasuo_idle_in","yasuo_run_fast_in"},
        };
        fromIdleToFastKeys = new List<string>(fromIdleToFast.Keys);


        ToIdleAnimations = new Dictionary<string, string>() {
            {"yasuo_run_out_loop", "yasuo_idle_out" },
            {"yasuo_run_out", "yasuo_idle_out" },
            {"yasuo_sheath_run", "yasuo_idle_in_sheathed" },
            { "yasuo_run1", "yasuo_idle_in_sheathed"},
            { "yasuo_run2", "yasuo_idle_in_sheathed"},
            {"yasuo_run_fast_in", "yasuo_idle_out" },
            {"yasuo_run_fast_loop", "yasuo_idle_out" },
        };
        ToIdleAnimationsKeys = new List<string>(ToIdleAnimations.Keys);

        EToIdle = new List<string>() {
            "yasuo_idle1", "yasuo_run1", "yasuo_run2"
        };
        eOvertype1 = "yasuo_idle1";
        eOvertype2 = "yasuo_idle_out";

        #endregion

    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1))
        {   // �ƶ����԰�ť
            animator.SetBool("RUN", true);
            animator.SetFloat("SPEED", speed);

            C1();
            if (ifChangeAnimation)
            {
                animator.CrossFade(targetAnimation, 0.1f);
            }
            ifRunning = true;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {   // ��ֹ���԰�ť
            animator.SetBool("RUN", false);

            C2();
            if (ifChangeAnimation)
            {
                animator.CrossFade(targetAnimation, 0.1f);
            }
            ifRunning = false;
        }

        if (Input.GetKeyUp(KeyCode.D))
        {   // ģ�⵽��ָ���ص�
            // ������Ҫ���⹤������Ϊ�����ͱ����Ĳ�ͬ
            animator.SetBool("RUN", false);

            C3();
            if (ifChangeAnimation)
            {
                animator.CrossFade(targetAnimation, crossFadeTime);
            }
            ifRunning = false;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            curAnimationName = $"yasuo_attack{(int)Random.Range(1, 5)}";
            animator.SetBool("RUN", false);
            ifRunning = false;
            animator.Play(curAnimationName);
           
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            char[] indices = { 'a', 'b', 'c' };
            curAnimationName = $"yasuo_spell1{(char)(97 + (int)Random.Range(0, 3))}";
            animator.SetBool("RUN", false);
            ifRunning = false;
            animator.Play(curAnimationName);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            curAnimationName = "yasuo_spell2";
            animator.SetBool("RUN", false);
            ifRunning = false;
            animator.Play(curAnimationName);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Echange();
            curAnimationName = "yasuo_spell3";
            animator.SetBool("RUN", false);
            ifRunning = false;
            animator.Play(curAnimationName);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            curAnimationName = "yasuo_spell4";
            animator.SetBool("RUN", false);
            ifRunning = false;
            animator.Play(curAnimationName);
        }
    }

    private void FixedUpdate()
    {

    }

    private void C1()
    {   // ��Ҫ�ƶ�ʱ�����Ķ����ĺ���
        // ��ȡ��ǰ״̬���㼶0�е�״̬��Ϣ
        // �����ٶ�����
        Debug.Log("C1!!!");
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // in sheathed => fast in sheathed
        if (speed <= 350)
        {   // normal
            for (int i = 0; i < ToMoveAnimationsKeys.Count; i++)
            {
                if (stateInfo.shortNameHash == Animator.StringToHash(ToMoveAnimationsKeys[i]))
                {
                    Debug.Log($"�Ҽ��ƶ���ʱ���ڽ��� {ToMoveAnimationsKeys[i]} �������л��������ܶ�");
                    curAnimationName = ToMoveAnimationsKeys[i];
                    targetAnimation = ToMoveAnimations[ToMoveAnimationsKeys[i]][(int)Random.Range(0, ToMoveAnimations[ToMoveAnimationsKeys[i]].Count)];
                    ifChangeAnimation = true;
                    return;
                }
            }
        }

        else
        {   // fast
            for (int i = 0; i < fromIdleToFastKeys.Count; i++)
            {
                if (stateInfo.shortNameHash == Animator.StringToHash(fromIdleToFastKeys[i]))
                {
                    targetAnimation = fromIdleToFast[fromIdleToFastKeys[i]];
                    ifChangeAnimation = true;
                    return;
                }
            }
        }

        Debug.Log("��������������Ҫ�л���������");
        ifChangeAnimation = false;
    }

    private void C2()
    {   // ����Sʱ����Ҫ���Ķ����ĺ���
        // ��ȡ��ǰ״̬���㼶0�е�״̬��Ϣ
        Debug.Log("C2!!!");
        if (ifRunning == true)
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            for (int i = 0; i < ToIdleAnimationsKeys.Count; i++)
            {
                if (stateInfo.shortNameHash == Animator.StringToHash(ToIdleAnimationsKeys[i]))
                {
                    Debug.Log($"���� S ��ʱ���ڽ��� {ToIdleAnimationsKeys[i]} �������л���վ��");
                    curAnimationName = ToIdleAnimationsKeys[i];
                    targetAnimation = ToIdleAnimations[ToIdleAnimationsKeys[i]];
                    ifChangeAnimation = true;
                    return;
                }
            }
        }
        else
        {
            Debug.Log("���� S ��ʱ�������������������л���վ��");
            ifChangeAnimation = false;
        }
    }

    private void C3()
    {   // �����ƶ��ص�ʱ����Ҫ�л������ĺ���
        Debug.Log("C3!!!");
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // ���״̬�Ƿ�ƥ��
        for (int i = 0; i < ToIdleAnimationsKeys.Count; i++)
        {
            if (stateInfo.shortNameHash == Animator.StringToHash(ToIdleAnimationsKeys[i]))
            {
                Debug.Log($"����Ŀ��ص��ʱ���ڽ��� {ToIdleAnimationsKeys[i]} �������л���վ��");
                curAnimationName = ToIdleAnimationsKeys[i];
                targetAnimation = ToIdleAnimations[ToIdleAnimationsKeys[i]];
                ifChangeAnimation = true;
                return;
            }
        }

        Debug.Log("����Ŀ��ص��ʱ�������������������л���վ��");
        ifChangeAnimation = false;
    }

    private void Echange()
    {   // �������E���ܸ����ĸ�״̬�л�
        // �ܹ���E�������յ�״̬���л���Ψ�����������idle�Լ�run������ģ�ͨͨ����idle_out
        // ��ô��E����Ӧ��Ҳ�����ó���
        Debug.Log("�ͷ���E����");
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
}
