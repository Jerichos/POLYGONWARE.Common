using UnityEngine;

namespace POLYGONWARE.Common.Game
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class SpawnPoint2D : MonoBehaviour
    {
        [SerializeField] private bool _active;
        [SerializeField] private UnityEngine.Sprite _inactiveSprite;
        [SerializeField] private UnityEngine.Sprite _activeSprite;
        [SerializeField] private SpriteRenderer _renderer;

        public bool Active
        {
            get => _active;
            set
            {
                if(_active == value)
                    return;
                
                _active = value;

                if (_active)
                    _renderer.sprite = _activeSprite;
                else
                    _renderer.sprite = _inactiveSprite;
            }
        }
        
        public delegate void SpawnPointDelegate(SpawnPoint2D spawnPoint2D);
        public event SpawnPointDelegate ETriggered;
        public Vector3 Position => transform.position;

        private void Awake()
        {
            if (_active)
            {
                ETriggered?.Invoke(this);
            }
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            var player = col.gameObject.GetComponent<Controllable>();
            if(!player)
                return;
            
            // TODO: So any controller can get trigger this?
            if(player.Controller is not Controller)
                return;

            ETriggered?.Invoke(this);
        }

        private void Reset()
        {
            GetComponent<BoxCollider2D>().isTrigger = true;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_active)
            {
                var levelManager = FindObjectOfType<LevelManager>();
                levelManager.SetSpawnPoint(this);
            }
            else
            {
                _renderer.sprite = _inactiveSprite;
            }
        }
#endif
    }
}