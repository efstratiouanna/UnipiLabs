﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UnipiLabs.ViewModels
{
    public class ListModel<T>
    {
        public List<T> Items { get; set; }

        public ListModel() {
            Items = new List<T>();
        }
        public ListModel(List<T> list)
        {
            Items = list;
        }
    }
}