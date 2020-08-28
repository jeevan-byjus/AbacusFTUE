using UnityEngine;
[CreateAssetMenu(fileName = "New Prompt", menuName = "Tutorial Prompt")]
public class TutorialPrompt : ScriptableObject
{
    public int answer;
    [TextArea(1,5)]
    public string questionPrompt;
    public AudioClip questionAudio;
    [TextArea(1,5)]
    public string explanationPrompt;
    public AudioClip explanationAudio;
    [TextArea(1,5)]
    public string hintPrompt;
    public AudioClip hintAudio;
}
