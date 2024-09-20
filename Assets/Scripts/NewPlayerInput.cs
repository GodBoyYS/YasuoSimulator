using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;

public class NewPlayerInput : MonoBehaviour
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

    AnimationChanger animationChanger;

    // public TextMeshProUGUI eSkillInfo;

    // ���ü����� -- �չ����赸�����������ͷŵļ���
    //[SerializeField] private Attack _attack;
    [SerializeField] private NewQSkill _qSkill;
    [SerializeField] private NewWskill _wSkill;
    [SerializeField] private NewEskill _eSkill;
    [SerializeField] private NewRskill _rSkill;
    [SerializeField] private NewAttack _attack;
    private Dance _dance;
    private bool _ifESkilling; // �Ƿ���E����;�У�Ϊ��ʵ��EQ����

    [SerializeField] private UIController _uiController;

    public bool IfESkilling
    {
        get { return _ifESkilling; }
        set { _ifESkilling = value; }
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

        _ifESkilling = false;// �Ƿ���E���ڼ䣬���Ҳ����Ҫ

        animationChanger = new AnimationChanger();
        animationChanger.Init();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q) && _ifESkilling && inputEnabled == false)
        {
            Debug.Log("����EQ");
            //animator.SetBool("EQ", true);
            _ifESkilling = false;
            _uiController.QskillColorGray();
            _qSkill.Release(animator, 1);
            //StartCoroutine(SetEQtoFalse(1f));
        }

        if (inputEnabled == true)
        {
            if (Input.GetMouseButtonDown(1))
            {   
                // �ƶ������赲�κ����� ==�� �ƶ�;�п��Խ��������������룬no input forbidened
                Ray ray = camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    destPoint = hit.point;
                    isMoving = true;
                    animationChanger.ToMove(animator, moveSpeed);
                }
            }

            if(Input.GetKeyDown(KeyCode.S))
            {
                // ͬ����S��Ȼ����ֹͣ�����ǲ����赲��������
                StopMoving();
                
                animationChanger.MoveToIdle(0, animator, isMoving);
            }

            if(Input.GetKey(KeyCode.A) && _attack.IfEnabled) 
            {
                StopMoving();
                _attack.Release(animator);
            }

            if (Input.GetKeyDown(KeyCode.Q) && _qSkill.IfEnabled)
            {   // �ͷ�Q�����ڼ��ܵ�ʱ�䡢CD�ȵȣ�Ӧ���ü��ܱ������
                StopMoving();
                RotateDirectoinForSkills(Input.mousePosition);
                StartCoroutine(DisableInputForSeconds(_qSkill.SkillLength));
                _uiController.QskillColorGray();
                _qSkill.Release(animator, 0);
                // ��ʼ���������赲

            }

            if(Input.GetKeyDown(KeyCode.W) && _wSkill.IfEnabled)
            {
                StopMoving();
                RotateDirectoinForSkills(Input.mousePosition);
                StartCoroutine(DisableInputForSeconds(_wSkill.SkillLength));
                _uiController.WskillColorGray();
                _wSkill.Release(animator, this, hero);
            }

            if(Input.GetKeyDown(KeyCode.E) && _eSkill.IfEnabled)
            {
                // calculate the dest point after E skill
                StopMoving();
                //DestPointAfterESkill(Input.mousePosition);
                RotateAndMoveForESkill(Input.mousePosition);
                _ifESkilling = true;

                StartCoroutine(DisableInputForSeconds(_eSkill.SkillLength));
                _uiController.EskillColorGray();
                _eSkill.Release(animator);
            }

            if(Input.GetKeyDown(KeyCode.R) && _rSkill.IfEnabled)
            {
                StopMoving();
                RotateDirectoinForSkills(Input.mousePosition);
                StartCoroutine(DisableInputForSeconds(_rSkill.SkillLength));
                _uiController.RskillColorGray();
                _rSkill.Release(animator);
            }

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
    }

    void FixedUpdate()
    {
        if(_ifESkilling)
        {
            Vector3 newPosition = Vector3.MoveTowards(rb.position, destPoint, moveSpeed * Time.fixedDeltaTime);
            rb.MovePosition(newPosition); 
        }
        else if(isMoving)
        {
            RotateDirection(destPoint);
            // ƽ���ƶ�
            Vector3 newPosition = Vector3.MoveTowards(rb.position, destPoint, moveSpeed * Time.fixedDeltaTime);
            rb.MovePosition(newPosition);
        }

        // ��������Ƿ��Ѿ�����Ŀ���
        if (Vector3.Distance(rb.position, destPoint) < 0.1f)
        {
            animationChanger.MoveToIdle(1, animator, isMoving);
            _ifESkilling = false;
            isMoving = false;
        }
    }

    private void DestPointAfterESkill(Vector3 faceDirection)
    {
        RotateDirectoinForSkills(faceDirection);

        Vector3 moveDirection = (faceDirection - rb.position).normalized;
        destPoint = rb.position + moveDirection * 300;
    }

    private void StopMoving()
    {
        isMoving = false;
    }

    private void RotateDirection(Vector3 faceDirection)
    {
        Vector3 direction = (faceDirection - rb.position).normalized;
        // ����Ŀ����ת
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        // ƽ����ת
        rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
    }

    void RotateDirectoinForSkills(Vector3 faceDirection)
    {
        Ray ray = camera.ScreenPointToRay(faceDirection);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 direction = hit.point - rb.position;
            direction.y = 0;

            rb.rotation = Quaternion.LookRotation(direction); ;
        }
    }

    void RotateAndMoveForESkill(Vector3 faceDirection)
    {
        Ray ray = camera.ScreenPointToRay(faceDirection);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 direction = hit.point - rb.position;
            direction.y = 0;

            rb.rotation = Quaternion.LookRotation(direction);

            destPoint = rb.position + direction.normalized * 300;
        }
    }

    IEnumerator DisableInputForSeconds(float seconds)
    {
        inputEnabled = false;
        yield return new WaitForSeconds(seconds);
        inputEnabled = true;
    }

    IEnumerator SetEQtoFalse(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        animator.SetBool("EQ", false);
    }
}
