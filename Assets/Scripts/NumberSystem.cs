using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberSystem : MonoBehaviour
{
    public string keySound;
    public string enterSound;
    public string cancelSound;
    public string correctSound;

    public GameObject superObject;
    public GameObject[] panel;
    public Text[] numberText;

    public Animator anim;

    public bool activated; // 대기 Until(() => )

    int count; // 배열의 크기
    int selectTextBox; // 선택된 자릿수 박스
    int result; // 플레이어가 도출해낸 값.
    int correctNumber; // 정답

    string tempNumber;

    bool keyInput; // 키처리 할성화
    bool correctFlag; // 정답 여부

    AudioManager theAudio;



    private void Start()
    {
        theAudio = FindObjectOfType<AudioManager>();
    }

    public void ShowNumber(int _correctNumber)
    {
        correctNumber = _correctNumber;
        activated = true;
        correctFlag = false;

        string temp = correctNumber.ToString();
        for(int i = 0; i < temp.Length; i++)
        {
            count = i;
            panel[i].SetActive(true);
            numberText[i].text = "0";
        }

        superObject.transform.position += new Vector3(-50, 0, 0) * (count + 1);
        selectTextBox = 0;
        result = 0;
        SetColor();
        anim.SetBool("Appear", true);
        keyInput = true;
    }

    public bool GetResult()
    {
        return correctFlag;
    }

    public void SetNumber(string _arrow)
    {
        int temp = int.Parse(numberText[selectTextBox].text);

        if(_arrow == "Down")
        {
            if(temp != 0)
            {
                temp--;
            }
            else
            {
                temp = 9;
            }
        }
        else if(_arrow == "Up")
        {
            if (temp != 9)
            {
                temp++;
            }
            else
            {
                temp = 0;
            }
        }
        numberText[selectTextBox].text = temp.ToString();
    }

    public void SetColor()
    {
        Color color = numberText[0].color;
        color.a = 0.3f;
        for(int i=0;i<=count; i++)
        {
            numberText[i].color = color;
        }
        color.a = 1f;
        numberText[selectTextBox].color = color;
    }

    private void Update()
    {
        if (keyInput)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                theAudio.Play(keySound);
                SetNumber("Down");
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                theAudio.Play(keySound);
                SetNumber("Up");
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                theAudio.Play(keySound);
                if (selectTextBox != 0)
                {
                    selectTextBox--;
                }
                else
                {
                    selectTextBox = count;
                }
                SetColor();
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                theAudio.Play(keySound);
                if (selectTextBox != count)
                {
                    selectTextBox++;
                }
                else
                {
                    selectTextBox = 0;
                }
                SetColor();
            }
            else if (Input.GetKeyDown(KeyCode.Z))
            {
                theAudio.Play(keySound);
                keyInput = false;
                StartCoroutine(OXCoroutine());
            }
            else if (Input.GetKeyDown(KeyCode.X))
            {
                theAudio.Play(keySound);
                keyInput = false;
                StartCoroutine(ExitCoroutine());
            }
        }
    }

    IEnumerator OXCoroutine()
    {
        Color color = numberText[0].color;
        color.a = 1f;

        for(int i = 0; i <= count; i++)
        {
            numberText[i].color = color;
            tempNumber += numberText[i].text;
        }

        yield return new WaitForSeconds(1f);

        result = int.Parse(tempNumber);

        if(result == correctNumber)
        {
            theAudio.Play(correctSound);
            correctFlag = true;
        }
        else
        {
            theAudio.Play(cancelSound);
            correctFlag = false;
        }

        StartCoroutine(ExitCoroutine());
    }

    IEnumerator ExitCoroutine()
    {
        result = 0;
        tempNumber = "";
        anim.SetBool("Appear", false);

        yield return new WaitForSeconds(0.15f);

        for(int i=0; i <= count; i++)
        {
            panel[i].SetActive(false);
        }
        superObject.transform.position -= new Vector3(-50, 0, 0) * (count + 1);

        activated = false;
    }

}
