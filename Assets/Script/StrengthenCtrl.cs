using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrengthenCtrl : MonoBehaviour
{
    public GameObject StrengthenChoose;
    public GameObject[] Equipment;
    public GameObject[] EquipmentShow;

    public AudioSource aud;
    public AudioClip Hit;

    private void Start()
    {
        aud.volume = 0.2f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (StrengthenChoose.transform.localPosition == Equipment[0].transform.localPosition)
            {
                StrengthenChoose.transform.localPosition = Equipment[1].transform.localPosition;
                EquipmentShow[0].SetActive(false);
                EquipmentShow[1].SetActive(true);
            }
            else if (StrengthenChoose.transform.localPosition == Equipment[1].transform.localPosition)
            {
                StrengthenChoose.transform.localPosition = Equipment[2].transform.localPosition;
                EquipmentShow[1].SetActive(false);
                EquipmentShow[2].SetActive(true);
            }
            else if (StrengthenChoose.transform.localPosition == Equipment[2].transform.localPosition)
            {
                StrengthenChoose.transform.localPosition = Equipment[3].transform.localPosition;
                EquipmentShow[2].SetActive(false);
                EquipmentShow[3].SetActive(true);
            }
            else if (StrengthenChoose.transform.localPosition == Equipment[3].transform.localPosition)
            {
                StrengthenChoose.transform.localPosition = Equipment[4].transform.localPosition;
                EquipmentShow[3].SetActive(false);
                EquipmentShow[4].SetActive(true);
            }
            else if (StrengthenChoose.transform.localPosition == Equipment[4].transform.localPosition)
            {
                StrengthenChoose.transform.localPosition = Equipment[0].transform.localPosition;
                EquipmentShow[4].SetActive(false);
                EquipmentShow[0].SetActive(true);
            }
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (StrengthenChoose.transform.localPosition == Equipment[0].transform.localPosition)
            {
                StrengthenChoose.transform.localPosition = Equipment[4].transform.localPosition;
                EquipmentShow[0].SetActive(false);
                EquipmentShow[4].SetActive(true);
            }
            else if (StrengthenChoose.transform.localPosition == Equipment[1].transform.localPosition)
            {
                StrengthenChoose.transform.localPosition = Equipment[0].transform.localPosition;
                EquipmentShow[1].SetActive(false);
                EquipmentShow[0].SetActive(true);
            }
            else if (StrengthenChoose.transform.localPosition == Equipment[2].transform.localPosition)
            {
                StrengthenChoose.transform.localPosition = Equipment[1].transform.localPosition;
                EquipmentShow[2].SetActive(false);
                EquipmentShow[1].SetActive(true);
            }
            else if (StrengthenChoose.transform.localPosition == Equipment[3].transform.localPosition)
            {
                StrengthenChoose.transform.localPosition = Equipment[2].transform.localPosition;
                EquipmentShow[3].SetActive(false);
                EquipmentShow[2].SetActive(true);
            }
            else if (StrengthenChoose.transform.localPosition == Equipment[4].transform.localPosition)
            {
                StrengthenChoose.transform.localPosition = Equipment[3].transform.localPosition;
                EquipmentShow[4].SetActive(false);
                EquipmentShow[3].SetActive(true);
            }
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            aud.PlayOneShot(Hit);
        }
    }
}
