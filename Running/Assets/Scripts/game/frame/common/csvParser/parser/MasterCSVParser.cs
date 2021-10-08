using System.Linq;
using UnityEngine;
using System.Collections.Generic;

public class MasterCSVParser : CSVParser
{
	private const string CARD = "m_card.csv";
	private const string GAME = "m_game.csv";
	private const string ITEM = "m_item.csv";
	private const string MISSION = "m_mission.csv";
	private const string PRODUCT = "m_product.csv";
	private const string CHARGE_LIMIT = "m_charge_limit.csv";
	private const string LOGIN_BONUS_DETAIL = "m_login_bonus_detail.csv";
	private const string EXCHANGE = "m_exchange.csv";
	private const string SOUND = "m_sound.csv";
	private const string CONVENTION = "m_convention.csv";
	private const string HELP = "m_help.csv";
	private const string NG = "m_ng.csv";
	private const string EVENT_RULE = "m_event_rule.csv";

	public override void Parse ()
	{
		if (FileManager.Exists (PathConstant.CLIENT_CSV_PATH + CARD)) {
			MasterCSV.cardCSV = csvContext.Read<CardCSVStructure> (PathConstant.CLIENT_CSV_PATH + CARD).ToList ();
		}
		if (FileManager.Exists (PathConstant.CLIENT_CSV_PATH + GAME)) {
			MasterCSV.gameCSV = csvContext.Read<GameCSVStructure> (PathConstant.CLIENT_CSV_PATH + GAME).ToList ();
		}
		if (FileManager.Exists (PathConstant.CLIENT_CSV_PATH + ITEM)) {
			MasterCSV.itemCSV = csvContext.Read<ItemCSVStructure> (PathConstant.CLIENT_CSV_PATH + ITEM).ToList ();
		}
		if (FileManager.Exists (PathConstant.CLIENT_CSV_PATH + MISSION)) {
			MasterCSV.missionCSV = csvContext.Read<MissionCSVStructure> (PathConstant.CLIENT_CSV_PATH + MISSION).ToList ();
		}
		if (FileManager.Exists (PathConstant.CLIENT_CSV_PATH + PRODUCT)) {
			MasterCSV.productCSV = csvContext.Read<ProductCSVStructure> (PathConstant.CLIENT_CSV_PATH + PRODUCT).ToList ();
		}
		if (FileManager.Exists (PathConstant.CLIENT_CSV_PATH + CHARGE_LIMIT)) {
			MasterCSV.chargeLimitCSV = csvContext.Read<ChargeLimitCSVStructure> (PathConstant.CLIENT_CSV_PATH + CHARGE_LIMIT).ToList ();
		}
		if (FileManager.Exists (PathConstant.CLIENT_CSV_PATH + LOGIN_BONUS_DETAIL)) {
			MasterCSV.loginBonusDetailCSV = csvContext.Read<LoginBonusDetailCSVStructure> (PathConstant.CLIENT_CSV_PATH + LOGIN_BONUS_DETAIL).ToList ();
		}
		if (FileManager.Exists (PathConstant.CLIENT_CSV_PATH + EXCHANGE)) {
			MasterCSV.exchangeCSV = csvContext.Read<ExchangeCSVStructure> (PathConstant.CLIENT_CSV_PATH + EXCHANGE).ToList ();
		}
		if (FileManager.Exists (PathConstant.CLIENT_CSV_PATH + SOUND)) {
			MasterCSV.soundCSV = csvContext.Read<SoundCSVStructure> (PathConstant.CLIENT_CSV_PATH + SOUND).ToList ();
		}
		if (FileManager.Exists (PathConstant.CLIENT_CSV_PATH + CONVENTION)) {
			MasterCSV.conventionCSV = csvContext.Read<ConventionCSVStructure> (PathConstant.CLIENT_CSV_PATH + CONVENTION).ToList ();
		}
		if (FileManager.Exists (PathConstant.CLIENT_CSV_PATH + HELP)) {
			MasterCSV.helpCSV = csvContext.Read<HelpCSVStructure> (PathConstant.CLIENT_CSV_PATH + HELP).ToList ();
		}
		if (FileManager.Exists (PathConstant.CLIENT_CSV_PATH + NG)) {
			MasterCSV.ngCSV = csvContext.Read<NGCSVStructure> (PathConstant.CLIENT_CSV_PATH + NG).ToList ();
		}
		if (FileManager.Exists (PathConstant.CLIENT_CSV_PATH + EVENT_RULE)) {
			MasterCSV.eventRuleCSV = csvContext.Read<EventRuleCSVStructure> (PathConstant.CLIENT_CSV_PATH + EVENT_RULE).ToList ();
		}
	}
}
