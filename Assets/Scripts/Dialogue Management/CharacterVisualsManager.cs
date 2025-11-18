using System.Collections.Generic;
using UnityEngine;

public class CharacterVisualsManager : MonoBehaviour
{
    // Assign all CharacterVisuals in the scene here in inspector
    public CharacterVisuals[] allCharacterVisuals;

    // Map from characterId to CharacterVisuals component
    private Dictionary<string, CharacterVisuals> characterVisualsById;

    private void Awake()
    {
        characterVisualsById = new Dictionary<string, CharacterVisuals>();

        foreach (var visuals in allCharacterVisuals)
        {
            // Assume CharacterVisuals GameObject name matches characterId
            // OR set a public string characterId on CharacterVisuals component
            string id = visuals.characterId; // see Step 3

            if (!string.IsNullOrEmpty(id))
            {
                if (!characterVisualsById.ContainsKey(id))
                {
                    characterVisualsById.Add(id, visuals);
                }
                else
                {
                    Debug.LogWarning($"Duplicate characterId '{id}' found in CharacterVisualsManager.");
                }
            }
            else
            {
                Debug.LogWarning($"CharacterVisuals on GameObject '{visuals.gameObject.name}' has empty characterId.");
            }
        }
    }

    public CharacterVisuals GetVisualsById(string characterId)
    {
        if (characterVisualsById.TryGetValue(characterId, out var visuals))
        {
            return visuals;
        }
        Debug.LogWarning($"No CharacterVisuals found for characterId '{characterId}'.");
        return null;
    }
}
