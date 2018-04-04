using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

    private static bool created = false;

    public Text nameText;
    public Text dialogueText;
    public Image portrait;

    public Animator animator;

    public Queue<Sentence> sentences;

    public AudioManager am;

    AudioSource source1;

    #region Singleton

    public static DialogueManager Instance;

    #endregion

    private void Awake() {
        if (!created) {
            DontDestroyOnLoad(gameObject);
            created = true;
        }

        Instance = this;
    }

    // Use this for initialization
    void Start () {
        sentences = new Queue<Sentence>();

        am = AudioManager.Instance;
        AudioSource[] sources = GetComponents<AudioSource>();
        source1 = sources[0];
	}

    public void StartDialogue(Dialogue dialogue) {

        animator.SetBool("IsOpen", true);

        sentences.Clear();

        foreach (Sentence sentence in dialogue.sentences) {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void EndDialogue() {
        animator.SetBool("IsOpen", false);

        Debug.Log("Ended conversation.");
    }

    public void DisplayNextSentence() {

        if (sentences.Count == 0) {
            EndDialogue();
            return;
        }

        Sentence sentence = sentences.Dequeue();



        nameText.text = sentence.speaker.name;
        portrait.sprite = sentence.speaker.portrait;

        if (sentence.strEvent != null)
            sentence.strEvent.Invoke("");

        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));

        if (sentence.speaker.name == "Skip") {
            DisplayNextSentence();
            return;
        }

        // Debug.Log(sentence);
        // dialogueText.text = sentence;
    }

    float count = 0;
    float waitTime = 0.035f;

    IEnumerator TypeSentence (Sentence sentence) {
        dialogueText.text = "";

        char[] text = sentence.text.ToCharArray();
        string lang = sentence.speaker.language.ToString();

        int i = 0;
        // int j = 0;
        int end = text.Length;
        while (i < end) {
            if (count < waitTime)
                count += Time.deltaTime;

            if (count >= waitTime && i < end) {
                string typeLetter = text[i].ToString();
                dialogueText.text += typeLetter;

                count = 0f;
                string sayLetter = text[i++].ToString().ToUpper();
                AudioClip toPlay = am.GetSound(lang + "_" + sayLetter);
                if (toPlay)
                    source1.PlayOneShot(toPlay);
            }
            yield return null;
        }
    }

    public void SkipDialogue() {
        bool shouldSkip = true;

        StopAllCoroutines();
        while (sentences.Count > 0 && shouldSkip) {

            Sentence sentence = sentences.Dequeue();

            nameText.text = sentence.speaker.name;
            portrait.sprite = sentence.speaker.portrait;

            if (sentence.strEvent.GetPersistentEventCount() != 0) {
                Debug.Log(sentence.strEvent.GetPersistentTarget(0));
                sentence.strEvent.Invoke("");

                StopAllCoroutines();
                StartCoroutine(TypeSentence(sentence));
                if (sentence.speaker.name != "Skip")
                    shouldSkip = false;
            }
        }

        if (sentences.Count == 0 && shouldSkip) {
            EndDialogue();
            return;
        }
    }
}
