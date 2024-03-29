﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVCWebUI.Entity;

namespace MVCWebUI.Models
{
    public class Cart
    {
        private List<CartLine> _cartLines = new List<CartLine>();

        public List<CartLine> CartLines
        {
            get { return _cartLines; }
        }

        public void AddProduct(Product product, int quantity)
        {
            var line = _cartLines.FirstOrDefault(x => x.Product.Id == product.Id);

            if (line == null)
            {
                _cartLines.Add(new CartLine()
                {
                    Product = product,
                    Quantity = quantity
                });
            }
            else
            {
                line.Quantity += quantity;
            }
        }

        public void DeleteProduct(Product product)
        {
            _cartLines.RemoveAll(x => x.Product.Id == product.Id);
        }

        public double Total()
        {
            return _cartLines.Sum(x => x.Product.Price * x.Quantity);
        }

        public void Clear()
        {
            _cartLines.Clear();
        }
    }

    public class CartLine
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}