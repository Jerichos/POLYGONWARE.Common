﻿using System;
using System.Threading.Tasks;
using POLYGONWARE.Common.Util;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

namespace POLYGONWARE.Common
{
    public static class Utils
    {
        public static bool HasLayer(this LayerMask layerMask, int layer)
        {
            return layerMask.value == 1 << layer;
        }
        
        public static async Task<T> RetryEconomyFunction<T>(Func<Task<T>> functionToRetry, int retryAfterSeconds)
        {
            if (retryAfterSeconds > 60)
            {
                Debug.Log($"Economy returned a rate limit exception with an extended Retry After time " +
                          $"of {retryAfterSeconds} seconds. Suggest manually retrying at a later time.");
                return default;
            }

            Debug.Log($"Economy returned a rate limit exception. Retrying after {retryAfterSeconds} seconds");

            try
            {
                // Using a CancellationToken allows us to ensure that the Task.Delay gets cancelled if we exit
                // playmode while it's waiting its delay time. Without it, it would continue trying to execute
                // the rest of this code, even outside of playmode.
                using (var cancellationTokenHelper = new CancellationTokenHelper())
                {
                    var cancellationToken = cancellationTokenHelper.cancellationToken;

                    await Task.Delay(retryAfterSeconds * 1000, cancellationToken);

                    // Call the function that we passed in to this method after the retry after time period has passed.
                    var result = await functionToRetry();

                    if (cancellationToken.IsCancellationRequested)
                    {
                        return default;
                    }

                    Debug.Log("Economy retry successfully completed");

                    return result;
                }
            }
            catch (OperationCanceledException)
            {
                return default;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }

            return default;
        }
        
        public static string GetElapsedTimeRange(DateTime startTime)
        {
            var elapsedTime = DateTime.Now - startTime;
            var elapsedSeconds = elapsedTime.TotalSeconds;

            if (elapsedSeconds < 0)
            {
                return "N/A";
            }

            // BottomRange is the nearest divisible-by-10 number less than elapsedSeconds.
            // For instance, 47.85 seconds has a bottom range of 40.
            var bottomRange = (int)Math.Floor(elapsedSeconds / 10) * 10;

            // TopRange is the nearest divisible-by-10 number greater than elapsedSeconds.
            // For instance, 47.85 seconds has a top range of 50.
            var topRange = bottomRange + 10;

            // In the string being returned `[` represents inclusive and `)` represents exclusive. So a range of
            // [40, 50) includes numbers from 40.0 to 49.99999 etc.
            return $"[{bottomRange}, {topRange}) seconds";
        }

        public static bool IsPointerOverUI()
        {
            return EventSystem.current.currentSelectedGameObject;
        }
        
        public static bool IsMouseOverUi
        {
            get
            {
                // [Only works well while there is not PhysicsRaycaster on the Camera)
                EventSystem eventSystem = EventSystem.current;
                return (eventSystem != null && eventSystem.IsPointerOverGameObject());
     
                // [Works with PhysicsRaycaster on the Camera. Requires New Input System. Assumes mouse.)
                if (EventSystem.current == null)
                {
                    return false;
                }
                
                RaycastResult lastRaycastResult = ((InputSystemUIInputModule)EventSystem.current.currentInputModule).GetLastRaycastResult(Mouse.current.deviceId);
                const int uiLayer = 5;
                return lastRaycastResult.gameObject != null && lastRaycastResult.gameObject.layer == uiLayer;
            }
        }

    }
}