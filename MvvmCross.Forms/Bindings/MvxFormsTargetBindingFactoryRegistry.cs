﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Forms.Bindings.Target;
using MvvmCross.Platform.Platform;

namespace MvvmCross.Forms.Bindings
{
    public class MvxFormsTargetBindingFactoryRegistry : MvxTargetBindingFactoryRegistry
    {
        protected override bool TryCreateReflectionBasedBinding(object target, string targetName,
                                                                out IMvxTargetBinding binding)
        {
            if (TryCreateBindablePropertyBasedBinding(target, targetName, out binding))
            {
                return true;
            }

            return base.TryCreateReflectionBasedBinding(target, targetName, out binding);
        }

        private static bool TryCreateBindablePropertyBasedBinding(object target, string targetName,
                                                             out IMvxTargetBinding binding)
        {
            if (target == null)
            {
                binding = null;
                return false;
            }

            if (string.IsNullOrEmpty(targetName))
            {
                MvxBindingLog.Error(
                                      "Empty binding target passed to MvxFormsTargetBindingFactoryRegistry");
                binding = null;
                return false;
            }

            var bindableProperty = target.GetType().FindBindableProperty(targetName);
            if (bindableProperty == null)
            {
                binding = null;
                return false;
            }

            var actualProperty = target.GetType().FindActualProperty(targetName);
            var actualPropertyType = actualProperty?.PropertyType ?? typeof(object);

            binding = new MvxBindablePropertyTargetBinding(target, bindableProperty, actualPropertyType);
            return true;
        }
    }
}
