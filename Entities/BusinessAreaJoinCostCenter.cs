using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApi.Interface;

namespace WebApi.Entities
{
    public class BusinessAreaJoinCostCenter : IEntity
    {
        public int BusinessAreaID { get; set; }
        public int CostCenterID { get; set; }
    }
}