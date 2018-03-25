using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

    public Text nameText;
    public Text dialogueText;

    public Animator animator;

    public Queue<Sentence> sentences;

    public AudioManager am;

    AudioSource source1;

    #region Singleton

    public static DialogueManager Instance;

    #endregion

    private void Awake() {
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

        nameText.text = sentence.speaker;

        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));

        // Debug.Log(sentence);
        // dialogueText.text = sentence;
    }

    float count = 0;
    float waitTime = 0.035f;

    IEnumerator TypeSentence (Sentence sentence) {
        dialogueText.text = "";

        char[] text = sentence.text.ToCharArray();
        string lang = sentence.language.ToString();

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

}
