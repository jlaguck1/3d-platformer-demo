using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpTextManager : MonoBehaviour
{
    Text txt;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        txt = GetComponent<Text>();
        animator = GetComponent<Animator>();
    }

    // animation event
    void ControlText()
    {
        txt.text = "Hold LSHIFT to run (you'll jump farther!)";
        animator.Play("ControlTextAnimation");
    }
}
