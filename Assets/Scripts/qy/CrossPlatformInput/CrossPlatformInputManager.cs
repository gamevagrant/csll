using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace qy.CrossPlatformInput
{
	public class CrossPlatformInputManager
	{
		protected static Dictionary<string, VirtualAxis> m_VirtualAxes = new Dictionary<string, VirtualAxis>();
		protected static Dictionary<string, VirtualButton> m_VirtualButtons = new Dictionary<string, VirtualButton>();

		public static bool AxisExists(string name)
		{
			return m_VirtualAxes.ContainsKey(name);
		}

		public static bool ButtonExists(string name)
		{
			return m_VirtualButtons.ContainsKey(name);
		}

		public static void RegisterVirtualAxis(VirtualAxis axis)
		{
			if (m_VirtualAxes.ContainsKey(axis.name))
			{
				Debug.LogError("There is already a virtual axis named " + axis.name + " registered.");
			}
			else
			{
				// add any new axes
				m_VirtualAxes.Add(axis.name, axis);

			}
		}


		public static void RegisterVirtualButton(VirtualButton button)
		{
			if (m_VirtualButtons.ContainsKey(button.name))
			{
				Debug.LogError("There is already a virtual axis named " + button.name + " registered.");
			}
			else
			{
				// add any new axes
				m_VirtualButtons.Add(button.name, button);

			}
		}


		public static void UnRegisterVirtualAxis(string name)
		{
			if (!string.IsNullOrEmpty(name))
			{
				m_VirtualAxes.Remove(name);
			}
			
		}


		public static void UnRegisterVirtualButton(string name)
		{
			if (!string.IsNullOrEmpty(name))
			{
				m_VirtualButtons.Remove(name);
			}
		}


		// returns the platform appropriate axis for the given name
		public static float GetAxis(string name)
		{
			if (!m_VirtualAxes.ContainsKey(name))
			{
				RegisterVirtualAxis(new VirtualAxis(name));
			}
			return m_VirtualAxes[name].GetValue;
		}


		// -- Button handling --
		public static bool GetButton(string name)
		{
			if (!m_VirtualButtons.ContainsKey(name))
			{
				RegisterVirtualButton(new VirtualButton(name));
			}
			return m_VirtualButtons[name].GetButton;
		}


		public static bool GetButtonDown(string name)
		{
			if (!m_VirtualButtons.ContainsKey(name))
			{
				RegisterVirtualButton(new VirtualButton(name));
			}
			return m_VirtualButtons[name].GetButtonDown;
		}


		public static bool GetButtonUp(string name)
		{
			if (!m_VirtualButtons.ContainsKey(name))
			{
				RegisterVirtualButton(new VirtualButton(name));
			}
			return m_VirtualButtons[name].GetButtonUp;
		}


		public static void SetButtonDown(string name)
		{
			if (!m_VirtualButtons.ContainsKey(name))
			{
				RegisterVirtualButton(new VirtualButton(name));
			}
			m_VirtualButtons[name].Pressed();
		}


		public static void SetButtonUp(string name)
		{
			if (!m_VirtualButtons.ContainsKey(name))
			{
				RegisterVirtualButton(new VirtualButton(name));
			}
			m_VirtualButtons[name].Released();
		}


		public static void SetAxisZero(string name)
		{
			if (!m_VirtualAxes.ContainsKey(name))
			{
				RegisterVirtualAxis(new VirtualAxis(name));
			}
			m_VirtualAxes[name].Update(0f);
		}


		public static void SetAxis(string name, float value)
		{
			if (!m_VirtualAxes.ContainsKey(name))
			{
				RegisterVirtualAxis(new VirtualAxis(name));
			}
			m_VirtualAxes[name].Update(value);
		}


		public class VirtualAxis
		{
			public string name { get; private set; }
			private float m_Value;


			public VirtualAxis(string name)
			{
				this.name = name;
			}


			// removes an axes from the cross platform input system
			public void Remove()
			{
				UnRegisterVirtualAxis(name);
			}


			// a controller gameobject (eg. a virtual thumbstick) should update this class
			public void Update(float value)
			{
				m_Value = value;
			}


			public float GetValue
			{
				get { return m_Value; }
			}
		}

		// a controller gameobject (eg. a virtual GUI button) should call the
		// 'pressed' function of this class. Other objects can then read the
		// Get/Down/Up state of this button.
		public class VirtualButton
		{
			public string name { get; private set; }

			private int m_LastPressedFrame = -5;
			private int m_ReleasedFrame = -5;
			private bool m_Pressed;


			public VirtualButton(string name)
			{
				this.name = name;
			}


			// A controller gameobject should call this function when the button is pressed down
			public void Pressed()
			{

				m_Pressed = true;
				m_LastPressedFrame = Time.frameCount;
			}


			// A controller gameobject should call this function when the button is released
			public void Released()
			{
				m_Pressed = false;
				m_ReleasedFrame = Time.frameCount;
			}


			// the controller gameobject should call Remove when the button is destroyed or disabled
			public void Remove()
			{
				UnRegisterVirtualButton(name);
			}


			// these are the states of the button which can be read via the cross platform input system
			public bool GetButton
			{
				get { return m_Pressed; }
			}


			public bool GetButtonDown
			{
				get
				{
					return m_LastPressedFrame - Time.frameCount == -1;
				}
			}


			public bool GetButtonUp
			{
				get
				{
					return (m_ReleasedFrame == Time.frameCount - 1);
				}
			}
		}
	}
}

