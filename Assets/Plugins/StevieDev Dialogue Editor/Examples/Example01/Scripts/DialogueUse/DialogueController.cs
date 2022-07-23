using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    [SerializeField] private GameObject _dialogueUI;
    [Header("Text")]
    [SerializeField] private Text _textName;
    [SerializeField] private Text _textBox;
    [Header("Image")]
    [SerializeField] private Image _leftImage;
    [SerializeField] private GameObject _leftImageGO;
    [SerializeField] private Image _rightImage;
    [SerializeField] private GameObject _rightImageGO;
    [Header("Buttons")]
    [SerializeField] private Button _button01;
    [SerializeField] private Text _buttonText01;
    [SerializeField] private Button _button02;
    [SerializeField] private Text _buttonText02;
    [SerializeField] private Button _button03;
    [SerializeField] private Text _buttonText03;
    [SerializeField] private Button _button04;
    [SerializeField] private Text _buttonText04;

    [SerializeField] private List<Button> _buttons = new List<Button>();
    [SerializeField] private List<Text> _buttonTexts = new List<Text>();

    private void Awake()
    {
        ShowDialogue(false);

        _buttons.Add(_button01);
        _buttons.Add(_button02);
        _buttons.Add(_button03);
        _buttons.Add(_button04);

        _buttonTexts.Add(_buttonText01);
        _buttonTexts.Add(_buttonText02);
        _buttonTexts.Add(_buttonText03);
        _buttonTexts.Add(_buttonText04);
    }

    public void ShowDialogue(bool show)
    {
        _dialogueUI.SetActive(show);
    }

    public void SetText(string name, string text)
    {
        _textName.text = name;
        _textBox.text = text;
    }

    public void SetImage(Sprite image, DialogueFaceImageType dialogueImageType)
    {
        _leftImageGO.SetActive(false);
        _rightImageGO.SetActive(false);

        if(image != null)
        {
            if(dialogueImageType == DialogueFaceImageType.Left)
            {
                _leftImage.sprite = image;
                _leftImageGO.SetActive(true);
            }
            else
            {
                _rightImage.sprite = image;
                _rightImageGO.SetActive(true);
            }
        }
    }

    public void SetButtons(List<string> buttonTexts, List<UnityAction> unityActions)
    {
        _buttons.ForEach(button => button.gameObject.SetActive(false));

        for (int i = 0; i < buttonTexts.Count; i++)
        {
            _buttonTexts[i].text = buttonTexts[i];
            _buttons[i].onClick = new Button.ButtonClickedEvent();
            _buttons[i].onClick.AddListener(unityActions[i]);
            _buttons[i].gameObject.SetActive(true);
        }        
    }
}
