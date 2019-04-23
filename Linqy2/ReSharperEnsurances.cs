using System.Diagnostics;

using JetBrains.Annotations;

// ReSharper disable UnusedParameter.Global
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

namespace Linqy2
{
    internal static class ReSharperEnsurances
    {
        [Conditional("DEBUG")]
        [ContractAnnotation("expression:false => halt")]
        internal static void assume(bool expression)
        {
        }
    }
}
