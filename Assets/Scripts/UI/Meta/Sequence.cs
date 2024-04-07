using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class Sequence
{
	private readonly List<Action> elements = new();
	private readonly List<int> delays = new();

	private Sequence() { }

	private async Task Start()
	{
		if (elements.Count == 0)
			return;

		for (int i = 0; i < elements.Count; i++)
		{
			await Task.Delay(delays[i]);
			elements[i]?.Invoke();
		}
	}

	public class Builder
	{
		private readonly Sequence sequence = new();

		public Builder() { }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="delay">Delay in milliseconds</param>
		/// <param name="element"></param>
		/// <returns></returns>
		public Builder AddElement(int delay, Action element) 
		{
			sequence.delays.Add(delay);
			sequence.elements.Add(element);
			return this;
		}

		public void Start() => sequence.Start().ConfigureAwait(false);
	}
}
