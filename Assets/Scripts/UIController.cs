using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] NewQSkill nqskill;
    [SerializeField] NewWskill nwskill;
    [SerializeField] NewEskill neskill;
    [SerializeField] NewRskill nrskill;

    [SerializeField] RawImage img_qskill;
    [SerializeField] RawImage img_wskill;
    [SerializeField] RawImage img_eskill;
    [SerializeField] RawImage img_rskill;

    [SerializeField] TextMeshProUGUI qcd;
    [SerializeField] TextMeshProUGUI wcd;
    [SerializeField] TextMeshProUGUI ecd;
    [SerializeField] TextMeshProUGUI rcd;

    [SerializeField] TextMeshProUGUI qKey;
    [SerializeField] TextMeshProUGUI wKey;
    [SerializeField] TextMeshProUGUI eKey;
    [SerializeField] TextMeshProUGUI rKey;
    [SerializeField] TextMeshProUGUI aKey;
    [SerializeField] TextMeshProUGUI sKey;
    [SerializeField] TextMeshProUGUI yKey;
    [SerializeField] TextMeshProUGUI space;

    float qcdtime;
    float wcdtime;
    float ecdtime;
    float rcdtime;
    // Start is called before the first frame update
    void Start()
    {
        qcdtime = nqskill.CoolDownTime;
        wcdtime = nwskill.CoolDownTime;
        ecdtime = neskill.CoolDownTime;
        rcdtime = nrskill.CoolDownTime;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            qKey.color = Color.green;
        }
        if(Input.GetKeyUp(KeyCode.Q))
        {
            qKey.color = Color.white;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            wKey.color = Color.green;
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            wKey.color = Color.white;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            eKey.color = Color.green;
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            eKey.color = Color.white;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            rKey.color = Color.green;
        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            rKey.color = Color.white;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            aKey.color = Color.green;
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            aKey.color = Color.white;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            sKey.color = Color.green;
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            sKey.color = Color.white;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            space.color = Color.green;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            space.color = Color.white;
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            yKey.color = Color.green;
        }
        if (Input.GetKeyUp(KeyCode.Y))
        {
            yKey.color = Color.white;
        }

        ShowCd();
    }


    public void QskillColorGray()
    {
        StartCoroutine(SkillCoolDown(img_qskill, qcdtime));
    }

    public void WskillColorGray()
    {
        StartCoroutine(SkillCoolDown(img_wskill, wcdtime));
    }

    public void EskillColorGray()
    {
        StartCoroutine(SkillCoolDown(img_eskill, ecdtime));
    }

    public void RskillColorGray()
    {
        StartCoroutine(SkillCoolDown(img_rskill, rcdtime));
    }


    IEnumerator SkillCoolDown(RawImage skillImage, float skillCDTime)
    {
        skillImage.color = Color.gray;
        yield return new WaitForSeconds(skillCDTime);
        skillImage.color = Color.white;

    }

    void ShowCd()
    {
        if(nqskill.IfEnabled == false)
        {
            qcdtime -= Time.deltaTime;
            qcd.text = RoundValue(qcdtime);
        }
        else
        {
            qcdtime = nqskill.CoolDownTime;
            qcd.text = "";
        }

        if (nwskill.IfEnabled == false)
        {
            wcdtime -= Time.deltaTime;
            wcd.text = RoundValue(wcdtime); 
        }
        else
        {
            wcdtime = nwskill.CoolDownTime;
            wcd.text = "";
        }

        if (neskill.IfEnabled == false)
        {
            ecdtime -= Time.deltaTime;
            ecd.text = RoundValue(ecdtime);
        }
        else
        {
            ecdtime = neskill.CoolDownTime;
            ecd.text = "";
        }

        if (nrskill.IfEnabled == false)
        {
            rcdtime -= Time.deltaTime;
            rcd.text = RoundValue(rcdtime);
        }
        else
        {
            rcdtime = nrskill.CoolDownTime;
            rcd.text = "";
        }
    }

    string RoundValue(float value)
    {
        return value.ToString("F1");
    }
}
