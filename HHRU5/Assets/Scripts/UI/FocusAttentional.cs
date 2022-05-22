using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FocusAttentional : MonoBehaviour
{
    [SerializeField] GameObject image;
    [SerializeField] Material mat1;
    [SerializeField] Material mat2;

    public void Hide()
    {
        image.SetActive(false);
    }

    public void ShowAttacking()
    {
        image.SetActive(true);
        image.GetComponent<Image>().color = mat1.color;
    }

    public void ShowAttacked()
    {
        image.SetActive(true);
        image.GetComponent<Image>().color = mat2.color;
    }
}
