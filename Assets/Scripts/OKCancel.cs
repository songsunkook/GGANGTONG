using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class OKCancel : MonoBehaviour
{
    public string keySound;
    public string enterSound;
    public string cancelSound;

    public GameObject upPanel;
    public GameObject downPanel;

    public Text upText;
    public Text downText;

    AudioManager theAudio;

    public bool activated;
    bool keyInput;
    bool result;

    private void Start()
    {
        theAudio = FindObjectOfType<AudioManager>();
    }

    public void Selected()
    {
        theAudio.Play(keySound);
        result = !result;

        if (result)
        {
            upPanel.gameObject.SetActive(false);
            downPanel.gameObject.SetActive(true);
        }
        else
        {
            upPanel.gameObject.SetActive(true);
            downPanel.gameObject.SetActive(false);
        }
    }

    public void ShowTwoChoice(string _upText, string _downText)
    {
        activated = true;
        result = true;
        upText.text = _upText;
        downText.text = _downText;

        upPanel.gameObject.SetActive(false);
        downPanel.gameObject.SetActive(true);

        StartCoroutine(ShowTwoChoiceCoroutine());
    }

    public bool GetResult()
    {
        return result;
    }

    IEnumerator ShowTwoChoiceCoroutine()
    {
        yield return new WaitForSeconds(0.01f);
        keyInput = true;
    }


    private void Update()
    {
        if (keyInput)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                Selected();
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                Selected();
            }
            else if (Input.GetKeyDown(KeyCode.Z))
            {
                theAudio.Play(enterSound);
                keyInput = false;
                activated = false;
            }
            else if (Input.GetKeyDown(KeyCode.X))
            {
                theAudio.Play(cancelSound);
                keyInput = false;
                activated = false;
                result = false;
            }
        }
    }
}
