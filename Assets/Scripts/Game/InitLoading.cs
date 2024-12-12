using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class InitLoading : MonoBehaviour
    {
        [Header("Variáveis de controle do loading")]
        public float timeWait;
        public string sceneName;

        #region Unity Methods
        void Start()
        {
            SceneManager.LoadScene("Loading", LoadSceneMode.Additive);
            StartCoroutine(WaitScene());
        }

        IEnumerator WaitScene()
        {
            yield return new WaitForSeconds(timeWait);
            SceneManager.LoadSceneAsync(sceneName);
        }
        #endregion
    }
}

