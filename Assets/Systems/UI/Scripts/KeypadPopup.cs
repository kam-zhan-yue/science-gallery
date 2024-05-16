using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Kuroneko.UIDelivery;
using Kuroneko.UtilityDelivery;
using Sirenix.OdinInspector;
using SuperMaxim.Messaging;
using TMPro;
using Unity.Properties;
using UnityEngine;

public class KeypadPopup : Popup
{
    [SerializeField] private PlanetDatabase planetDatabase;
    [SerializeField] private TMP_Text success;
    [SerializeField] private TMP_Text error;
    [SerializeField] private TMP_Text display;
    
    [NonSerialized, ReadOnly, ShowInInspector] 
    private KeypadButtonPopupItem[] _buttons = Array.Empty<KeypadButtonPopupItem>();

    private bool _interactive = false;

    public Action onComplete;

    protected override void InitPopup()
    {
        ServiceLocator.Instance.Get<IPopupService>().Register(this);
        _buttons = GetComponentsInChildren<KeypadButtonPopupItem>();
        display.SetText(string.Empty);
    }

    private void Start()
    {
        for (int i = 0; i < _buttons.Length; ++i)
        {
            _buttons[i].onPressed += OnPressed;
        }
    }

    public override void ShowPopup()
    {
        base.ShowPopup();
        _interactive = true;
    }

    private void OnPressed(string value)
    {
        if(display.text.Length < 4)
            display.text += value;
    }

    public void Submit()
    {
        if (!_interactive) return;
        if (planetDatabase.ValidCode(display.text))
        {
            SubmitAsync(this.GetCancellationTokenOnDestroy()).Forget();
        }
        else
        {
            ErrorAsync(this.GetCancellationTokenOnDestroy()).Forget();
        }
    }

    private async UniTask SubmitAsync(CancellationToken token)
    {
        _interactive = false;
        success.gameObject.SetActiveFast(true);
        error.gameObject.SetActive(false);
        // Play some sound effect here
        await UniTask.WaitForSeconds(1f, cancellationToken: token);
        
        CodePayload codePayload = new CodePayload { code = display.text };
        Messenger.Default.Publish(codePayload);
        HidePopup();
        onComplete?.Invoke();
    }

    private async UniTask ErrorAsync(CancellationToken token)
    {
        _interactive = false;
        success.gameObject.SetActiveFast(false);
        error.gameObject.SetActive(true);
            
        // Optional: Stop any existing tweens on this RectTransform to avoid conflicts
        mainHolder.DOKill();

        // Shake the RectTransform
        mainHolder.DOShakePosition(
            duration: 0.5f,      // Duration of the shake
            strength: new Vector3(10f, 0f, 0f), // Shake strength along the x-axis
            vibrato: 10,         // Number of shakes
            randomness: 90,      // Randomness of the shake
            snapping: false,     // Snap to integer positions
            fadeOut: true        // Smoothly fade out the shake
        );
        await UniTask.WaitForSeconds(0.5f, cancellationToken: token);
        _interactive = true;
    }

    public void Delete()
    {
        if(display.text.Length > 0)
            display.SetText(display.text.Remove(display.text.Length - 1));
    }

    public override void CloseButtonClicked()
    {
        base.CloseButtonClicked();
        onComplete?.Invoke();
        HidePopup();
    }
}
