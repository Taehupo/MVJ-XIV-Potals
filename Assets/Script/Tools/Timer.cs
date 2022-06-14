using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


#region Data Types

/// <summary></summary>
public enum EDeltaTime
{
    /// <summary>Uses Time.deltaTime, takes Time.timeScale into consideration</summary>
    Scaled,
    /// <summary>Uses Time.unscaledDeltaTime</summary>
    Unscaled,
}

#endregion


/// <summary>
/// Timer with scalable/unscalable delta time
/// add OnEnd function that will be called when timer end ""
/// you can get Timer advancement by adding a callback to SetValue
/// </summary>
public class Timer : MonoBehaviour
{
    #region Members

    /// <summary>Evolves from 1 to 0</summary>
    public Action<float> SetValue;

    /// <summary></summary>
    public Action OnEnd;

    float m_T = -1.0f;
    bool m_GrowValue = true;
    EDeltaTime m_DeltaTime = EDeltaTime.Scaled;
    float m_MaxDeltaTime = -1.0f;

    #endregion


    #region Public Accessors

    /// <summary></summary>
    public bool CallOnEndAtReset { get; set; } = true;

    /// <summary></summary>
    public bool CallOnEndAtShutdown { get; set; } = true;

    /// <summary>
    /// The total time of the timer (when playing timer as linear, multiply the T value to get current time)
    /// </summary>
    public float Time { get; private set; }

    /// <summary>
    /// Prevents having a long frame that fucks up a fade for instance
    /// -1 == no limit
    /// </summary>
    public float MaxDeltaTime
    {
        get { return m_MaxDeltaTime; }
        set { m_MaxDeltaTime = value; }
    }

    #endregion


    #region Inherited Manipulators

    /// <summary>
    /// 
    /// </summary>
    void Update()
    {
        // update timer
        if (m_T >= 0.0f)
        {
            // compute delta time
            float dt = m_DeltaTime == EDeltaTime.Scaled ? UnityEngine.Time.deltaTime : UnityEngine.Time.unscaledDeltaTime;
            if (m_MaxDeltaTime > 0.0f && dt > m_MaxDeltaTime)
                dt = m_MaxDeltaTime;

            // update t value
            m_T -= dt / Time;
            ApplyValue();

            // end process
            if (m_T < 0.0f)
                StopTimer(true);
        }
    }

    /// <summary>
    /// Will stop the timer, call OnEnd depending on CallOnEndAtReset
    /// Warning, it willnot nullify OnEnd and SetValue delegates
    /// </summary>
    public void Reset()
    {
        if (enabled)
            StopTimer(CallOnEndAtReset);
    }

    /// <summary>
    /// Will stop the timer, call OnEnd depending on CallOnEndAtShutdown
    /// </summary>
    private void OnDestroy()
    {
        if (enabled)
            StopTimer(CallOnEndAtShutdown);

        // force callback unregistration
        OnEnd = null;
        SetValue = null;
    }

    private void Awake()
    {
        enabled = false;
    }

    #endregion


    #region Private Manipulators

    void ModifyValueFromGrowValue(ref float value)
    {
        if (m_GrowValue)
            value = 1.0f - value;
    }

    void ApplyValue()
    {
        if (SetValue != null)
        {
            float t = UnityEngine.Mathf.Clamp01(m_T);
            ModifyValueFromGrowValue(ref t);
            SetValue(t);
        }
    }

    #endregion


    #region Public Manipulators

    /// <summary>
    /// Updates a timer and sends event through SetValue(float value), value evolves between 0 and 1 depending on growValue parameter
    /// </summary>
    /// <param name="time">Time to end timer (OnEnd() will be called)</param>
    /// <param name="growValue">The T value will go from 0 to 1 if true, from 1 to 0 otherwise</param>
    /// <param name="deltaTime">Specifies the delta time computation system</param>
    /// <remarks>In case of time close to zero (or negative), a SetValue will be called and OnEnd will follow immediatly</remarks>
    public void StartTimer(float time = 1.0f, bool growValue = true, EDeltaTime deltaTime = EDeltaTime.Scaled)
    {
        // error control
        if (enabled)
        {
            Debug.LogError("XKTimer.StartTimer() failed - Timer is already running");
            return;
        }

        enabled = true;
        m_GrowValue = growValue;
        m_DeltaTime = deltaTime;

        if (time <= 0.0f)
        {
            m_T = 0.0f;
            ApplyValue();
            StopTimer(true);
        }
        else
        {
            m_T = 1.0f;
            Time = UnityEngine.Mathf.Max(time, 0.016f); // make sure the timer lasts one frame
            ApplyValue();
        }
    }

    /// <summary></summary>
    /// <param name="callOnEnd"></param>
    public void StopTimer(bool callOnEnd = true)
    {
        // error control
        if (!enabled)
        {
            Debug.LogError("XKTimer.StopTimer() failed - Timer is not running");
            return;
        }

        // end process
        m_T = -1.0f;
        enabled = false;
        if (callOnEnd && OnEnd != null)
            OnEnd();
    }

    #endregion
}