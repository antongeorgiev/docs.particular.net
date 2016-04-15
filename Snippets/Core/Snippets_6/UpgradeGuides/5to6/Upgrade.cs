﻿namespace Core6.UpgradeGuides._5to6
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Transactions;
    using NServiceBus;
    using NServiceBus.ConsistencyGuarantees;
    using NServiceBus.MessageMutator;
    using NServiceBus.Settings;

    class Upgrade
    {
        #region 5to6ReAddWinIdNameHeader

        public class WinIdNameMutator : IMutateOutgoingTransportMessages
        {
            public Task MutateOutgoing(MutateOutgoingTransportMessageContext context)
            {
                context.OutgoingHeaders["WinIdName"] = Thread.CurrentPrincipal.Identity.Name;
                return Task.FromResult(0);
            }
        }

        #endregion

        void StaticHeaders(EndpointConfiguration endpointConfiguration)
        {
            #region 5to6header-static-endpoint

            endpointConfiguration.AddHeaderToAllOutgoingMessages("MyGlobalHeader", "some static value");

            #endregion
        }


        void TransportTransactions(EndpointConfiguration endpointConfiguration)
        {
            #region 5to6DoNotWrapHandlersInTransaction

            endpointConfiguration.UseTransport<MyTransport>()
                .Transactions(TransportTransactionMode.ReceiveOnly);

            #endregion
        }


        void CriticalError(EndpointConfiguration endpointConfiguration)
        {
            // ReSharper disable RedundantDelegateCreation
            // ReSharper disable ConvertToLambdaExpression

            #region 5to6CriticalError

            endpointConfiguration.DefineCriticalErrorAction(
                new Func<ICriticalErrorContext, Task>(context =>
                {
                    // place you custom handling here 
                    return Task.FromResult(0);
                }));

            #endregion

            // ReSharper restore RedundantDelegateCreation
            // ReSharper restore ConvertToLambdaExpression
        }

        void SuppressDistributedTransactions(ReadOnlySettings readOnlySettings)
        {
            #region 5to6SuppressDistributedTransactions

            bool suppressDistributedTransactions = readOnlySettings.GetRequiredTransactionModeForReceives() != TransportTransactionMode.TransactionScope;

            #endregion
        }

        void IsTransactional(ReadOnlySettings readOnlySettings)
        {
            #region 5to6IsTransactional

            bool isTransactional = readOnlySettings.GetRequiredTransactionModeForReceives() != TransportTransactionMode.None;

            #endregion
        }

        void TransportTransactionIsolationLevelAndTimeout(EndpointConfiguration endpointConfiguration)
        {
            #region 5to6TransportTransactionScopeOptions

            endpointConfiguration.UseTransport<MyTransport>()
                .Transactions(TransportTransactionMode.TransactionScope)
                .TransactionScopeOptions(isolationLevel: IsolationLevel.RepeatableRead, timeout: TimeSpan.FromSeconds(30));

            #endregion
        }

        void WrapHandlersExecutionInATransactionScope(EndpointConfiguration endpointConfiguration)
        {
            #region 5to6WrapHandlersExecutionInATransactionScope

            endpointConfiguration.UnitOfWork()
                .WrapHandlersInATransactionScope();

            #endregion
        }

        async Task DelayedDelivery(IMessageHandlerContext handlerContext, object message)
        {
            #region 5to6delayed-delivery

            SendOptions sendOptions = new SendOptions();
            sendOptions.DelayDeliveryWith(TimeSpan.FromMinutes(30));
            //OR
            sendOptions.DoNotDeliverBefore(new DateTime(2016, 12, 25));

            await handlerContext.Send(message, sendOptions);

            #endregion
        }

        void EnableTransactions(EndpointConfiguration endpointConfiguration)
        {
            #region 5to6EnableTransactions

            // Using a transport will enable transactions automatically.
            endpointConfiguration.UseTransport<MyTransport>();

            #endregion
        }

        void DisableTransactions(EndpointConfiguration endpointConfiguration)
        {
            #region 5to6DisableTransactions

            endpointConfiguration.UseTransport<MyTransport>()
                .Transactions(TransportTransactionMode.None);

            #endregion
        }

        void EnableDistributedTransactions(EndpointConfiguration endpointConfiguration)
        {
            #region 5to6EnableDistributedTransactions

            endpointConfiguration.UseTransport<MyTransport>()
                .Transactions(TransportTransactionMode.TransactionScope);

            #endregion
        }

        void DisableDistributedTransactions(EndpointConfiguration endpointConfiguration)
        {
            #region 5to6DisableDistributedTransactions

            endpointConfiguration.UseTransport<MyTransport>()
                .Transactions(TransportTransactionMode.ReceiveOnly);

            #endregion
        }

        void DisableDistributedTransactionsNative(EndpointConfiguration endpointConfiguration)
        {
            #region 5to6DisableDistributedTransactionsNative

            endpointConfiguration.UseTransport<MyTransport>()
                .Transactions(TransportTransactionMode.SendsAtomicWithReceive);

            #endregion
        }
    }
}
