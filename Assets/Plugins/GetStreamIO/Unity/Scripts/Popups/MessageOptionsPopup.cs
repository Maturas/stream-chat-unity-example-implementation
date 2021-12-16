using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Plugins.GetStreamIO.Unity.Scripts.Popups
{
    /// <summary>
    /// Context menu for message
    /// </summary>
    public class MessageOptionsPopup : BasePopup<MessageOptionsPopup.Args>
    {
        public readonly struct Args : IPopupArgs
        {
            public bool HideOnPointerExit { get; }
            public bool HideOnButtonClicked { get; }
            public IReadOnlyList<MenuOptionEntry> Options => _options;

            public Args(bool hideOnPointerExit, bool hideOnButtonClicked, IEnumerable<MenuOptionEntry> options)
            {
                HideOnPointerExit = hideOnPointerExit;
                HideOnButtonClicked = hideOnButtonClicked;
                _options = options.ToList();
            }

            private readonly List<MenuOptionEntry> _options;
        }

        public bool IsPointerOver { get; private set; }

        protected override void OnShow(Args args)
        {
            base.OnShow(args);

            ClearAllButtons();

            foreach (var option in args.Options)
            {
                var instance = Instantiate(_buttonPrefab, _buttonsContainer);
                _buttons.Add(instance);

                instance.onClick.AddListener(() =>
                {
                    TryHide();
                    option.OnClick();
                });
                instance.GetComponentInChildren<TextMeshProUGUI>().text = option.Name;
            }

            IsPointerOver = true;
        }

        private readonly IList<Button> _buttons = new List<Button>();

        [SerializeField]
        private Transform _buttonsContainer;

        [SerializeField]
        private Button _buttonPrefab;

        private void ClearAllButtons()
        {
            foreach (var button in _buttons)
            {
                button.onClick.RemoveAllListeners();
                Destroy(button.gameObject);
            }

            _buttons.Clear();
        }

        private void TryHide()
        {
            if (SelfArgs.HideOnButtonClicked)
            {
                Hide();
            }
        }
    }
}