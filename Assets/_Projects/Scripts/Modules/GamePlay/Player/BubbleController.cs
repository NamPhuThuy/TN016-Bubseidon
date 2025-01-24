using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleController : MonoBehaviour
{
    [Header("Components")] 
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _transform;

    [Header("Animations")]
    [SerializeField] private AnimationClip _createAnimClip;
    [SerializeField] private AnimationClip _popAnimClip;


    public void Show()
    {
        StopAllCoroutines();
        _animator.Play(_createAnimClip.name); 
        gameObject.SetActive(true);
    }

    public IEnumerator Hide()
    {
        gameObject.SetActive(false);
        _animator.Play(_popAnimClip.name);
        yield return new WaitForSeconds(_popAnimClip.length);
    }
    
    
    #region MonoBehaviour Methods
    void Start()
    {
        //Retrieve Components
        _animator = GetComponent<Animator>();
        _transform = transform;
        
        //De-activate
        gameObject.SetActive(false);
    }

    #endregion
}
