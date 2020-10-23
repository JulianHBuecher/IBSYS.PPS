using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace IBSYS.PPS.Models.Generated
{

		[XmlRoot(ElementName = "forecast")]
		public class Forecast
		{
			[XmlAttribute(AttributeName = "p1")]
			public string P1 { get; set; }
			[XmlAttribute(AttributeName = "p2")]
			public string P2 { get; set; }
			[XmlAttribute(AttributeName = "p3")]
			public string P3 { get; set; }
		}

		[XmlRoot(ElementName = "article")]
		public class Article
		{
			[XmlAttribute(AttributeName = "id")]
			public string Id { get; set; }
			[XmlAttribute(AttributeName = "amount")]
			public string Amount { get; set; }
			[XmlAttribute(AttributeName = "startamount")]
			public string Startamount { get; set; }
			[XmlAttribute(AttributeName = "pct")]
			public string Pct { get; set; }
			[XmlAttribute(AttributeName = "price")]
			public string Price { get; set; }
			[XmlAttribute(AttributeName = "stockvalue")]
			public string Stockvalue { get; set; }
		}

		[XmlRoot(ElementName = "warehousestock")]
		public class Warehousestock
		{
			[XmlElement(ElementName = "article")]
			public List<Article> Article { get; set; }
			[XmlElement(ElementName = "totalstockvalue")]
			public string Totalstockvalue { get; set; }
		}

		[XmlRoot(ElementName = "order")]
		public class Order
		{
			[XmlAttribute(AttributeName = "orderperiod")]
			public string Orderperiod { get; set; }
			[XmlAttribute(AttributeName = "id")]
			public string Id { get; set; }
			[XmlAttribute(AttributeName = "mode")]
			public string Mode { get; set; }
			[XmlAttribute(AttributeName = "article")]
			public string Article { get; set; }
			[XmlAttribute(AttributeName = "amount")]
			public string Amount { get; set; }
			[XmlAttribute(AttributeName = "time")]
			public string Time { get; set; }
			[XmlAttribute(AttributeName = "materialcosts")]
			public string Materialcosts { get; set; }
			[XmlAttribute(AttributeName = "ordercosts")]
			public string Ordercosts { get; set; }
			[XmlAttribute(AttributeName = "entirecosts")]
			public string Entirecosts { get; set; }
			[XmlAttribute(AttributeName = "piececosts")]
			public string Piececosts { get; set; }
			[XmlElement(ElementName = "batch")]
			public List<Batch> Batch { get; set; }
			[XmlAttribute(AttributeName = "period")]
			public string Period { get; set; }
			[XmlAttribute(AttributeName = "item")]
			public string Item { get; set; }
			[XmlAttribute(AttributeName = "quantity")]
			public string Quantity { get; set; }
			[XmlAttribute(AttributeName = "cost")]
			public string Cost { get; set; }
			[XmlAttribute(AttributeName = "averageunitcosts")]
			public string Averageunitcosts { get; set; }
		}

		[XmlRoot(ElementName = "inwardstockmovement")]
		public class Inwardstockmovement
		{
			[XmlElement(ElementName = "order")]
			public List<Order> Order { get; set; }
		}

		[XmlRoot(ElementName = "futureinwardstockmovement")]
		public class Futureinwardstockmovement
		{
			[XmlElement(ElementName = "order")]
			public List<Order> Order { get; set; }
		}

	[XmlRoot(ElementName = "workplace")]
	public class Workplace
	{
		[XmlAttribute(AttributeName = "id")]
		public string Id { get; set; }
		[XmlAttribute(AttributeName = "setupevents")]
		public string Setupevents { get; set; }
		[XmlAttribute(AttributeName = "idletime")]
		public string Idletime { get; set; }
		[XmlAttribute(AttributeName = "wageidletimecosts")]
		public string Wageidletimecosts { get; set; }
		[XmlAttribute(AttributeName = "wagecosts")]
		public string Wagecosts { get; set; }
		[XmlAttribute(AttributeName = "machineidletimecosts")]
		public string Machineidletimecosts { get; set; }
		[XmlAttribute(AttributeName = "timeneed")]
		public string Timeneed { get; set; }
		[XmlElement(ElementName = "waitinglist")]
		public List<Waitinglist> Waitinglist { get; set; }
		[XmlAttribute(AttributeName = "period")]
		public string Period { get; set; }
		[XmlAttribute(AttributeName = "order")]
		public string Order { get; set; }
		[XmlAttribute(AttributeName = "batch")]
		public string Batch { get; set; }
		[XmlAttribute(AttributeName = "item")]
		public string Item { get; set; }
		[XmlAttribute(AttributeName = "amount")]
		public string Amount { get; set; }
	}

	[XmlRoot(ElementName = "waitinglist")]
	public class Waitinglist
	{
		[XmlAttribute(AttributeName = "period")]
		public string Period { get; set; }
		[XmlAttribute(AttributeName = "order")]
		public string Order { get; set; }
		[XmlAttribute(AttributeName = "firstbatch")]
		public string Firstbatch { get; set; }
		[XmlAttribute(AttributeName = "lastbatch")]
		public string Lastbatch { get; set; }
		[XmlAttribute(AttributeName = "item")]
		public string Item { get; set; }
		[XmlAttribute(AttributeName = "amount")]
		public string Amount { get; set; }
		[XmlAttribute(AttributeName = "timeneed")]
		public string Timeneed { get; set; }
	}

	[XmlRoot(ElementName = "sum")]
		public class Sum
		{
			[XmlAttribute(AttributeName = "setupevents")]
			public string Setupevents { get; set; }
			[XmlAttribute(AttributeName = "idletime")]
			public string Idletime { get; set; }
			[XmlAttribute(AttributeName = "wageidletimecosts")]
			public string Wageidletimecosts { get; set; }
			[XmlAttribute(AttributeName = "wagecosts")]
			public string Wagecosts { get; set; }
			[XmlAttribute(AttributeName = "machineidletimecosts")]
			public string Machineidletimecosts { get; set; }
		}

		[XmlRoot(ElementName = "idletimecosts")]
		public class Idletimecosts
		{
			[XmlElement(ElementName = "workplace")]
			public List<Workplace> Workplace { get; set; }
			[XmlElement(ElementName = "sum")]
			public Sum Sum { get; set; }
			[XmlAttribute(AttributeName = "current")]
			public string Current { get; set; }
			[XmlAttribute(AttributeName = "average")]
			public string Average { get; set; }
			[XmlAttribute(AttributeName = "all")]
			public string All { get; set; }
		}

		[XmlRoot(ElementName = "waitinglistworkstations")]
		public class Waitinglistworkstations
		{
			[XmlElement(ElementName = "workplace")]
			public List<Workplace> Workplace { get; set; }
		}

		[XmlRoot(ElementName = "missingpart")]
		public class Missingpart
		{
			[XmlAttribute(AttributeName = "id")]
			public string Id { get; set; }
		}

		[XmlRoot(ElementName = "waitingliststock")]
		public class Waitingliststock
		{
			[XmlElement(ElementName = "missingpart")]
			public List<Missingpart> Missingpart { get; set; }
		}

		[XmlRoot(ElementName = "batch")]
		public class Batch
		{
			[XmlAttribute(AttributeName = "id")]
			public string Id { get; set; }
			[XmlAttribute(AttributeName = "amount")]
			public string Amount { get; set; }
			[XmlAttribute(AttributeName = "cycletime")]
			public string Cycletime { get; set; }
			[XmlAttribute(AttributeName = "cost")]
			public string Cost { get; set; }
		}

		[XmlRoot(ElementName = "completedorders")]
		public class Completedorders
		{
			[XmlElement(ElementName = "order")]
			public List<Order> Order { get; set; }
		}

		[XmlRoot(ElementName = "cycletimes")]
		public class Cycletimes
		{
			[XmlAttribute(AttributeName = "startedorders")]
			public string Startedorders { get; set; }
			[XmlAttribute(AttributeName = "waitingorders")]
			public string Waitingorders { get; set; }
		}

		[XmlRoot(ElementName = "capacity")]
		public class Capacity
		{
			[XmlAttribute(AttributeName = "current")]
			public string Current { get; set; }
			[XmlAttribute(AttributeName = "average")]
			public string Average { get; set; }
			[XmlAttribute(AttributeName = "all")]
			public string All { get; set; }
		}

		[XmlRoot(ElementName = "possiblecapacity")]
		public class Possiblecapacity
		{
			[XmlAttribute(AttributeName = "current")]
			public string Current { get; set; }
			[XmlAttribute(AttributeName = "average")]
			public string Average { get; set; }
			[XmlAttribute(AttributeName = "all")]
			public string All { get; set; }
		}

		[XmlRoot(ElementName = "relpossiblenormalcapacity")]
		public class Relpossiblenormalcapacity
		{
			[XmlAttribute(AttributeName = "current")]
			public string Current { get; set; }
			[XmlAttribute(AttributeName = "average")]
			public string Average { get; set; }
			[XmlAttribute(AttributeName = "all")]
			public string All { get; set; }
		}

		[XmlRoot(ElementName = "productivetime")]
		public class Productivetime
		{
			[XmlAttribute(AttributeName = "current")]
			public string Current { get; set; }
			[XmlAttribute(AttributeName = "average")]
			public string Average { get; set; }
			[XmlAttribute(AttributeName = "all")]
			public string All { get; set; }
		}

		[XmlRoot(ElementName = "effiency")]
		public class Effiency
		{
			[XmlAttribute(AttributeName = "current")]
			public string Current { get; set; }
			[XmlAttribute(AttributeName = "average")]
			public string Average { get; set; }
			[XmlAttribute(AttributeName = "all")]
			public string All { get; set; }
		}

		[XmlRoot(ElementName = "sellwish")]
		public class Sellwish
		{
			[XmlAttribute(AttributeName = "current")]
			public string Current { get; set; }
			[XmlAttribute(AttributeName = "average")]
			public string Average { get; set; }
			[XmlAttribute(AttributeName = "all")]
			public string All { get; set; }
		}

		[XmlRoot(ElementName = "salesquantity")]
		public class Salesquantity
		{
			[XmlAttribute(AttributeName = "current")]
			public string Current { get; set; }
			[XmlAttribute(AttributeName = "average")]
			public string Average { get; set; }
			[XmlAttribute(AttributeName = "all")]
			public string All { get; set; }
		}

		[XmlRoot(ElementName = "deliveryreliability")]
		public class Deliveryreliability
		{
			[XmlAttribute(AttributeName = "current")]
			public string Current { get; set; }
			[XmlAttribute(AttributeName = "average")]
			public string Average { get; set; }
			[XmlAttribute(AttributeName = "all")]
			public string All { get; set; }
		}

		[XmlRoot(ElementName = "idletime")]
		public class Idletime
		{
			[XmlAttribute(AttributeName = "current")]
			public string Current { get; set; }
			[XmlAttribute(AttributeName = "average")]
			public string Average { get; set; }
			[XmlAttribute(AttributeName = "all")]
			public string All { get; set; }
		}

		[XmlRoot(ElementName = "storevalue")]
		public class Storevalue
		{
			[XmlAttribute(AttributeName = "current")]
			public string Current { get; set; }
			[XmlAttribute(AttributeName = "average")]
			public string Average { get; set; }
			[XmlAttribute(AttributeName = "all")]
			public string All { get; set; }
		}

		[XmlRoot(ElementName = "storagecosts")]
		public class Storagecosts
		{
			[XmlAttribute(AttributeName = "current")]
			public string Current { get; set; }
			[XmlAttribute(AttributeName = "average")]
			public string Average { get; set; }
			[XmlAttribute(AttributeName = "all")]
			public string All { get; set; }
		}

		[XmlRoot(ElementName = "general")]
		public class General
		{
			[XmlElement(ElementName = "capacity")]
			public Capacity Capacity { get; set; }
			[XmlElement(ElementName = "possiblecapacity")]
			public Possiblecapacity Possiblecapacity { get; set; }
			[XmlElement(ElementName = "relpossiblenormalcapacity")]
			public Relpossiblenormalcapacity Relpossiblenormalcapacity { get; set; }
			[XmlElement(ElementName = "productivetime")]
			public Productivetime Productivetime { get; set; }
			[XmlElement(ElementName = "effiency")]
			public Effiency Effiency { get; set; }
			[XmlElement(ElementName = "sellwish")]
			public Sellwish Sellwish { get; set; }
			[XmlElement(ElementName = "salesquantity")]
			public Salesquantity Salesquantity { get; set; }
			[XmlElement(ElementName = "deliveryreliability")]
			public Deliveryreliability Deliveryreliability { get; set; }
			[XmlElement(ElementName = "idletime")]
			public Idletime Idletime { get; set; }
			[XmlElement(ElementName = "idletimecosts")]
			public Idletimecosts Idletimecosts { get; set; }
			[XmlElement(ElementName = "storevalue")]
			public Storevalue Storevalue { get; set; }
			[XmlElement(ElementName = "storagecosts")]
			public Storagecosts Storagecosts { get; set; }
		}

		[XmlRoot(ElementName = "quantity")]
		public class Quantity
		{
			[XmlAttribute(AttributeName = "current")]
			public string Current { get; set; }
			[XmlAttribute(AttributeName = "average")]
			public string Average { get; set; }
			[XmlAttribute(AttributeName = "all")]
			public string All { get; set; }
		}

		[XmlRoot(ElementName = "costs")]
		public class Costs
		{
			[XmlAttribute(AttributeName = "current")]
			public string Current { get; set; }
			[XmlAttribute(AttributeName = "average")]
			public string Average { get; set; }
			[XmlAttribute(AttributeName = "all")]
			public string All { get; set; }
		}

		[XmlRoot(ElementName = "defectivegoods")]
		public class Defectivegoods
		{
			[XmlElement(ElementName = "quantity")]
			public Quantity Quantity { get; set; }
			[XmlElement(ElementName = "costs")]
			public Costs Costs { get; set; }
		}

		[XmlRoot(ElementName = "salesprice")]
		public class Salesprice
		{
			[XmlAttribute(AttributeName = "current")]
			public string Current { get; set; }
			[XmlAttribute(AttributeName = "average")]
			public string Average { get; set; }
			[XmlAttribute(AttributeName = "all")]
			public string All { get; set; }
		}

		[XmlRoot(ElementName = "profit")]
		public class Profit
		{
			[XmlAttribute(AttributeName = "current")]
			public string Current { get; set; }
			[XmlAttribute(AttributeName = "average")]
			public string Average { get; set; }
			[XmlAttribute(AttributeName = "all")]
			public string All { get; set; }
		}

		[XmlRoot(ElementName = "profitperunit")]
		public class Profitperunit
		{
			[XmlAttribute(AttributeName = "current")]
			public string Current { get; set; }
			[XmlAttribute(AttributeName = "average")]
			public string Average { get; set; }
			[XmlAttribute(AttributeName = "all")]
			public string All { get; set; }
		}

		[XmlRoot(ElementName = "normalsale")]
		public class Normalsale
		{
			[XmlElement(ElementName = "salesprice")]
			public Salesprice Salesprice { get; set; }
			[XmlElement(ElementName = "profit")]
			public Profit Profit { get; set; }
			[XmlElement(ElementName = "profitperunit")]
			public Profitperunit Profitperunit { get; set; }
		}

		[XmlRoot(ElementName = "contractpenalty")]
		public class Contractpenalty
		{
			[XmlAttribute(AttributeName = "current")]
			public string Current { get; set; }
			[XmlAttribute(AttributeName = "average")]
			public string Average { get; set; }
			[XmlAttribute(AttributeName = "all")]
			public string All { get; set; }
		}

		[XmlRoot(ElementName = "directsale")]
		public class Directsale
		{
			[XmlElement(ElementName = "profit")]
			public Profit Profit { get; set; }
			[XmlElement(ElementName = "contractpenalty")]
			public Contractpenalty Contractpenalty { get; set; }
		}

		[XmlRoot(ElementName = "marketplacesale")]
		public class Marketplacesale
		{
			[XmlElement(ElementName = "profit")]
			public Profit Profit { get; set; }
		}

		[XmlRoot(ElementName = "summary")]
		public class Summary
		{
			[XmlElement(ElementName = "profit")]
			public Profit Profit { get; set; }
		}

		[XmlRoot(ElementName = "result")]
		public class Result
		{
			[XmlElement(ElementName = "general")]
			public General General { get; set; }
			[XmlElement(ElementName = "defectivegoods")]
			public Defectivegoods Defectivegoods { get; set; }
			[XmlElement(ElementName = "normalsale")]
			public Normalsale Normalsale { get; set; }
			[XmlElement(ElementName = "directsale")]
			public Directsale Directsale { get; set; }
			[XmlElement(ElementName = "marketplacesale")]
			public Marketplacesale Marketplacesale { get; set; }
			[XmlElement(ElementName = "summary")]
			public Summary Summary { get; set; }
		}

		[XmlRoot(ElementName = "ordersinwork")]
		public class Ordersinwork
		{
			[XmlElement(ElementName = "workplace")]
			public List<Workplace> Workplace { get; set; }
		}

	[XmlRoot(ElementName = "results")]
		public class Results
		{
			[XmlElement(ElementName = "forecast")]
			public Forecast Forecast { get; set; }
			[XmlElement(ElementName = "warehousestock")]
			public Warehousestock Warehousestock { get; set; }
			[XmlElement(ElementName = "inwardstockmovement")]
			public Inwardstockmovement Inwardstockmovement { get; set; }
			[XmlElement(ElementName = "futureinwardstockmovement")]
			public Futureinwardstockmovement Futureinwardstockmovement { get; set; }
			[XmlElement(ElementName = "idletimecosts")]
			public Idletimecosts Idletimecosts { get; set; }
			[XmlElement(ElementName = "waitinglistworkstations")]
			public Waitinglistworkstations Waitinglistworkstations { get; set; }
			[XmlElement(ElementName = "waitingliststock")]
			public Waitingliststock Waitingliststock { get; set; }
			[XmlElement(ElementName = "ordersinwork")]
			public Ordersinwork Ordersinwork { get; set; }
			[XmlElement(ElementName = "completedorders")]
			public Completedorders Completedorders { get; set; }
			[XmlElement(ElementName = "cycletimes")]
			public Cycletimes Cycletimes { get; set; }
			[XmlElement(ElementName = "result")]
			public Result Result { get; set; }
			[XmlAttribute(AttributeName = "game")]
			public string Game { get; set; }
			[XmlAttribute(AttributeName = "group")]
			public string Group { get; set; }
			[XmlAttribute(AttributeName = "period")]
			public string Period { get; set; }
	}
}
