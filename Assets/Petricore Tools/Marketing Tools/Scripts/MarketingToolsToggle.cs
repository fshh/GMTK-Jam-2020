using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

namespace Petricore.MarketingTool
{
    public class MarketingToolsToggle : MonoBehaviour
    {
        private static KeyCode TOGGLE_BUTTON_1 = KeyCode.LeftControl, TOGGLE_BUTTON_2 = KeyCode.LeftShift;

        private bool childrenEnabled = false;

        /// <summary>
        /// Run when you open or close the GUI, bool is true if turning them on, false if turning off
        /// </summary>
        public static Action<bool> togglingMarketingTools;

        private void Awake()
        {
            //Just in case they're still enabled, turn them off to start
            SetChildrenEnabled(false);
        }

        // Update is called once per frame
        void Update()
        {

            if (Input.GetKey(TOGGLE_BUTTON_1) && Input.GetKeyDown(TOGGLE_BUTTON_2))
            {
                childrenEnabled = !childrenEnabled;
                togglingMarketingTools?.Invoke(childrenEnabled);

                SetChildrenEnabled(childrenEnabled);
            }
        }

        private void SetChildrenEnabled(bool on)
        {
            foreach (Transform child in transform)
                child.gameObject.SetActive(on);
        }
    }
}