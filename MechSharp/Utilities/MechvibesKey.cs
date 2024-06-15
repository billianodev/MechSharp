using System;
using System.Linq;
using SharpHook.Native;

namespace MechSharp.Utilities;

public static class MechvibesKey
{
	public const int MkNone = 0;
	public const int MkEscape = 1;
	public const int Mk1 = 2;
	public const int Mk2 = 3;
	public const int Mk3 = 4;
	public const int Mk4 = 5;
	public const int Mk5 = 6;
	public const int Mk6 = 7;
	public const int Mk7 = 8;
	public const int Mk8 = 9;
	public const int Mk9 = 10;
	public const int Mk0 = 11;
	public const int MkMinus = 12;
	public const int MkEquals = 13;
	public const int MkBackspace = 14;
	public const int MkTab = 15;
	public const int MkQ = 16;
	public const int MkW = 17;
	public const int MkE = 18;
	public const int MkR = 19;
	public const int MkT = 20;
	public const int MkY = 21;
	public const int MkU = 22;
	public const int MkI = 23;
	public const int MkO = 24;
	public const int MkP = 25;
	public const int MkOpenBracket = 26;
	public const int MkCloseBracket = 27;
	public const int MkEnter = 28;
	public const int MkLeftControl = 29;
	public const int MkA = 30;
	public const int MkS = 31;
	public const int MkD = 32;
	public const int MkF = 33;
	public const int MkG = 34;
	public const int MkH = 35;
	public const int MkJ = 36;
	public const int MkK = 37;
	public const int MkL = 38;
	public const int MkSemicolon = 39;
	public const int MkQuote = 40;
	public const int MkBackQuote = 41;
	public const int MkLeftShift = 42;
	public const int MkBackslash = 43;
	public const int MkZ = 44;
	public const int MkX = 45;
	public const int MkC = 46;
	public const int MkV = 47;
	public const int MkB = 48;
	public const int MkN = 49;
	public const int MkM = 50;
	public const int MkComma = 51;
	public const int MkPeriod = 52;
	public const int MkSlash = 53;
	public const int MkRightShift = 54;
	public const int MkNumPadMultiply = 55;
	public const int MkLeftAlt = 56;
	public const int MkSpace = 57;
	public const int MkCapsLock = 58;
	public const int MkF1 = 59;
	public const int MkF2 = 60;
	public const int MkF3 = 61;
	public const int MkF4 = 62;
	public const int MkF5 = 63;
	public const int MkF6 = 64;
	public const int MkF7 = 65;
	public const int MkF8 = 66;
	public const int MkF9 = 67;
	public const int MkF10 = 68;
	public const int MkNumLock = 69;
	public const int MkScrollLock = 70;
	public const int MkNumPad7 = 71;
	public const int MkNumPad8 = 72;
	public const int MkNumPad9 = 73;
	public const int MkNumPadSubtract = 74;
	public const int MkNumPad4 = 75;
	public const int MkNumPad5 = 76;
	public const int MkNumPad6 = 77;
	public const int MkNumPadAdd = 78;
	public const int MkNumPad1 = 79;
	public const int MkNumPad2 = 80;
	public const int MkNumPad3 = 81;
	public const int MkNumPad0 = 82;
	public const int MkNumPadDecimal = 83;
	public const int MkF11 = 87;
	public const int MkF12 = 88;
	public const int MkNumPadEnter = 3621;
	public const int MkRightControl = 3613;
	public const int MkNumPadDivide = 3637;
	public const int MkPrintScreen = 3639;
	public const int MkRightAlt = 3640;
	public const int MkPause = 3653;
	public const int MkHome = 3655;
	public const int MkPageUp = 3657;
	public const int MkEnd = 3663;
	public const int MkPageDown = 3665;
	public const int MkInsert = 3666;
	public const int MkDelete = 3667;
	public const int MkLeftMeta = 3675;
	public const int MkRightMeta = 3676;
	public const int MkContextMenu = 3677;
	public const int MkUp = 57416;
	public const int MkLeft = 57419;
	public const int MkRight = 57421;
	public const int MkDown = 57424;

	public static readonly int[] Alphanum =
	{
		Mk1,
		Mk2,
		Mk3,
		Mk4,
		Mk5,
		Mk6,
		Mk7,
		Mk8,
		Mk9,
		Mk0,
		MkQ,
		MkW,
		MkE,
		MkR,
		MkT,
		MkY,
		MkU,
		MkI,
		MkO,
		MkP,
		MkA,
		MkS,
		MkD,
		MkF,
		MkG,
		MkH,
		MkJ,
		MkK,
		MkL,
		MkZ,
		MkX,
		MkC,
		MkV,
		MkB,
		MkN,
		MkM,
	};

	public static readonly int[] Special =
	{
		MkBackspace,
		MkTab,
		MkEnter,
		MkLeftControl,
		MkLeftShift,
		MkRightShift,
		MkLeftAlt,
		MkSpace,
		MkCapsLock,
		MkRightControl,
		MkRightAlt,
	};

	public static int Map(KeyCode keyCode, bool random)
	{
		var code = keyCode switch
		{
			KeyCode.VcEscape => MkEscape,
			KeyCode.Vc1 => Mk1,
			KeyCode.Vc2 => Mk2,
			KeyCode.Vc3 => Mk3,
			KeyCode.Vc4 => Mk4,
			KeyCode.Vc5 => Mk5,
			KeyCode.Vc6 => Mk6,
			KeyCode.Vc7 => Mk7,
			KeyCode.Vc8 => Mk8,
			KeyCode.Vc9 => Mk9,
			KeyCode.Vc0 => Mk0,
			KeyCode.VcMinus => MkMinus,
			KeyCode.VcEquals => MkEquals,
			KeyCode.VcBackspace => MkBackspace,
			KeyCode.VcTab => MkTab,
			KeyCode.VcQ => MkQ,
			KeyCode.VcW => MkW,
			KeyCode.VcE => MkE,
			KeyCode.VcR => MkR,
			KeyCode.VcT => MkT,
			KeyCode.VcY => MkY,
			KeyCode.VcU => MkU,
			KeyCode.VcI => MkI,
			KeyCode.VcO => MkO,
			KeyCode.VcP => MkP,
			KeyCode.VcOpenBracket => MkOpenBracket,
			KeyCode.VcCloseBracket => MkCloseBracket,
			KeyCode.VcEnter => MkEnter,
			KeyCode.VcLeftControl => MkLeftControl,
			KeyCode.VcA => MkA,
			KeyCode.VcS => MkS,
			KeyCode.VcD => MkD,
			KeyCode.VcF => MkF,
			KeyCode.VcG => MkG,
			KeyCode.VcH => MkH,
			KeyCode.VcJ => MkJ,
			KeyCode.VcK => MkK,
			KeyCode.VcL => MkL,
			KeyCode.VcSemicolon => MkSemicolon,
			KeyCode.VcQuote => MkQuote,
			KeyCode.VcBackQuote => MkBackQuote,
			KeyCode.VcLeftShift => MkLeftShift,
			KeyCode.VcBackslash => MkBackslash,
			KeyCode.VcZ => MkZ,
			KeyCode.VcX => MkX,
			KeyCode.VcC => MkC,
			KeyCode.VcV => MkV,
			KeyCode.VcB => MkB,
			KeyCode.VcN => MkN,
			KeyCode.VcM => MkM,
			KeyCode.VcComma => MkComma,
			KeyCode.VcPeriod => MkPeriod,
			KeyCode.VcSlash => MkSlash,
			KeyCode.VcRightShift => MkRightShift,
			KeyCode.VcNumPadMultiply => MkNumPadMultiply,
			KeyCode.VcLeftAlt => MkLeftAlt,
			KeyCode.VcSpace => MkSpace,
			KeyCode.VcCapsLock => MkCapsLock,
			KeyCode.VcF1 => MkF1,
			KeyCode.VcF2 => MkF2,
			KeyCode.VcF3 => MkF3,
			KeyCode.VcF4 => MkF4,
			KeyCode.VcF5 => MkF5,
			KeyCode.VcF6 => MkF6,
			KeyCode.VcF7 => MkF7,
			KeyCode.VcF8 => MkF8,
			KeyCode.VcF9 => MkF9,
			KeyCode.VcF10 => MkF10,
			KeyCode.VcNumLock => MkNumLock,
			KeyCode.VcScrollLock => MkScrollLock,
			KeyCode.VcNumPad7 => MkNumPad7,
			KeyCode.VcNumPad8 => MkNumPad8,
			KeyCode.VcNumPad9 => MkNumPad9,
			KeyCode.VcNumPadSubtract => MkNumPadSubtract,
			KeyCode.VcNumPad4 => MkNumPad4,
			KeyCode.VcNumPad5 => MkNumPad5,
			KeyCode.VcNumPad6 => MkNumPad6,
			KeyCode.VcNumPadAdd => MkNumPadAdd,
			KeyCode.VcNumPad1 => MkNumPad1,
			KeyCode.VcNumPad2 => MkNumPad2,
			KeyCode.VcNumPad3 => MkNumPad3,
			KeyCode.VcNumPad0 => MkNumPad0,
			KeyCode.VcNumPadDecimal => MkNumPadDecimal,
			KeyCode.VcF11 => MkF11,
			KeyCode.VcF12 => MkF12,
			KeyCode.VcNumPadEnter => MkNumPadEnter,
			KeyCode.VcRightControl => MkRightControl,
			KeyCode.VcNumPadDivide => MkNumPadDivide,
			KeyCode.VcPrintScreen => MkPrintScreen,
			KeyCode.VcRightAlt => MkRightAlt,
			KeyCode.VcPause => MkPause,
			KeyCode.VcHome => MkHome,
			KeyCode.VcPageUp => MkPageUp,
			KeyCode.VcEnd => MkEnd,
			KeyCode.VcPageDown => MkPageDown,
			KeyCode.VcInsert => MkInsert,
			KeyCode.VcDelete => MkDelete,
			KeyCode.VcLeftMeta => MkLeftMeta,
			KeyCode.VcRightMeta => MkRightMeta,
			KeyCode.VcContextMenu => MkContextMenu,
			KeyCode.VcUp => MkUp,
			KeyCode.VcLeft => MkLeft,
			KeyCode.VcRight => MkRight,
			KeyCode.VcDown => MkDown,
			_ => MkNone
		};
		if (code == 0 || (random && !Special.Contains(code)))
		{
			code = Alphanum[Random.Shared.Next(Alphanum.Length)];
		}
		return code;
	}
}
