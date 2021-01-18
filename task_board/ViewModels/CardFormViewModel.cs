using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using task_board.Models.EntityFramework;

namespace task_board.ViewModels
{
    public class CardFormViewModel
    {
        public IEnumerable<tblProje> Projeler { get; set; }
        public IEnumerable<tblPersonel> Personeller { get; set; }
        public tblKart Kart { get; set; }

    }
}