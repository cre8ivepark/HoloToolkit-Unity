// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.Utilities.Solvers;
using System.Collections;
using UnityEngine;

namespace Microsoft.MixedReality.Toolkit.UI
{
    [RequireComponent(typeof(RadialView))]
    public class FollowMeToggle : MonoBehaviour
    {
        /// <summary>
        /// An optional object for visualizing the 'Follow Me' mode state.
        /// </summary>
        public GameObject VisualizationObject
        {
            get { return visualizationObject; }
            set { visualizationObject = value; }
        }

        [SerializeField]
        [Tooltip("An optional object for visualizing the 'Follow Me' mode state.")]
        private GameObject visualizationObject = null;

        /// <summary>
        /// An optional Interactable to select/deselect when toggling the follow behavior.
        /// </summary>
        public Interactable InteractableObject
        {
            get { return interactableObject; }
            set { interactableObject = value; }
        }

        [SerializeField]
        [Tooltip("An optional Interactable to select/deselect when toggling the follow behavior.")]
        private Interactable interactableObject = null;

        /// <summary>
        /// Should following be automatically enabled when the user is further than a certain distance away?
        /// </summary>
        public bool AutoFollowAtDistance
        {
            get { return autoFollowAtDistance; }
            set
            {
                if (autoFollowAtDistance != value)
                {
                    autoFollowAtDistance = value;

                    if (autoFollowAtDistance)
                    {
                        autoFollowDistanceCheck = StartCoroutine(AutoFollowDistanceCheck());
                    }
                    else
                    {
                        StopCoroutine(autoFollowDistanceCheck);
                    }
                }
            }
        }

        [SerializeField]
        [Tooltip("Should following be automatically enabled when the user is further than a certain distance away?")]
        private bool autoFollowAtDistance = false;

        /// <summary>
        /// If autoFollowAtDistance is enabled, what distance to trigger auto following at.
        /// </summary>
        public float AutoFollowDistance
        {
            get { return autoFollowDistance; }
            set { autoFollowDistance = value; }
        }

        [SerializeField]
        [Tooltip("If autoFollowAtDistance is enabled, what distance to trigger auto following at.")]
        private float autoFollowDistance = 2.0f;

        /// <summary>
        /// Optional transform to use when using autoFollowAtDistance. If not specified the local transform is used.
        /// </summary>
        public Transform AutoFollowTransform
        {
            get { return autoFollowTransform; }
            set
            {
                autoFollowTransform = value;

                if (autoFollowTransform == null)
                {
                    autoFollowTransform = transform;
                }
            }
        }

        [SerializeField]
        [Tooltip("Optional transform to use when using autoFollowAtDistance. If not specified the local transform is used.")]
        private Transform autoFollowTransform = null;

        private RadialView radialView = null;
        private Coroutine autoFollowDistanceCheck = null;

        #region MonoBehaviour Implementation

        private void Awake()
        {
            radialView = GetComponent<RadialView>();

            if (autoFollowTransform == null)
            {
                autoFollowTransform = transform;
            }

            if (autoFollowAtDistance)
            {
                autoFollowDistanceCheck = StartCoroutine(AutoFollowDistanceCheck());
            }
        }

        #endregion MonoBehaviour Implementation

        /// <summary>
        /// Toggles the current follow behavior of the solver.
        /// </summary>
        public void ToggleFollowMeBehavior()
        {
            if (radialView != null)
            {
                SetFollowMeBehavior(!radialView.enabled);
            }
        }

        /// <summary>
        /// Enables or disables the solver based on the follow parameter.
        /// </summary>
        /// <param name="follow">True if the solver should be active.</param>
        public void SetFollowMeBehavior(bool follow)
        {
            if (radialView != null && radialView.enabled != follow)
            {
                // Toggle Radial Solver component
                // You can tweak the detailed positioning behavior such as offset, lerping time, orientation type in the Inspector panel
                radialView.enabled = follow;

                if (visualizationObject != null)
                {
                    visualizationObject.SetActive(follow);
                }

                if (interactableObject != null)
                {
                    interactableObject.SetDimensionIndex(follow ? 0 : 1);
                }
            }
        }

        /// <summary>
        /// Coroutine which checks how far away this transom is from the user and enables the follow behavior at a specified distance.
        /// </summary>
        /// <returns>Coroutine enumerator.</returns>
        private IEnumerator AutoFollowDistanceCheck()
        {
            while (true)
            {
                var mainCamera = CameraCache.Main;

                if (mainCamera != null)
                {
                    float autoFollowDistanceSq = autoFollowDistance * autoFollowDistance;

                    if ((mainCamera.transform.position - autoFollowTransform.position).sqrMagnitude >= autoFollowDistanceSq)
                    {
                        SetFollowMeBehavior(true);
                    }
                }

                yield return null;
            }
        }
    }
}