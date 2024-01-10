using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DSUIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textBox;
    [SerializeField] Image sprite;
    [SerializeField] GameObject dialogueBoxHolder;

    [SerializeField] GameObject narratorDialogueBoxHolder;
    [SerializeField] TextMeshProUGUI narratorTextBox;

    [SerializeField] GameObject ButtonsHolder;
    [SerializeField] GameObject buttonPrefab;
    [SerializeField] List<Button> buttons;

    public string currentText;
    public Character currentCharacter;

    public Action<int> onChoiceButton;
    private void Awake()
    {
        CloseDialogueBox("s");
        CloseDialogueBox("n");
    }

    public void SetCurrentCharacter(Character character, string mood)
    {
        currentCharacter = character;
        Sprite portrait;
        character.data.TryGetValue(mood,out portrait);

        if(currentCharacter == null || currentCharacter.name == "Narrator")
        {
            OpenDialogueBox("n");
            return;
        }
        OpenDialogueBox("s");
        sprite.sprite = portrait;
    }

    public void DisAttachCharacter()
    {
        currentCharacter = null;
        currentText = null;
    }

    public void StopDialogue()
    {
        CloseDialogueBox("s");
        CloseDialogueBox("n");
        DisAttachCharacter();
    }

    public void DeleteButtons()
    {
        foreach(Button button in buttons)
        {
            Destroy(button.gameObject);
        }
        buttons.Clear();
    }

    public void CreateButtons(List<string> options)
    {
        foreach(string option in options)
        {
            Button button = Instantiate(buttonPrefab, ButtonsHolder.transform).GetComponent<Button>();
            buttons.Add(button);
            button.GetComponentInChildren<TextMeshProUGUI>().text = option;
            Button param = button;

            button.onClick.AddListener(delegate { OnButtonTrigger(param); });
        }
    }

    public void OnButtonTrigger(Button button)
    {
        print("Trigger");
        foreach(Button option in buttons)
        {
            if (option == button)
            {
                onChoiceButton?.Invoke(buttons.FindIndex(x => x == button));
                break;
            }
            else continue;
        }
    }


    public void SetCurrentText(string text, string n)
    {
        currentText = text;

        if (n == "s")
        {
            textBox.text = currentText;
        }
        else
        {
            narratorTextBox.text = currentText;
        }
    }


    public void OpenDialogueBox(string n)
    {
        if(n == "s")
            dialogueBoxHolder.SetActive(true);
        else narratorDialogueBoxHolder.SetActive(true);
    }

    public void CloseDialogueBox(string n)
    {
        if(n == "s")
        {
            dialogueBoxHolder.SetActive(false);
            textBox.text = "";
            sprite.sprite = null;
        }
        else
        {
            narratorDialogueBoxHolder.SetActive(false);
            narratorTextBox.text = "";
        }
    }
}
