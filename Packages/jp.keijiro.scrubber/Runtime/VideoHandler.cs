using UnityEngine;
using Klak.Hap;
using Klak.Math;

namespace Scrubber
{
    public sealed class VideoHandler : MonoBehaviour
    {
        [SerializeField] float _wheelSpeed = 1;
        [SerializeField] float _tweenSpeed = 5;

        HapPlayer _player;
        float _time;
        CdsTween _tween;

        public void Open(string name, bool autoPlay, bool loop)
        {
            _player = GetComponent<HapPlayer>();
            _player.Open(name + ".mov");
            _player.speed = autoPlay ? 1 : 0;
            _player.loop = loop;
        }

        void Update()
        {
            if (_player == null) return;

            var wheel = Input.mouseScrollDelta.y;

            if (Input.GetKeyDown(KeyCode.Space))
                _player.speed = 1 - _player.speed;

            if (wheel == 0 && _player.speed == 1)
            {
                _tween.Current = _time = _player.time;
                 return;
            }

            _time += wheel * _wheelSpeed;

            if (!_player.loop)
                _time = Mathf.Clamp(_time, 0, (float)_player.streamDuration);

            _tween.Speed = _tweenSpeed;
            _tween.Step(_time);

            _player.time = _tween.Current;
            _player.speed = 0;
        }
    }
}
