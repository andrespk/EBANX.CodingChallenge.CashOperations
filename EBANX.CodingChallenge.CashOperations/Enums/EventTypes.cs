using System.ComponentModel;

namespace EBANX.CodingTest.CashOperations.Enums
{
    public enum EventTypes
    {
        [Description("deposit")]
        Deposit,

        [Description("withdraw")]
        Withdraw,

        [Description("transfer")]
        Transfer,
    }
}