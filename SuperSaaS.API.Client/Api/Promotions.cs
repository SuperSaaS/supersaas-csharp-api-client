using System;
using SuperSaaS.API.Models;

namespace SuperSaaS.API.Api
{
    public class Promotions : BaseApi
    {
        public Promotions(IClient client) : base(client)
        {
        }

        public Promotion[] List(int? limit = null, int? offset = null)
        {
            string path = "/promotions";
            var parameters = new JsonArgs();

            if (limit.HasValue) parameters.Add("limit", ValidateNumber(limit.Value).ToString());
            if (offset.HasValue) parameters.Add("offset", ValidateNumber(offset.Value).ToString());

            return this.Client.Get<Promotion[]>(path, parameters) ?? Array.Empty<Promotion>();
        }

        public Promotion[] GetPromotion(string promotionCode)
        {
            string path = "/promotions";
            var query = new JsonArgs
            {
                { "promotion_code", ValidatePromotion(promotionCode) }
            };

            return this.Client.Get<Promotion[]>(path, query) ?? Array.Empty<Promotion>();
        }

        public void DuplicatePromotionCode(string promotionCode, string templateCode)
        {
            string path = "/promotions";
            var query = new JsonArgs
            {
                { "id", ValidatePromotion(promotionCode) },
                { "template_code", ValidatePromotion(templateCode) }
            };

            this.Client.Post<object>(path, new NestedJsonArgs(), query);
        }
    }
}
