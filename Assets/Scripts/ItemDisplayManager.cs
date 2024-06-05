using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDisplayManager : MonoBehaviour
{
    public static ItemDisplayManager Instance { get; set; }
    
    private GameObject player;
    
    public GameObject itemsCanvas;
    
    public Camera[] camerasToDisable;
    
    [Header("Items")]
    public GameObject[] items;
    public bool[] itemsCollected;
    public int currentItemIndex;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        
    }
    void Start()
    {
        itemsCanvas.SetActive(false);
        
        player = GameObject.Find("Player");
        
        itemsCollected = new bool[items.Length];
        
        itemsCollected[0] = true;
        itemsCollected[1] = true;
        
        items[0].SetActive(true);
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) && !itemsCanvas.activeSelf)
        {
            itemsCanvas.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            
            player.gameObject.GetComponent<MouseMovement>().enabled = false;
            player.gameObject.GetComponent<PlayerMovementScript>().enabled = false;
            
            WeaponManager.Instance.DisableActiveWeapon();
            
            foreach(Camera c in camerasToDisable)
            {
                c.enabled = false;
            }
        }
        else if ((Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.Escape)) && itemsCanvas.activeSelf)
        {
            itemsCanvas.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            
            player.gameObject.GetComponent<MouseMovement>().enabled = true;
            player.gameObject.GetComponent<PlayerMovementScript>().enabled = true;
            
            WeaponManager.Instance.EnableActiveWeapon();
            
            foreach(Camera c in camerasToDisable)
            {
                c.enabled = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            NextItem();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            PreviousItem();
        }
    }

    private void NextItem()
    {
        do
        {
            currentItemIndex++;
            currentItemIndex %= items.Length;
        } while (itemsCollected[currentItemIndex] == false);

        
        for (int i = 0; i < items.Length; i++)
        {
            if (i == currentItemIndex && itemsCollected[i])
            {
                items[i].SetActive(true);
            }
            else
            {
                items[i].SetActive(false);
            }
        }
    }

    private void PreviousItem()
    {
        do
        {
            currentItemIndex--;
            if (currentItemIndex < 0)
            {
                currentItemIndex = items.Length - 1;
            }
        } while (itemsCollected[currentItemIndex] == false);
        
        for (int i = 0; i < items.Length; i++)
        {
            if (i == currentItemIndex && itemsCollected[i])
            {
                items[i].SetActive(true);
            }
            else
            {
                items[i].SetActive(false);
            }
        }
    }
    
    public void CollectItem(StoryItem item)
    {
        itemsCollected[item.itemIndex] = true;
    }
}
