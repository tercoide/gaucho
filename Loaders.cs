
using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.Win32.SafeHandles;
using OpenTK;
public class GlxBindingsContext : IBindingsContext
{
	[DllImport("libGL", CharSet = CharSet.Ansi)]
	private static extern IntPtr glXGetProcAddress(string procName);
	public IntPtr GetProcAddress(string procName)
	{
		return glXGetProcAddress(procName);
	}
}

public class NativeBindingsContext : IBindingsContext
{
  private static IBindingsContext? _context;

  public NativeBindingsContext()
  {
		if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
		{
			_context = new WglBindingsContext();
			Console.WriteLine("Using WINDOWS BindingsContext");
		}
		else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
		{

			_context = new GlxBindingsContext();
			Console.WriteLine("Using LINUX BindingsContext");
		}
		else
		{
			throw new PlatformNotSupportedException();
		}
  }

  public  IntPtr GetProcAddress(string procName)

  {

	IntPtr p = _context?.GetProcAddress(procName) ?? IntPtr.Zero;
	
    return p;
  }
}
public class WglBindingsContext : IBindingsContext
{
	[DllImport("opengl32.dll", CharSet = CharSet.Ansi)]
	private static extern IntPtr wglGetProcAddress(string procName);

	private readonly ModuleSafeHandle _openGlHandle;

	public WglBindingsContext()
	{
		_openGlHandle = Kernel32.LoadLibrary("opengl32.dll");
	}

	public IntPtr GetProcAddress(string procName)
	{
		IntPtr addr = wglGetProcAddress(procName);
		return addr != IntPtr.Zero ? addr : Kernel32.GetProcAddress(_openGlHandle, procName);
	}

	private static class Kernel32
	{
		[DllImport("kernel32", SetLastError = true, CharSet = CharSet.Ansi)]
		public static extern ModuleSafeHandle LoadLibrary([MarshalAs(UnmanagedType.LPStr)] string lpFileName);

		[DllImport("kernel32", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool FreeLibrary(IntPtr hModule);

		[DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
		public static extern IntPtr GetProcAddress(ModuleSafeHandle hModule, string procName);
	}

	// [SecurityPermission(SecurityAction.InheritanceDemand, UnmanagedCode = true)]
	// [SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
	// ReSharper disable once ClassNeverInstantiated.Local
	private class ModuleSafeHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		public ModuleSafeHandle() : base(true)
		{
		}

		// [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		protected override bool ReleaseHandle()
		{
			return Kernel32.FreeLibrary(handle);
		}
	}
}