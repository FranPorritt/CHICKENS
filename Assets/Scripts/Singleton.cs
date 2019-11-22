using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T s_Instance;
	public static T Instance
	{
		get
		{
			if(s_Instance == null)
			{
				s_Instance = (T)FindObjectOfType<T>();

				if(s_Instance == null)
				{
					// Create a new instance of this singleton.
					GameObject singletonObject = new GameObject(typeof(T).ToString());
					s_Instance = singletonObject.AddComponent<T>();
				}
			}

			return s_Instance;
		}
	}

	private void Awake()
	{
		if(s_Instance == null)
		{
			s_Instance = this as T;
		}
		else if(s_Instance != this)
		{
			Destroy(gameObject);
		}
	}
}

public abstract class SingletonPersistent<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T s_Instance;
	public static T Instance
	{
		get
		{
			if(s_Instance == null)
			{
				s_Instance = (T)FindObjectOfType<T>();

				if(s_Instance == null)
				{
					// Create a new instance of this singleton.
					GameObject singletonObject = new GameObject(typeof(T).ToString());
					s_Instance = singletonObject.AddComponent<T>();

					DontDestroyOnLoad(singletonObject);
				}
			}

			return s_Instance;
		}
	}

	private void Awake()
	{
		if(s_Instance == null)
		{
			s_Instance = this as T;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}
}