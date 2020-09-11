using EBANX.CodingTest.CashOperations.Enums;
using EBANX.CodingTest.CashOperations.Interfaces;
using EBANX.CodingTest.CashOperations.Models;
using Hanssens.Net;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;

namespace EBANX.CodingTest.CashOperations.Services
{
    public class AccountService : IAccountService
    {
        private readonly int _notFoundStatus = (int)HttpStatusCode.NotFound;
        private readonly LocalStorage _localStorage = new LocalStorage();
        protected IList<Account> _accounts;

        public AccountService()
        {
            _accounts = _localStorage.Exists("accounts") ? _localStorage.Get<IList<Account>>("accounts") : new List<Account>();
        }

        public IActionResult HandleEvent(AccountEvent accountEvent)
        {
            IActionResult actionResult;

            switch (getEventType(accountEvent?.Type))
            {
                case EventTypes.Deposit:
                    var depositAccount = _accounts.FirstOrDefault(a => a.Id == accountEvent?.Origin);
                    if (depositAccount == null)
                        depositAccount = new Account(accountEvent.Origin, accountEvent.Amount);
                    else
                        depositAccount.MakeDeposit(accountEvent.Amount);
                    updateAccountList(depositAccount);
                    actionResult = new CreatedAtRouteResult(null, new { origin = depositAccount });
                    break;

                case EventTypes.Withdraw:
                    var withdrawAccount = _accounts.FirstOrDefault(a => a.Id == accountEvent?.Origin);
                    if (withdrawAccount == null) return new StatusCodeResult(_notFoundStatus);
                    else
                    {
                        withdrawAccount.MakeWithdraw(accountEvent.Amount);
                        updateAccountList(withdrawAccount);
                        actionResult = new CreatedAtRouteResult(null, new { origin = withdrawAccount });
                    }
                    break;

                case EventTypes.Transfer:
                    var fromAccount = _accounts.FirstOrDefault(a => a.Id == accountEvent?.Origin);
                    if (fromAccount == null) return new StatusCodeResult(_notFoundStatus);
                    else
                    {
                        var toAccount = _accounts.FirstOrDefault(a => a.Id == accountEvent?.Destination);
                        if (toAccount == null) return new StatusCodeResult((int)HttpStatusCode.NotFound);
                        fromAccount.MakeTransfer(accountEvent.Amount, ref toAccount);
                        updateAccountList(fromAccount);
                        updateAccountList(toAccount);
                        actionResult = new CreatedAtRouteResult(null, new { origin = fromAccount, destination = toAccount });
                    }
                    break;

                default:
                    return new StatusCodeResult(_notFoundStatus);
            }

            _localStorage.Store("accounts", _accounts);
            _localStorage.Persist();
            return actionResult;
        }

        public IActionResult GetBalance(int accountId)
        {
            var account = _accounts.FirstOrDefault(a => a.Id == accountId);
            if (account == null) return new StatusCodeResult(_notFoundStatus);
            return new OkObjectResult(account.Balance);
        }

        private void updateAccountList(Account account)
        {
            _accounts = _accounts.Where(a => a.Id != account.Id).ToList();
            _accounts.Add(account);
        }

        private EventTypes getEventType(string stringValue)
        {
            if (!typeof(EventTypes).IsEnum) throw new InvalidOperationException();
            foreach (var field in typeof(EventTypes).GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field,
                    typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attribute != null)
                    if (attribute.Description == stringValue.ToLower())
                        return (EventTypes)field.GetValue(null);
                    else
                    if (field.Name == stringValue.ToLower())
                        return (EventTypes)field.GetValue(null);
            }
            throw new ArgumentException("The Enum value could not be located.", nameof(stringValue));
        }
    }
}