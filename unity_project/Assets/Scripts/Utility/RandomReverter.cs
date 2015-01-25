using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class RandomReverter
{
	public RandomReverter ()
	{
		_seed = UnityEngine.Random.Range(0, int.MaxValue);
		Revert();
	}

	public void Revert()
	{
		_rand = new System.Random(_seed);
	}
	
	public int Range(int low, int hi)
	{
		return _rand.Next() % (hi - low + 1) + low;
	}

	public float Range(float low, float hi)
	{
		return (float) _rand.NextDouble() % (hi - low) + low;
	}

	private System.Random _rand;
	private int _seed;
}

