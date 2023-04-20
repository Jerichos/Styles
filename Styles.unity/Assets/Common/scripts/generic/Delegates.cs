namespace Styles.Common
{
public delegate void GenericDelegate<in T>(T value);
public delegate void GenericDelegate<in T1, in T2>(T1 value1, T2 value2);
public delegate void GenericDelegate<in T1, in T2, in T3>(T1 value1, T2 value2, T3 value3);
}