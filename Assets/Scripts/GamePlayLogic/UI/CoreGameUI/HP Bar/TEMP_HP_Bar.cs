using System.Collections;
using UnityEngine;

namespace GameplayLogic.UI.HPBar
{
    public class TEMP_HP_Bar : MonoBehaviour
    {
        [SerializeField] Transform fillSprite;
        [SerializeField] Color defaultBarColor;
        [SerializeField] float drainDuration;

        float _maxValue;

        Vector3 _originalScale;
        private SpriteRenderer _fillSpriteRenderer;

        //float targetValue;
        Coroutine _runningSmoothBar;

        public void Init(float max, Color color)
        {
            //fillImage.fillAmount = 1f; //just to make sure. later on this will be removed and it will just read the value
            _originalScale = fillSprite.localScale;
            fillSprite.localScale = new Vector3(1f, _originalScale.y, _originalScale.z);
            
            _fillSpriteRenderer = fillSprite.GetComponentInChildren<SpriteRenderer>();
            _fillSpriteRenderer.color = color;
            
            _maxValue = max;
        }
        public void Init(float max)
        {
            //fillImage.fillAmount = 1f; //just to make sure. later on this will be removed and it will just read the value
            _originalScale = fillSprite.localScale;
            fillSprite.localScale = new Vector3(1f, _originalScale.y, _originalScale.z);
            
            _fillSpriteRenderer = fillSprite.GetComponentInChildren<SpriteRenderer>();
            _fillSpriteRenderer.color = defaultBarColor;
            
            _maxValue = max;
        }

        public void SetBarValue(float value)
        {
            //fillImage.fillAmount = value/ _maxValue;
            fillSprite.localScale = new Vector3(value / _maxValue, _originalScale.y, _originalScale.z);
        }

        public void SetBarValueSmoothly(float value)
        {
            //targetValue = value;
            if (_runningSmoothBar != null)
            {
                StopCoroutine(_runningSmoothBar);
            }

            _runningSmoothBar = StartCoroutine(SmoothBar(drainDuration, value / _maxValue));
        }

        IEnumerator SmoothBar(float duration, float targetValue)
        {
            float t = 0f;
            float startValue = fillSprite.localScale.x;
            float currentValue = fillSprite.localScale.x;
            while (t <= duration)
            {
                currentValue = Mathf.Lerp(startValue, targetValue, t);
                t += Time.deltaTime / duration;
                SetBarValue(currentValue);
                yield return null;
            }
        }
    }
}