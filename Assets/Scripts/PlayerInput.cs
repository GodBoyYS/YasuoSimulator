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

    // ���ü����� -- �չ����赸�����������ͷŵļ���
    //[SerializeField] private Attack _attack;
    //[SerializeField] private QSkill _qSkill;
    //[SerializeField] private WSkill _wSkill;
    //[SerializeField] private ESkill _eSkill;
    //[SerializeField] private RSkill _rSkill;
    private Dance _dance;
    private bool _ifESkilling; // �Ƿ���E����;�У�Ϊ��ʵ��EQ����
    private bool isSwordOut;

    private Queue<KeyCode> inputs;
    private bool ifAQWERed;

    public bool IfESkilling
    {
        get { return _ifESkilling;}
        set { _ifESkilling = value;}
    }

    public bool IfAQWERed
    {   // ������Ҫ��ÿ�������ͷ���ɺ󣬻��߰���S֮�������������
        set { ifAQWERed = value;}
    }

    // Start is called before the first frame update
    void Start()
    {
        hero.transform.position = startPoint.transform.position;
        rb = hero.GetComponent<Rigidbody>();
        animator = hero.GetComponent<Animator>();

        isMoving = false;   // ��Ϸ�տ�ʼ����ɫû���ƶ�
        Cursor.SetCursor(cursor1, Vector2.zero, CursorMode.Auto);

        inputEnabled = true;
        _dance = gameObject.AddComponent<Dance>();

        isSwordOut = false; // A Q W R�����ý����ʣ���Ҫ���ʲôʱ�򽣻���

        ifAQWERed = false; // �����û���κ�����
        _ifESkilling = false;// �Ƿ���E���ڼ䣬���Ҳ����Ҫ
    }

    // Update is called once per frame
    void Update()
    {
        if(inputEnabled == true)
        {
            if (Input.GetMouseButtonDown(1))
            {   // ���ڣ��Ϳ��Ը����ĸ����ܺ��չ���������������ж��Ƿ���Ҫ����������
                Ray ray = camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    destPoint = hit.point;
                    isMoving = true;
                    //_attack.IfAttackContinuos = false;
                }
            }
            
            // ���ܵ������࣬���� = ���� + ��Ч
            // ����ʱ���������ܶ��� + ��Ӧ��Ч





            // �˳���Ϸģʽ
            if (Input.GetKeyDown(KeyCode.Escape))
            {
#if UNITY_EDITOR
                EditorApplication.ExitPlaymode();
#else
                Application.Quit();
#endif
            }
        }

        // �Ȳ���EQ
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
            // ����Ŀ����ת
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            // ƽ����ת
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
            // ƽ���ƶ�
            Vector3 newPosition = Vector3.MoveTowards(rb.position, destPoint, moveSpeed * Time.fixedDeltaTime);
            rb.MovePosition(newPosition);

            // ��������Ƿ��Ѿ�����Ŀ���
            if (Vector3.Distance(rb.position, destPoint) < 0.1f)
            {
                animator.SetBool("RUNNING", false);
                isMoving = false; // ֹͣ�ƶ�
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
