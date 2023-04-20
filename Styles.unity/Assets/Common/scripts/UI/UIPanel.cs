using System;
using UnityEngine;
using UnityEngine.UI;

namespace Styles.Common
{
public class UIPanel : MonoBehaviour, IUIPanel
{
    [SerializeField] private bool _showOnStart;
    [SerializeField] private Button _buttonExit;
    [SerializeField] protected GameObject _panel;
    
    protected void Start()
    {
        if (!_panel)
            _panel = gameObject;
        
        if(_showOnStart)
            Open();
        else
            Close();
    }

    public virtual void Open()
    {
        OnOpen();
    }

    public virtual  void Close()
    {
        OnClose();
    }

    public virtual void Toggle()
    {
        var open = !_panel.activeSelf;
        
        if(open)
            OnOpen();
        else
            OnClose();
    }
    
    protected virtual void OnOpen()
    {
        _panel.SetActive(true);
    }

    protected virtual void OnClose()
    {
        _panel.SetActive(false);
    }

    protected virtual void OnEnable()
    {
        Debug.Log("OnEnable");
        if(_buttonExit)
            _buttonExit.onClick.AddListener(Close);
    }

    protected virtual void OnDisable()
    {
        if(_buttonExit)
            _buttonExit.onClick.RemoveAllListeners();
    }
}
}