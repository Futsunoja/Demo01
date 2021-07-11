using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST : MonoBehaviour
{
    public int max = 0;
    public GameObject[] cha;

    private void Start()
    {
        cha[1].transform.SetSiblingIndex(-1);    //移至第N層（-1最下層，0最上層）
        cha[1].transform.SetAsFirstSibling();    //移至最上層
        cha[1].transform.SetAsLastSibling();     //移至最下層
    }
}
