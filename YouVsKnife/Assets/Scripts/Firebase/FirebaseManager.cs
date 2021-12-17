/**
 * Alubecki Native
 * © Aurélien Lubecki 2019
 * All Rights Reserved
 */

using System;
using UnityEngine;
using Firebase.Analytics;
using Firebase.Crashlytics;


public class FirebaseManager : MonoBehaviour {


    public static void trackEvent(String name, params Parameter[] parameters) {

        if (name == null) {
            throw new ArgumentException();
        }

        FirebaseAnalytics.LogEvent(name, parameters);
    }

    public static void trackEvent(String name, String paramName, String paramValue) {

        if (paramName == null) {
            throw new ArgumentException();
        }

        trackEvent(name, new Parameter[] {
            new Parameter(paramName, paramValue)
        });
    }

    public static void setUserProperty(String name, String property) {

        if (name == null) {
            throw new ArgumentException();
        }

        FirebaseAnalytics.SetUserProperty(name, property);
    }


    protected void Awake() {

        //disable analytics for debug
        FirebaseAnalytics.SetAnalyticsCollectionEnabled(!Debug.isDebugBuild);

        //disable crashes for debug
        Crashlytics.IsCrashlyticsCollectionEnabled = !Debug.isDebugBuild;
    }

}
