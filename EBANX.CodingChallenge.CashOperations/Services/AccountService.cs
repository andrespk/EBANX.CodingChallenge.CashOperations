﻿using EBANX.CodingChallenge.CashOperations.DTOs;
using EBANX.CodingTest.CashOperations.Enums;
using EBANX.CodingTest.CashOperations.Interfaces;
using EBANX.CodingTest.CashOperations.Models;
using Hanssens.Net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace EBANX.CodingTest.CashOperations.Services
{
    public class AccountService : IAccountService
    {
        private readonly LocalStorage _localStorage = new LocalStorage(
            new LocalStorageConfiguration
            {
                AutoLoad = true,
                AutoSave = true,
                Filename = "tempdata.json"
            });

        protected IList<Account> _accounts;

        public AccountService()
        {
            _accounts = _localStorage.Exists("accounts") ?
                        _localStorage.Get<IList<Account>>("accounts") : new List<Account>();
        }

        public ResponseDTO HandleEvent(AccountEvent accountEvent)
        {
            var response = new ResponseDTO();

            switch (getEventType(accountEvent?.Type))
            {
                case EventTypes.Deposit:
                    var depositAccount = _accounts.FirstOrDefault(a => a.Id == accountEvent?.Destination);
                    if (depositAccount == null)
                        depositAccount = new Account(accountEvent.Destination, accountEvent.Amount);
                    else
                        depositAccount.MakeDeposit(accountEvent.Amount);
                    updateAccountList(depositAccount);
                    response.StatusCode = 201;
                    response.Data = new { destination = depositAccount };
                    break;

                case EventTypes.Withdraw:
                    var withdrawAccount = _accounts.FirstOrDefault(a => a.Id == accountEvent?.Origin);
                    if (withdrawAccount == null) return new ResponseDTO { StatusCode = 404, Data = 0 };
                    else
                    {
                        withdrawAccount.MakeWithdraw(accountEvent.Amount);
                        updateAccountList(withdrawAccount);
                        response.StatusCode = 201;
                        response.Data = new { origin = withdrawAccount };
                    }
                    break;

                case EventTypes.Transfer:
                    var fromAccount = _accounts.FirstOrDefault(a => a.Id == accountEvent?.Origin);
                    if (fromAccount == null) return new ResponseDTO { StatusCode = 404, Data = 0 };
                    else
                    {
                        var toAccount = _accounts.FirstOrDefault(a => a.Id == accountEvent?.Destination);
                        if (toAccount == null) toAccount = new Account(accountEvent.Destination);
                        fromAccount.MakeTransfer(accountEvent.Amount, ref toAccount);
                        updateAccountList(fromAccount);
                        updateAccountList(toAccount);
                        response.StatusCode = 201;
                        response.Data = new { origin = fromAccount, destination = toAccount };
                    }
                    break;

                default:
                    return new ResponseDTO { StatusCode = 404 };
            }
            _localStorage.Store("accounts", _accounts);
            _localStorage.Persist();
            return response;
        }

        public ResponseDTO GetBalance(string accountId)
        {
            var account = _accounts.FirstOrDefault(a => a.Id == accountId);
            if (account == null) return new ResponseDTO { StatusCode = 404, Data = 0 };
            return new ResponseDTO { StatusCode = 200, Data = account.Balance };
        }

        public void Reset()
        {
            _localStorage.Store("accounts", new List<Account>());
            _localStorage.Persist();
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