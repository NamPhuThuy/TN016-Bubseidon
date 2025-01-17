using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    [Header("Visual related")]
    [SerializeField] private Sprite _default;
    [SerializeField] private Sprite _pressed;
    
    private Image _image;
    
    [Space(10)]
    [Header("Scales")]
    [SerializeField] private float _pointerHoverScale = 1.2f;
    [SerializeField] private float _pointerReleaseScale = 1f;
    [SerializeField] private float _pointerClickScale = 0.8f;

    [Space(10)]
    [Header("Audio Clips")]
    
    private RectTransform _rectTransform;
    private float _changeY = 5.6f;
    
    
    private Vector3 _originalLocalScale;
    
    private void Awake() 
    {
        _image = GetComponent<Image>();
        _rectTransform = GetComponent<RectTransform>();
        
        _originalLocalScale = this.transform.localScale;
    }
    
    public void OnPointerUp(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(TweenScale(_originalLocalScale, 0.15f));
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(TweenScale(_originalLocalScale * _pointerClickScale, 0.15f));
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(TweenScale(_originalLocalScale * _pointerHoverScale, 0.2f));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(TweenScale(_originalLocalScale, 0.2f));
    }

    private IEnumerator TweenScale(Vector3 targetScale, float duration)
    {
        Vector3 startScale = transform.localScale;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            transform.localScale = Vector3.Lerp(startScale, targetScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale;
    }
    
    // detect and handle events when a pointer (e.g., mouse cursor or touch) moves over a UI element
    public void OnPointerMove(PointerEventData eventData)
    {
        // transform.localScale = localScaleOld * _pointerHoverScale;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }
}
