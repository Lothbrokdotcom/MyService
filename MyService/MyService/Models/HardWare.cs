namespace MyService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class HardWare
    {
        public int id { get; set; }

        public string pc_name { get; set; }

        public string manufacturer { get; set; }

        public string users { get; set; }

        public int? cpu { get; set; }

        public int? ram { get; set; }
        public string date { get; set; }
    }
}
