# Query

Query the CLDR for the fractional digits of the Japanese Yen (JPY)

    var jpy = Cldr.Instance
        .GetDocuments("common/supplemental/supplementalData.xml")
        .FirstElement("supplementalData/currencyData/fractions/info[@iso4217='JPY']");
    Assert.AreEqual("0", jpy.Attribute("digits").Value);
