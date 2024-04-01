// Workaround so that C# record types work in Unity
// See the following for more info:
// https://stackoverflow.com/questions/73100829/compile-error-when-using-record-types-with-unity3d
// https://docs.unity3d.com/2022.3/Documentation/Manual/CSharpCompiler.html
namespace System.Runtime.CompilerServices
{
    using System.ComponentModel;

    [EditorBrowsable(EditorBrowsableState.Never)]
    internal class IsExternalInit { }
}
