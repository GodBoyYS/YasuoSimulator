// version2 -- + input codes
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// ����ű�ֻ���𶯻����л��������𶯻��ľ��岥��
// ���嶯�����磺���ܡ��չ����赸�ȵ�
// �þ���������ɣ��ýű����������۵���
// director class

public class AnimationChanger
{
    #region properties about animation
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


    // ��ʼ������
    public void Init()
    {
        #region entity for animations
        curAnimationName = "";
        targetAnimation = "";
        ifChangeAnimation = false;
        ifRunning = false;

        ToMoveAnimations = new Dictionary<string, List<string>>() {
            { "yasuo_idle_in", new List<string>{ "yasuo_run1", "yasuo_run2" } },
            {"yasuo_idle_in_sheathed", new List<string>{ "yasuo_run1", "yasuo_run2" } },
            {"yasuo_idle1", new List<string>{ "yasuo_run1", "yasuo_run2" } },
            { "yasuo_idle_out", new List<string>{ "yasuo_run_out_loop" } },
            {"yasuo_idle2", new List<string>{ "yasuo_run1", "yasuo_run2" } }
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


    public void ToMove(Animator animator, float speed)
    {
        animator.SetBool("RUN", true);
        animator.SetFloat("SPEED", speed);

        C1(animator);
        if (ifChangeAnimation)
        {
            animator.CrossFade(targetAnimation, 0f);
        }
    }

    public void MoveToIdle(int idleFalg, Animator animator, bool isMoving)
    {
        if(idleFalg == 0)
        {
            C2(animator, isMoving);
        }
        else if(idleFalg == 1) {
            C3(animator);
        }

        animator.SetBool("RUN", false);
        if (ifChangeAnimation)
        {
            animator.CrossFade(targetAnimation, 0.1f);
        }
        
    }


    public void AttackRelease(Animator animator)
    {
        curAnimationName = $"yasuo_attack{(int)Random.Range(1, 5)}";
        animator.SetBool("RUN", false);
        animator.Play(curAnimationName);
    }


    // �ƶ�ʱ�Ķ����л�
    private void C1(Animator animator)
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

    // ����S�Ķ����л�
    private void C2(Animator animator, bool isMoving)
    {   // ����Sʱ����Ҫ���Ķ����ĺ���
        // ��ȡ��ǰ״̬���㼶0�е�״̬��Ϣ
        Debug.Log("C2!!!");
        
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
        
        Debug.Log("���� S ��ʱ�������������������л���վ��");
        ifChangeAnimation = false;
    }

    // ����Ŀ�ĵصĶ����л�
    private void C3(Animator animator)
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

}
