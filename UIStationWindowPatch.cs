using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace TowerFactory
{
    /// <summary>
    /// 在运输塔窗口右上角加一个"自动填原料"按钮。
    /// </summary>
    [HarmonyPatch(typeof(UIStationWindow))]
    public static class UIStationWindowPatch
    {
        private static Button _autoFillButton;
        private static Text _autoFillText;
        private static AutoFillButtonHost _buttonHost;

        [HarmonyPostfix]
        [HarmonyPatch("_OnOpen")]
        [YummiHarness.Guard("12e97c0fdc1081fd")]
        public static void OnOpen_Postfix(UIStationWindow __instance)
        {
            EnsureButton(__instance);
        }

        [HarmonyPostfix]
        [HarmonyPatch("OnStationIdChange")]
        [YummiHarness.Guard("a94c837d579532f7")]
        public static void OnStationIdChange_Postfix(UIStationWindow __instance)
        {
            EnsureButton(__instance);
            if (_autoFillButton == null)
            {
                return;
            }
            StationComponent station = GetStation(__instance);
            bool visible = __instance.active && station != null && !station.isCollector && !station.isVeinCollector;
            _autoFillButton.gameObject.SetActive(visible);
            if (visible)
            {
                UpdateButtonText();
            }
        }

        private static void UpdateButtonText()
        {
            string label = StationProductionPatch.Tr("塔厂：填原料", "Fill Ingredients");
            if (_autoFillText.text == label)
            {
                return;
            }
            _autoFillText.text = label;
            RectTransform btnRect = (RectTransform)_autoFillButton.transform;
            btnRect.sizeDelta = new Vector2(Mathf.Max(100f, _autoFillText.preferredWidth + 16f), btnRect.sizeDelta.y);
        }

        private static StationComponent GetStation(UIStationWindow window)
        {
            PlanetTransport transport = window.transport ?? window.factory?.transport;
            int stationId = window.stationId;
            if (transport == null || stationId <= 0 || stationId >= transport.stationCursor)
            {
                return null;
            }
            StationComponent station = transport.stationPool[stationId];
            return station != null && station.id == stationId ? station : null;
        }

        private static void EnsureButton(UIStationWindow window)
        {
            if (window == null || window.windowTrans == null)
            {
                return;
            }
            if (_autoFillButton == null)
            {
                CreateButton(window);
            }
            if (_autoFillButton == null)
            {
                return;
            }
            PlaceInTabRow(window, (RectTransform)_autoFillButton.transform);
            _autoFillButton.transform.SetAsLastSibling();
            _buttonHost = _autoFillButton.GetComponent<AutoFillButtonHost>();
            if (_buttonHost == null)
            {
                _buttonHost = _autoFillButton.gameObject.AddComponent<AutoFillButtonHost>();
            }
            _buttonHost.window = window;
        }

        /// <summary>
        /// 放在分割线右下：与「物流站设置 / 总控面板」标签同一行、靠窗口右边。
        /// 标签行随窗口高度移动，所以挂到标签所在的父节点，并沿用它的纵向锚点。
        /// </summary>
        private static void PlaceInTabRow(UIStationWindow window, RectTransform btnRect)
        {
            RectTransform tab = window.controlPanelBtnRt;
            RectTransform parent = tab != null ? tab.parent as RectTransform : null;
            if (parent == null)
            {
                parent = window.windowTrans;
                if (btnRect.parent != parent)
                {
                    btnRect.SetParent(parent, false);
                }
                btnRect.anchorMin = new Vector2(1f, 1f);
                btnRect.anchorMax = new Vector2(1f, 1f);
                btnRect.pivot = new Vector2(1f, 1f);
                btnRect.anchoredPosition = new Vector2(-20f, -36f);
                return;
            }
            if (btnRect.parent != parent)
            {
                btnRect.SetParent(parent, false);
            }
            btnRect.anchorMin = new Vector2(1f, tab.anchorMin.y);
            btnRect.anchorMax = new Vector2(1f, tab.anchorMax.y);
            btnRect.pivot = new Vector2(1f, tab.pivot.y);
            float tabHeight = tab.rect.height > 1f ? tab.rect.height : 22f;
            float height = btnRect.sizeDelta.y;
            float centerShift = (tabHeight - height) * (0.5f - tab.pivot.y);
            Vector3 rightLocal = parent.InverseTransformPoint(window.windowTrans.TransformPoint(new Vector3(window.windowTrans.rect.xMax, 0f, 0f)));
            btnRect.anchoredPosition = new Vector2(rightLocal.x - 20f - parent.rect.xMax, tab.anchoredPosition.y + centerShift);
        }

        private static void CreateButton(UIStationWindow window)
        {
            GameObject btnObj = new GameObject("tower-factory-autofill");
            btnObj.transform.SetParent(window.windowTrans, false);
            RectTransform btnRect = btnObj.AddComponent<RectTransform>();
            btnRect.anchorMin = new Vector2(1f, 1f);
            btnRect.anchorMax = new Vector2(1f, 1f);
            btnRect.pivot = new Vector2(1f, 1f);
            btnRect.anchoredPosition = new Vector2(-20f, -36f);
            btnRect.sizeDelta = new Vector2(100f, 22f);

            Image btnImg = btnObj.AddComponent<Image>();
            btnImg.color = new Color(0.24f, 0.55f, 0.65f, 0.9f);
            btnImg.raycastTarget = true;

            _autoFillButton = btnObj.AddComponent<Button>();
            var colors = _autoFillButton.colors;
            colors.highlightedColor = new Color(1.2f, 1.2f, 1.2f, 1f);
            colors.pressedColor = new Color(0.7f, 0.7f, 0.7f, 1f);
            _autoFillButton.colors = colors;
            _buttonHost = btnObj.AddComponent<AutoFillButtonHost>();
            _autoFillButton.onClick.AddListener(() => _buttonHost.OnClick());

            GameObject textObj = new GameObject("Text");
            textObj.transform.SetParent(btnObj.transform, false);
            Text text = textObj.AddComponent<Text>();
            _autoFillText = text;
            text.font = window.titleText != null ? window.titleText.font : Resources.GetBuiltinResource<Font>("Arial.ttf");
            text.fontSize = 14;
            text.alignment = TextAnchor.MiddleCenter;
            text.color = Color.white;
            text.raycastTarget = false;
            RectTransform textRect = textObj.GetComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = Vector2.zero;
            textRect.offsetMax = Vector2.zero;
        }

        private static void RefreshStationWindow(UIStationWindow window)
        {
            if (window == null)
            {
                return;
            }
            var method = AccessTools.Method(typeof(UIStationWindow), "OnStationIdChange");
            method?.Invoke(window, null);
        }

        internal static void OnAutoFillClick(UIStationWindow window)
        {
            if (window == null)
            {
                return;
            }
            PlanetTransport transport = window.transport ?? window.factory?.transport;
            StationComponent station = GetStation(window);
            if (transport == null || station == null)
            {
                UIRealtimeTip.Popup(StationProductionPatch.Tr("无法读取当前运输塔", "Can't read this station"), true, 0);
                return;
            }
            bool filled = StationProductionPatch.TryAutoFill(transport, station, out string message);
            if (string.IsNullOrEmpty(message))
            {
                message = StationProductionPatch.Tr("填原料失败", "Failed to fill ingredients");
            }
            UIRealtimeTip.Popup(message, !filled, 0);
            RefreshStationWindow(window);
        }

        private sealed class AutoFillButtonHost : MonoBehaviour
        {
            public UIStationWindow window;

            public void OnClick()
            {
                UIStationWindowPatch.OnAutoFillClick(window);
            }
        }
    }
}
