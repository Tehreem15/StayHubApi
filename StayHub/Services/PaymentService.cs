using StayHub.Data.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient.DataClassification;
using Microsoft.Extensions.Configuration;

using Stripe;
using System;
using System.Collections.Generic;
using System.Drawing;
namespace StayHub.Services
{
    public class PaymentService
    {
       
        public PaymentService() {
          
        }
       
        public Charge PayWithStripe(PayWithStripeViewModel model)
        {
            _ = new Charge();

            TokenCreateOptions options = new TokenCreateOptions
            {
                Card = new TokenCardOptions
                {
                    Name = model.NameOnCard,
                    Number = model.CardNumber,
                    ExpMonth = model.ExpiryMonth,
                    ExpYear =model.ExpiryYear,
                    Cvc = model.CVV,
                },
            };
            TokenService service = new TokenService();
            Token GetTokenResponse = service.Create(options);

            //create customer object then register customer on Stripe  

            Stripe.CustomerCreateOptions customer = new Stripe.CustomerCreateOptions
            {
                Email = model.Email,

                Source = GetTokenResponse.Id
            };

            CustomerService custService = new Stripe.CustomerService();

            Stripe.Customer stripeCustomer = custService.Create(customer);

            //create credit card charge object with details of charge  

            ChargeCreateOptions PayOptions = new Stripe.ChargeCreateOptions
            {
                Amount = (long)Convert.ToDouble(model.price * 100),

                //For Live Testing
                //Amount = (long)Convert.ToDouble(.50 * 100),

                Currency = model.Currency,

                ReceiptEmail = model.Email,

                Customer = stripeCustomer.Id,

                Description = Convert.ToString(model.ReferenceNumber), //Optional  

            };

            //and Create Method of this object is doing the payment execution.  

            ChargeService PayService = new Stripe.ChargeService();

            Charge charge = PayService.Create(PayOptions);
            return charge;

        }

        public async Task<Charge> PayWithStripeAsync(PayWithStripeViewModel model)
        {
            TokenCreateOptions options = new TokenCreateOptions
            {
                Card = new TokenCardOptions
                {
                    Name = model.NameOnCard,
                    Number = model.CardNumber,
                    ExpMonth = model.ExpiryMonth,
                    ExpYear =model.ExpiryYear,
                    Cvc = model.CVV,
                },
            };
            TokenService service = new TokenService();
            var GetTokenResponse = await service.CreateAsync(options);

            //create customer object then register customer on Stripe  

            CustomerCreateOptions customer = new CustomerCreateOptions
            {
                Email = model.Email,
                Source = GetTokenResponse.Id
            };

            CustomerService custService = new CustomerService();

            Customer stripeCustomer = await custService.CreateAsync(customer);

            //create credit card charge object with details of charge  

            ChargeCreateOptions PayOptions = new ChargeCreateOptions
            {
                Amount = Convert.ToInt64(model.price * 100),

                //For Live Testing
                //Amount = (long)Convert.ToDouble(.50 * 100),

                Currency = model.Currency,

                ReceiptEmail = model.Email,

                Customer = stripeCustomer.Id,

                Description = Convert.ToString(model.ReferenceNumber), //Optional  

            };

            //and Create Method of this object is doing the payment execution.  

            ChargeService PayService = new ChargeService();

            return await PayService.CreateAsync(PayOptions);

        }


       
    }
}
