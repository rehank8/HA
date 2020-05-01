using System;
using System.Collections.Generic;
using System.Text;

namespace HA.DependencyInjection
{
	public interface ICheckPermission
	{
		bool IsgpsEnabled();
		bool IsLocationGrantedForApplication();
	}
}
