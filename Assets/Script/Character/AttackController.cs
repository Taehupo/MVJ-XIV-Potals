using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
	#region Members

	#endregion


	#region Public Manipulators

	#endregion


	#region Inherited Manipulators

	private void Awake()
	{
		// register callback
		ShapeController.OnShapeChanged += OnShapeChanged;
	}

	private void OnDestroy()
	{
		// unregister callback
		ShapeController.OnShapeChanged -= OnShapeChanged;
	}

	#endregion


	#region Private Manipulators

	void OnShapeChanged(ECharacterShape shape)
	{

	}

	#endregion
}
