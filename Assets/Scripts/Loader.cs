using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour
{
    [SerializeField] private List<Transform> _images;
    [SerializeField] private Animator _doors;
    [SerializeField] private AudioSource _source;
    [SerializeField] private AudioClip _open;
    [SerializeField] private AudioClip _close;

    private void Start()
    {
        StartCoroutine(ShowImage());
    }

    private IEnumerator ShowImage()
    {
        _doors.Play("Open");
        _source.PlayOneShot(_open);

        var r = Random.Range(0, _images.Count);
        _images[r].gameObject.SetActive(true);

        yield return new WaitForSeconds(3);

        _doors.Play("Close");
        _source.PlayOneShot(_close);

        yield return new WaitForSeconds(0.5f);

        SceneManager.LoadScene(2);
    }
}
