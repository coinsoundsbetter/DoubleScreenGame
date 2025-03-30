using Code.Client.HUD;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code {
    public class Entry_Menu : MonoBehaviour {
        [SerializeField] private GameObject uiMenu;

        private MenuCanvas menu;
        
        private void Start() {
            menu = uiMenu.GetComponent<MenuCanvas>();
            menu.local.onClick.AddListener(() => {
                BoostrapConfig.Use(new BoostrapConfig() {
                    IsStartServer = true,
                    IsStartClient = true,
                });
                SceneManager.LoadScene("Main", LoadSceneMode.Single);
            });
            menu.online.onClick.AddListener(() => {
                BoostrapConfig.Use(new BoostrapConfig() {
                    IsStartServer = false,
                    IsStartClient = true,
                });
                SceneManager.LoadScene("Main", LoadSceneMode.Single);
            });
            uiMenu.SetActive(true);
        }

        private void OnDestroy() {
            menu.local.onClick.RemoveAllListeners();
        }
    }
}
