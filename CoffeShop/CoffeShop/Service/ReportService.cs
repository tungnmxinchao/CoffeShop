using CoffeShop.DTOs;
using CoffeShop.Models;
using Microsoft.EntityFrameworkCore;

namespace CoffeShop.Service
{
	public class ReportService
	{

		public List<ProfitLossReport> GenerateProfitLossReport()
		{

			var materialCosts = from pi in CoffeShopContext.Ins.ProductIngredients
								join i in CoffeShopContext.Ins.Inventories on pi.ItemId equals i.ItemId
								group new { pi, i } by pi.MenuId into g
								select new
								{
									MenuId = g.Key,
									TotalMaterialCost = g.Sum(x => (x.pi.QuantityPerProduct / 1000.0m) * x.i.Price)
								};


			var orderRevenues = from o in CoffeShopContext.Ins.Orders
								where o.Status == "Confirmed"
								select new
								{
									o.OrderId,
									o.TotalPrice
								};


			var orderCosts = from od in CoffeShopContext.Ins.OrderDetails
							 join mc in materialCosts on od.MenuId equals mc.MenuId
							 group new { od, mc } by od.OrderId into g
							 select new
							 {
								 OrderId = g.Key,
								 TotalMaterialCost = g.Sum(x => x.od.Quantity * x.mc.TotalMaterialCost)
							 };

			var report = from orv in orderRevenues
						 join oc in orderCosts on orv.OrderId equals oc.OrderId
						 select new ProfitLossReport
						 {
							 OrderId = orv.OrderId,
							 TotalRevenue = orv.TotalPrice ?? 0,
							 TotalMaterialCost = oc.TotalMaterialCost ?? 0, 
							 ProfitLoss = (orv.TotalPrice ?? 0) - (oc.TotalMaterialCost ?? 0) 
						 };

			return report.ToList();
		}

		public List<ProfitLossReportPerDay> GenerateProfitLossReportPerDay()
		{
		
			var materialCosts = from pi in CoffeShopContext.Ins.ProductIngredients
								join i in CoffeShopContext.Ins.Inventories on pi.ItemId equals i.ItemId
								group new { pi, i } by pi.MenuId into g
								select new
								{
									MenuId = g.Key,
									TotalMaterialCost = g.Sum(x => (x.pi.QuantityPerProduct / 1000.0m) * x.i.Price)
								};


			var orderRevenues = from o in CoffeShopContext.Ins.Orders
								where o.Status == "Confirmed"
								select new
								{
									o.OrderId,
									o.TotalPrice,							
									OrderDate = o.CreatedAt.HasValue ? o.CreatedAt.Value.Date : (DateTime?)null
								};


			var orderCosts = from od in CoffeShopContext.Ins.OrderDetails
							 join mc in materialCosts on od.MenuId equals mc.MenuId
							 group new { od, mc } by od.OrderId into g
							 select new
							 {
								 OrderId = g.Key,
								 TotalMaterialCost = g.Sum(x => x.od.Quantity * x.mc.TotalMaterialCost)
							 };

		
			var report = from orv in orderRevenues
						 join oc in orderCosts on orv.OrderId equals oc.OrderId
						 group new { orv, oc } by orv.OrderDate into g
						 select new ProfitLossReportPerDay
						 {
							 OrderDate = g.Key,
							 TotalRevenue = g.Sum(x => x.orv.TotalPrice ?? 0),
							 TotalMaterialCost = g.Sum(x => x.oc.TotalMaterialCost ?? 0),
							 ProfitLoss = g.Sum(x => (x.orv.TotalPrice ?? 0) - (x.oc.TotalMaterialCost ?? 0))
						 };

			return report.ToList();
		}

	}
}
