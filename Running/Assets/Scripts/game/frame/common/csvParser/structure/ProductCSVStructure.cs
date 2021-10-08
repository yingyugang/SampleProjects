using CSV;

public class ProductCSVStructure
{
	[CsvColumn (CanBeNull = true)]
	public int id{ get; set; }

	[CsvColumn (CanBeNull = true)]
	public string product_id{ get; set; }

	[CsvColumn (CanBeNull = true)]
	public int platform_type{ get; set; }

	[CsvColumn (CanBeNull = true)]
	public string platform_name{ get; set; }

	[CsvColumn (CanBeNull = true)]
	public string name{ get; set; }

	[CsvColumn (CanBeNull = true)]
	public string description{ get; set; }

	[CsvColumn (CanBeNull = true)]
	public string image_resouce_name{ get; set; }

	[CsvColumn (CanBeNull = true)]
	public int coin{ get; set; }

	[CsvColumn (CanBeNull = true)]
	public int free_coin{ get; set; }

	[CsvColumn (CanBeNull = true)]
	public int jpy_amount{ get; set; }

	[CsvColumn (CanBeNull = true)]
	public int is_ad_product{ get; set; }

	public int limit_shop_id{ get; set; }

	public int limit_count { get; set; }

	public int buy_count { get; set; }

	[CsvColumn (CanBeNull = true)]
	public int is_special { get; set; }
}
