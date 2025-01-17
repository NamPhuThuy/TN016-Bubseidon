using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ScrollViewHelper : MonoBehaviour
{
    [Header("Data")] 
    [SerializeField] private TowerDatas _towerDatas;

    [Header("View")] 
    [SerializeField] private RectTransform _content;
    [SerializeField] private GameObject _contentElementPrefab;
    [SerializeField] private GridLayoutGroup _contentGridLayoutGroup;
    [SerializeField] private ScrollRect _scrollRect;

    private Vector2 _elementSpacing;
    private Vector2 _elementSize;
    private Vector2 _contentBuffer = new Vector2(0f, 30f); 
    
    void Start()
    {
        Setup();
        
        
    }

    private void OnEnable()
    {
        //Update content in scroll view
        UpdateScrollViewContent();
    }

    private void Setup()
    {
        //GET REFERENCES
        _contentGridLayoutGroup = _content.GetComponent<GridLayoutGroup>();
        _scrollRect = GetComponent<ScrollRect>();

        //GET THE SIZES
        _elementSize = _contentGridLayoutGroup.cellSize;
        _elementSpacing = _contentGridLayoutGroup.spacing;

        _elementSize = new Vector2(_contentGridLayoutGroup.cellSize.x * (_scrollRect.horizontal ? 1 : 0), _contentGridLayoutGroup.cellSize.y * (_scrollRect.vertical ? 1 : 0));
        
        Debug.Log($"element size: {_elementSize}");
        
        //Calculate the size of content-view
        _content.sizeDelta = _towerDatas.towerDatas.Count * _elementSize +
                             (_towerDatas.towerDatas.Count + 1) * _elementSpacing + 
                             _contentBuffer;
    }

    public void UpdateScrollViewContent()
    {
        //Resize the content-view-size
        
        
        
        //Update content
        foreach (TowerData p in _towerDatas.towerDatas)
        {
            GameObject element = Instantiate(_contentElementPrefab);
            TowerElement towerElement = element.GetComponent<TowerElement>();
            
            //Assign value 
            towerElement.Name = p.name;
            towerElement.Image = p.image;
            towerElement.Cost = p.cost;

            element.transform.parent = _content.transform;
            element.SetActive(true);
        }
        
    }
}
