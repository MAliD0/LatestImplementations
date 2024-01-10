using DS.Graph;
using DS.Nodes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class DSManager : MonoBehaviour
{
    public static DSManager instance;

    public DialogueGraph dialogue;
    public DSUIManager manager;
    public CharacterDataBase characterDataBase;
    public AudioSource audioSource;

    public string currentText;
    private string textToUI;
    private int i;

    [SerializeField] int frequency;

    [SerializeField] float currentTextSpeed;
    [SerializeField] float idleTextSpeed;
    [SerializeField] float punctuationTextSpeed;
    [SerializeField] float skipTextSpeedValue;

    public bool dsStarted = false;
    private bool canContinue = true;
    private bool isDialogueSkip;

    public Character currentSpeaker;
    public string n;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        StartDialogue(dialogue);
    }

    private void Update()
    {
        if (dialogue == null || !dsStarted) return;

        if (Input.GetKeyDown(KeyCode.Escape) && canContinue)
        {
            isDialogueSkip = false;
            ContinueDialogue();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            isDialogueSkip = true;
        }
    }

    private void ConnectDialogue(DialogueGraph graph)
    {
        currentTextSpeed = idleTextSpeed;
        this.dialogue = graph;
        
        dialogue.textData += SetCurrentText;
        dialogue.dialogueEnd += EndDialogue;
        dialogue.choiceNode += CreateChoices;
        manager.onChoiceButton += OnButtonChoisen;
    }

    public void StartDialogue(DialogueGraph dialogue)
    {
        if (!dsStarted)
        {
            ConnectDialogue(dialogue);
            dsStarted = true;
            dialogue.StartDialogue();
        }
    }

    public void StartDialogue(string text)
    {
        if (!dsStarted)
        {
            dsStarted = true;
            SetCurrentText(text);
        }
    }

    private void ResetData()
    {
        dsStarted = false;
        canContinue = true;
        dialogue = null;
        currentText = string.Empty;
        currentTextSpeed = idleTextSpeed;
    }

    public void EndDialogue()
    {
        manager.StopDialogue();

        ResetData();
    }

    public void ContinueDialogue()
    {
        dialogue.ContinueDialogue();
    }

    public void OnButtonChoisen(int buttonIndex)
    {
        dialogue.ContinueDialogue("answers " + buttonIndex);
        manager.DeleteButtons();
    }

    private IEnumerator CreateChoicesTimer()
    {
        yield return new WaitUntil(() => canContinue == true);
        List<string> choices = new List<string>();
        DSChoiceNode dSChoiceNode = (dialogue.current as DSChoiceNode);
        foreach (Answer answer in dSChoiceNode.answers)
        {
            choices.Add(answer.answer);
        }
        canContinue = false;
        manager.CreateButtons(choices);
    }
    public void CreateChoices()
    {
        StartCoroutine("CreateChoicesTimer");
    }

    public void SetCurrentText(string text)
    {
        currentText = text;

        canContinue = false;
        currentTextSpeed = idleTextSpeed;

        SetCharacter();
        manager.SetCurrentText("", n);
        manager.OpenDialogueBox(n);

        StartCoroutine("TextSpeech");
    }

    public void SetCharacter()
    {
        if(dialogue != null)
        {
            currentSpeaker = characterDataBase.FindCharacterByName(dialogue.GetCurrentSpeaker()[0]);
        }
        else
        {
            currentSpeaker = characterDataBase.characters[0];
        }
        if (currentSpeaker.name == "Narrator")
        {
            manager.SetCurrentCharacter(currentSpeaker, "Idle");
            n = "n";
        }
        else
        {
            manager.SetCurrentCharacter(currentSpeaker, dialogue.GetCurrentSpeaker()[1]);
            n = "s";
        }
    }

    private IEnumerator TextSpeech()
    {
        i = 0;
        textToUI = "";
        while (i < currentText.Length)
        {
            textToUI += currentText[i];
            PlaySound(currentText[i]);
            manager.SetCurrentText(textToUI, n);
            yield return new WaitForSeconds(currentTextSpeed);
            i++;
        }
        canContinue = true;
    }

    private void PlaySound(char sign)
    {

        if (char.IsPunctuation(sign))
        {
            audioSource.PlayOneShot(currentSpeaker.punctuationVoice);
            if ("!?.,".Contains(sign)){
                ChangeTextSpeed(punctuationTextSpeed);
            }
        }
        else if (char.IsLetter(sign))
        {
            if (i % frequency == 0)
            {
                audioSource.PlayOneShot(currentSpeaker.vowelVoice[0]);
                ChangeTextSpeed(idleTextSpeed);
            }
        }
        else
        {
            ChangeTextSpeed(idleTextSpeed);
        }
    }

    private void ChangeTextSpeed(float speed)
    {
        currentTextSpeed = isDialogueSkip? speed / skipTextSpeedValue:speed;
    }
}
