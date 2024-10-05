using System.Runtime.Serialization;

namespace Trainee_Test.Resources;

public enum MaritalStatus
{
    [EnumMember(Value = "Single")]
    Single = 0,
    [EnumMember(Value = "MaritalStatus")]
    Married = 1
}

public static class EnumExtension
{
    public static string GetEnumMemberValue(this Enum enumValue)
    {
        var enumMemberAttribute = enumValue.GetType()?
            .GetField(enumValue.ToString())?
            .GetCustomAttributes(typeof(EnumMemberAttribute), false)
            as EnumMemberAttribute[];

        return enumMemberAttribute?.Length > 0 ? enumMemberAttribute[0].Value : enumValue.ToString();
    }
}