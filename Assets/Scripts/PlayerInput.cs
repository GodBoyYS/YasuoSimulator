using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class PlayerInput : MonoBehaviour
{
    public GameObject hero;
    public GameObject startPoint;
    public Camera camera;
    private Rigidbody rb;
    private Animator animator;

    private bool isMoving;
    Vector3 destPoint;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float moveSpeed;

    public Texture2D cursor1;
    public Texture2D cursor2;

    private bool inputEnabled;

   // public TextMeshProUGUI eSkillInfo;

    // 采用技能类 -- 普攻、舞蹈都看作可以释放的技能
    //[SerializeField] private Attack _attack;
    //[SerializeField] private QSkill _qSkill;
    //[SerializeField] private WSkill _wSkill;
    //[SerializeField] private ESkill _eSkill;
    //[SerializeField] private RSkill _rSkill;
    private Dance _dance;
    private bool _ifESkilling; // 是否在E技能途中？为了实现EQ连放
    private bool isSwordOut;

    private Queue<KeyCode> inputs;
    private bool ifAQWERed;

    public bool IfESkilling
    {
        get { return _ifESkilling;}
        set { _ifESkilling = value;}
    }

    public bool IfAQWERed
    {   // 我们需要在每个技能释放完成后，或者按下S之后，重置这个变量
        set { ifAQWERed = value;}
    }

    // Start is called before the first frame update
    void Start()
    {
        hero.transform.position = startPoint.transform.position;
        rb = hero.GetComponent<Rigidbody>();
        animator = hero.GetComponent<Animator>();

        isMoving = false;   // 游戏刚开始，角色没有移动
        Cursor.SetCursor(cursor1, Vector2.zero, CursorMode.Auto);

        inputEnabled = true;
        _dance = gameObject.AddComponent<Dance>();

        isSwordOut = false; // A Q W R都会让剑出鞘，还要检测什么时候剑回鞘

        ifAQWERed = false; // 最初，没有任何输入
        _ifESkilling = false;// 是否在E的期间，这个也很重要
    }

    // Update is called once per frame
    void Update()
    {
        if(inputEnabled == true)
        {
            if (Input.GetMouseButtonDown(1))
            {   // 现在，就可以根据四个技能和普攻的输入情况，来判断是否需要连续动画了
                Ray ray = camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    destPoint = hit.point;
                    isMoving = true;
                    //_attack.IfAttackContinuos = false;
                }
            }
            
            // 技能单独成类，技能 = 动画 + 音效
            // 输入时，启动技能动画 + 对应音效





            // 退出游戏模式
            if (Input.GetKeyDown(KeyCode.Escape))
            {
#if UNITY_EDITOR
                EditorApplication.ExitPlaymode();
#else
                Application.Quit();
#endif
            }
        }

        // 先不管EQ
        //if (inputEnabled == false && _ifESkilling && Input.GetKeyDown(KeyCode.Q))
        //{   // eq
        //    Debug.Log("EQ");
        //    _qSkill.Release(this.animator, this, this.hero);
        //    isSwordOut = true;
        //}

    }

    void FixedUpdate()
    {
        //animator.SetBool("RUNNING", ifAQWERed);// ***************
        if (isMoving || _ifESkilling)
        {
            Vector3 direction = (destPoint - rb.position).normalized;
            // 计算目标旋转
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            // 平滑旋转
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
            // 平滑移动
            Vector3 newPosition = Vector3.MoveTowards(rb.position, destPoint, moveSpeed * Time.fixedDeltaTime);
            rb.MovePosition(newPosition);

            // 检查物体是否已经到达目标点
            if (Vector3.Distance(rb.position, destPoint) < 0.1f)
            {
                animator.SetBool("RUNNING", false);
                isMoving = false; // 停止移动
                _ifESkilling = false;
                switch (isSwordOut)
                {
                    case true:
                        animator.Play("yasuo_idle_out");
                        break;
                    case false:
                        animator.Play("yasuo_idle_in_sheathed");
                        break;
                }
            }
        }
    }

    private void DestPointAfterESkill()
    {
        destPoint = hero.transform.forward * 300;
        destPoint += hero.transform.position;
    }

    private IEnumerator DisableInputForSeconds(float seconds)
    {
        inputEnabled = false;
        yield return new WaitForSeconds(seconds);
        inputEnabled = true;
    }

    private IEnumerator WaitForEndOfESkill(float eSkillAnimationLength)
    {
        _ifESkilling = true;
        yield return new WaitForSeconds(eSkillAnimationLength);
        _ifESkilling = false;
    }
}
