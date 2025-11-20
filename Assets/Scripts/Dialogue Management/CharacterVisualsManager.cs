using System.Collections.Generic;
using UnityEngine;

public class CharacterVisualsManager : MonoBehaviour
{
    public CharacterVisuals[] allCharacterVisuals;

    private Dictionary<string, CharacterVisuals> characterVisualsById;

    private void Awake()
    {
        characterVisualsById = new Dictionary<string, CharacterVisuals>();

        foreach (var visuals in allCharacterVisuals)
        {
            string id = visuals.characterId;

            if (!string.IsNullOrEmpty(id))
            {
                if (!characterVisualsById.ContainsKey(id))
                    characterVisualsById.Add(id, visuals);
                else
                    Debug.LogWarning($"Duplicate characterId '{id}' in CharacterVisualsManager.");
            }
            else
            {
                Debug.LogWarning($"Character '{visuals.gameObject.name}' has empty characterId.");
            }
        }
    }

    public CharacterVisuals GetVisualsById(string characterId)
    {
        if (characterVisualsById.TryGetValue(characterId, out var visuals))
            return visuals;

        Debug.LogWarning($"No CharacterVisuals found for '{characterId}'.");
        return null;
    }

    public Transform GetCameraTargetById(string characterId)
    {
        if (characterVisualsById.TryGetValue(characterId, out var visuals))
            return visuals.cameraTarget;

        Debug.LogWarning($"No camera target for characterId '{characterId}'.");
        return null;
    }
}
