using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using SharpHook.Native;

namespace MechSharp.Utilities;

public static class MechvibesMapper
{
	public static readonly int[] Alphanum = [2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 30, 31, 32, 33, 34, 35, 36, 37, 38, 44, 45, 46, 47, 48, 49, 50];
	public static readonly int[] Special = [14, 15, 28, 29, 42, 54, 56, 57, 58];

	public static string? MapKeyCode(KeyCode key, bool random)
	{
		int? code = key switch
		{
			KeyCode.VcEscape => 1,
			KeyCode.Vc1 => 2,
			KeyCode.Vc2 => 3,
			KeyCode.Vc3 => 4,
			KeyCode.Vc4 => 5,
			KeyCode.Vc5 => 6,
			KeyCode.Vc6 => 7,
			KeyCode.Vc7 => 8,
			KeyCode.Vc8 => 9,
			KeyCode.Vc9 => 10,
			KeyCode.Vc0 => 11,
			KeyCode.VcMinus => 12,
			KeyCode.VcEquals => 13,
			KeyCode.VcBackspace => 14,
			KeyCode.VcTab => 15,
			KeyCode.VcQ => 16,
			KeyCode.VcW => 17,
			KeyCode.VcE => 18,
			KeyCode.VcR => 19,
			KeyCode.VcT => 20,
			KeyCode.VcY => 21,
			KeyCode.VcU => 22,
			KeyCode.VcI => 23,
			KeyCode.VcO => 24,
			KeyCode.VcP => 25,
			KeyCode.VcOpenBracket => 26,
			KeyCode.VcCloseBracket => 27,
			KeyCode.VcEnter => 28,
			KeyCode.VcLeftControl => 29,
			KeyCode.VcA => 30,
			KeyCode.VcS => 31,
			KeyCode.VcD => 32,
			KeyCode.VcF => 33,
			KeyCode.VcG => 34,
			KeyCode.VcH => 35,
			KeyCode.VcJ => 36,
			KeyCode.VcK => 37,
			KeyCode.VcL => 38,
			KeyCode.VcSemicolon => 39,
			KeyCode.VcQuote => 40,
			KeyCode.VcBackQuote => 41,
			KeyCode.VcLeftShift => 42,
			KeyCode.VcBackslash => 43,
			KeyCode.VcZ => 44,
			KeyCode.VcX => 45,
			KeyCode.VcC => 46,
			KeyCode.VcV => 47,
			KeyCode.VcB => 48,
			KeyCode.VcN => 49,
			KeyCode.VcM => 50,
			KeyCode.VcComma => 51,
			KeyCode.VcPeriod => 52,
			KeyCode.VcSlash => 53,
			KeyCode.VcRightShift => 54,
			KeyCode.VcNumPadMultiply => 55,
			KeyCode.VcLeftAlt => 56,
			KeyCode.VcSpace => 57,
			KeyCode.VcCapsLock => 58,
			KeyCode.VcF1 => 59,
			KeyCode.VcF2 => 60,
			KeyCode.VcF3 => 61,
			KeyCode.VcF4 => 62,
			KeyCode.VcF5 => 63,
			KeyCode.VcF6 => 64,
			KeyCode.VcF7 => 65,
			KeyCode.VcF8 => 66,
			KeyCode.VcF9 => 67,
			KeyCode.VcF10 => 68,
			KeyCode.VcNumLock => 69,
			KeyCode.VcScrollLock => 70,
			KeyCode.VcNumPad7 => 71,
			KeyCode.VcNumPad8 => 72,
			KeyCode.VcNumPad9 => 73,
			KeyCode.VcNumPadSubtract => 74,
			KeyCode.VcNumPad4 => 75,
			KeyCode.VcNumPad5 => 76,
			KeyCode.VcNumPad6 => 77,
			KeyCode.VcNumPadAdd => 78,
			KeyCode.VcNumPad1 => 79,
			KeyCode.VcNumPad2 => 80,
			KeyCode.VcNumPad3 => 81,
			KeyCode.VcNumPad0 => 82,
			KeyCode.VcNumPadDecimal => 83,
			KeyCode.VcF11 => 87,
			KeyCode.VcF12 => 88,
			KeyCode.VcNumPadEnter => 3621,
			KeyCode.VcRightControl => 3613,
			KeyCode.VcNumPadDivide => 3637,
			KeyCode.VcPrintScreen => 3639,
			KeyCode.VcRightAlt => 3640,
			KeyCode.VcPause => 3653,
			KeyCode.VcHome => 3655,
			KeyCode.VcPageUp => 3657,
			KeyCode.VcEnd => 3663,
			KeyCode.VcPageDown => 3665,
			KeyCode.VcInsert => 3666,
			KeyCode.VcDelete => 3667,
			KeyCode.VcLeftMeta => 3675,
			KeyCode.VcRightMeta => 3676,
			KeyCode.VcContextMenu => 3677,
			KeyCode.VcUp => 57416,
			KeyCode.VcLeft => 57419,
			KeyCode.VcRight => 57421,
			KeyCode.VcDown => 57424,
			_ => null
		};
		if (!code.HasValue || (random && !Special.Contains(code.Value)))
		{
			code = Alphanum[Random.Shared.Next(Alphanum.Length)];
		}
		return code.ToString();
	}

	public static string? MapKeyCodeUp(KeyCode key, bool random)
	{
		return MapUp(MapKeyCode(key, random));
	}

	public static string? MapMouseButton(MouseButton button)
	{
		int? code = button switch
		{
			MouseButton.Button1 => 1,
			MouseButton.Button2 => 2,
			MouseButton.Button3 => 3,
			_ => null
		};
		return code.ToString();
	}

	public static string? MapMouseButtonUp(MouseButton button)
	{
		return MapUp(MapMouseButton(button));
	}

	[return: NotNullIfNotNull(nameof(code))]
	public static string? MapUp(string? code)
	{
		return code == null ? null : "0" + code;
	}
}
