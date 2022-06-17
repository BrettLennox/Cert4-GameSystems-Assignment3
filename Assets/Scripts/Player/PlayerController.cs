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

    private void Awake()
    {
        _keyBinds = GameObject.Find("MenuHandler").GetComponent<KeyBinds>();
        _saveHandler = GameObject.Find("MenuHandler").GetComponent<SaveHandler>();
    }

    private void Start()
    {
        _keyBinds.LoadKeys();
        //_saveHandler.LoadGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !pausePanel.activeInHierarchy)
        {
            pausePanel.SetActive(true);
        }
        if (!pausePanel.activeInHierarchy)
        {
            HandleMove();
        }
    }

    private void HandleMove()
    {
        Vector3 moveDir = Vector3.zero;
        moveDir.y += Input.GetAxisRaw("Vertical");
        moveDir.x += Input.GetAxisRaw("Horizontal");
        moveDir.Normalize();
        Vector3 move = moveDir * speed;

        Vector3 rotation = new Vector3(0f, 0f, -Input.GetAxisRaw("Horizontal"));
        transform.Rotate((rotation * (rotationSpeed * Time.deltaTime)), Space.Self);
        transform.localPosition += move * Time.deltaTime;
    }
}
