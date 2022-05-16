using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackController
{
	#region Members

	#endregion


	#region Public Manipulators

	public abstract void Attack();

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
