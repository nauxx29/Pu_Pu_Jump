using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private GameObject _ghost;
    [SerializeField] private GameObject _tutorial;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _ghost.SetActive(true);
            UiManager.Instance.ToggleHighestScore(false);
            Destroy(_tutorial);
        }
    }
}
