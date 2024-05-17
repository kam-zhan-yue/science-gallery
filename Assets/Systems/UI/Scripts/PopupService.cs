using System.Collections.Generic;
using Kuroneko.UIDelivery;
using Kuroneko.UtilityDelivery;
using UnityEngine;

[DefaultExecutionOrder(-50)]
public class PopupService : MonoBehaviour, IPopupService
{
    private readonly Dictionary<string, Popup> _popups = new();

    private void Awake()
    {
        ServiceLocator.Instance.Register<IPopupService>(this);
    }
    
    public T GetPopup<T>(string id = "") where T : Popup
    {
        string key = string.IsNullOrEmpty(id) ? typeof(T).Name : id;
        return (T)_popups[key];
    }

    public T ShowPopup<T>(string id = "") where T : Popup
    {
        T popup = GetPopup<T>(id);
        popup.ShowPopup();
        return popup;
    }

    public T HidePopup<T>(string id = "") where T : Popup
    {
        T popup = GetPopup<T>(id);
        popup.HidePopup();
        return popup;
    }

    public void Register<T>(T popup, string id = "") where T : Popup
    {
        _popups.Add(string.IsNullOrEmpty(id) ? typeof(T).Name : id, popup);
    }
}
