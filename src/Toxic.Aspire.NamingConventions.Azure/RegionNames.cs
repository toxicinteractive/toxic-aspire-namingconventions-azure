using Azure.Core;

namespace Toxic.Aspire.NamingConventions.Azure;

/// <summary>
/// Contains abbreviations for common Azure regions.
/// Taken from https://github.com/Azure/terraform-azurerm-avm-utl-regions/blob/main/locals.geo.codes.tf.json
/// </summary>
public static class RegionNames
{
    public static readonly IDictionary<string, string> Regions = new Dictionary<string, string>
    {
        { AzureLocation.AustraliaCentral.Name, "acl" },
        { AzureLocation.AustraliaCentral2.Name, "acl2" },
        { AzureLocation.AustraliaEast.Name, "ae" },
        { AzureLocation.AustraliaSoutheast.Name, "ase" },
        { AzureLocation.BrazilSouth.Name, "brs" },
        { AzureLocation.BrazilSoutheast.Name, "bse" },
        { AzureLocation.CanadaCentral.Name, "cnc" },
        { AzureLocation.CanadaEast.Name, "cne" },
        { AzureLocation.CentralIndia.Name, "inc" },
        { AzureLocation.CentralUS.Name, "cus" },
        { AzureLocation.DenmarkEast.Name, "dke" },
        { AzureLocation.EastAsia.Name, "ea" },
        { AzureLocation.EastUS.Name, "eus" },
        { AzureLocation.EastUS2.Name, "eus2" },
        { AzureLocation.FranceCentral.Name, "frc" },
        { AzureLocation.FranceSouth.Name, "frs" },
        { AzureLocation.GermanyNorth.Name, "gn" },
        { AzureLocation.GermanyWestCentral.Name, "gwc" },
        { AzureLocation.IndonesiaCentral.Name, "idc" },
        { AzureLocation.IsraelCentral.Name, "ilc" },
        { AzureLocation.ItalyNorth.Name, "itn" },
        { AzureLocation.JapanEast.Name, "jpe" },
        { AzureLocation.JapanWest.Name, "jpw" },
        { AzureLocation.KoreaCentral.Name, "krc" },
        { AzureLocation.KoreaSouth.Name, "krs" },
        { AzureLocation.MalaysiaWest.Name, "myw" },
        { AzureLocation.MexicoCentral.Name, "mxc" },
        { AzureLocation.NewZealandNorth.Name, "nzn" },
        { AzureLocation.NorthCentralUS.Name, "ncus" },
        { AzureLocation.NorthEurope.Name, "ne" },
        { AzureLocation.NorwayEast.Name, "nwe" },
        { AzureLocation.NorwayWest.Name, "nww" },
        { AzureLocation.PolandCentral.Name, "plc" },
        { AzureLocation.QatarCentral.Name, "qac" },
        { AzureLocation.SouthAfricaNorth.Name, "san" },
        { AzureLocation.SouthAfricaWest.Name, "saw" },
        { AzureLocation.SouthCentralUS.Name, "scus" },
        { AzureLocation.SoutheastAsia.Name, "sea" },
        { AzureLocation.SouthIndia.Name, "ins" },
        { AzureLocation.SpainCentral.Name, "spc" },
        { AzureLocation.SwedenCentral.Name, "sdc" },
        { AzureLocation.SwedenSouth.Name, "sds" },
        { AzureLocation.SwitzerlandNorth.Name, "szn" },
        { AzureLocation.SwitzerlandWest.Name, "szw" },
        { AzureLocation.UAECentral.Name, "uac" },
        { AzureLocation.UAENorth.Name, "uan" },
        { AzureLocation.UKSouth.Name, "uks" },
        { AzureLocation.UKWest.Name, "ukw" },
        { AzureLocation.WestCentralUS.Name, "wcus" },
        { AzureLocation.WestEurope.Name, "we" },
        { AzureLocation.WestIndia.Name, "inw" },
        { AzureLocation.WestUS.Name, "wus" },
        { AzureLocation.WestUS2.Name, "wus2" },
        { AzureLocation.WestUS3.Name, "wus3" }
    };
}
