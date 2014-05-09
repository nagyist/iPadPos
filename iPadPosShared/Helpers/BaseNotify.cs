using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using iPadPos;

namespace System.ComponentModel
{
    public static class BaseNotify
    {

		public static bool SetProperty<T>(this PropertyChangedEventHandler handler, object sender, ref T currentValue, T newValue, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(currentValue, newValue))
                return false ;
            currentValue = newValue;
			if (sender is iDirty)
				(sender as iDirty).IsDirty = true;
            if (handler == null)
                return true;

            handler.Invoke(sender, new PropertyChangedEventArgs(propertyName));
            return true;
        }
    }
}
