using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowWithCurtains : MonoBehaviour
{
    [SerializeField] private SpriteRenderer curtainRenderer;
    [SerializeField] private Color hiddenColor = Color.gray; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Stickman player = other.GetComponent<Stickman>();
            if (player != null)
            {
                player.SetHidden(true);
                if (curtainRenderer != null)
                {
                    curtainRenderer.color = hiddenColor; // Indicate hiding visually
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("hello");
            Stickman player = other.GetComponent<Stickman>();
            if (player != null)
            {
                player.SetHidden(false);
                if (curtainRenderer != null)
                {
                    curtainRenderer.color = Color.white; // Reset curtain color
                }
            }
        }
    }
}
