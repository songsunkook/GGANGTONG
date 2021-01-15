using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    public CameraManager theCamera;

    public string keySound;
    public string enterSound;
    public string cancelSound;
    public string openSound;
    public string beepSound;
    public string healthSound;

    public Text descriptionText; // 부연 설명
    public string[] tabDescription; // 탭 부연 설명

    public Transform tf; // 슬롯의 부모 객체

    public GameObject go; // 인벤토리 활성화 비활성화
    public GameObject[] selectedTabImages;
    public GameObject goOOC; // 선택지 활성화 비활성화
    public GameObject prefabFloatingText;


    AudioManager theAudio;
    PlayerManager thePlayer;
    OKCancel theOOC;

    InventorySlot[] slots;

    List<Item> inventoryItemList; // 플레이어가 소지한 아이템 리스트
    List<Item> inventoryTabList; // 선택한 탭에 따라 다르게 보여질 아이템 리스트

    DatabaseManager theDatabase;

    int selectedItem;
    int selectedTab;

    bool activated; // 인벤토리 활성화
    bool tabActivated; // 탭 활성화
    bool itemActivated; // 아이템 활성화
    bool stopKeyInput; // 소비할 때 질의 시간 동안 키입력 방지
    bool preventExec; // 중복실행 제한

    WaitForSeconds waitTime = new WaitForSeconds(0.01f);

    private void Start()
    {
        instance = this;
        theAudio = FindObjectOfType<AudioManager>();
        thePlayer = FindObjectOfType<PlayerManager>();
        theDatabase = FindObjectOfType<DatabaseManager>();
        theOOC = FindObjectOfType<OKCancel>();
        inventoryItemList = new List<Item>();
        inventoryTabList = new List<Item>();
        slots = tf.GetComponentsInChildren<InventorySlot>();
    }
    
    public void GetAnItem(int _itemID, int _count = 1)
    {
        for(int i = 0; i < theDatabase.itemList.Count; i++) // 데이터베이스 아이템 검색
        {
            if(_itemID == theDatabase.itemList[i].itemID) // 아이템 찾음
            {
                GameObject clone = Instantiate(prefabFloatingText,
                    Vector3.zero,
                    Quaternion.identity);
                clone.transform.SetParent(this.transform);
                clone.GetComponent<RectTransform>().anchoredPosition = theCamera.GetVector3PlayerperCanvas();

                StringBuilder sb = new StringBuilder();
                sb.Append(theDatabase.itemList[i].itemName);
                sb.Append(" ").Append(_count).Append("개 획득 +");
                clone.GetComponent<Text>().text = sb.ToString();

                if (theDatabase.itemList[i].itemType.Equals(Item.ItemType.Use))
                {
                    for (int j = 0; j < inventoryItemList.Count; j++) // 인벤토리에 같은 아이템 검색
                    {
                        if (inventoryItemList[j].itemID == _itemID) // 아이템 찾음
                        {
                            inventoryItemList[j].itemCount += _count; // 개수만 증가
                            return;
                        }
                    }
                }
                inventoryItemList.Add(theDatabase.itemList[i]);
                inventoryItemList[inventoryItemList.Count - 1].itemCount = _count;
                return;
            }
        }
        Debug.LogError("데이터베이스에 해당 ID값을 가진 아이템이 존재하지 않습니다.");
    }

    public void RemoveSlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].RemoveItem();
            slots[i].gameObject.SetActive(false);
        }
    }

    public void ShowTab()
    {
        RemoveSlot();
        SelectedTab();
    }
    public void SelectedTab()
    {
        StopAllCoroutines();
        Color color = selectedTabImages[selectedTab].GetComponent<Image>().color;
        color.a = 0f;
        for (int i = 0; i < selectedTabImages.Length; i++)
        {
            selectedTabImages[i].GetComponent<Image>().color = color;
        }
        descriptionText.text = tabDescription[selectedTab];
        StartCoroutine(SelectedTabEffectCoroutine());
    }
    IEnumerator SelectedTabEffectCoroutine()
    {
        while (tabActivated)
        {
            Color color = selectedTabImages[0].GetComponent<Image>().color;
            while (color.a < 0.5f)
            {
                color.a += 0.03f;
                selectedTabImages[selectedTab].GetComponent<Image>().color = color;
                yield return waitTime;
            }
            while (color.a > 0f)
            {
                color.a -= 0.03f;
                selectedTabImages[selectedTab].GetComponent<Image>().color = color;
                yield return waitTime;
            }

            yield return new WaitForSeconds(0.3f);
        }
    }

    public void ShowItem()
    {
        inventoryTabList.Clear();
        RemoveSlot();
        selectedItem = 0;

        switch (selectedTab)
        {
            case 0:
                for (int i = 0; i < inventoryItemList.Count; i++)
                {
                    if (Item.ItemType.Use == inventoryItemList[i].itemType)
                    {
                        inventoryTabList.Add(inventoryItemList[i]);
                    }
                }
                break;

            case 1:
                for (int i = 0; i < inventoryItemList.Count; i++)
                {
                    if (Item.ItemType.Equip == inventoryItemList[i].itemType)
                    {
                        inventoryTabList.Add(inventoryItemList[i]);
                    }
                }
                break;

            case 2:
                for (int i = 0; i < inventoryItemList.Count; i++)
                {
                    if (Item.ItemType.Quest == inventoryItemList[i].itemType)
                    {
                        inventoryTabList.Add(inventoryItemList[i]);
                    }
                }
                break;

            case 3:
                for (int i = 0; i < inventoryItemList.Count; i++)
                {
                    if (Item.ItemType.ETC == inventoryItemList[i].itemType)
                    {
                        inventoryTabList.Add(inventoryItemList[i]);
                    }
                }
                break;
        }

        for (int i = 0; i < inventoryTabList.Count; i++)
        {
            slots[i].gameObject.SetActive(true);
            slots[i].AddItem(inventoryTabList[i]);
        } // 인벤토리 탭 리스트 내용 슬롯에 추가.

        SelectedItem();
    }
    public void SelectedItem()
    {
        StopAllCoroutines();
        if (inventoryTabList.Count > 0)
        {
            Color color = slots[0].selectedItem.GetComponent<Image>().color;
            color.a = 0;
            for (int i = 0; i < inventoryTabList.Count; i++)
            {
                slots[i].selectedItem.GetComponent<Image>().color = color;
            }

            descriptionText.text = inventoryTabList[selectedItem].itemDescription;
            StartCoroutine(SelectedItemEffectCoroutine());
        }
        else
        {
            descriptionText.text = "해당 타입의 아이템을 소유하고 있지 않습니다.";
        }
    }
    IEnumerator SelectedItemEffectCoroutine()
    {
        while (itemActivated)
        {
            Color color = slots[0].selectedItem.GetComponent<Image>().color;
            while (color.a < 0.5f)
            {
                color.a += 0.03f;
                slots[selectedItem].selectedItem.GetComponent<Image>().color = color;
                yield return waitTime;
            }
            while (color.a > 0f)
            {
                color.a -= 0.03f;
                slots[selectedItem].selectedItem.GetComponent<Image>().color = color;
                yield return waitTime;
            }

            yield return new WaitForSeconds(0.3f);
        }
    }

    private void Update()
    {
        if (!stopKeyInput)
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                activated = !activated;

                if (activated)
                {
                    theAudio.Play(openSound);
                    thePlayer.StopMove();
                    thePlayer.canWalk = false;
                    go.SetActive(true);
                    selectedTab = 0;
                    tabActivated = true;
                    itemActivated = false;
                    ShowTab();
                }
                else
                {
                    theAudio.Play(cancelSound);
                    StopAllCoroutines();
                    go.SetActive(false);
                    tabActivated = false;
                    itemActivated = false;
                    thePlayer.canWalk = true;
                }
            }

            if (activated)
            {
                if (tabActivated)
                {
                    if (Input.GetKeyDown(KeyCode.RightArrow))
                    {
                        if (selectedTab < selectedTabImages.Length - 1)
                        {
                            selectedTab++;
                        }
                        else
                        {
                            selectedTab = 0;
                        }
                        theAudio.Play(keySound);
                        SelectedTab();
                    }
                    else if (Input.GetKeyDown(KeyCode.LeftArrow))
                    {
                        if (selectedTab > 0)
                        {
                            selectedTab--;
                        }
                        else
                        {
                            selectedTab = selectedTabImages.Length - 1;
                        }
                        theAudio.Play(keySound);
                        SelectedTab();
                    }
                    else if (Input.GetKeyDown(KeyCode.Z))
                    {
                        theAudio.Play(enterSound);
                        Color color = selectedTabImages[selectedTab].GetComponent<Image>().color;
                        color.a = 0.25f;
                        selectedTabImages[selectedTab].GetComponent<Image>().color = color;
                        itemActivated = true;
                        tabActivated = false;
                        preventExec = true;
                        ShowItem();
                    }
                } // 탭 키 처리
                else if (itemActivated)
                {
                    if(inventoryTabList.Count > 0)
                    {
                        if (Input.GetKeyDown(KeyCode.DownArrow))
                        {
                            if (selectedItem < inventoryTabList.Count - 2)
                            {
                                selectedItem += 2;
                            }
                            else
                            {
                                selectedItem %= 2;
                            }
                            theAudio.Play(keySound);
                            SelectedItem();
                        }
                        else if (Input.GetKeyDown(KeyCode.UpArrow))
                        {
                            if (selectedItem > 1)
                            {
                                selectedItem -= 2;
                            }
                            else
                            {
                                selectedItem = inventoryTabList.Count - 1
                                    - (selectedItem + (inventoryTabList.Count % 2 == 0 ? 1 : 0)) % 2;
                            }
                            theAudio.Play(keySound);
                            SelectedItem();
                        }
                        else if (Input.GetKeyDown(KeyCode.RightArrow))
                        {
                            if (selectedItem < inventoryTabList.Count - 1)
                            {
                                selectedItem++;
                            }
                            else
                            {
                                selectedItem = 0;
                            }
                            theAudio.Play(keySound);
                            SelectedItem();
                        }
                        else if (Input.GetKeyDown(KeyCode.LeftArrow))
                        {
                            if (selectedItem > 0)
                            {
                                selectedItem--;
                            }
                            else
                            {
                                selectedItem = inventoryTabList.Count - 1;
                            }
                            theAudio.Play(keySound);
                            SelectedItem();
                        }
                        else if (Input.GetKeyDown(KeyCode.Z) && !preventExec)
                        {
                            if (selectedTab == 0)
                            {
                                // 소모품
                                theAudio.Play(enterSound);
                                stopKeyInput = true;
                                StartCoroutine(OOCCoroutine());
                            }
                            else if (selectedTab == 1)
                            {
                                // 장비
                            }
                            else
                            {
                                theAudio.Play(beepSound);
                            }
                        }
                    }

                    if (Input.GetKeyDown(KeyCode.X))
                    {
                        theAudio.Play(cancelSound);
                        StopAllCoroutines();
                        itemActivated = false;
                        tabActivated = true;
                        ShowTab();
                    }
                } // 아이템 키 처리

                if (Input.GetKeyUp(KeyCode.Z)) // 탭에서 Z를 눌렀을 때 아이템에서 Z가 바로 눌리는 것을 방지.
                {
                    preventExec = false;
                }
            }
        }
    }

    IEnumerator OOCCoroutine()
    {
        goOOC.SetActive(true);
        theOOC.ShowTwoChoice("사용", "취소");
        yield return new WaitUntil(() => !theOOC.activated);

        if (theOOC.GetResult())
        {
            for(int i=0; i < inventoryItemList.Count; i++)
            {
                if(inventoryItemList[i].itemID == inventoryTabList[selectedItem].itemID)
                {
                    theDatabase.UseItem(inventoryItemList[i].itemID);

                    if (inventoryItemList[i].itemCount > 1)
                    {
                        inventoryItemList[i].itemCount--;
                    }
                    else
                    {
                        inventoryItemList.RemoveAt(i);
                    }

                    theAudio.Play(healthSound);
                    ShowItem();
                    break;
                }
            }
        }

        stopKeyInput = false;
        goOOC.SetActive(false);
    }

}
