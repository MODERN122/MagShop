
using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using ApplicationCore.Interfaces;

namespace ApplicationCore.Entities
{
    public class Store : BaseDateTimeEntity
    {
        public Store()
        {

        }
        public Store(string id, string sellerId, string name, string approveDocument = null)
        {
            Id = id;
            ApproveDocument = approveDocument;
            SellerId = sellerId;
            Name = name;
        }
        /// <summary>
        /// Use for real operations
        /// </summary>
        /// <param name="sellerId">Идентфикатор продавца</param>
        /// <param name="name">Имя магазина</param>
        /// <param name="approveDocument">Подтверждающий документ (путь до файла в хранилище)</param>
        public Store(string sellerId, string name, string approveDocument)
        {
            SellerId = sellerId;
            Name = name;
            ApproveDocument = approveDocument;
        }
        public string Id { get; set; }
        public string Name { get; set; }
        public float Rating { get; set; }
        public List<Product> StoreProducts { get; set; }
        public string SellerId { get; set; }
        public string ApproveDocument { get; set; }
        [JsonIgnore]
        public Seller Seller { get; set; }


    }
}
