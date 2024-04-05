using UnityEngine;

public class LoadCharacter : MonoBehaviour
{
    public Material[] characterMaterials;
    public GameObject character;
    public SkinnedMeshRenderer characterRenderer;
    void Start()
    {
        int selectedCharacter = PlayerPrefs.GetInt("selectedCharacter");
        ChangeCharacterMaterials(selectedCharacter);
    }
    void ChangeCharacterMaterials(int selectedIndex)
    {
        if (characterRenderer != null && characterMaterials != null && selectedIndex >= 0 &&
            selectedIndex < characterMaterials.Length)
        {
            Material newMaterial = characterMaterials[selectedIndex];
            characterRenderer.material = newMaterial;
            CharacterSelection characterScript = GetComponent<CharacterSelection>();
            if (characterScript != null)
            {
                characterScript.materials = characterMaterials;
            }
        }
    }
}