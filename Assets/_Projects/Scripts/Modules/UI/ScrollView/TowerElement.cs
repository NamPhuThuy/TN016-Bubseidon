using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class TowerElement : MonoBehaviour
{
    //DATA
    private string _name;
    private int _cost;
    private Sprite _image;

    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            _nameText.text = _name;
        }
    }

    public int Cost
    {
        get => _cost;
        set
        {
            _cost = value;
            _costText.text = _cost.ToString();
        }
    }

    public Sprite Image
    {
        get => _image;
        set
        {
            _image = value;
            _avatarUI.sprite = _image;
        }
    }

    //VIEW
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _costText;
    [SerializeField] private Image _avatarUI;
}