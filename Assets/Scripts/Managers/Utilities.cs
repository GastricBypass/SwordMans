using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utilities {

	public static IEnumerator DestroyAfterTime(GameObject obj, float seconds)
    {
        yield return new WaitForSeconds(seconds);

        GameObject.Destroy(obj);
    }
}
