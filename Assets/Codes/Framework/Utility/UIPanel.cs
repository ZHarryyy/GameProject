using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace QFramework
{
    public class UIPanel : MonoBehaviour
    {
        private Dictionary<string, List<UIBehaviour>> mControls;

        protected virtual void Awake()
        {
            mControls = new Dictionary<string, List<UIBehaviour>>();

            FindChildrenControl<Button>((name, control) => control.onClick.AddListener(() => OnClick(name)));
            FindChildrenControl<Toggle>((name, control) => control.onValueChanged.AddListener(isSelect => onValueChanged(name, isSelect)));

            FindChildrenControl<Image>();
            FindChildrenControl<Text>();
            FindChildrenControl<RectMask2D>();
        }

        protected virtual void OnClick(string name)
        {

        }

        protected virtual void onValueChanged(string name, bool value)
        {

        }

        protected T GetControl<T>(string name) where T : UIBehaviour
        {
            if(mControls.TryGetValue(name, out var controls))
            {
                for(int i = 0; i < controls.Count; i++)
                {
                    if(controls[i] is T) return controls[i] as T;
                }
            }
            return null;
        }

        protected void FindChildrenControl<T>(Action<string, T> callback = null) where T : UIBehaviour
        {
            T[] controls = GetComponentsInChildren<T>();
            for(int i = 0; i < controls.Length; i++)
            {
                T control = controls[i];
                string name = control.gameObject.name;

                callback?.Invoke(name, control);

                if(mControls.ContainsKey(name))
                {
                    mControls[name].Add(control);
                }
                else
                {
                    mControls.Add(name, new List<UIBehaviour>() { control });
                }
            }
        }
    }
}
