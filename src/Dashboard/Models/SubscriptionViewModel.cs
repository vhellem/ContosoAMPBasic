﻿using System;

namespace Dashboard.Models
{
    using System.Collections.Generic;

    using Dashboard.Marketplace;

    using SaaSFulfillmentClient.Models;

    public class SubscriptionViewModel : MarketplaceSubscription
    {
        public IEnumerable<ActionsEnum> NextActions
        {
            get
            {
                switch (this.State)
                {
                    case StatusEnum.Provisioning:
                        return new List<ActionsEnum> { ActionsEnum.Activate };

                    case StatusEnum.Subscribed:
                        return new List<ActionsEnum> { ActionsEnum.Update, ActionsEnum.Unsubscribe };

                    case StatusEnum.Suspended:
                        return new List<ActionsEnum>();

                    case StatusEnum.Unsubscribed:
                        return new List<ActionsEnum>();

                    case StatusEnum.NotStarted:
                        break;

                    case StatusEnum.PendingFulfillmentStart:
                        break;

                    default:
                        return new List<ActionsEnum>();
                }

                return new List<ActionsEnum>();
            }
        }

        public bool PendingOperations { get; set; }

        public static SubscriptionViewModel FromSubscription(Subscription marketplaceSubscription)
        {
            return new SubscriptionViewModel
            {
                PlanId = marketplaceSubscription.PlanId,
                Quantity = marketplaceSubscription.Quantity,
                SubscriptionId = marketplaceSubscription.SubscriptionId,
                OfferId = marketplaceSubscription.OfferId,
                State = marketplaceSubscription.SaasSubscriptionStatus,
                SubscriptionName = marketplaceSubscription.Name,
                PurchaserEmail = marketplaceSubscription.Purchaser.EmailId,
                PurchaserTenantId = marketplaceSubscription.Purchaser.TenantId,
                TermStart = marketplaceSubscription.Term != null 
                    && marketplaceSubscription.Term.StartDate != DateTimeOffset.MinValue ? 
                        marketplaceSubscription.Term.StartDate.ToString("d") : 
                        string.Empty,
                TermEnd = marketplaceSubscription.Term != null && 
                    marketplaceSubscription.Term.EndDate != DateTimeOffset.MinValue ? 
                        marketplaceSubscription.Term.EndDate.ToString("d") : 
                        string.Empty,
                TermUnit = marketplaceSubscription.Term.TermUnit == TermUnitEnum.P1M ? "Monthly" : "Annual"
            };
        }

        public Guid PurchaserTenantId { get; set; }

        public string PurchaserEmail { get; set; }
        public bool ExistingOperations { get; internal set; }
        public int OperationCount { get; internal set; }

        public string TermStart { get; private set; }
        public string TermEnd { get; private set; }
        public string TermUnit { get; private set; }
    }
}