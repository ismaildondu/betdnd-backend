using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BetDND.Enums;

namespace BetDND.Models
{
    public class AddBulk {
        public AddCategory AddCategory { get; set; }
        public List<AddOptionsBulk> AddOptionsBulk { get; set; }
    }

    public class AddOptionsBulk {
        public string Description { get; set; }
        public string Name { get; set; }
        public List<AddSelectBulk> AddSelects { get; set; }
    }

    public class AddSelectBulk {
        public string Name { get; set; }
        public double Odd { get; set; }
    }
}