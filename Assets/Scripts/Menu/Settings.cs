using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Menu
{
    public class Settings : MonoBehaviour
    {
        private TMP_Text soundsText;
        private TMP_Text musicText;

        private Slider soundsSlider;
        private Slider musicSlider;

        public void Awake()
        {
            var sounds = GameObject.Find("Sounds");
            var music = GameObject.Find("Music");

            soundsText = sounds.GetComponentInChildren<TMP_Text>();
            musicText = music.GetComponentInChildren<TMP_Text>();

            soundsSlider = sounds.GetComponent<Slider>();
            musicSlider = music.GetComponent<Slider>();

            soundsSlider.value = 70; // Replace with DB data
            musicSlider.value = 70;

            OnSoundsVolumeChanged(0);
            OnMusicVolumeChanged(0);
        }

        public void OnSoundsVolumeChanged(float dummy)
        {
            soundsText.text = $"Sounds volume: {soundsSlider.value}%";
        }

        public void OnMusicVolumeChanged(float dummy)
        {
            musicText.text = $"Music volume: {musicSlider.value}%";
        }

        public void BackToMainMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}