using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class LoaderController : MonoBehaviour
    {
        [SerializeField] private List<Transform> _images;
        [SerializeField] private Animator _doors;
        [SerializeField] private AudioSource _source;
        [SerializeField] private AudioClip _open;
        [SerializeField] private AudioClip _close;

        private void Start()
        {
            Show();
        }

        private async void Show()
        {
            _doors.Play("Open");
            _source.PlayOneShot(_open);

            var r = Random.Range(0, _images.Count);
            _images[r].gameObject.SetActive(true);

            await Task.Delay(3000);

            _doors.Play("Close");
            _source.PlayOneShot(_close);

            await Task.Delay(3000);

            SceneManager.LoadScene(2);
        }
    }
}
