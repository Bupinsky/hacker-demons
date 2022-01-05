using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSpace : MonoBehaviour
{
    public GameObject player;
    public Material material;
    public Texture texture;
    public Texture texture2;
    bool onTrigger;
    bool triggered;

    void Start()
    {
        onTrigger = false;
        material.SetTexture("_MainTex", texture);
        triggered = false;
    }

    void Update()
    {
        if (Mathf.Abs(player.transform.position.x - this.transform.position.x) < 7 &&
            Mathf.Abs(player.transform.position.z - this.transform.position.z) < 7)
        {
            if (!onTrigger)
            {
                material.SetTexture("_MainTex", texture2);
                onTrigger = true;
            }
        } else if (onTrigger)
        {
            material.SetTexture("_MainTex", texture);
            onTrigger = false;
        }
        if (Input.GetKey(KeyCode.Space) && onTrigger && !triggered)
        {
            FindObjectOfType<DialogueTrigger>().StartDialogue();
            triggered = true;
        }
    }
}