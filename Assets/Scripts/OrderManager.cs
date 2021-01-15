using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    public static OrderManager instance;
    PlayerManager thePlayer; // 이벤트 도중 키 입력 방지.
    List<Player> characters;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        thePlayer = FindObjectOfType<PlayerManager>();
    }


    public void PreLoadCharacter()
    {
        characters = ToList();
    }

    public List<Player> ToList()
    {
        List<Player> tempList = new List<Player>();
        Player[] temp = FindObjectsOfType<Player>();

        for(int i = 0; i < temp.Length; i++)
        {
            tempList.Add(temp[i]);
        }

        return tempList;
    }

    public void Move(string _name, string _dir)
    {
        for(int i = 0; i < characters.Count; i++)
        {
            if(_name == characters[i].characterName)
            {
                characters[i].Move(_dir);
            }
        }
    }

    public void Turn(string _name, string _dir)
    {
        for (int i = 0; i < characters.Count; i++)
        {
            if (_name == characters[i].characterName)
            {
                switch (_dir)
                {
                    case "Up":
                        characters[i].animator.SetFloat("DirX", 0);
                        characters[i].animator.SetFloat("DirY", 1);
                        break;

                    case "Down":
                        characters[i].animator.SetFloat("DirX", 0);
                        characters[i].animator.SetFloat("DirY", -1);
                        break;

                    case "Left":
                        characters[i].animator.SetFloat("DirX", -1);
                        characters[i].animator.SetFloat("DirY", 0);
                        break;

                    case "Right":
                        characters[i].animator.SetFloat("DirX", 1);
                        characters[i].animator.SetFloat("DirY", 0);
                        break;
                }
            }
        }
    }

}
