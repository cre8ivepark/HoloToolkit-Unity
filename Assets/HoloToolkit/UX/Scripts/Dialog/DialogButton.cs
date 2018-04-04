﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using HoloToolkit.Unity.Buttons;
using HoloToolkit.UX.Dialog;
using UnityEngine;

namespace HoloToolkit.UX.Buttons
{
    /// <summary>
    /// Handling click event and dismiss dialog
    /// </summary>
    public class DialogButton : MonoBehaviour
    {
        private DialogShell _parentDialog;
        private Button buttonComponent;

        public DialogShell ParentDialog
        {
            get
            {
                return _parentDialog;
            }
            set
            {
                _parentDialog = value;
            }
        }

        public Dialog.Dialog.ButtonTypeEnum ButtonTypeEnum;

        private void OnEnable()
        {
            buttonComponent = GetComponent<Button>();
            buttonComponent.OnButtonClicked += OnButtonClicked;
        }

        private void OnDisable()
        {
            if (buttonComponent != null)
            {
                buttonComponent.OnButtonClicked -= OnButtonClicked;
            }
        }

        public void OnButtonClicked(GameObject obj)
        {
            if (_parentDialog != null)
            {
                _parentDialog.Result.Result = ButtonTypeEnum;
                _parentDialog.DismissDialog();
            }
        }

        public void SetTitle(string title)
        {
            CompoundButtonText compoundButtonText = GetComponent<CompoundButtonText>();
            if (compoundButtonText)
            {
                compoundButtonText.Text = title;
            }
        }
    }
}