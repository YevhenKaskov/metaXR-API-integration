using System.IO;
using TMPro;
using UnityEngine;

public class SaveNameButton : MonoBehaviour
{
    [Header("UI")]
    public TMP_InputField nameInputField;

    [Header("File Settings")]
    public string fileName = "username.txt";

    public void SaveName()
    {
        string projectRootPath = Directory.GetParent(Application.dataPath).FullName;
        string filePath = Path.Combine(projectRootPath, fileName);

        string nameInput = nameInputField.text.Trim();
        string existingFileContents = "";

        if (File.Exists(filePath))
            existingFileContents = File.ReadAllText(filePath).Trim();

        if (string.IsNullOrEmpty(nameInput))
        {
            if (string.IsNullOrEmpty(existingFileContents))
                nameInput = "Unknown human";
            else
                nameInput = existingFileContents;
        }

        File.WriteAllText(filePath, nameInput);

        Debug.Log($"Name saved: '{nameInput}' at:\n{filePath}");
    }
}
