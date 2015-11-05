using UnityEngine;
using System.Collections;

public static class AudioExtensions
{
    public static IEnumerator WhilePlaying(this AudioSource audio) {        
		
		do
        {			
            yield return null;
        } while (audio.isPlaying);
    }
}