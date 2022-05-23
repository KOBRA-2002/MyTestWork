using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FocusAttentional : MonoBehaviour
{
    [SerializeField] GameObject image;
    [SerializeField] Material mat1;
    [SerializeField] Material mat2;

    /// <summary>
    /// Убрать выделление с юнита
    /// </summary>
    public void Hide()
    {
        image.SetActive(false);
    }

    /// <summary>
    /// Выделить атакующего юнита
    /// </summary>
    public void ShowAttacking()
    {
        image.SetActive(true);
        image.GetComponent<Image>().color = mat1.color;
    }
    /// <summary>
    /// Выделить атакуемого юнита
    /// </summary>
    public void ShowAttacked()
    {
        image.SetActive(true);
        image.GetComponent<Image>().color = mat2.color;
    }
}
