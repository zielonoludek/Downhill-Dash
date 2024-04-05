using UnityEngine;

public class CharacterSelection : MonoBehaviour
{
    public Material[] materials;
    public int selectedCharacter;
    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
        selectedCharacter = Mathf.Clamp(selectedCharacter, 0, materials.Length - 1);
        UpdateMaterial();
    }
    void UpdateMaterial()
    {
        if (rend != null && materials != null && materials.Length > 0)
        {
            rend.sharedMaterial = materials[selectedCharacter];
        }
    }
    public void NextCharacter()
    {
        selectedCharacter = (selectedCharacter + 1) % materials.Length;
        UpdateMaterial();
    }
    public void PreviousCharacter()
    {
        selectedCharacter--;
        if (selectedCharacter < 0)
        {
            selectedCharacter = materials.Length - 1;
        }
        UpdateMaterial();
    }
    public void SaveCharacterState()
    {
        PlayerPrefs.SetInt("selectedCharacter", selectedCharacter);
    }
}