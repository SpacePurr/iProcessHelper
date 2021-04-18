﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iProcessHelper.Models
{
    public class OperationType
    {
        public string Name { get; set; }
        public int Value { get; set; }

        public OperationType(string name, int value)
        {
            Name = name;
            Value = value;
        }
    }
}
