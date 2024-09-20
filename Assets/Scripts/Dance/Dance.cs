using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dance : MonoBehaviour
{
    List<string> danceAnimationNames;
    // Start is called before the first frame update
    void Start()
    {
        danceAnimationNames = new List<string>() {
            "yasuo_dance1",
            "yasuo_dance2",
            "yasuo_dance3",
        };
    }

    public void Release(Animator animator, int danceIndex)
    {
        switch (danceIndex)
        {
            case 0:
                animator.Play(danceAnimationNames[0]);
                break;
            case 1:
                animator.Play(danceAnimationNames[1]);
                break;
            case 2:
                animator.Play(danceAnimationNames[2]);
                break;
            default:
                Debug.Log("No such Dance selection");
                break;
        }
    }
}
