namespace Utage
{
    using System;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    [AddComponentMenu("Utage/ADV/Extra/LoadScene")]
    public class AdvLoadScene : MonoBehaviour
    {
        private void LoadScene(AdvCommandSendMessageByName command)
        {
            SceneManager.LoadScene(command.ParseCell<string>(AdvColumnName.Arg3));
        }
    }
}

