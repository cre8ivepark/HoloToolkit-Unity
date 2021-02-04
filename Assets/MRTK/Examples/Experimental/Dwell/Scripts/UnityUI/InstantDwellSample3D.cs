// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.MixedReality.Toolkit.Input;
using TMPro;
using UnityEngine;

namespace Microsoft.MixedReality.Toolkit.Experimental.Dwell
{
    /// <summary>
    /// Dwell sample with no DwellIntended delay
    /// </summary>
    [AddComponentMenu("Scripts/MRTK/Examples/InstantDwellSample")]
    public class InstantDwellSample3D : BaseDwellSample3D
    {
        [SerializeField]
        private Transform listItems = null;

        public void Update()
        {
            float value = dwellHandler.DwellProgress;
            dwellVisualImage.rect.size = new Vector2(value, 1, 0);
            //dwellVisualImage.transform.localScale = new Vector3(value, 1, 0);
        }

        public override void DwellCompleted(IMixedRealityPointer pointer)
        {
            dwellVisualImage.transform.localScale = Vector3.zero;
            base.DwellCompleted(pointer);
        }

        public override void ButtonExecute()
        {
            var textMeshObjects = listItems.GetComponentsInChildren<TextMeshProUGUI>();

            foreach (var textObject in textMeshObjects)
            {
                textObject.text = int.Parse(textObject.text) + 5 + "";
            }
        }
    }
}