using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private float rotationSpeed = 3f;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private KeyBinds _keyBinds;
    [SerializeField] private SaveHandler _saveHandler;
    Vector2 input;
    Vector3 moveDir;

    private void Awake()
    {
        _keyBinds = GameObject.Find("MenuHandler").GetComponent<KeyBinds>();
        _saveHandler = GameObject.Find("MenuHandler").GetComponent<SaveHandler>();
    }

    private void Start()
    {
        _keyBinds.LoadKeys();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !pausePanel.activeInHierarchy) //displays the pause menu on input
        {
            pausePanel.SetActive(true);
        }
        if (!pausePanel.activeInHierarchy) //allows movement if not paused
        {
            HandleMove();
        }
    }

    private void HandleMove() //moves the player and rotates using the KeyBinds the player has saved
    {
        Vector3 moveDir = Vector3.zero;
        input.y = Input.GetKey(KeyBinds.keys["Forward"]) ? 1 : input.y = Input.GetKey(KeyBinds.keys["Backward"]) ? -1 : 0;
        input.x = Input.GetKey(KeyBinds.keys["Right"]) ? 1 : input.x = Input.GetKey(KeyBinds.keys["Left"]) ? -1 : 0;

        moveDir = transform.TransformDirection(new Vector3(input.x, input.y, 0));
        moveDir *= speed;

        transform.localPosition += moveDir * Time.deltaTime;
        
        Vector3 rotation = new Vector3(0f, 0f, input.x);
        transform.Rotate((rotation * (rotationSpeed * Time.deltaTime)), Space.Self);
    }
}
