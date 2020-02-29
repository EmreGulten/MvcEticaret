﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCWebUI.Entity
{
    public class Category
    {
        public int Id { get; set; }

        [DisplayName("Kategori Adı")]
        [StringLength(maximumLength:20,ErrorMessage="En Fazla 20 karakter")]
        public string Name { get; set; }

        [DisplayName("Açıklama")]
        public string Description { get; set; }

        public List<Product> Products { get; set; }
    }
}