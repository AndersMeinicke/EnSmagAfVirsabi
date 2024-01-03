using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Animations;


/// <summary>
/// Visual scripting node that loads localized text/audio and sends it to a
/// EntryReceiver (abstract class), and waits for the dialogue entry duration
/// before continuing to the next node. Optionally, the node can specify a look at target and
/// enter/exit animation state to pass on the EntryReceiver.
/// </summary>
/// <remarks>Author: Alexander Larsen.</remarks>
[UnitCategory("Content Control")]
[UnitTitle("Content Node")]
public class ContentNode : WaitUnit
{
    [System.Flags]
    public enum ContentType
    {
        Text = 1,
        Audio = 2,
        Sprite = 4,
        Timeline = 8
    }

    [DoNotSerialize, PortLabel("Receiver")]
    public ValueInput receiverInput;

    [DoNotSerialize, PortLabel("Localization Key")]
    public ValueInput keyInput;

    [DoNotSerialize, PortLabel("Content Type")]
    public ValueInput contentTypesInput;

    [DoNotSerialize, PortLabel("Text Duration")]
    public ValueInput textDurationInput;

    [DoNotSerialize, PortLabel("Sprite Duration")]
    public ValueInput spriteDurationInput;


    [DoNotSerialize, PortLabel("Exit Time Delay")]
    public ValueInput exitDelayInput;

    [DoNotSerialize, PortLabel("Skip Key")]
    public ValueInput skipKey;




    protected override void Definition()
    {
        base.Definition();

        receiverInput = ValueInput<GameObject>(nameof(receiverInput), null);
        keyInput = ValueInput<string>(nameof(keyInput), string.Empty);
        contentTypesInput = ValueInput<ContentType>(nameof(contentTypesInput), ContentType.Text);
        exitDelayInput = ValueInput<float>(nameof(exitDelayInput), 1f);
        skipKey = ValueInput<KeyCode>(nameof(skipKey), KeyCode.Space);
        textDurationInput = ValueInput<float>(nameof(textDurationInput), 0f);
        spriteDurationInput = ValueInput<float>(nameof(spriteDurationInput), 0f);
    }

    protected override IEnumerator Await(Flow flow)
    {

        string localizationKey = flow.GetValue<string>(keyInput);
        string stringTableRef = Variables.Graph(flow.stack).Get<string>("StringTableName");
        string audioTableRef = Variables.Graph(flow.stack).Get<string>("AudioTableName");
        string spriteTableRef = Variables.Graph(flow.stack).Get<string>("SpriteTableName");
        string timelineTableRef = Variables.Graph(flow.stack).Get<string>("TimelineTableName");
        float exitDelay = flow.GetValue<float>(exitDelayInput);
        float textDuration = flow.GetValue<float>(textDurationInput);
        float spriteDuration = flow.GetValue<float>(spriteDurationInput);
        KeyCode skip = flow.GetValue<KeyCode>(skipKey);



        if (flow.GetValue<ContentType>(contentTypesInput).HasFlag(ContentType.Text))
        {
            ContentReceiver<string> receiver = flow.GetValue<ContentReceiver<string>>(receiverInput);

            yield return AwaitText(receiver, stringTableRef, localizationKey, false, textDuration, skip);

            receiver.HideContent(0f);
        }

        if (flow.GetValue<ContentType>(contentTypesInput).HasFlag(ContentType.Sprite))
        {
            ContentReceiver<Sprite> receiver = flow.GetValue<ContentReceiver<Sprite>>(receiverInput);

            Sprite sprite;

            if (localizationKey != "")
            {

                yield return sprite = LocalizationSettings.AssetDatabase.GetTable(spriteTableRef).GetAssetAsync<Sprite>(localizationKey).Result;

                yield return new WaitUntil(() => sprite != null);

                if (sprite == null)
                {
                    Debug.LogError($"Can not find sprite localization key: {localizationKey}");
                    yield break;
                }
            }
            else
            {
                sprite = null;
            }

            receiver.ReceiveContent(sprite);

            if (spriteDuration != 0)
            {
                receiver.HideContent(spriteDuration);
            }
        }

        if (flow.GetValue<ContentType>(contentTypesInput).HasFlag(ContentType.Audio))
        {
            ContentReceiver<AudioClip> receiver = flow.GetValue<ContentReceiver<AudioClip>>(receiverInput);

            yield return AwaitAudio(receiver, audioTableRef, localizationKey, skip);
        }

        if (flow.GetValue<ContentType>(contentTypesInput).HasFlag(ContentType.Timeline))
        {

        }


        if (exitDelay < 0) Debug.LogWarning($"A negative exit delay of {exitDelay} seconds will be treated as 0");
        yield return new WaitForSeconds(exitDelay);



        if (flow.GetValue<ContentType>(contentTypesInput).HasFlag(ContentType.Text))
        {
            // receiver.HideText();
        }


        yield return exit;
    }

    private IEnumerator AwaitText(ContentReceiver<string> dialogueReceiver, string stringTableRef, string localizationKey, bool continueImmediately, float textDuration, KeyCode skip)
    {

        StringTable stringTable = LocalizationSettings.StringDatabase.GetTable(stringTableRef);
        string localizedText;

        /* Get localized string entry if table exists and table contains key.
         * Else kill coroutine. */
        if (stringTable == null)
        {
            Debug.LogError($"Can not find string localization table: {stringTableRef}");
            yield break;
        }
        else if (!stringTable.SharedData.Contains(localizationKey))
        {
            Debug.LogError($"Can not find string localization table key: {localizationKey}");
            yield break;
        }
        else
        {
            localizedText = stringTable.GetEntry(localizationKey).GetLocalizedString();
        }

        dialogueReceiver.ReceiveContent(localizedText);


        if (!continueImmediately)
        {
            //Custom text display duration allows for quick debugging when in "Text only" mode
            if (textDuration < 0)
            {
                Debug.LogError($"Text display duration can't be a negative (key: {localizationKey}, actor: {dialogueReceiver.name}).");
                yield break;
            }
            else if (textDuration > 0) yield return CancelableWait(textDuration, skip);
        }


    }

    private IEnumerator AwaitAudio(ContentReceiver<AudioClip> dialogueReceiver, string audioTableRef, string localizationKey, KeyCode skip)
    {
        AssetTable audioTable = LocalizationSettings.AssetDatabase.GetTable(audioTableRef);
        AudioClip localizedAudioClip;

        if (audioTable == null)
        {
            Debug.LogError($"Can not find audio localization table {audioTableRef}");
            yield break;
        }
        else if (!audioTable.SharedData.Contains(localizationKey))
        {
            Debug.LogError($"Can not find audio localization table key {localizationKey}");
            yield break;
        }
        else
        {
            AsyncOperationHandle<AudioClip> audioAsyncOperation = audioTable.GetAssetAsync<AudioClip>(localizationKey);

            if (audioAsyncOperation.IsValid() && !audioAsyncOperation.IsDone)
                yield return audioAsyncOperation;

            localizedAudioClip = audioAsyncOperation.Result;
        }

        dialogueReceiver.ReceiveContent(localizedAudioClip);



        yield return CancelableWait(localizedAudioClip.length, skip);
    }

    private IEnumerator CancelableWait(float duration, KeyCode skip)
    {
        float timeElapsed = 0f;
        while (timeElapsed < duration)
        {
            if (Input.GetKeyDown(skip) || OVRInput.GetDown(OVRInput.Button.Two))
            {
                yield break;
            }
            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }

    // Calculates a standardized text display time based on the length of the text.
    private float GetTextDisplayTime(string text)
    {
        float wpm = 150f; // Comfortable readable words per minute (WPM)
        float averageWordLength = 5f; // Standardized number of chars in a word
        float delay = 1.5f; // Seconds before user starts reading the text
        return (text.Length / averageWordLength) / wpm * 60f + delay;
    }
}
