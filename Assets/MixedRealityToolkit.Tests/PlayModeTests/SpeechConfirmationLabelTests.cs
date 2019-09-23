#if !WINDOWS_UWP
// When the .NET scripting backend is enabled and C# projects are built
// The assembly that this file is part of is still built for the player,
// even though the assembly itself is marked as a test assembly (this is not
// expected because test assemblies should not be included in player builds).
// Because the .NET backend is deprecated in 2018 and removed in 2019 and this
// issue will likely persist for 2018, this issue is worked around by wrapping all
// play mode tests in this check.

using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using NUnit.Framework;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.TestTools;

namespace Microsoft.MixedReality.Toolkit.Tests
{
    class SpeechConfirmationLabelTests
    {
        private SpeechInputHandler speechInputHandler;

        // Setup a scene, initialize MRTK and the playspace - this method is called before the start of each test listed below
        [SetUp]
        public void Setup()
        {
            PlayModeTestUtilities.Setup();

            // Change the position of the main camera to (0, 0, 0), in PlayModeTestUtilites.Setup() 
            // the camera is set to position (1, 1.5, -2)
            TestUtilities.PlayspaceToOriginLookingForward();
        }

        // Destroy the scene - this method is called after each test listed below has completed 
        [TearDown]
        public void TearDown()
        {
            PlayModeTestUtilities.TearDown();
        }

        #region Tests

        /// <summary>
        /// Skeleton for a new MRTK play mode test.
        /// </summary>
        [UnityTest]
        public IEnumerator TestSpeechConfirmationLabel()
        {
            // This test uses the input system to simulate speech commands.
            IMixedRealityInputSystem inputSystem = PlayModeTestUtilities.GetInputSystem();
            Assert.IsNotNull(inputSystem, "The input system is not enabled in the scene.");
            yield return null;

            int frameDelay = 10;

            // 1. Create a Unity primitive at 2m
            var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = new Vector3(0, 0, 2);

            // 2. Assign SpeechInputHandler.cs
            speechInputHandler = cube.AddComponent<SpeechInputHandler>();

            // 3. Assign SpeechConfirmationTooltip.prefab

            // 4. Select the keyword in the SpeechInputHandler (non-system-reservied keyword)

            // 5. Trigger speech input
            var gazeInputSource = inputSystem.DetectedInputSources.Where(x => x.SourceName.Equals("Gaze")).First();
            inputSystem.RaiseSpeechCommandRecognized(
                gazeInputSource,
                RecognitionConfidenceLevel.High,
                new TimeSpan(),
                DateTime.Now,
                new SpeechCommands("[KEYWORD SELECTED IN SpeechInputHandler]", KeyCode.Alpha9, MixedRealityInputAction.None));
            // It may take a few frames before the event is handled and the system responds to the state change.
            for (int i = 0; i < frameDelay; i++) { yield return null; }

            // 6. Check if SpeechConfirmationTooltip.prefab is instantiated and animated 

            // 7. Check if SpeechConfirmationTooltip.prefab instance is destroyed



            // Verify that the VisualProfiler is disabled.
            // Assert.IsFalse( , "The VisualProfiler is active (should be inactive).");
            yield return null;
        }
        #endregion
    }
}
#endif