using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Interactable
{
    [Header("Contents")]
    public Item m_contents;
    public Inventory m_playerInventory;
    public bool m_isOpen;
    public FloatValue m_storedOpenBoofloat; // Using a float here because bool serialization seems to be nitpicky (and thus buggy unless you figure it out).

    [Header("Signals and Dialog")]
    public SignalObj m_raiseItem;
    public GameObject m_dialogBox;
    public UnityEngine.UI.Text m_dialogText;

    [Header("Animation")]
    public Animator m_animator;

    // Start is called before the first frame update
    void Start()
    {
        m_animator = GetComponent<Animator>();
        m_isOpen = (m_storedOpenBoofloat.RuntimeValue > 0.5f);
        if(m_isOpen)
        {
            m_animator.Play("open", 0, 1.0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && m_playerInRange) // TODO: Make key assignable
        {
            if(!m_isOpen)
            {
                // Open the chest
                OpenChest();
            }
            else
            {
                // Chest is already open
                ChestAlreadyOpen();
            }
        }
    }

    public void OpenChest()
    {
        // Dialog window on
        m_dialogBox.SetActive(true);
        // dialog text = contents text
        m_dialogText.text = m_contents.m_itemDescription;
        // add contents to the inventory
        m_playerInventory.AddItem(m_contents);
        m_playerInventory.m_currentItem = m_contents;
        // Raise the signal to the player to animate
        m_raiseItem.Raise();
        // raise the context clue
        m_contextOff.Raise();
        // set the chest to opened
        m_isOpen = true;
        m_storedOpenBoofloat.RuntimeValue = 1.0f;   // 1.0f means "Open"/true  (and 0.0f means "Closed"/false)
        m_animator.SetBool("opened", true);
    }

    public void ChestAlreadyOpen()
    {
        // dialog off
        m_dialogBox.SetActive(false);
        // raise the signal to the player to stop animating
        m_raiseItem.Raise();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if(!m_isOpen)
        {
            base.OnTriggerEnter2D(collision);
        }
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        if(!m_isOpen)
        {
            base.OnTriggerExit2D(collision);
        }
    }
}
