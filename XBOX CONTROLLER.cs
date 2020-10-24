using System.Threading;
using WindowsInput;
using SharpDX.XInput;
using System.Diagnostics;
using System;

namespace Xbox_Controller_As_Mouse
{
	public class XBoxControllerAsMouse
	{
		private const int MovementDivider = 2_000;
		private const int ScrollDivider = 10_000;
		private const int RefreshRate = 60;

		private Timer _timer;
		private Controller _controller;
		private IMouseSimulator _mouseSimulator;

		private bool _wasADown;
		private bool _wasBDown;

		public double currentRX;
		public double currentRY;
		public double currentLX;
		public double currentLY;
		public string BPress;
		public string APress;

		public XBoxControllerAsMouse()
		{
			_controller = new Controller(UserIndex.One);
			_mouseSimulator = new InputSimulator().Mouse;
			_timer = new Timer(obj => Update());
		}

		public void Start()
		{
			_timer.Change(0, 1000 / RefreshRate);
		}

		public void Update()
		{
			_controller.GetState(out var state);
			Movement(state);
			Scroll(state);
			LeftButton(state);
			RightButton(state);
			//Console.Write("\rLX = {0} LY = {1} : RX = {2} RY = {3} {4} {5}                  ", currentLX * MovementDivider, currentLY * MovementDivider, currentRX * ScrollDivider, currentRY * ScrollDivider);
		}

		private void RightButton(State state)
		{
			var isBDown = state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.B);
			if (isBDown && !_wasBDown) _mouseSimulator.RightButtonDown();
			if (!isBDown && _wasBDown) _mouseSimulator.RightButtonUp();
			_wasBDown = isBDown;
		}

		private void LeftButton(State state)
		{
			var isADown = state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.A);
			if (isADown && !_wasADown) _mouseSimulator.LeftButtonDown(); APress = "A";
			if (!isADown && _wasADown) _mouseSimulator.LeftButtonUp(); APress = null;
			_wasADown = isADown;
		}

		public void Scroll(State state)
		{
			var x = state.Gamepad.RightThumbX / ScrollDivider;
			var y = state.Gamepad.RightThumbY / ScrollDivider;
			_mouseSimulator.HorizontalScroll(x);
			_mouseSimulator.VerticalScroll(y);
			currentRX = Convert.ToDouble(x);
			currentRY = Convert.ToDouble(y);
			//Debug.WriteLine("\rRX = {0}	RY = {1}",x,y);
		}

		public void Movement(State state)
		{
			var x = state.Gamepad.LeftThumbX / MovementDivider;
			var y = state.Gamepad.LeftThumbY / MovementDivider;
			if (x == 1 || x == -1) { x = 0; }
			if (y == 1 || y == -1) { y = 0; }
			currentLX = Convert.ToDouble(x);
			currentLY = Convert.ToDouble(y);
			_mouseSimulator.MoveMouseBy(x, -y);

			//Debug.WriteLine("\rLX = {0}	LY = {1}", x, y);
		}
	}
}