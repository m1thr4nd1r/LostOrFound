using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.UI;

public class PlayerBehavior : MonoBehaviour
{
    public float _lightDuration = 3f;
    public Color _fullLights = new Color(66, 40, 55);
    public Text _resultUI;
    public Image _messageUI;
    public ItemGeneration _generator;

    bool _firstLightActivation;
    Camera _camera;
    Light2D _light2D;
    Sprite _chosenSprite;
    AudioSource _fireSpell;

    public void CheckPlayerChoice(bool choice)
    {
        var response = _generator.IsSpriteOnArea(_chosenSprite);

        if (Debug.isDebugBuild)
            print("Player chose: " + choice);

        _resultUI.text = (choice == response) ? "Congratulations !!!" : "Sorry, wrong anwser...";

        var audioClip = (choice == response) ? "434612__jens-enk__completed" : null;
        var audioSource = _resultUI.GetComponent<AudioSource>();
        audioSource.clip = Resources.Load<AudioClip>(audioClip);
        _resultUI.gameObject.SetActive(true);
    }

#region Private Methods

    void Start()
    {
        _camera = Camera.main;
        _camera.backgroundColor = Color.black;

        _resultUI ??= GameObject.Find("Result").GetComponent<Text>();
        _resultUI.gameObject.SetActive(false);

        _messageUI ??= GameObject.Find("Message").GetComponent<Image>();
        _messageUI.gameObject.SetActive(false);

        _generator ??= GameObject.Find("ItemArea").GetComponent<ItemGeneration>();
        _chosenSprite = _generator.GetSprite();

        _firstLightActivation = true;
        _light2D = _camera.GetComponent<Light2D>();
        _fireSpell = GetComponent<AudioSource>();
        var spriteRenders = GetComponentsInChildren<SpriteRenderer>();
        spriteRenders[spriteRenders.Length - 1].sprite = _chosenSprite;
    }

    void Update()
    {
        if (Input.anyKeyDown && _firstLightActivation)
            StartCoroutine(ActivateLights());
    }

    IEnumerator ActivateLights()
    {
        _firstLightActivation = false;
        _fireSpell.Play();

        yield return new WaitForSeconds(0.2f);

        _light2D.intensity = 1;
        _camera.backgroundColor = _fullLights;
        yield return new WaitForSeconds(_lightDuration / 3 * 2);

        _light2D.intensity = .5f;
        _camera.backgroundColor = _fullLights * .5f;
        yield return new WaitForSeconds(_lightDuration / 3);

        _light2D.intensity = 0;
        _camera.backgroundColor = Color.black;
        _messageUI.gameObject.SetActive(true);
    }

#endregion
}
