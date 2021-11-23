using GraphQL;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GraphQlSourceGenerator.Common.Interfaces
{
    public interface IGraphQlClient
    {
        /// <summary>Mutation gql-request</summary>
        /// <typeparam name="TResponse">Response type</typeparam>
        /// <param name="request">Request structure</param>
        Task<TResponse> MutateAsync<TResponse>(GraphQLRequest request);

        /// <summary>Query gql-request</summary>
        /// <typeparam name="TResponse">Response type</typeparam>
        /// <param name="request">Request structure</param>
        Task<TResponse> QueryAsync<TResponse>(GraphQLRequest request);

        /// <summary>Subscription gql-request</summary>
        /// <typeparam name="TSubscriptionResponse">Response type</typeparam>
        /// <param name="request">Request structure</param>
        /// <param name="onNextCallBack">Subscription event callback</param>
        /// <param name="onErrorSubscriptionCallBack">Subscription error event callback</param>
        /// <param name="onCompletedSubscriptionCallBack">Subscription completing callback</param>
        /// <param name="unsubscriber">Subscription cancellation token</param>
        void Subscribe<TSubscriptionResponse>(GraphQLRequest request,
                                              Action<GraphQLResponse<TSubscriptionResponse>> onNextCallBack,
                                              Action<Exception> onErrorSubscriptionCallBack,
                                              Action onCompletedSubscriptionCallBack,
                                              CancellationToken unsubscriber);
    }
}
