public class VersionCSVParser : CSVParser
{
	public override void Parse ()
	{
		VersionCSV.versionCSV = csvContext.Read<VersionCSVStructure> (PathConstant.CLIENT_SERVER_VERSION_CSV);
	}
}
